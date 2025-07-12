using UnityEngine;

public class RooftopZone : MonoBehaviour
{
    public static bool PlayerOnRooftop { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOnRooftop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOnRooftop = false;
        }
    }
}
