using UnityEngine;

public class CreditsPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _creditsPanel;
    [SerializeField]
    private GameObject _optionsMenuToToggle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _creditsPanel.activeSelf)
        {
            ToggleCreditsPanel();
        }
    }

    public void ToggleCreditsPanel()
    {
        if (_creditsPanel.activeSelf)
        {
            _creditsPanel.SetActive(false);
            _optionsMenuToToggle.SetActive(true);
        }
        else
        {
            _creditsPanel.SetActive(true);
            _optionsMenuToToggle.SetActive(false);
        }
    }
}
