using UnityEngine;

/// <summary>
/// Comportamiento de los objetos que se pueden recoger y apilar.
/// </summary>
public class PickupItem : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;
    public bool IsClean { get; private set; } = false;

    private Vector3 targetLocalPosition;
    private float moveSpeed = 5f;
    private bool moving = false;

    void Update()
    {
        if (moving)
        {
            // Mueve el objeto suavemente a su posición deseada en el stack
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, targetLocalPosition) < 0.01f)
            {
                transform.localPosition = targetLocalPosition;
                moving = false;
            }
        }
    }

    /// <summary>
    /// Marca el objeto como recogido para evitar múltiples recolecciones.
    /// </summary>
    public void SetCollected(bool value)
    {
        IsCollected = value;
    }

    /// <summary>
    /// Marca el objeto como limpio (lavado).
    /// </summary>
    public void SetClean(bool value)
    {
        IsClean = value;
    }

    /// <summary>
    /// Inicia el movimiento hacia la posición dentro del stack.
    /// </summary>
    public void StartMoveToPosition(Vector3 localPosition, float speed)
    {
        targetLocalPosition = localPosition;
        moveSpeed = speed;
        moving = true;
    }
    public void StopMovement()
    {
        moving = false;
    }

    public void EnableMovement()
    {
        moving = true;
    }

}
