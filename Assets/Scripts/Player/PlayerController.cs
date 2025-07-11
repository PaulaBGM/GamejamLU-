using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Chequeo de Suelo")]
    public float raycastDistance = 0.2f;
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

        // Saltar con tecla W o botón de salto, solo si está en el suelo
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("JumpStart", true);
        }

        // Voltear sprite y cámara
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
        // Movimiento lateral
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Raycast desde el centro del personaje hacia abajo
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastDistance, groundLayer);
        isGrounded = hit.collider != null;
        anim.SetBool("IsGrounded", isGrounded);

        // Opcional: resetear animación de salto si empieza a caer
        if (rb.linearVelocity.y <= 0)
        {
            anim.SetBool("JumpStart", false);
        }

#if UNITY_EDITOR
        Debug.DrawRay(origin, Vector2.down * raycastDistance, isGrounded ? Color.green : Color.red);
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
        Vector3 targetPos = new Vector3(-startPos.x, startPos.y, startPos.z);

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
