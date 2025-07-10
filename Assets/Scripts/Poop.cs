using UnityEngine;

// Script para que la caca caiga y ensucie al jugador
public class Poop : MonoBehaviour
{
    public float fallSpeed = 5f; // Velocidad de caída

    void Update()
    {
        // Movemos la caca hacia abajo
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Si cae demasiado, la destruimos para optimizar
        if (transform.position.y < -10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            PlayerLaundry player = other.GetComponent<PlayerLaundry>();
            if (player != null)
                player.SoilLaundry(); // Llamamos al método para ensuciar la ropa

            Destroy(gameObject); // Destruimos la caca tras golpear
        }
    }
}
