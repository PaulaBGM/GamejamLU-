using UnityEngine;

public class BroomZone : MonoBehaviour
{
    public GameObject broomPrefab;
    public ParticleSystem mountParticles;

    private bool playerInZone = false;
    private PlayerBroomController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerBroomController>();
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInZone && player != null && Input.GetKeyDown(KeyCode.E))
        {
            player.MountBroom(broomPrefab, mountParticles);
            Destroy(gameObject); // Opcional
        }
    }
}