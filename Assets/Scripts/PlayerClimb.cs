using UnityEngine;

/// <summary>
/// Maneja el comportamiento del jugador al subir escaleras.
/// Incluye detección de interacción, alineación horizontal y desactivación de colisión con el techo.
/// </summary>
public class PlayerClimb : MonoBehaviour
{
    [Header("Configuración de Escaleras")]
    [SerializeField] private float climbSpeed = 3f;               // Velocidad al escalar
    [SerializeField] private KeyCode interactKey = KeyCode.E;     // Tecla para comenzar a escalar

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;

    private bool isOnStairs = false;      // Si está dentro del trigger de escalera
    private bool isClimbing = false;      // Si está escalando activamente

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

        // Comenzar a escalar al presionar la tecla y estar en escaleras
        if (isOnStairs && !isClimbing && Input.GetKeyDown(interactKey))
        {
            StartClimbing();
        }

        // Detener movimiento vertical si no hay input
        if (isClimbing && Mathf.Abs(verticalInput) < 0.01f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        // Salir de escalera si se va del trigger
        if (!isOnStairs && isClimbing)
        {
            ResetClimb();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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
    /// Inicia el modo de escalada y alinea al jugador al centro de las escaleras.
    /// </summary>
    private void StartClimbing()
    {
        if (isClimbing) return;

        isClimbing = true;
        rb.gravityScale = 0f;

        // Centrar posición X del jugador en la escalera para evitar desalineación
        if (currentStairZone != null)
        {
            Vector3 pos = transform.position;

            Collider2D stairCollider = currentStairZone.GetComponent<Collider2D>();
            if (stairCollider != null)
            {
                pos.x = stairCollider.bounds.center.x;
                transform.position = pos;
            }

            // Desactivar colisión con techo si existe
            if (currentStairZone.ceilingCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.ceilingCollider, true);
                Physics2D.IgnoreCollision(playerCollider, currentStairZone.stairCollider, true);

            }
        }

        /*
        if (animator != null)
        {
            animator.SetBool("isClimbing", true);
        }
        */
    }

    /// <summary>
    /// Sale del modo de escalada y restaura colisiones.
    /// </summary>
    private void ResetClimb()
    {
        isClimbing = false;
        rb.gravityScale = 1f;

        // Restaurar colisión si hay un techo definido
        if (currentStairZone != null && currentStairZone.ceilingCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, currentStairZone.ceilingCollider, false);
            Physics2D.IgnoreCollision(playerCollider, currentStairZone.stairCollider, false);
        }

        currentStairZone = null;

        /*
        if (animator != null)
        {
            animator.SetBool("isClimbing", false);
        }
        */
    }
}
