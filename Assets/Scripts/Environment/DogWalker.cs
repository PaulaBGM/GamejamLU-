using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DogWalker : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float speedVariation = 0.8f; // qué tanto fluctúa (0.8 = ±80%)
    [SerializeField] private float variationFrequency = 0.5f; // velocidad de cambio (ruido Perlin)

    [Header("Detección de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Detección de paredes")]
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private float wallCheckRadius = 0.1f;

    [SerializeField] private LayerMask groundLayer;

    [Header("Cooldown")]
    [SerializeField] private float flipCooldown = 0.3f;
    private float lastFlipTime = -Mathf.Infinity;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        float noise = Mathf.PerlinNoise(Time.time * variationFrequency, 0f);
        float fluctuatingSpeed = baseSpeed * (1f + (noise - 0.5f) * 2f * speedVariation); // ajusta entre ±variation

        Vector2 currentVelocity = rb.linearVelocity;
        currentVelocity.x = (movingRight ? 1 : -1) * fluctuatingSpeed;
        rb.linearVelocity = currentVelocity;

        bool isBlocked = movingRight
            ? Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, groundLayer)
            : Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, groundLayer);

        if (isBlocked && Time.time - lastFlipTime > flipCooldown)
        {
            Flip();
            lastFlipTime = Time.time;
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(initialScale.x) * (movingRight ? 1 : -1);
        transform.localScale = scale;

    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        if (rightWallCheck != null)
            Gizmos.DrawWireSphere(rightWallCheck.position, wallCheckRadius);
        if (leftWallCheck != null)
            Gizmos.DrawWireSphere(leftWallCheck.position, wallCheckRadius);
    }
}
