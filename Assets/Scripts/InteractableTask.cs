using UnityEngine;

public abstract class InteractableTask : MonoBehaviour
{
    protected bool playerInRange = false;

    // Este m�todo ser� implementado por cada tarea espec�fica
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
