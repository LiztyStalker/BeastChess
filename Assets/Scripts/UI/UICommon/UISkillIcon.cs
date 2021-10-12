using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillIcon : MonoBehaviour, IPointerClickHandler
{ 
    [SerializeField]
    Image _icon;

    SkillData _skillData;


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetData(SkillData skillData)
    {
        _skillData = skillData;
        _icon.sprite = skillData.Icon;
    }

    public void Hide()
    {
        _icon.sprite = null;
        _skillData = null;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _skillInformationEvent?.Invoke(_skillData, eventData.position);
        }
    }

    public event System.Action<SkillData, Vector2> _skillInformationEvent;
    
}
