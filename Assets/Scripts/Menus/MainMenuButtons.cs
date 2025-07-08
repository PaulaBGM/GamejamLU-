using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    //Exit panel
    [SerializeField]
    private GameObject _exitPanel;

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
    }
    //Exit the game
    public void ExitGame() 
    {
        Application.Quit();
    }
}
