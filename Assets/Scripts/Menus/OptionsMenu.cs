using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    //Options Menu Prefab to instantiate on every new scene once
    [SerializeField]
    private GameObject _optionsMenuPrefab;
    //Private GameObject to store the Instantiated prefab
    private GameObject _optionsMenu;
    //Point to spawn the options menu prefab
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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_optionsMenu != null && _optionsMenu.activeSelf)
            {
                //If the options menu is already open, close it
                _optionsMenu.SetActive(false);
                return;
            }
        }
        if (_optionsMenu != null)
        {

            if (_optionsMenu.activeSelf)
            {
                IsOpen = true;
                TurnOffAllButtons();
            }
            else
            {
                IsOpen = false;
                TurnOnAllButtons();
            }
        }
    }

    private void TurnOffAllButtons()
    {
        //Toggle the active state of all buttons in the list
        foreach (GameObject button in _buttonsToDisable)
        {
            button.SetActive(false);
        }
    }
    private void TurnOnAllButtons()
    {
        //Toggle the active state of all buttons in the list
        foreach (GameObject button in _buttonsToDisable)
        {
            button.SetActive(true);
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