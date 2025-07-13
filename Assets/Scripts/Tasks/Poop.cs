using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        if (transform.position.y < -10f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform stackPoint = other.transform.Find("stackpoint");

            if (stackPoint != null)
            {
                foreach (Transform child in stackPoint)
                {
                    PickupItem item = child.GetComponent<PickupItem>();
                    if (item != null)
                    {
                        item.SetCollected(true);
                        item.SetClean(false);
                        item.CheckPickUpState();
                    }
                }
            }
            else
            {
                Debug.LogWarning("No se encontró StackPoint en el jugador.");
            }

            Destroy(gameObject); //  IMPORTANTE: se destruye al tocar al jugador
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject); //  Se destruye si toca algo en la capa Ground
        }
    }
}
