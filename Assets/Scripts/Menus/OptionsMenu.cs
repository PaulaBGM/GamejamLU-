using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    //Options Menu Prefab to instantiate on every new scene once
    [SerializeField]
    private GameObject _optionsMenuPrefab;
    //Private GameObject to store the Instantiated prefab
    private GameObject _optionsMenu;
    //Point to spawn the options menu prefab
    [SerializeField]
    private GameObject _mainCanva;
    //Bool to see if the menu is open
    public bool IsOpen;
    [SerializeField]
    private List<GameObject> _buttonsToDisable;

    private void Awake()
    {
        //Singleton pattern to ensure only one instance of OptionsMenu exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _mainCanva = GameObject.Find("Canvas");
        IsOpen = false;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("LanguageId")];
    }

    private void Update()
    {
        if (_mainCanva == null)
        {
            CheckMainCanva();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionsMenu == null)
            {
                ToggleOptionsMenu();
            }
            if (_optionsMenu != null && _optionsMenu.activeSelf)
            {
                ToggleOptionsMenu();
                return;
            }
            else
            {
                ToggleOptionsMenu();
                return;
            }
        }
        if (_optionsMenu != null)
        {

            if (_optionsMenu.activeSelf)
            {
                IsOpen = true;
                if(_buttonsToDisable != null && _buttonsToDisable.Count > 0)
                {
                    //If the options menu is open, disable all buttons in the list
                    TurnOffAllButtons();
                }
            }
            else
            {
                IsOpen = false;
                if(_buttonsToDisable != null && _buttonsToDisable.Count > 0)
                {
                    //If the options menu is closed, enable all buttons in the list
                    TurnOnAllButtons();
                }
            }
        }
    }

    public void CheckMainCanva()
    {
        _mainCanva = GameObject.FindGameObjectWithTag("MainCanva");
    }

    private void TurnOffAllButtons()
    {
        _buttonsToDisable.RemoveAll(b => b == null);

        foreach (GameObject button in _buttonsToDisable)
        {
            if (button) button.SetActive(false);
        }
    }

    private void TurnOnAllButtons()
    {
        _buttonsToDisable.RemoveAll(b => b == null);

        foreach (GameObject button in _buttonsToDisable)
        {
            if (button) button.SetActive(true);
        }
    }

    public void ToggleOptionsMenu()
    {
        //If the options menu isnt already on the scene
        if(_optionsMenu == null)
        {
            //Instantiate the options menu prefab and set it active
            _optionsMenu = Instantiate(_optionsMenuPrefab, _mainCanva.transform);
            _optionsMenu.SetActive(true);
        }
        else
        {
            //Toggle the active state of the options menu
            _optionsMenu.SetActive(!_optionsMenu.activeSelf);
        }
    }
}