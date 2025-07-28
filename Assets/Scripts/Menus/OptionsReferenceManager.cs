using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    [SerializeField] private Image _brightnessOverlay; // Imagen blanca con color alpha para simular brillo (debe estar encima del juego)

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

    void Update()
    {
        // Debug de volumen (opcional)
        float music;
        _audioMixer.GetFloat("MusicVolume", out music);
      
    }

    private void LoadSettings()
    {
        // Brillo
        float brightness = PlayerPrefs.GetFloat(BrightnessKey, 1.0f);
        _brightSlider.value = brightness;

        // Música
        float music = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        _musicVolumeSlider.value = music;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp01(music)) * 20f);

        // Efectos
        float sfx = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
        _sfxVolumeSlider.value = sfx;
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp01(sfx)) * 20f);
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
        SceneManager.LoadScene("MainScene"); // Cambia por el nombre que uses
    }

    public void TogglePanel() => OptionsMenu.Instance.ToggleOptionsMenu();

    public void OpenCreditsMenu()
    {
        _creditsMenu.SetActive(true);
        _optionMenu.SetActive(false);
    }

    public void SaveBrightness()
    {
        float value = _brightSlider.value;
        PlayerPrefs.SetFloat(BrightnessKey, value);
        ApplyBrightness(value);
    }

    public void SaveMusicVolume()
    {
        float value = Mathf.Clamp01(_musicVolumeSlider.value);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
    }

    public void SaveSFXVolume()
    {
        float value = Mathf.Clamp01(_sfxVolumeSlider.value);
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
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
            Color color = _brightnessOverlay.color;
            color.a = 1.0f - value; // Más alpha = más oscuro
            _brightnessOverlay.color = color;
        }
    }
}
