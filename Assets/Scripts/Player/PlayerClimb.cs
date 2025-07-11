using UnityEngine;

/// <summary>
/// Maneja el comportamiento del jugador al subir escaleras.
/// Soporta escaleras frontales (alineación horizontal) y escaleras laterales.
/// Ignora colisiones con el techo y el suelo de la escalera mientras se escala.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerClimb : MonoBehaviour
{
    [Header("Configuración de Escaleras")]
    [SerializeField] private float climbSpeed = 3f;               // Velocidad al escalar
    [SerializeField] private KeyCode interactKey = KeyCode.E;     // Tecla para comenzar a escalar (opcional)

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;

    private bool isOnStairs = false;
    private bool isClimbing = false;

    private float verticalInput;
    private StairZone currentStairZone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");

        // Iniciar escalada si se pulsa tecla o se mueve hacia arriba/abajo
        if (isOnStairs && !isClimbing && (Input.GetKeyDown(interactKey) || Mathf.Abs(verticalInput) > 0.1f))
        {
            StartClimbing();
        }

        // Cancelar escalada si sale del trigger
        if (!isOnStairs && isClimbing)
        {
            ResetClimb();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            // Movimiento vertical fluido mientras escala
            if (Mathf.Abs(verticalInput) > 0.01f)
            {
                rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = true;
            currentStairZone = other.GetComponent<StairZone>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = false;
        }
    }

    /// <summary>
    /// Inicia la escalada: sin gravedad, alinea si es necesario y desactiva colisiones.
    /// </summary>
    private void StartClimbing()
    {
        if (isClimbing || currentStairZone == null) return;

        isClimbing = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        // Alinear en X solo si no es escalera lateral
        if (!currentStairZone.isSideStair && currentStairZone.stairCollider != null)
        {
            Vector3 pos = transform.position;
            pos.x = currentStairZone.stairCollider.bounds.center.x;
            transform.position = pos;
        }

        // Ignorar colisiones
        if (playerCollider != null)
        {
            if (currentStairZone.ceilingCollider != null)
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.ceilingCollider, true);

            if (currentStairZone.stairCollider != null)
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.stairCollider, true);
        }

        // Animación
        if (animator != null)
        {
            animator.SetBool("isClimbing", true);
        }
    }

    /// <summary>
    /// Sale del modo de escalada y restaura colisiones.
    /// </summary>
    private void ResetClimb()
    {
        isClimbing = false;
        rb.gravityScale = 1f;

        if (playerCollider != null && currentStairZone != null)
        {
            if (currentStairZone.ceilingCollider != null)
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.ceilingCollider, false);

            if (currentStairZone.stairCollider != null)
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.stairCollider, false);
        }

        if (animator != null)
        {
            animator.SetBool("isClimbing", false);
        }

        currentStairZone = null;
    }
}