using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Chequeo de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    private bool facingRight = true;

    // Para suavizar la cámara
    [SerializeField] private float flipSmoothTime = 0.3f;
    [SerializeField] private string cameraChildName = "Camera"; // Nombre del hijo cámara para suavizar flip
    [SerializeField] Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", moveInput);
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

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
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    private void FlipChildren(bool faceRight)
    {
        foreach (Transform child in transform)
        {
            if (child.name == cameraChildName)
            {
                // Suavizamos la posición local de la cámara
                StopAllCoroutines();
                StartCoroutine(SmoothFlipCamera(child, faceRight));
            }
            else
            {
                // Flip inmediato para otros hijos
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

        // Cambio inmediato del sprite flip para evitar que se vea al revés
        if (sr != null)
            sr.flipX = faceRight ? false : true;

        while (elapsed < duration)
        {
            cam.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = targetPos;
    }
}
