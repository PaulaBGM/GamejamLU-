using UnityEngine;

public class BroomZone : MonoBehaviour
{
    public GameObject broomPrefab; // Prefab de la escoba que se activa al montarse
    public ParticleSystem mountParticles;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerBroomController broomController = other.GetComponent<PlayerBroomController>();
            if (broomController != null)
            {
                broomController.MountBroom(broomPrefab, mountParticles);
                Destroy(gameObject); // Opcional: eliminar escoba de suelo
            }
        }
    }
}
