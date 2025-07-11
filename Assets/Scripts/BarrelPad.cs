using UnityEngine;

public class BarrelPad : MonoBehaviour
{
    [Header("Visuales")]
    [SerializeField] private ParticleSystem splashEffect;
    [SerializeField] private Transform splashSpawnPoint;
    [SerializeField] private AudioSource splashSound;

    [Header("Progreso")]
    [SerializeField] private float perfectFill = 0.5f;
    [SerializeField] private float goodFill = 0.35f;
    [SerializeField] private float okFill = 0.2f;
    [SerializeField] private float badFill = 0.05f;

    public void OnStomp()
    {
        float timing = Conductor.instance.GetInputTimingAccuracy();
        float amount = GetFillAmountByTiming(timing);

        // Feedback visual
        if (splashEffect != null && splashSpawnPoint != null)
            Instantiate(splashEffect, splashSpawnPoint.position, Quaternion.identity);
        if (splashSound != null) splashSound.Play();

        // Enviar progreso al sistema
        WineProgressManager.Instance.AddProgress(amount);
    }

    // Determina cuánto llenar según el timing
    private float GetFillAmountByTiming(float timing)
    {
        if (timing <= Mathf.Epsilon)
        {
            Debug.Log("Perfect timing!");
            return perfectFill;
        }
        else if (timing <= 0.01f)
        {
            Debug.Log("Good timing!");
            return goodFill;
        }
        else if (timing <= 0.02f)
        {
            Debug.Log("OK timing!");
            return okFill;
        }
        else
        {
            Debug.Log("Bad timing!");
            return badFill;
        }
    }
}
