using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSupply : MonoBehaviour
{
    [SerializeField]
    private Text _supplyText;

    [SerializeField]
    private Slider _supplySlider;

    // Start is called before the first frame update
    public void SetSupply(int value, float rate)
    {
        _supplyText.text = value.ToString();
        _supplySlider.value = rate;
    }
}
