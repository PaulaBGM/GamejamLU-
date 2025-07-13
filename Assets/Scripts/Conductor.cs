using UnityEngine;

/// <summary>
/// Controla la sincronización con la música y evalúa el timing del jugador.
/// </summary>
public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Configuración de la canción")]
    [SerializeField] private float songBpm = 165f;
    [SerializeField] private float firstBeatOffset = 0f;
    [SerializeField] private float beatsPerLoop = 16f;

    [Header("Referencias")]
    [SerializeField] private AudioSource musicSource;

    private float secPerBeat;
    private float dspSongTime;
    private float songPosition;
    private float songPositionInBeats;
    private float inputTimeDifference;

    private int completedLoops = 0;
    private float loopPositionInBeats;
    private float loopPositionInAnalog;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;

        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;

        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    /// <summary>
    /// Evalúa el desfase respecto al beat más cercano.
    /// </summary>
    public float GetInputTimingAccuracy()
    {
        inputTimeDifference = songPositionInBeats % 1;

        // Ajuste: ampliar la ventana de tolerancia
        float diff = inputTimeDifference;
        if (diff > 0.5f) diff = 1f - diff; // Simetría, para manejar beats cercanos a 0 o 1

        if (diff <= 0.1f)
        {
            Debug.Log("Perfecto! inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else if (diff <= 0.2f)
        {
            Debug.Log("Bien! inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else if (diff <= 0.3f)
        {
            Debug.Log("Regular... inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else
        {
            Debug.Log("Mal timing. inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }

        return diff;
    }

    public float SongPosition => songPosition;
    public float SongPositionInBeats => songPositionInBeats;
    public float LoopPositionInAnalog => loopPositionInAnalog;
    public float SecPerBeat => secPerBeat;
}