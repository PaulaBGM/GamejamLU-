using UnityEngine;

// Singleton para gestionar el input activo en el juego
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public IPlayerInput CurrentInput { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Busca un componente IPlayerInput en este GameObject y lo asigna
        CurrentInput = GetComponent<IPlayerInput>();
        if (CurrentInput == null)
        {
            Debug.LogError("No se encontró ningún componente IPlayerInput en InputManager.");
        }
    }
}
