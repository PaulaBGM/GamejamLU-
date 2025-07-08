using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBroomController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool onBroom = false;

    public float broomSpeed = 8f;
    public Transform broomMountPoint;

    private GameObject broomObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (onBroom)
        {
            // Movimiento en escoba
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(moveX, moveY).normalized;

            rb.linearVelocity = movement * broomSpeed;

            if (broomObject != null)
            {
                broomObject.transform.position = broomMountPoint.position;
            }

            // Desmontar con Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DismountBroom();
            }
        }
    }

    public void MountBroom(GameObject broomPrefab, ParticleSystem particles)
    {
        if (onBroom) return;

        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity).Play();
        }

        broomObject = Instantiate(broomPrefab, broomMountPoint.position, Quaternion.identity);
        broomObject.transform.SetParent(transform);
        onBroom = true;

        Debug.Log("¡Montado en la escoba!");
    }

    public void DismountBroom()
    {
        if (!onBroom) return;

        if (broomObject != null)
        {
            Destroy(broomObject);
        }

        onBroom = false;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("¡Desmontado de la escoba!");
    }

    public bool IsOnBroom()
    {
        return onBroom;
    }
}
