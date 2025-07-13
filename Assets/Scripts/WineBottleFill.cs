using UnityEngine;
using UnityEngine.UI;

public class WineBottleFill : MonoBehaviour
{
    [Header("Relleno de la botella")]
    [SerializeField] private Image wineFillImage; // Image tipo Filled (Vertical, Bottom to Top)

    [Header("Referencia al progreso")]
    [SerializeField] private WineProgressManager progressManager;

    private void Update()
    {
        if (wineFillImage != null && progressManager != null)
        {
            wineFillImage.fillAmount = progressManager.ProgressNormalized;
        }
    }
}
