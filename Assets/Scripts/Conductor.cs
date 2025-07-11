using UnityEngine;

/// <summary>
/// Controla la sincronización con la música y permite evaluar el timing del jugador.
/// </summary>
public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Configuración de la canción")]
    [SerializeField] private float songBpm = 120f;
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
    /// Devuelve el desfase en beats respecto al beat más cercano (entre 0 y 1).
    /// Además, muestra por consola si fue perfecto, bueno o malo.
    /// </summary>
    public float GetInputTimingAccuracy()
    {
        inputTimeDifference = songPositionInBeats % 1;

        // Evalúa y muestra por consola el tipo de precisión
        if (inputTimeDifference <= 0.05f || inputTimeDifference >= 0.95f)
        {
            Debug.Log(" ¡Perfecto! inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else if (inputTimeDifference <= 0.1f || inputTimeDifference >= 0.9f)
        {
            Debug.Log(" ¡Bien! inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else if (inputTimeDifference <= 0.2f || inputTimeDifference >= 0.8f)
        {
            Debug.Log(" Regular... inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }
        else
        {
            Debug.Log(" Mal timing. inputTimeDifference = " + inputTimeDifference.ToString("F3"));
        }

        return inputTimeDifference;
    }

    // Getters públicos si necesitas acceder desde otros scripts
    public float SongPosition => songPosition;
    public float SongPositionInBeats => songPositionInBeats;
    public float LoopPositionInAnalog => loopPositionInAnalog;
    public float SecPerBeat => secPerBeat;
}
