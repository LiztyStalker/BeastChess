using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformation : MonoBehaviour
{
    [SerializeField]
    UIUnitFormation uiFormation;

    [SerializeField]
    UIUnitSkillLayout _uiSkillLayout;

    [SerializeField]
    RectTransform _tr;

    [SerializeField]
    Image icon;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text groupText;

    [SerializeField]
    Text classText;

    [SerializeField]
    Text positionText;

    [SerializeField]
    Text squadText;

    [SerializeField]
    Slider expSlider;

    [SerializeField]
    Text expText;

    [SerializeField]
    Slider healthSlider;

    [SerializeField]
    Text healthText;

    [SerializeField]
    Text attackRangeTypeText;

    [SerializeField]
    Text attackValueText;

    [SerializeField]
    Text attackCountText;

    [SerializeField]
    Text attackRangeText;

    [SerializeField]
    Text attackMinRangeText;

    [SerializeField]
    Text attackBaseText;

    [SerializeField]
    Text priorityText;

    [SerializeField]
    Text movementText;

    [SerializeField]
    Text movementTypeText;

    [SerializeField]
    Text employCostText;

    [SerializeField]
    Text maintenanceCostText;

    [SerializeField]
    Text appearCostText;
        
    public void Initialize()
    {
        _uiSkillLayout.Initialize();
        Hide();
    }

    public void ShowActor(UnitActor uActor)
    {
        ShowData(uActor.unitCard, uActor.position);        
    }

    public void ShowData(UnitCard _uCard, Vector2 position)
    {
        uiFormation.ShowFormation(_uCard);
        icon.sprite = _uCard.Icon;
        nameText.text = _uCard.UnitName;
        groupText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_GROUP), _uCard.typeUnitGroup.ToString(), "Name");
        classText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_CLASS), _uCard.typeUnitClass.ToString(), "Name");
        positionText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_FORMATION), _uCard.typeUnit.ToString(), "Name");
        squadText.text = string.Format("{0} / {1}", _uCard.LiveSquadCount, _uCard.squadCount);
        healthSlider.value = _uCard.TotalHealthRate();
        healthText.text = string.Format("{0} / {1}", _uCard.totalNowHealthValue, _uCard.totalMaxHealthValue);
        attackValueText.text = _uCard.damageValue.ToString();
        attackCountText.text = _uCard.attackCount.ToString();
        attackRangeTypeText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_TARGET_RANGE), _uCard.AttackTargetData.TypeTargetRange.ToString(), "Name");
        attackRangeText.text = _uCard.AttackTargetData.TargetRange.ToString();
        attackMinRangeText.text = _uCard.AttackTargetData.TargetStartRange.ToString();
        attackBaseText.text = (_uCard.BulletData == null) ? "근거리" : "원거리";
        priorityText.text = _uCard.priorityValue.ToString();
        movementText.text = _uCard.movementValue.ToString();
        movementTypeText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_MOVEMENT), _uCard.typeMovement.ToString(), "Name");
        employCostText.text = _uCard.employCostValue.ToString();
        maintenanceCostText.text = _uCard.maintenenceCostValue.ToString();
        appearCostText.text = _uCard.AppearCostValue.ToString();

        _uiSkillLayout.Show(_uCard);

        gameObject.SetActive(true);

        transform.position = Camera.main.WorldToScreenPoint(position);

    }

    public void SetOnTextEvent(System.Action<string> listener)
    {
        var arr = transform.GetComponentsInChildren<UITextInformation>(true);
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i].SetOnShowListener(listener);
        }
    }

    public void SetPosition(Vector2 pos)
    {
        _tr.transform.position = pos;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
