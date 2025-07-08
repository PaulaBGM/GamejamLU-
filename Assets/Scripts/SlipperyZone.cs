using UnityEngine;

public class SlipperyZone : MonoBehaviour
{
    private float lifeTime = 30f;
    private bool isClean = true;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Invoke(nameof(Expire), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isClean) return;

        if (other.CompareTag("Player") || other.CompareTag("Cat") || other.CompareTag("Dog") || other.CompareTag("Mom"))
        {
            Debug.Log("Zona ensuciada por " + other.tag);
            isClean = false;
            sr.color = Color.gray; // ejemplo visual
        }
    }

    private void Expire()
    {
        // Considerar autoensuciarse al paso del tiempo (opcional)
        if (isClean)
        {
            Destroy(gameObject);
        }
    }
}
