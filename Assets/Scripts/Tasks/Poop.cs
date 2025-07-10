using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private bool isBlackPoop = false;

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        if (transform.position.y < -10f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {/*
        if (other.CompareTag("Player"))
        {
            //PlayerLaundry player = other.GetComponent<PlayerLaundry>();
            if (player != null)
            {
                if (isBlackPoop)
                    player.SoilLaundry(true);  // negra: ensucia toda la pila
                else
                    player.SoilLaundry(false); // blanca: ensucia solo la de arriba
            }

            Destroy(gameObject);
        }*/
    }
}
