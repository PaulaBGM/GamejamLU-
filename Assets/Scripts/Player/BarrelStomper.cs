using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BarrelStomper : MonoBehaviour
{
    [Header("Configuración de Pisotón")]
    [SerializeField] private float stompCooldown = 0.1f;

    [Header("Multiplicadores de Caída")]
    [SerializeField] private float normalFallMultiplier = 1.5f;
    [SerializeField] private float boostedFallMultiplier = 3.5f;

    private Rigidbody2D rb;
    private float lastStompTime;
    private bool minigameStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Aplicamos el fallMultiplier solo cuando el personaje está cayendo
        if (minigameStarted && rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (boostedFallMultiplier - 1f) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Barrel")) return;
        if (Time.time - lastStompTime < stompCooldown) return;

        lastStompTime = Time.time;

        // Iniciar el minijuego si aún no lo hemos hecho
        if (!minigameStarted)
        {
            minigameStarted = true;
            Debug.Log("Minijuego iniciado: velocidad de caída aumentada.");
        }

        // Notificar al barril que fue pisado
        BarrelPad pad = collision.collider.GetComponent<BarrelPad>();
        if (pad != null) pad.OnStomp();
    }

    // Método público para finalizar el minijuego
    public void EndMinigame()
    {
        if (!minigameStarted) return;

        minigameStarted = false;
        Debug.Log("Minijuego terminado: velocidad de caída restaurada.");
    }
}
