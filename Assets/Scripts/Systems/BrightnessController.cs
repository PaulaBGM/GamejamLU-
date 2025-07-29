using UnityEngine;
using UnityEngine.UI;
public class BrightnessController : MonoBehaviour
{
    public Scrollbar slider;
    public float sliderValue;
    public Image brightnessPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);

        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }

    public void ChangeSlider(float value) 
    {
        sliderValue = value;
       PlayerPrefs.SetFloat("Brightness", sliderValue);

        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, slider.value);
    }
}
