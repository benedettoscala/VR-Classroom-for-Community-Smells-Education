using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;

public class TimelineController : UdonSharpBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;
    public float velocity = 0.05f;
    public float valueSliderToReach = 0f;

    private void Start()
    {
        slider.value = 0;
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (slider.value < valueSliderToReach)
        {
            slider.value += velocity * Time.deltaTime;
            UpdateSliderColor();
        }

        if (slider.value >= valueSliderToReach)
        {
            slider.value = valueSliderToReach;
        }
    }

    private void UpdateSliderColor()
    {
        slider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, slider.value);
    }

    public void SetSliderTarget(float target)
    {
        valueSliderToReach = target;
    }

    public void SetSliderVisibility(bool isVisible)
    {
        slider.gameObject.SetActive(isVisible);
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }
}