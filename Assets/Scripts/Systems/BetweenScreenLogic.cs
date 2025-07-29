using UnityEngine;

public class BetweenScreenLogic : MonoBehaviour
{
    private static BetweenScreenLogic _instance;

    private void Awake()
    {
        // Solo aplicar a objetos raíz
        if (transform.parent != null)
        {
            Debug.LogWarning("BetweenScreenLogic debe estar en la raíz del prefab/objeto para usar DontDestroyOnLoad correctamente.");
            return;
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
