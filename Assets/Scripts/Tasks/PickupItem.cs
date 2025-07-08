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

    private static int _totalCollected = 0;
    private const int _totalRequired = 6;
    private static bool _taskAlreadyCompleted = false;

    void Update()
    {
        if (moving)
        {
            // Mueve el objeto suavemente a su posici�n deseada en el stack
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, targetLocalPosition) < 0.01f)
            {
                transform.localPosition = targetLocalPosition;
                moving = false;
            }
        }
    }

    /// <summary>
    /// Marca el objeto como recogido para evitar m�ltiples recolecciones.
    /// </summary>
    public void SetCollected(bool value)
    {
        if (IsCollected == false && value == true) // Solo cuenta la primera vez
        {
            _totalCollected++;
            Debug.Log($"Prendas recogidas: {_totalCollected}");

            float percentage = Mathf.Clamp01((float)_totalCollected / _totalRequired) * 100f;

            // Solo informar a TaskManager si a�n no se ha completado
            if (!_taskAlreadyCompleted)
            {
                TaskManager.Instance.EndTask(5, percentage);

                if (_totalCollected >= _totalRequired)
                {
                    _taskAlreadyCompleted = true;
                    Debug.Log("Misi�n de recoger ropa completada al 100%");
                }
            }
        }

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
    /// Inicia el movimiento hacia la posici�n dentro del stack.
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
