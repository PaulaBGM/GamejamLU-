using UnityEngine;

public class SweepTask : InteractableTask
{
    public float skateSpeed = 4f;
    public float sweepDuration = 5f;

    private bool sweeping = false;
    private float sweepTimer = 0f;
    private PlayerController playerController;

    protected override void PerformTask()
    {
        if (sweeping) return;

        // Buscar al PlayerController
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        if (playerController == null) return;

        // Activamos el modo skate
        sweeping = true;
        sweepTimer = sweepDuration;

        // Bloquear el control del jugador
        playerController.enabled = false;

        Debug.Log("¡Te subiste a la escoba!");
    }

    protected override void Update()
    {
        base.Update();

        if (!sweeping) return;

        // Mover al jugador automáticamente hacia la derecha
        if (playerController != null)
        {
            playerController.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(skateSpeed, 0f);
        }

        // Contador de duración
        sweepTimer -= Time.deltaTime;
        if (sweepTimer <= 0f)
        {
            // Fin del barrido
            sweeping = false;
            if (playerController != null)
            {
                playerController.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                playerController.enabled = true;
            }

            Debug.Log("¡Tarea de barrer completada!");
            gameObject.SetActive(false);
        }
    }
}
