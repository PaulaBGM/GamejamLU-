using UnityEngine;

/// <summary>
/// Representa una prenda de ropa que se puede recoger, lavar y tender.
/// </summary>
public class PickupItem : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;
    public bool IsClean { get; private set; } = false;

    private Vector3 targetLocalPosition;
    private float moveSpeed = 5f;
    private bool moving = false;

    private void Update()
    {
        // Movimiento suave al apilar
        if (moving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, moveSpeed * Time.deltaTime);

            if (Vector3.SqrMagnitude(transform.localPosition - targetLocalPosition) < 0.0001f)
            {
                transform.localPosition = targetLocalPosition;
                moving = false;
            }
        }
    }

    public void SetCollected(bool value) => IsCollected = value;
    public void SetClean(bool value) => IsClean = value;

    public void StartMoveToPosition(Vector3 localPos, float speed)
    {
        targetLocalPosition = localPos;
        moveSpeed = speed;
        moving = true;
    }
    public void StopMovement()
    {
        moving = false;
    }

}
