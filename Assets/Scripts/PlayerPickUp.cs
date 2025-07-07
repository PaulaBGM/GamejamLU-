using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float stackHeight = 0.5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange = 1f;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;

    private readonly List<PickupItem> collectedItems = new();

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickupLayer);

        foreach (var hit in hits)
        {
            PickupItem item = hit.GetComponent<PickupItem>();

            if (item != null && !item.IsCollected && !item.IsClean)
            {
                item.SetCollected(true);
                item.transform.SetParent(holdPoint);

                int index = collectedItems.Count;
                Vector3 localPos = Vector3.up * (index * stackHeight);
                item.transform.localPosition = localPos;

                item.StartMoveToPosition(localPos, moveSpeed);

                Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                collectedItems.Add(item);
                break; // Solo recoge uno por pulsación
            }
        }
    }

    public bool HasItems()
    {
        return collectedItems.Count > 0;
    }

    /// <summary>
    /// Suelta todos los ítems recogidos y los mueve al transform indicado (por ejemplo, la lavadora).
    /// </summary>
    public List<PickupItem> DropAllItemsTo(Transform destination)
    {
        List<PickupItem> itemsToDrop = new(collectedItems);
        collectedItems.Clear();

        Debug.Log("Chema deja los objetos");

        foreach (PickupItem item in itemsToDrop)
        {
            item.transform.SetParent(destination);
            item.SetCollected(false);

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        return itemsToDrop;
    }

    /// <summary>
    /// Recibe los objetos limpios desde la lavadora y los apila sobre el jugador.
    /// </summary>
    public void ReceiveCleanItems(List<PickupItem> cleanedItems)
    {
        foreach (PickupItem item in cleanedItems)
        {
            item.SetCollected(true);
            item.transform.SetParent(holdPoint);

            int index = collectedItems.Count;
            Vector3 localPos = Vector3.up * (index * stackHeight);
            item.transform.localPosition = localPos;

            item.StartMoveToPosition(localPos, moveSpeed);

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            collectedItems.Add(item);
        }
    }
}
