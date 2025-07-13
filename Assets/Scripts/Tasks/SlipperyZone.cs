using UnityEngine;

public class SlipperyZone : MonoBehaviour
{
    public float lifeTime = 10f;
    private float lifetimeTimer = 0f;
    public float activationDelay = 0.1f;

    private bool isClean = true;
    private bool isActive = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Invoke(nameof(Activate), activationDelay); // Espera 5s para activarse
        Invoke(nameof(Expire), lifeTime);

        // Ajustar posicion para alinearse con el suelo
        AlignWithGround();
    }

    private void Update()
    {
        lifetimeTimer += Time.deltaTime;
        // Destruir al pasar el tiempo de vida
        if (lifetimeTimer >= lifeTime)
        { 
            Destroy(gameObject);
            Debug.Log("se destruye el awawa");
        }
    }

    private void Activate()
    {
        isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isClean || !isActive) return;

        if (other.CompareTag("Player") || other.CompareTag("Cat") || other.CompareTag("Dog") || other.CompareTag("Mom"))
        {
            Debug.Log("Zona ensuciada por " + other.tag);
            isClean = false;
            sr.color = Color.gray; // efecto visual
        }
    }

    private void Expire()
    {
        if (isClean)
        {
            Destroy(gameObject);
        }
    }

    private void AlignWithGround()
    {
        // Raycast hacia abajo para encontrar el suelo en el layer "Ground"
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            // Alinearse con la posicion del suelo 
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;
            transform.position = newPos;
        }
       
    }
}