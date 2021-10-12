using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillInformation : MonoBehaviour, IPointerClickHandler
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

    public void Show(SkillData skillData, Vector2 pos)
    {
        _icon.sprite = skillData.Icon;
        _nameText.text = skillData.SkillName;
        _typeSkillActivateText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_SKILL_CAST), skillData.typeSkillCast.ToString(), "Name");

        if(skillData.typeSkillCast != TYPE_SKILL_CAST.DeployCast)
            _typeSkillConditionText.text = $"스킬발동확률 {skillData.skillCastRate}";
        else
            _typeSkillConditionText.text = $"배치시 발동";

        _descriptionText.text = skillData.Description;
        _optionText.text = skillData.SkillManualDescription;

        transform.position = pos;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
}
