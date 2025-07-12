using UnityEngine;

public class BarrelPad : MonoBehaviour
{
    [Header("Visuales")]
    [SerializeField] private ParticleSystem splashEffectPrefab;
    [SerializeField] private Transform splashSpawnPoint;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private GameObject wineStreamPrefab;
    [SerializeField] private Transform wineStreamSpawnPoint;

    private ParticleSystem splashEffectInstance;

    [Header("Progreso")]
    [SerializeField] private float perfectFill = 0.5f;
    [SerializeField] private float goodFill = 0.35f;
    [SerializeField] private float okFill = 0.2f;
    [SerializeField] private float badFill = 0.05f;

    private void Start()
    {
        if (splashEffectPrefab != null)
        {
            splashEffectInstance = Instantiate(splashEffectPrefab, splashSpawnPoint.position, Quaternion.identity);
            splashEffectInstance.transform.SetParent(transform); // Opcional: mantenerlo ordenado en jerarquía
            splashEffectInstance.Stop();
        }
    }

    public void OnStomp()
    {
        float timing = Conductor.instance.GetInputTimingAccuracy();
        float amount = GetFillAmountByTiming(timing);

        // Splash único
        if (splashEffectInstance != null)
        {
            splashEffectInstance.transform.position = splashSpawnPoint.position;
            splashEffectInstance.Play();
        }

        if (splashSound != null)
            splashSound.Play();

        // Efecto del chorro solo si es perfecto
        if (timing <= Mathf.Epsilon && wineStreamPrefab != null && wineStreamSpawnPoint != null)
        {
            GameObject stream = Instantiate(wineStreamPrefab, wineStreamSpawnPoint.position, Quaternion.identity);
            ParticleSystem ps = stream.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
            Destroy(stream, 2f); // autodestrucción
        }

        WineProgressManager.Instance.AddProgress(amount);
    }

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
