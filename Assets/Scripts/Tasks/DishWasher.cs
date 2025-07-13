using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DishWasher : MonoBehaviour
{
    [SerializeField] private float washingTime = 20f;
    [SerializeField] private Transform washingPoint;
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip washingSoundLoop;
    [SerializeField] private AudioClip finishedSound;

    [SerializeField] private Animator _anim;

    private List<PickupDish> itemsInside = new();
    private bool isWashing = false;
    private bool isFinished = false;
    private int _cleanedCount = 0;
    private const int _totalRequired = 4;

    private void Awake()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

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
                    _audioSource.Stop();
                    _anim.SetBool("IsWashing", false);
                }
                else if (!isWashing && player.HasItems())
                {
                    // 1. Recibimos la lista tal cual la devuelve el Player (MonoBehaviour)
                    List<MonoBehaviour> dropped = player.DropAllItemsTo(washingPoint);

                    _anim.SetBool("IsWashing", true);

                    // 2. Se la pasamos a ReceiveItems ›› ya hemos cambiado su firma
                    ReceiveItems(dropped);
                }


                break; // Solo interactuar con un jugador
            }
        }
    }

    private void ReturnCleanItemsToPlayer(PlayerPickUp player)
    {
        if (itemsInside.Count == 0) return;

        foreach (var item in itemsInside)
        {
            SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            Collider2D col = item.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        List<MonoBehaviour> cleaned = new();
        foreach (var dish in itemsInside)
        {
            cleaned.Add(dish); // PickupDish hereda de MonoBehaviour
        }

        player.ReceiveCleanItems(cleaned);
        itemsInside.Clear();
        isFinished = false;
        Debug.Log("Chema recoge los objetos limpios");
    }

    public bool CanAcceptItems() => !isWashing;

    public void ReceiveItems(List<MonoBehaviour> items)
    {
        itemsInside.Clear();

        foreach (var mb in items)
        {
            if (mb is PickupDish item)
            {
                item.transform.position = washingPoint.position;
                item.SetClean(false);

                SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) sr.enabled = false;

                Collider2D col = item.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;

                Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                itemsInside.Add(item);
            }
        }

        if (itemsInside.Count > 0)
        {
            StartCoroutine(WashItems());
        }
    }

    private IEnumerator WashItems()
    {
        isWashing = true;

        // Reproducir sonido de lavado en bucle
        if (_audioSource != null && washingSoundLoop != null)
        {
            _audioSource.clip = washingSoundLoop;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(washingTime);

        foreach (var item in itemsInside)
        {
            item.SetClean(true);
            _cleanedCount++;
        }

        isWashing = false;
        isFinished = true;

        // Detener el bucle y reproducir sonido de fin
        if (_audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.loop = false;

            if (finishedSound != null)
                _audioSource.PlayOneShot(finishedSound);
        }

        _anim.SetBool("IsWashing", false);
        TaskManager.Instance.EndTask(4, (float)_cleanedCount / _totalRequired * 100f);
        Debug.Log("Lavadora terminó de lavar");
        Debug.Log($"Objetos lavados: {(float)_cleanedCount / _totalRequired * 100f}%");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
