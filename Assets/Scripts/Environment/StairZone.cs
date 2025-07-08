using UnityEngine;

/// <summary>
/// Define una zona de escalera y qu� collider del piso debe ignorarse durante la escalada.
/// </summary>
public class StairZone : MonoBehaviour
{
    [Tooltip("Collider del piso superior que debe ignorarse al escalar")]
    public Collider2D ceilingCollider;
    public Collider2D stairCollider;


}
