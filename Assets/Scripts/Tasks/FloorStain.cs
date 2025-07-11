using UnityEngine;

public class FloorStain : MonoBehaviour
{
    [SerializeField] private int passesToClean = 1;
    private int currentPasses = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerToolRider broom = other.GetComponent<PlayerToolRider>();
            if (broom != null && broom.isOnBroom)
            {
                CleanPass();
            }
        }
    }

    private void CleanPass()
    {
        currentPasses++;
        Debug.Log("Pasada de escoba: " + currentPasses);

        if (currentPasses >= passesToClean)
        {
            Destroy(gameObject);
        }
    }
}