using UnityEngine;
using UnityEngine.UI;

public class WineProgressManager : MonoBehaviour
{
    public static WineProgressManager Instance;

    [SerializeField] private float totalProgressNeeded = 3f;
    private float currentProgress = 0f;
    public int perfectCount = 0;

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

    public void AddPerfect()
    {
        if (perfectCount < 3)
        {
            perfectCount++;
        }
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
            Debug.Log("Botella de vino llena!");
            // Aquí puedes llamar a un evento o animación final
        }
    }

    public float ProgressNormalized => currentProgress / totalProgressNeeded;
}
