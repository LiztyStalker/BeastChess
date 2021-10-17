using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMockBattleField : MonoBehaviour
{

    //맵 데이터 리스트
    private BattleFieldData[] _battleFields;

    [SerializeField]
    private Image _mapImage;

    [SerializeField]
    private UIScroll _mapNameScroll;
    [SerializeField]
    private UIScroll _mapSizeScroll;
    [SerializeField]
    private UIScroll _costScroll;
    [SerializeField]
    private UIScroll _levelScroll;

    [SerializeField]
    private Text _descriptionText;

    private int _battleFieldIndex = 0;

    public void Initialize()
    {
        _battleFields = DataStorage.Instance.GetAllDataArrayOrZero<BattleFieldData>();// GetBattleFields();
        _mapNameScroll.AddOnLeftBtnClickListener(OnLeftClicked);
        _mapNameScroll.AddOnRightBtnClickListener(OnRightClicked);

    }

    public void CleanUp()
    {
        _mapNameScroll.RemoveOnLeftBtnClickListener(OnLeftClicked);
        _mapNameScroll.RemoveOnRightBtnClickListener(OnRightClicked);
    }

    public void RefreshBattleField(BattleFieldData battleFieldData)
    {
        _mapImage.sprite = battleFieldData.background;
        _descriptionText.text = battleFieldData.description;
        _mapNameScroll.SetText(battleFieldData.Name);
    }

    public void SetBattleField()
    {
        //전장 등록하기
        var battleFieldData = _battleFields[_battleFieldIndex];
        _battlefieldEvent?.Invoke(battleFieldData);
    }

    private void OnLeftClicked()
    {
        if (_battleFieldIndex - 1 < 0)
            _battleFieldIndex = _battleFields.Length - 1;
        else
            _battleFieldIndex--;

        SetBattleField();
    }

    private void OnRightClicked()
    {
        if (_battleFieldIndex + 1 >= _battleFields.Length)
            _battleFieldIndex = 0;
        else
            _battleFieldIndex++;

        SetBattleField();
    }

    #region ##### Listener #####

    private System.Action<BattleFieldData> _battlefieldEvent;
    public void SetOnBattleFieldListener(System.Action<BattleFieldData> act) => _battlefieldEvent = act;
    #endregion

}
