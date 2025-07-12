using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Chequeo de Suelo por Raycast")]
    public float groundCheckDistance = 1.6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool facingRight = true;

    [SerializeField] private float flipSmoothTime = 0.3f;
    [SerializeField] private string cameraChildName = "Camera";
    [SerializeField] private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Saltar con W o Espacio si est? en el suelo
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("JumpStart", true);
        }

        // Voltear sprite y c?mara
        if (moveInput > 0 && !facingRight)
        {
            FlipChildren(true);
            facingRight = true;
        }
        else if (moveInput < 0 && facingRight)
        {
            FlipChildren(false);
            facingRight = false;
        }

        anim.SetFloat("verticalSpeed", rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Detecci?n de suelo con raycast desde el centro hacia abajo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        anim.SetBool("IsGrounded", isGrounded);

        // Resetear animaci?n de salto si est? cayendo
        if (rb.linearVelocity.y <= 0)
        {
            anim.SetBool("JumpStart", false);
        }

#if UNITY_EDITOR
        // Visualizar raycast en Scene View
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
#endif
    }

    private void FlipChildren(bool faceRight)
    {
        foreach (Transform child in transform)
        {
            if (child.name == cameraChildName)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothFlipCamera(child, faceRight));
            }
            else
            {
                Vector3 localPos = child.localPosition;
                localPos.x = -localPos.x;
                child.localPosition = localPos;

                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = child.GetComponentInChildren<SpriteRenderer>();

                if (sr != null)
                    sr.flipX = !sr.flipX;
            }
        }
    }

    private IEnumerator SmoothFlipCamera(Transform cam, bool faceRight)
    {
        Vector3 startPos = cam.localPosition;
        Vector3 targetPos = startPos;
        targetPos.x = -startPos.x;

        float elapsed = 0f;
        float duration = flipSmoothTime;

        SpriteRenderer sr = cam.GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = cam.GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
            sr.flipX = !faceRight;

        while (elapsed < duration)
        {
            cam.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = targetPos;
    }
}