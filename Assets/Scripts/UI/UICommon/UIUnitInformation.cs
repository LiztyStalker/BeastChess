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
    Text attackTypeText;

    [SerializeField]
    Text attackValueText;

    [SerializeField]
    Text attackCountText;

    [SerializeField]
    Text attackRangeTypeText;

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
        
    public void Initialize()
    {
        _uiSkillLayout.Initialize();
        Hide();
    }

    public void ShowActor(UnitActor uActor)
    {
        uiFormation.ShowFormation(uActor.unitCard);
        icon.sprite = uActor.unitCard.Icon;
        nameText.text = uActor.unitCard.UnitName;
        groupText.text = uActor.typeUnitGroup.ToString();
        classText.text = uActor.typeUnitClass.ToString();
        positionText.text = uActor.typeUnit.ToString();
        // squadText.text = string.Format("{0} / {1}", uActor.LiveSquadCount, uActor.squad);
        healthSlider.value = uActor.unitCard.TotalHealthRate();
        healthText.text = string.Format("{0} / {1}", uActor.nowHealthValue, uActor.maxHealthValue);
        attackTypeText.text = "";// _uCard.typeUnitAttack.ToString();
        attackValueText.text = uActor.damageValue.ToString();
        attackCountText.text = uActor.attackCount.ToString();
        attackRangeTypeText.text = "";//_uCard.typeUnitAttackRange.ToString();
        attackRangeText.text = "";// _uCard.attackRangeValue.ToString();
        attackMinRangeText.text = "";// _uCard.attackMinRangeValue.ToString();
        //attackBaseText.text = (uActor.BulletData == null) ? "근거리" : "원거리";
        priorityText.text = uActor.priorityValue.ToString();
        movementText.text = uActor.movementValue.ToString();
        movementTypeText.text = uActor.typeMovement.ToString();
        //employCostText.text = uActor.employCostValue.ToString();
        //maintenanceCostText.text = uActor.maintenenceCostValue.ToString();
        _uiSkillLayout.Show(uActor.unitCard);

        gameObject.SetActive(true);

        transform.position = Camera.main.WorldToScreenPoint(uActor.position);
    }

    public void ShowData(UnitCard _uCard)
    {
        uiFormation.ShowFormation(_uCard);
        icon.sprite = _uCard.Icon;
        nameText.text = _uCard.UnitName;
        groupText.text = _uCard.typeUnitGroup.ToString();
        classText.text = _uCard.typeUnitClass.ToString();
        positionText.text = _uCard.typeUnit.ToString();
        squadText.text = string.Format("{0} / {1}", _uCard.LiveSquadCount, _uCard.squadCount);
        healthSlider.value = _uCard.TotalHealthRate();
        healthText.text = string.Format("{0} / {1}", _uCard.totalNowHealthValue, _uCard.totalMaxHealthValue);
        attackTypeText.text = "";// _uCard.typeUnitAttack.ToString();
        attackValueText.text = _uCard.damageValue.ToString();
        attackCountText.text = _uCard.attackCount.ToString();
        attackRangeTypeText.text = "";//_uCard.typeUnitAttackRange.ToString();
        attackRangeText.text = "";// _uCard.attackRangeValue.ToString();
        attackMinRangeText.text = "";// _uCard.attackMinRangeValue.ToString();
        attackBaseText.text = (_uCard.BulletData == null) ? "근거리" : "원거리";
        priorityText.text = _uCard.priorityValue.ToString();
        movementText.text = _uCard.movementValue.ToString();
        movementTypeText.text = _uCard.typeMovement.ToString();
        employCostText.text = _uCard.employCostValue.ToString();
        maintenanceCostText.text = _uCard.maintenenceCostValue.ToString();

        _uiSkillLayout.Show(_uCard);

        gameObject.SetActive(true);        
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
