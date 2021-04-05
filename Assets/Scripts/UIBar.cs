using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    public void SetBar(float value)
    {
        value *= 0.9f;
        value += 0.1f;
        _slider.value = Mathf.Clamp(value, 0.1f, 1f);
    }
}
