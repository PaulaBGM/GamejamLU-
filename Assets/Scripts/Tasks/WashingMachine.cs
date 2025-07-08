using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{
    [SerializeField] private float washingTime = 20f;
    [SerializeField] private Transform washingPoint;
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private List<PickupItem> itemsInside = new();
    private bool isWashing = false;
    private bool isFinished = false;
    private int _cleanedCount = 0;
    private const int _totalRequired = 6;

    private void Update()
    {
        if (!Input.GetKeyDown(interactionKey)) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (Collider2D collider in colliders)
        {
            PlayerPickUp player = collider.GetComponent<PlayerPickUp>();
            if (player != null)
            {
                if (isFinished)
                {
                    ReturnCleanItemsToPlayer(player);
                }
                else if (!isWashing && player.HasItems())
                {
                    List<PickupItem> items = player.DropAllItemsTo(washingPoint);
                    ReceiveItems(items);
                }

                break; // Solo interactuar con un jugador
            }
        }
    }

    private void ReturnCleanItemsToPlayer(PlayerPickUp player)
    {
        if (itemsInside.Count == 0) return;

        // Hacer visibles los objetos antes de devolverlos
        foreach (var item in itemsInside)
        {
            SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            // Reactivar colisiones si tienes collider
            Collider2D col = item.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            // Cambiar Rigidbody a cinemático para control del jugador
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        player.ReceiveCleanItems(itemsInside);
        itemsInside.Clear();
        isFinished = false;
        Debug.Log("Chema recoge los objetos limpios");
    }

    public bool CanAcceptItems()
    {
        return !isWashing;
    }

    public void ReceiveItems(List<PickupItem> items)
    {
        if (!CanAcceptItems()) return;

        itemsInside = items;

        foreach (var item in itemsInside)
        {
            item.transform.position = washingPoint.position;
            item.SetClean(false); // Aún están sucios al llegar

            // Ocultar objeto
            SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            // Desactivar colisiones
            Collider2D col = item.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // Poner Rigidbody dinámico para que no caiga y no se mueva
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        StartCoroutine(WashItems());
    }

    private IEnumerator WashItems()
    {
        isWashing = true;
        yield return new WaitForSeconds(washingTime);

        foreach (var item in itemsInside)
        {
            item.SetClean(true);
            _cleanedCount++;
        }

        isWashing = false;
        isFinished = true;
        TaskManager.Instance.EndTask(6, (float)_cleanedCount / _totalRequired * 100f);
        Debug.Log("Lavadora terminó de lavar");
        Debug.Log($"Objetos lavados: {(float)_cleanedCount / _totalRequired * 100f}%");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

