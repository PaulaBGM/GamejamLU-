using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageController : MonoBehaviour
{
    private int _id;
    [SerializeField]
    private TextMeshProUGUI _languageText;
    private void Awake()
    {
        UnityEngine.Localization.Locale locale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("LanguageId")];
        LocalizationSettings.SelectedLocale = locale;
        _id = PlayerPrefs.GetInt("LanguageId");
        SetLanguageText();
    }

    public void IncreaseId()
    {
        if(_id <= 3)
        {
            _id++;
            CheckLanguage();
            SetLanguageText();

        }
        else
        {
            return;
        }
    }
    public void DecreaseId()
    {
        if (_id >= 1)
        {
            _id--;
            CheckLanguage();
            SetLanguageText();
        }
        else
        {
            return;
        }
    }

    private void CheckLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_id];
        PlayerPrefs.SetInt("LanguageId", _id);
    }
    private void SetLanguageText()
    {
        _languageText.text = LocalizationSettings.SelectedLocale.Identifier.Code switch
        {
            "es" => "Español",
            "en" => "English",
            "gl" => "Galego",
            "ca-ES" => "Catalá",
            "pt" => "Portuguese",
            _ => "English"
        };
    }
}