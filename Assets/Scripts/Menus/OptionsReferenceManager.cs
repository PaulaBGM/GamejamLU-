using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsReferenceManager : MonoBehaviour
{
    [SerializeField]
    private Scrollbar _brightSlider;
    [SerializeField]
    private Scrollbar _musicVolumeSlider;
    [SerializeField]
    private Scrollbar _sfxVolumeSlider;
    [SerializeField]
    private GameObject _optionsMenuExit;
    [SerializeField]
    private AudioMixer _audioMixer;

    private bool hasChanges;

    private void Awake()
    {
        // Load saved volume settings if they exist
        if (PlayerPrefs.HasKey("Brightness"))
        {
            _brightSlider.value = PlayerPrefs.GetFloat("Brightness");
        }
        else
        {
            _brightSlider.value = 1.0f; // Default value
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float music = PlayerPrefs.GetFloat("MusicVolume");
            _musicVolumeSlider.value = music;
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(music, 0.0001f, 1f)) * 20f);
        }
        else
        {
            float music = 1.0f;
            _musicVolumeSlider.value = music;
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(music) * 20f);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfx = PlayerPrefs.GetFloat("SFXVolume");
            _sfxVolumeSlider.value = sfx;
            _audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sfx, 0.0001f, 1f)) * 20f);
        }
        else
        {
            float sfx = 1.0f;
            _sfxVolumeSlider.value = sfx;
            _audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfx) * 20f); 
        }
    }

    void Update()
    {
        float music;
        _audioMixer.GetFloat("MusicVolume", out music);
        Debug.Log("MusicVolume actual: " + music);
    }


    public void CheckChanges()
    {
        // Check if any of the sliders have changed from their default values
        hasChanges = _brightSlider.value != PlayerPrefs.GetFloat("Brightness") 
            ||
            _musicVolumeSlider.value != PlayerPrefs.GetFloat("MusicVolume") 
            ||
            _sfxVolumeSlider.value != PlayerPrefs.GetFloat("SFXVolume");
        if(hasChanges)
        {
            ExitOptionsMenu();
        }
        else
        {
            TogglePanel();
        }
    }
    public void ExitOptionsMenu()
    {
        _optionsMenuExit.SetActive(!_optionsMenuExit.activeSelf);
    }
    public void TogglePanel() => OptionsMenu.Instance.ToggleOptionsMenu();
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("4");
    }
    public void SaveBrightness()
    {
        PlayerPrefs.SetFloat("Brightness", _brightSlider.value);
    }
    public void SaveMusicVolume()
    {
        float volume = Mathf.Log10(Mathf.Clamp(_musicVolumeSlider.value, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);
    }
    public void SaveSFXVolume()
    {
        float volume = Mathf.Log10(Mathf.Clamp(_sfxVolumeSlider.value, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolumeSlider.value);
    }
    public void SaveAllSettings()
    {
        SaveBrightness();
        SaveMusicVolume();
        SaveSFXVolume();
    }
}