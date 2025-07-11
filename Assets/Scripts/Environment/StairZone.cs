using UnityEngine;

/// <summary>
/// Define una zona de escalera, permitiendo controlar colisiones y alineacion del jugador.
/// </summary>
public class StairZone : MonoBehaviour
{
    [Tooltip("Collider del techo que debe ignorarse al escalar")]
    public Collider2D ceilingCollider;

    [Tooltip("Collider de la escalera, si se desea ignorar al escalar")]
    public Collider2D stairCollider;

    [Tooltip("Es una escalera lateral (vista de perfil)? Si no, se asume frontal.")]
    public bool isSideStair = false;
}