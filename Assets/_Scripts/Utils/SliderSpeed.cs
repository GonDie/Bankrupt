using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSpeed : MonoBehaviour
{
    Slider _slider;

	void Start ()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener((float f) => Time.timeScale = f);

        Time.timeScale = _slider.value;
    }
}