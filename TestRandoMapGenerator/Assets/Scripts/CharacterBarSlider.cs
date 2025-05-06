using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarSlider : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
