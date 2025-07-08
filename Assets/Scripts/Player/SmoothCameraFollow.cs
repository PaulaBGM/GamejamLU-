using UnityEngine;

public class SmoothCameraMouseFollow : MonoBehaviour
{
    [Header("Sensibilidad del movimiento")]
    public float horizontalSensitivity = 0.5f;  // Movimiento horizontal (más)
    public float verticalSensitivity = 0.2f;    // Movimiento vertical (menos)

    [Header("Suavizado")]
    public float smoothSpeed = 5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    void LateUpdate()
    {
        // Obtener el movimiento del mouse (de -1 a 1)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculamos el movimiento deseado
        float moveX = mouseX * horizontalSensitivity;
        float moveY = mouseY * verticalSensitivity;

        // Actualizamos la posición objetivo respecto a la posición inicial
        targetPosition = initialPosition + new Vector3(moveX, moveY, 0);

        // Suavizamos la transición de posición
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
