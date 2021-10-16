using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICommanderSkillIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _icon;

    private SkillData _skillData;

    public void SetData(SkillData skillData)
    {
        _skillData = skillData;
        _icon.sprite = skillData.Icon;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _skillData = null;
        gameObject.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _skillInforEvent?.Invoke(_skillData, eventData.position);
        }
    }

    #region ##### Listener #####

    private System.Action<SkillData, Vector2> _skillInforEvent;
    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _skillInforEvent = act;

    #endregion
}
