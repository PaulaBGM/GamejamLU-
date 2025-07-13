using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] private Transform storagePoint;

    public void ReceiveWine(WineBottle bottle)
    {
        if (bottle == null) return;

        bottle.SetCollected(false);
        bottle.AttachTo(storagePoint, Vector3.zero);

        Rigidbody2D rb = bottle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D col = bottle.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("La nevera ha recibido la botella de vino");
    }
}
