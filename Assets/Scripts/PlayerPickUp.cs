using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permite al jugador recoger objetos interactuables (ropa, platos, etc.)
/// y apilarlos en su espalda.
/// </summary>
public class PlayerPickUp : MonoBehaviour
{
    [Header("Configuración de Recolección")]
    [SerializeField] private Transform stackPoint;
    [SerializeField] private float itemSpacing = 0.5f;
    [SerializeField] private float pickupRadius = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask pickupLayer;

    private readonly List<PickupItem> collectedItems = new();

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryPickupNearestItem();
        }
    }

    private void TryPickupNearestItem()
    {
        // Buscar todos los colliders en el radio de recolección
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupLayer);

        float closestSqrMag = float.MaxValue;
        PickupItem closestItem = null;
        Vector2 currentPos = transform.position;

        foreach (var col in colliders)
        {
            PickupItem item = col.GetComponent<PickupItem>();
            if (item == null || item.IsCollected)
                continue;

            float sqrMag = ((Vector2)item.transform.position - currentPos).sqrMagnitude;
            if (sqrMag < closestSqrMag)
            {
                closestSqrMag = sqrMag;
                closestItem = item;
            }
        }

        if (closestItem != null)
        {
            Pickup(closestItem);
        }
    }

    private void Pickup(PickupItem item)
    {
        item.SetCollected(true);
        collectedItems.Add(item);

        item.transform.SetParent(stackPoint);
        Vector3 localTarget = new(0f, itemSpacing * (collectedItems.Count - 1), 0f);

        item.StartMoveToPosition(localTarget, moveSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
