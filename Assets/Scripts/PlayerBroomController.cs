using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBroomController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Broom Settings")]
    public float broomSpeed = 8f;
    public Transform broomMountPoint;
    public GameObject broomPrefab;
    public ParticleSystem mountParticles;

    [HideInInspector]
    public bool inBroomZone = false;

    private bool onBroom = false;
    private GameObject broomObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (onBroom)
            {
                DismountBroom();
            }
            else if (inBroomZone && broomPrefab != null)
            {
                MountBroom();
            }
        }

        if (onBroom)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 move = new Vector2(moveX, moveY).normalized;
            rb.linearVelocity = move * broomSpeed;

            if (broomObject != null)
            {
                broomObject.transform.position = broomMountPoint.position;
            }
        }
    }

    private void MountBroom()
    {
        onBroom = true;

        if (mountParticles != null)
        {
            Instantiate(mountParticles, transform.position, Quaternion.identity).Play();
        }

        broomObject = Instantiate(broomPrefab, broomMountPoint.position, Quaternion.identity);
        broomObject.transform.SetParent(transform);

        Debug.Log("🧹 Montado en la escoba");
    }

    private void DismountBroom()
    {
        onBroom = false;

        if (broomObject != null)
        {
            Destroy(broomObject);
        }

        if (mountParticles != null)
        {
            Instantiate(mountParticles, transform.position, Quaternion.identity).Play();
        }

        Debug.Log("❌ Desmontado de la escoba");
    }

    public bool IsOnBroom()
    {
        return onBroom;
    }
}
