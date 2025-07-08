using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            _musicVolumeSlider.value = 1.0f; // Default value
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            _sfxVolumeSlider.value = 1.0f; // Default value
        }
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
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);
    }
    public void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolumeSlider.value);
    }
    public void SaveAllSettings()
    {
        SaveBrightness();
        SaveMusicVolume();
        SaveSFXVolume();
    }
}
