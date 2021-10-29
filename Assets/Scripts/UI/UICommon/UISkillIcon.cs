using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillIcon : MonoBehaviour, IPointerClickHandler
{ 
    [SerializeField]
    private Image _icon;

    private SkillData _skillData;

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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _skillInformationEvent?.Invoke(_skillData, eventData.position);
        }
    }

    #region ##### Listener #####
    private event System.Action<SkillData, Vector2> _skillInformationEvent;

    public void SetOnSkillInformationEvent(System.Action<SkillData, Vector2> act) => _skillInformationEvent = act;
    #endregion

}
