using UnityEngine;
using UnityEngine.UI;
using System;

public class UISkillInformation : MonoBehaviour, ICanvas
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private RectTransform _rectTr;

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

    [SerializeField]
    private Button _exitButton;

    public void Initialize()
    {
        _exitButton.onClick.AddListener(OnExitClickedEvent);
        Hide();
    }

    public void CleanUp()
    {
        _exitButton.onClick.RemoveListener(OnExitClickedEvent);
    }


    public void Show(SkillData skillData, Vector2 screenPosition)
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

        _rectTr.position = RectTransformExtend.GetRectTransformInWindow(screenPosition, _rectTr);

        gameObject.SetActive(true);
    }

    public void Hide(Action callback = null)
    {
        _icon.sprite = null;
        _nameText.text = "";
        _typeSkillActivateText.text = "";
        _typeSkillConditionText.text = "";
        _descriptionText.text = "";
        _optionText.text = "";
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    private void OnExitClickedEvent()
    {
        Hide();
    }

}
