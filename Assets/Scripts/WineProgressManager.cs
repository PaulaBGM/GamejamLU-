using UnityEngine;
using UnityEngine.UI;

public class WineProgressManager : MonoBehaviour
{
    public static WineProgressManager Instance;

    [SerializeField] private float totalProgressNeeded = 1f;
    private float currentProgress = 0f;

    [Header("UI")]
    [SerializeField] private Image progressBar;

    [Header("Eventos")]
    public bool bottleFilled = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddProgress(float amount)
    {
        if (bottleFilled) return;

        currentProgress += amount;
        currentProgress = Mathf.Clamp(currentProgress, 0f, totalProgressNeeded);

        if (progressBar != null)
            progressBar.fillAmount = currentProgress / totalProgressNeeded;

        if (currentProgress >= totalProgressNeeded)
        {
            bottleFilled = true;
            Debug.Log("¡Botella de vino llena!");
        }
    }
}
