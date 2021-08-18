using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarSkillIcon : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Text _text;

    public void SetData(Sprite icon, int turnCount)
    {
        _icon.sprite = icon;
        _text.gameObject.SetActive(turnCount >= 0);
        if (turnCount >= 0)
            _text.text = turnCount.ToString();
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        _icon.sprite = null;
        _text.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
