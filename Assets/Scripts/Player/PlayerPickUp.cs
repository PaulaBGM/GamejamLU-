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
    private readonly List<PickupDish> collectedDishes = new();

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
            // Primero intentamos recoger un PickupItem
            PickupItem item = hit.GetComponent<PickupItem>();
            if (item != null && !item.IsCollected && !item.IsClean)
            {
                item.SetCollected(true);
                item.CheckPickUpState();
                item.transform.SetParent(holdPoint);

                int index = collectedItems.Count + collectedDishes.Count;
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
                break;
            }

            // Luego intentamos con un PickupDish
            PickupDish dish = hit.GetComponent<PickupDish>();
            if (dish != null && !dish.IsCollected && !dish.IsClean)
            {
                dish.SetCollected(true);
                dish.CheckPickUpState();
                dish.transform.SetParent(holdPoint);

                int index = collectedItems.Count + collectedDishes.Count;
                Vector3 localPos = Vector3.up * (index * stackHeight);
                dish.transform.localPosition = localPos;
                dish.transform.localRotation = Quaternion.identity;
                dish.StartMoveToPosition(localPos, moveSpeed);

                Rigidbody2D rb = dish.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                collectedDishes.Add(dish);
                break;
            }
        }
    }

    public bool HasItems()
    {
        return collectedItems.Count > 0 || collectedDishes.Count > 0;
    }

    public List<MonoBehaviour> DropAllItemsTo(Transform destination)
    {
        List<MonoBehaviour> itemsToDrop = new();

        foreach (PickupItem item in collectedItems)
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

            itemsToDrop.Add(item);
        }

        foreach (PickupDish dish in collectedDishes)
        {
            dish.transform.SetParent(destination);
            dish.SetCollected(false);
            dish.CheckPickUpState();

            Rigidbody2D rb = dish.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            itemsToDrop.Add(dish);
        }

        collectedItems.Clear();
        collectedDishes.Clear();

        return itemsToDrop;
    }

    public void ReceiveCleanItems(List<MonoBehaviour> cleanedItems)
    {
        foreach (MonoBehaviour mb in cleanedItems)
        {
            if (mb is PickupItem item)
            {
                item.SetCollected(true);
                item.IsClean = true;
                item.CheckPickUpState();
                item.transform.SetParent(holdPoint);

                int index = collectedItems.Count + collectedDishes.Count;
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
            else if (mb is PickupDish dish)
            {
                dish.SetCollected(true);
                dish.IsClean = true;
                dish.CheckPickUpState();
                dish.transform.SetParent(holdPoint);

                int index = collectedItems.Count + collectedDishes.Count;
                Vector3 localPos = Vector3.up * (index * stackHeight);
                dish.transform.localPosition = localPos;
                dish.transform.localRotation = Quaternion.identity;
                dish.StartMoveToPosition(localPos, moveSpeed);

                Rigidbody2D rb = dish.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                collectedDishes.Add(dish);
            }
        }
    }

    public MonoBehaviour RemoveFirstCleanItem()
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

        for (int i = 0; i < collectedDishes.Count; i++)
        {
            if (collectedDishes[i].IsClean)
            {
                PickupDish dish = collectedDishes[i];
                collectedDishes.RemoveAt(i);
                dish.transform.SetParent(null);
                return dish;
            }
        }

        return null;
    }
}
