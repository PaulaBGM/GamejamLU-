using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BarrelStomper : MonoBehaviour
{
    [SerializeField] private float stompCooldown = 0.1f; // evita múltiples colisiones
    private float lastStompTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Barrel")) return;

        if (Time.time - lastStompTime < stompCooldown) return; // evita doble entrada
        lastStompTime = Time.time;

        // Notificar al pad que fue pisado
        BarrelPad pad = collision.collider.GetComponent<BarrelPad>();
        if (pad != null)
            pad.OnStomp();
    }
}
