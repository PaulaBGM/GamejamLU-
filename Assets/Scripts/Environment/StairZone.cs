using UnityEngine;

/// <summary>
/// Define una zona de escalera.
/// </summary>
public class StairZone : MonoBehaviour
{
    [Tooltip("Collider del suelo de la plataforma superior.")]
    public Collider2D floorCollider;

    [Tooltip("Collider de la escalera (para alineación, no se ignora aquí).")]
    public Collider2D stairCollider;

    [Tooltip("Escalera lateral? Si no, se asume frontal.")]
    public bool isSideStair = false;
}