using UnityEngine;
using UnityEngine.UI;

public class WineProgressManager : MonoBehaviour
{
    [SerializeField] private float totalProgressNeeded = 3f;
    public float CurrentProgress = 0f;
    public int perfectCount = 0;

    [Header("UI")]
    [SerializeField] private Image progressBar;

    [Header("Eventos")]
    public bool bottleFilled = false;

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

        CurrentProgress += amount;
        CurrentProgress = Mathf.Clamp(CurrentProgress, 0f, totalProgressNeeded);

        if (progressBar != null)
            progressBar.fillAmount = CurrentProgress / totalProgressNeeded;

        if (CurrentProgress >= totalProgressNeeded)
        {
            bottleFilled = true;
            Debug.Log("Botella de vino llena!");
            // Aquí puedes llamar a un evento o animación final
        }
    }

    public float ProgressNormalized => CurrentProgress / totalProgressNeeded;
}
