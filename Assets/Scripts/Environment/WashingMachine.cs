using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lógica de lavado y devolución de ropa limpia.
/// </summary>
public class WashingMachine : MonoBehaviour
{
    [SerializeField] private float washingTime = 20f;
    [SerializeField] private Transform washingPoint;
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private List<PickupItem> itemsInside = new();
    private bool isWashing = false;
    private bool isFinished = false;

    private void Update()
    {
        if (!Input.GetKeyDown(interactionKey)) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        foreach (Collider2D col in colliders)
        {
            PlayerPickUp player = col.GetComponent<PlayerPickUp>();
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
                break;
            }
        }
    }

    private void ReturnCleanItemsToPlayer(PlayerPickUp player)
    {
        foreach (var item in itemsInside)
        {
            var sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.enabled = true;

            var col = item.GetComponent<Collider2D>();
            if (col) col.enabled = true;

            var rb = item.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
        }

        player.ReceiveCleanItems(itemsInside);
        itemsInside.Clear();
        isFinished = false;
    }

    public void ReceiveItems(List<PickupItem> items)
    {
        if (isWashing) return;

        itemsInside = items;

        foreach (var item in itemsInside)
        {
            item.transform.position = washingPoint.position;
            item.SetClean(false);

            var sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.enabled = false;

            var col = item.GetComponent<Collider2D>();
            if (col) col.enabled = false;

            var rb = item.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
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
            item.SetClean(true);

        isWashing = false;
        isFinished = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
