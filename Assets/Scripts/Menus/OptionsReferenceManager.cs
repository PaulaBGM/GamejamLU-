using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsReferenceManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Scrollbar _brightSlider;
    [SerializeField] private Scrollbar _musicVolumeSlider;
    [SerializeField] private Scrollbar _sfxVolumeSlider;

    [Header("UI Panels")]
    [SerializeField] private GameObject _optionsMenuExit;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _optionMenu;

    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Brightness")]
    [SerializeField] private UnityEngine.UI.Image _brightnessOverlay;

    private bool hasChanges;

    private const string BrightnessKey = "Brightness";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        LoadSettings();
        ApplyBrightness(_brightSlider.value);
        _creditsMenu.SetActive(false);
        _optionsMenuExit.SetActive(false);
    }

    private void LoadSettings()
    {
        float brightness = PlayerPrefs.GetFloat(BrightnessKey, 1.0f);
        _brightSlider.value = brightness;

        float music = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        _musicVolumeSlider.value = music;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp01(music)) * 20f);

        float sfx = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
        _sfxVolumeSlider.value = sfx;
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp01(sfx)) * 20f);

        ApplyBrightness(brightness);
    }

    public void SaveBrightness()
    {
        float value = _brightSlider.value;
        ApplyBrightness(value);
        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }

    public void SaveMusicVolume()
    {
        float value = Mathf.Clamp01(_musicVolumeSlider.value);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SaveSFXVolume()
    {
        float value = Mathf.Clamp01(_sfxVolumeSlider.value);
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SaveAllSettings()
    {
        SaveBrightness();
        SaveMusicVolume();
        SaveSFXVolume();
    }

    public void OnBrightnessChanged()
    {
        ApplyBrightness(_brightSlider.value);
    }

    private void ApplyBrightness(float value)
    {
        if (_brightnessOverlay != null)
        {
            _brightnessOverlay.color = new Color(0, 0, 0, 1f - value); // Más opaco = más oscuro
        }
    }

    public void ExitCreditsMenu()
    {
        _creditsMenu.SetActive(false);
        _optionMenu.SetActive(true);
    }

    public void CheckChanges()
    {
        hasChanges =
            _brightSlider.value != PlayerPrefs.GetFloat(BrightnessKey, 1.0f) ||
            _musicVolumeSlider.value != PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f) ||
            _sfxVolumeSlider.value != PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);

        if (hasChanges)
            ExitOptionsMenu();
        else
            TogglePanel();
    }

    public void ExitOptionsMenu()
    {
        _optionsMenuExit.SetActive(true);
        _optionMenu.SetActive(false);
    }

    public void CancelExit()
    {
        _optionsMenuExit.SetActive(false);
        _optionMenu.SetActive(true);
    }

    public void ConfirmExitToMainMenu()
    {
        SaveAllSettings();
        SceneManager.LoadScene("MainScene");
    }

    public void TogglePanel()
    {
        OptionsMenu.Instance?.ToggleOptionsMenu();
    }

    public void OpenCreditsMenu()
    {
        _creditsMenu.SetActive(true);
        _optionMenu.SetActive(false);
    }
}
