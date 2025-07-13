using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialPanel;

    private void Update()
    {
        if (_tutorialPanel.activeSelf)
        {
            OptionsMenu.Instance.IsOpen = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _tutorialPanel.SetActive(false);
        }
    }
}
