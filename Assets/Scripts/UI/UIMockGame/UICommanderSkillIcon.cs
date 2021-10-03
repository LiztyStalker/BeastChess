using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICommanderSkillIcon : MonoBehaviour
{
    [SerializeField]
    Image _icon;

    public void SetData(SkillData skillData)
    {
        _icon.sprite = skillData.Icon;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
