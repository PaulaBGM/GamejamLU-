using UnityEngine;

public class PlayerCarryWine : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float pickupRange = 1f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask wineLayer;

    private WineBottle carriedBottle;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (carriedBottle == null)
            {
                TryPickUpWine();
            }
            else
            {
                TryDeliverWine();
            }
        }
    }

    private void TryPickUpWine()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, wineLayer);
        foreach (var hit in hits)
        {
            WineBottle bottle = hit.GetComponent<WineBottle>();
            if (bottle != null && !bottle.IsCollected)
            {
                carriedBottle = bottle;
                bottle.SetCollected(true);
                bottle.AttachTo(holdPoint, Vector3.zero);
                Debug.Log("Botella recogida");
                break;
            }
        }
    }

    private void TryDeliverWine()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (var hit in hits)
        {
            Fridge fridge = hit.GetComponent<Fridge>();
            if (fridge != null)
            {
                fridge.ReceiveWine(carriedBottle);
                carriedBottle = null;
                Debug.Log("Botella entregada a la nevera");
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
