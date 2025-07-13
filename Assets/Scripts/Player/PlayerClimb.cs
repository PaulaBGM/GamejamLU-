using UnityEngine;

/// <summary>
/// Maneja la escalada ignorando el suelo de la plataforma superior.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerClimb : MonoBehaviour
{
    [Header("Configuración de Escaleras")]
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isOnStairs = false;
    private bool isClimbing = false;
    private float verticalInput;
    private StairZone currentStairZone;

    private Collider2D[] playerColliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerColliders = GetComponents<Collider2D>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isOnStairs && !isClimbing && Input.GetKeyDown(interactKey))
        {
            StartClimbing();
        }

        if (!isOnStairs && isClimbing)
        {
            ResetClimb();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;

            if (Mathf.Abs(verticalInput) > 0.01f)
            {
                rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.gravityScale = 9f;
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

    private void StartClimbing()
    {
        if (isClimbing || currentStairZone == null) return;

        isClimbing = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        // Alinear X si es escalera frontal
        if (!currentStairZone.isSideStair && currentStairZone.stairCollider != null)
        {
            Vector3 pos = transform.position;
            pos.x = currentStairZone.stairCollider.bounds.center.x;
            transform.position = pos;
        }

        IgnoreFloorCollision(true);

        if (animator != null)
            animator.SetBool("isClimbing", true);

        Debug.Log("Escalando: suelo ignorado.");
    }

    private void ResetClimb()
    {
        if (!isClimbing) return;

        isClimbing = false;
        rb.gravityScale = 9f;
        rb.linearVelocity = Vector2.zero;

        IgnoreFloorCollision(false);

        if (animator != null)
            animator.SetBool("isClimbing", false);

        currentStairZone = null;

        Debug.Log("Fin de escalada: suelo restaurado.");
    }

    private void IgnoreFloorCollision(bool ignore)
    {
        if (currentStairZone == null || currentStairZone.floorCollider == null) return;

        foreach (var playerCol in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCol, currentStairZone.floorCollider, ignore);
        }
    }
}