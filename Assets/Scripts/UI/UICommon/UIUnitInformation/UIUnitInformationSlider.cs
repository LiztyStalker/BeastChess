using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformationSlider : MonoBehaviour
{
    [SerializeField]
    private Text _levelText;

    [SerializeField]
    private Slider _expSlider;

    [SerializeField]
    private Text _expText;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Text _healthText;

    public void SetData(UnitCard uCard)
    {
        _levelText.text = $"Lv 1";
        _expSlider.value = 0f;
        _expText.text = "-";
        _healthSlider.value = uCard.TotalHealthRate();
        _healthText.text = $"{uCard.totalNowHealthValue} / {uCard.totalMaxHealthValue}";
    }
}
