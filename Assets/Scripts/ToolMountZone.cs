using UnityEngine;

public class ToolMountZone : MonoBehaviour
{
    public GameObject toolPrefab;
    public ParticleSystem mountParticles;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerToolRider rider = other.GetComponent<PlayerToolRider>();
            if (rider != null)
            {
                rider.MountTool(toolPrefab);
                if (mountParticles != null)
                    Instantiate(mountParticles, transform.position, Quaternion.identity).Play();

                Destroy(gameObject); // opcional
            }
        }
    }
}
