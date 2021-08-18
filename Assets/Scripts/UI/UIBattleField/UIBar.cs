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

    List<Image> _iconList = new List<Image>();

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

    public void ShowSkill(SkillElement[] skillElements)
    {
        Clear();

        for(int i = 0; i < skillElements.Length; i++)
        {
            var image = GetBlock();
            image.sprite = skillElements[i].skillData.icon;
        }
    }

    private void Clear()
    {
        for (int i = 0; i < _iconList.Count; i++)
        {
            _iconList[i].sprite = null;
            _iconList[i].gameObject.SetActive(false);
        }
    }

    private Image GetBlock()
    {
        for(int i = 0; i < _iconList.Count; i++)
        {
            if (!_iconList[i].gameObject.activeSelf) return _iconList[i];
        }

        var obj = new GameObject();
        var rectTr = obj.AddComponent<RectTransform>();
        var image = obj.AddComponent<Image>();
        obj.transform.SetParent(_tr);
        obj.transform.localScale = Vector2.one;
        rectTr.sizeDelta = Vector2.one * 16f;
        _iconList.Add(image);
        return image;
    }
}
