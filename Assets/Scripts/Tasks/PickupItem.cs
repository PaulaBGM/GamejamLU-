using UnityEngine;

/// <summary>
/// Comportamiento de los objetos que se pueden recoger y apilar.
/// </summary>
public class PickupItem : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;
    public bool IsClean { get; set; } = false;

    private Vector3 targetLocalPosition;
    private float moveSpeed = 5f;
    private bool moving = false;

    [SerializeField]
    private SpriteRenderer _itemImage;

    [SerializeField]
    private Sprite _dirtySprite;
    [SerializeField]
    private Sprite _collectedDirtySprite;
    [SerializeField]
    private Sprite _collectedCleanSprite;
    [SerializeField]
    private Sprite _cleanSprite;

    private static int _totalCollected = 0;
    private const int _totalRequired = 6;
    private static bool _taskAlreadyCompleted = false;

    private void Start()
    {
        CheckPickUpState();
    }
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
        if (IsCollected == false && value == true) // Solo cuenta la primera vez
        {
            _totalCollected++;
            Debug.Log($"Prendas recogidas: {_totalCollected}");

            float percentage = Mathf.Clamp01((float)_totalCollected / _totalRequired) * 100f;

            // Solo informar a TaskManager si aún no se ha completado
            if (!_taskAlreadyCompleted)
            {
                TaskManager.Instance.EndTask(5, percentage);

                if (_totalCollected >= _totalRequired)
                {
                    _taskAlreadyCompleted = true;
                    Debug.Log("Misión de recoger ropa completada al 100%");
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

    public void CheckPickUpState()
    {
        if (IsCollected)
        {
            if (IsClean)
            {
                _itemImage.sprite = _collectedCleanSprite;
            }
            else
            {
                _itemImage.sprite = _collectedDirtySprite;
            }
        }
        else
        {
            if (IsClean)
            {
                _itemImage.sprite = _cleanSprite;
            }
            else
            {
                _itemImage.sprite = _dirtySprite;
            }
        }
    }
}
