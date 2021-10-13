using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleField : MonoBehaviour
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

        SetBattleField();
        ShowBattleField();
    }

    public void OnDestroy()
    {
        _mapNameScroll.RemoveOnLeftBtnClickListener(OnLeftClicked);
        _mapNameScroll.RemoveOnRightBtnClickListener(OnRightClicked);
    }

    private void ShowBattleField()
    {
        _mapImage.sprite = _battleFields[_battleFieldIndex].background;
        _descriptionText.text = _battleFields[_battleFieldIndex].description;
        _mapNameScroll.SetText(_battleFields[_battleFieldIndex].Name);

        MockGameOutpost.instance.battleFieldData = _battleFields[_battleFieldIndex];
    }

    private void SetBattleField()
    {
        //전장 등록하기
    }

    private void OnLeftClicked()
    {
        if (_battleFieldIndex - 1 < 0)
            _battleFieldIndex = _battleFields.Length - 1;
        else
            _battleFieldIndex--;

        SetBattleField();
        ShowBattleField();

    }

    private void OnRightClicked()
    {
        if (_battleFieldIndex + 1 >= _battleFields.Length)
            _battleFieldIndex = 0;
        else
            _battleFieldIndex++;

        SetBattleField();
        ShowBattleField();
    }

}
