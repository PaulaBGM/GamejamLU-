using UnityEngine;

public class ToolMountZone : MonoBehaviour
{
    public GameObject toolPrefab;
    public ParticleSystem mountParticles;

    private bool playerInZone = false;
    private PlayerToolRider rider;

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && rider != null && !rider.IsMounted())
        {
            rider.MountTool(toolPrefab);

            if (mountParticles != null)
                Instantiate(mountParticles, transform.position, Quaternion.identity).Play();

            Destroy(gameObject); // opcional
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rider = other.GetComponent<PlayerToolRider>();
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            rider = null;
        }
    }
}