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
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(moveX, moveY).normalized;

            rb.linearVelocity = movement * broomSpeed;

            if (broomObject != null)
            {
                broomObject.transform.position = broomMountPoint.position;
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

    // ✅ Método que faltaba
    public bool IsOnBroom()
    {
        return onBroom;
    }
}
