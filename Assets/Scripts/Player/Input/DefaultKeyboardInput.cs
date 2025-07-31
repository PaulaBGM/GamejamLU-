using UnityEngine;

// Implementa la interfaz con el input tradicional de Unity (teclado)
public class DefaultKeyboardInput : MonoBehaviour, IPlayerInput
{
    public float GetHorizontal() => Input.GetAxisRaw("Horizontal");
    public float GetVertical() => Input.GetAxisRaw("Vertical");
    public bool JumpPressed() => Input.GetKeyDown(KeyCode.Space);
    public bool InteractPressed() => Input.GetKeyDown(KeyCode.E);
    public bool DismountPressed() => Input.GetKeyDown(KeyCode.Q);
}
