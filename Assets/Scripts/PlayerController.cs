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

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("JumpStart", true); // se activa el salto inicial
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

        // Actualizar verticalSpeed para el Animator
        anim.SetFloat("verticalSpeed", rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Actualizar estado de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        anim.SetBool("IsGrounded", isGrounded);

        // Resetear "JumpStart" si ya estamos cayendo
        if (rb.linearVelocity.y <= 0)
        {
            anim.SetBool("JumpStart", false);
        }
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
