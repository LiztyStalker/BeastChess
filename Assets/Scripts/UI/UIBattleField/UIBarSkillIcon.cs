using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarSkillIcon : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Text _turnText;

    [SerializeField]
    private Text _overlapText;

    public void SetData(Sprite icon, int turnCount, bool isOverlapped, int overlapCount)
    {
        _icon.sprite = icon;
        _turnText.gameObject.SetActive(turnCount >= 0);
        if (turnCount >= 0)
            _turnText.text = turnCount.ToString();

        _overlapText.gameObject.SetActive(isOverlapped);
        _overlapText.text = overlapCount.ToString();

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        _icon.sprite = null;
        _turnText.gameObject.SetActive(false);
        _overlapText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
