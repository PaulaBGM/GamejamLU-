using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    //Exit panel
    [SerializeField]
    private GameObject _exitPanel;
    [SerializeField]
    private List<GameObject> _buttonsToDisable;

    private void Awake()
    {
        //By default the exit panel start as false
        _exitPanel.SetActive(false);
    }

    //Public method to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    //Toggle the panel to exit the game
    public void ToggleExitPanel()
    {
        _exitPanel.SetActive(!_exitPanel.activeSelf);
        if (_exitPanel.activeSelf)
        {
            TurnOffAllButtons();
        }
        else
        {
            TurnOnAllButtons();
        }
    }
    //Exit the game
    public void ExitGame() 
    {
        Application.Quit();
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
}