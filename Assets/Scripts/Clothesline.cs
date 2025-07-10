using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tendedero que permite colgar prendas limpias si el jugador salta y llega hasta él.
/// </summary>
public class Clothesline : MonoBehaviour
{
    [SerializeField] private Transform[] hangPoints; // Puntos de colgado
    [SerializeField] private KeyCode hangKey = KeyCode.E;
    [SerializeField] private float interactionRadius = 1.5f;

    private bool playerInRange = false;
    private PlayerPickUp player;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(hangKey) && player != null && player.HasCleanItems())
        {
            HangCleanItems(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerPickUp>();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerInRange = false;
        }
    }

    private void HangCleanItems(PlayerPickUp player)
    {
        List<PickupItem> items = player.DropOnlyCleanItems();

        for (int i = 0; i < items.Count && i < hangPoints.Length; i++)
        {
            PickupItem item = items[i];
            item.transform.SetParent(hangPoints[i]);
            item.transform.localPosition = Vector3.zero;
            item.StopMovement();

            // Reaparece visualmente colgado
            SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.enabled = true;
        }

        Debug.Log("Ropa colgada en el tendedero");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
