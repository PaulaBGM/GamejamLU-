// Define la interfaz común para el input del jugador
public interface IPlayerInput
{
    float GetHorizontal();
    float GetVertical();
    bool JumpPressed();
    bool InteractPressed();
    bool DismountPressed();
}
