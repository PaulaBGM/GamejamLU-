using TMPro;
using UnityEngine;

public class BottleHintBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;

    private void Update()
    {
        if (WineProgressManager.Instance == null) return;

        int perfects = Mathf.Min(WineProgressManager.Instance.perfectCount, 3);

        if (perfects < 3)
        {
            hintText.text = perfects.ToString();
        }
        else
        {
            hintText.text = "E";
        }
    }
}
