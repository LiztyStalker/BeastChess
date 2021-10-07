using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillIcon : MonoBehaviour
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
    
    public void OnClicked()
    {
        _skillInformationEvent?.Invoke(_skillData);
    }


    public event System.Action<SkillData> _skillInformationEvent;
    
}
