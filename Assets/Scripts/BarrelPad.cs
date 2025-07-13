using UnityEngine;

public class BarrelPad : MonoBehaviour
{
    [Header("Visuales")]
    [SerializeField] private ParticleSystem splashEffectPrefab;
    [SerializeField] private Transform splashSpawnPoint;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private GameObject wineStreamPrefab;
    [SerializeField] private Transform wineStreamSpawnPoint;

    [Header("Progreso")]
    [SerializeField] private float perfectFill = 0.5f;
    [SerializeField] private float goodFill = 0.35f;
    [SerializeField] private float okFill = 0.2f;
    [SerializeField] private float badFill = 0.05f;

    [Header("Osu Circle")]
    [SerializeField] private OsuCircle osuCircle;

    private ParticleSystem splashEffectInstance;

    [SerializeField]
    private WineProgressManager wineProgressManager;

    private float lastBeat = -1f;

    private void Start()
    {
        if (splashEffectPrefab != null)
        {
            splashEffectInstance = Instantiate(splashEffectPrefab, splashSpawnPoint.position, Quaternion.identity);
            splashEffectInstance.transform.SetParent(transform);
            splashEffectInstance.Stop();
        }
    }

    private void Update()
    {
        // Detectar nuevo beat para pulsar el círculo
        if (Conductor.instance != null)
        {
            float currentBeat = Mathf.Floor(Conductor.instance.SongPositionInBeats);
            if (currentBeat != lastBeat)
            {
                lastBeat = currentBeat;
                if (osuCircle != null)
                    osuCircle.PulseBeat();
            }
        }

        // Detectar input (ejemplo: tecla espacio para stomp)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnStomp();
        }
    }
public void OnStomp()
    {
        float timing = Conductor.instance.GetInputTimingAccuracy();
        float amount = GetFillAmountByTiming(timing);

        bool isPerfect = timing <= 0.1f; // Umbral para "Perfect"

        if (isPerfect)
        {
        wineProgressManager.AddPerfect();
        Debug.Log("[BarrelPad] PERFECT timing! Triggering splash and wine stream.");

            if (splashEffectInstance != null)
            {
                splashEffectInstance.transform.position = splashSpawnPoint.position;
                splashEffectInstance.Play();
            }

            if (wineStreamPrefab != null && wineStreamSpawnPoint != null)
            {
                Instantiate(wineStreamPrefab, wineStreamSpawnPoint.position, Quaternion.identity);
            }
        }

        if (splashSound != null)
        {
            splashSound.Play();
        }

        wineProgressManager.AddProgress(amount);

        // Contraer círculo al stomp
        if (osuCircle != null)
            osuCircle.TriggerShrink();
    }

    private float GetFillAmountByTiming(float timing)
    {
        if (timing <= 0.1f)
        {
            Debug.Log("[BarrelPad] Fill value: PERFECT");
            return perfectFill;
        }
        else if (timing <= 0.2f)
        {
            Debug.Log("[BarrelPad] Fill value: GOOD");
            return goodFill;
        }
        else if (timing <= 0.3f)
        {
            Debug.Log("[BarrelPad] Fill value: OK");
            return okFill;
        }
        else
        {
            Debug.Log("[BarrelPad] Fill value: BAD");
            return badFill;
        }
    }
}
