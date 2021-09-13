using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    [SerializeField]
    private Transform _tr;

    [SerializeField]
    private UIBarSkillIcon _uiIcon;

    List<UIBarSkillIcon> _iconList = new List<UIBarSkillIcon>();

    public void SetBar(float value)
    {
        if (value == 0f)
            _slider.value = 0f;
        else
        {
            value *= 0.9f;
            value += 0.1f;
            _slider.value = Mathf.Clamp(value, 0.1f, 1f);
        }
    }

    public void ShowSkill(StatusElement[] skillElements)
    {
        Clear();

        //for(int i = 0; i < skillElements.Length; i++)
        //{
        //    var block = GetBlock();
        //    block.SetData(skillElements[i].statusData., skillElements[i].turnCount, skillElements[i].IsOverlaped(), skillElements[i].overlapCount);
        //}
    }

    private void Clear()
    {
        for (int i = 0; i < _iconList.Count; i++)
        {
            _iconList[i].Clear();
        }
    }

    private UIBarSkillIcon GetBlock()
    {
        for(int i = 0; i < _iconList.Count; i++)
        {
            if (!_iconList[i].gameObject.activeSelf) return _iconList[i];
        }

        var block = Instantiate(_uiIcon);
        block.transform.SetParent(_tr);
        block.transform.localScale = Vector2.one;
        _iconList.Add(block);
        return block;
    }
}
