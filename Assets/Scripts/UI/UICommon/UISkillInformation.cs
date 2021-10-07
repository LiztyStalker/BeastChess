using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInformation : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _typeSkillActivateText;

    [SerializeField]
    private Text _typeSkillConditionText;

    [SerializeField]
    private Text _descriptionText;

    [SerializeField]
    private Text _optionText;

    public void Show(SkillData skillData)
    {
        _icon.sprite = skillData.Icon;
        _nameText.text = skillData.SkillName;
        _typeSkillActivateText.text = skillData.typeSkillCast.ToString();
        _typeSkillConditionText.text = "";
        _descriptionText.text = skillData.Description;
        _optionText.text = "";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _icon.sprite = null;
        _nameText.text = "";
        _typeSkillActivateText.text = "";
        _typeSkillConditionText.text = "";
        _descriptionText.text = "";
        _optionText.text = "";
        gameObject.SetActive(false);
    }
   
}
