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
