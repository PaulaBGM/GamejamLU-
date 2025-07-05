using UnityEngine;

public abstract class InteractableTask : MonoBehaviour
{
    protected bool playerInRange = false;

    // Este método será implementado por cada tarea específica
    protected abstract void PerformTask();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    protected virtual void Update()
    {
        if (playerInRange)
        {
            PerformTask();
        }
    }
}
