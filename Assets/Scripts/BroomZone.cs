using UnityEngine;

public class BroomZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerBroomController>();
            if (controller != null)
            {
                controller.inBroomZone = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerBroomController>();
            if (controller != null)
            {
                controller.inBroomZone = false;
            }
        }
    }
}
