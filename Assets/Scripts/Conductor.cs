using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    public float songBpm;
    public float secPerBeat;
    public float firstBeatOffset;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;
    public float inputTimeDifference;

    public AudioSource musicSource;

    public float beatsPerLoop;
    public int completedLoops = 0;
    public float loopPositionInBeats;
    public float loopPositionInAnalog;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
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

    // Método accesible desde otros scripts para evaluar el timing de un input
    public float GetInputTimingAccuracy()
    {
        inputTimeDifference = songPositionInBeats % 1;
        return inputTimeDifference;
    }
}

