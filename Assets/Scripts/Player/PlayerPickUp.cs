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
                item.CheckPickUpState();
                item.transform.SetParent(holdPoint);

                int index = collectedItems.Count;
                Vector3 localPos = Vector3.up * (index * stackHeight);
                item.transform.localPosition = localPos;
                item.transform.localRotation = Quaternion.identity;
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

    public List<PickupItem> DropAllItemsTo(Transform destination)
    {
        List<PickupItem> itemsToDrop = new(collectedItems);
        collectedItems.Clear();

        foreach (PickupItem item in itemsToDrop)
        {
            item.transform.SetParent(destination);
            item.SetCollected(false);
            item.CheckPickUpState();

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
            item.IsClean = true;
            item.CheckPickUpState();
            item.transform.SetParent(holdPoint);

            int index = collectedItems.Count;
            Vector3 localPos = Vector3.up * (index * stackHeight);
            item.transform.localPosition = localPos;
            item.transform.localRotation = Quaternion.identity;
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

    public PickupItem RemoveFirstCleanItem()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            if (collectedItems[i].IsClean)
            {
                PickupItem item = collectedItems[i];
                collectedItems.RemoveAt(i);
                item.transform.SetParent(null);
                return item;
            }
        }

        return null;
    }
}
