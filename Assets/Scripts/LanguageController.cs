using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageController : MonoBehaviour
{
    private int _id;
    [SerializeField]
    private TextMeshProUGUI _languageText;
    private void Start()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("LanguageId")];
    }

    public void IncreaseId()
    {
        if(_id <= 2)
        {
            _id++;
            CheckLanguage();
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
        switch (_id) 
        {
            case 0:
                _languageText.text = "Español";
                break;
            case 1:
                _languageText.text = "English";
                break;
            case 2:
                _languageText.text = "Galego";
                break;
            case 3:
                _languageText.text = "Catalá";
                break;
        }
    }
}
