using UnityEngine;

public class BroomZone : MonoBehaviour
{
    public GameObject broomPrefab;
    public ParticleSystem mountParticles;

    private bool playerInZone = false;
    private PlayerBroomController player;
    
    [SerializeField]
    private int _mountAmount = 0;
    private int _cleanedMounts;
    private float _cleanedMountsPercent;

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
            _cleanedMounts++;
            _cleanedMountsPercent = (float)_cleanedMounts / _mountAmount * 100f;
            TaskManager.Instance.EndTask(2, _cleanedMountsPercent);
            Destroy(gameObject); // Opcional
        }
    }
}