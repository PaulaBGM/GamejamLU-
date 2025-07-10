using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja la lógica de recogida y transporte de ítems.
/// </summary>
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
        if (Input.GetKeyDown(pickupKey)) TryPickup();
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
                item.StartMoveToPosition(localPos, moveSpeed);

                Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                collectedItems.Add(item);
                break;
            }
        }
    }

    public bool HasItems() => collectedItems.Count > 0;

    public List<PickupItem> DropAllItemsTo(Transform destination)
    {
        List<PickupItem> itemsToDrop = new(collectedItems);
        collectedItems.Clear();

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

    public void ReceiveCleanItems(List<PickupItem> cleanedItems)
    {
        foreach (PickupItem item in cleanedItems)
        {
            item.SetCollected(true);
            item.transform.SetParent(holdPoint);

            int index = collectedItems.Count;
            Vector3 localPos = Vector3.up * (index * stackHeight);
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

    public List<PickupItem> GetCleanItems()
    {
        return collectedItems.FindAll(i => i.IsClean);
    }

    public void RemoveItem(PickupItem item)
    {
        if (collectedItems.Contains(item))
        {
            collectedItems.Remove(item);
        }
    }
    // Devuelve una lista de solo los objetos limpios y los elimina del stack
    public List<PickupItem> DropOnlyCleanItems()
    {
        List<PickupItem> cleanItems = collectedItems.FindAll(item => item.IsClean);
        collectedItems.RemoveAll(item => item.IsClean);

        foreach (var item in cleanItems)
        {
            item.SetCollected(false);
            item.transform.SetParent(null);

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.zero;
            }
        }

        return cleanItems;
    }

    public bool HasCleanItems()
    {
        return collectedItems.Exists(item => item.IsClean);
    }

}
