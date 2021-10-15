using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSquadLayout : MonoBehaviour
{

    private UIUnitBattleButton _uiButton;

    private List<UIUnitBattleButton> _list = new List<UIUnitBattleButton>();

    [SerializeField]
    private Transform _tr;

    [SerializeField]
    private Button _leftBtn;

    [SerializeField]
    private Button _rightBtn;

    [SerializeField]
    private RectTransform _scrollRect;

    public void Initialize()
    {
        var ui = DataStorage.Instance.GetDataOrNull<GameObject>("UIUnitBattleButton", null, null);
        _uiButton = ui.GetComponent<UIUnitBattleButton>();

        _leftBtn.onClick.AddListener(Left);
        _rightBtn.onClick.AddListener(Right);

        var pos = _scrollRect.anchoredPosition;
        pos.x += _leftBtn.GetComponent<RectTransform>().sizeDelta.x;
        _scrollRect.anchoredPosition = pos;


    }

    public void CleanUp()
    {
        _list.Clear();
    }

    public void SetUnitData(UnitCard[] unitDataArray)
    {
        for (int i = 0; i < unitDataArray.Length; i++)
        {
            var btn = Instantiate(_uiButton);
            btn.SetData(unitDataArray[i]);
            btn.AddUnitDownListener(DragEvent);
            btn.AddUnitUpListener(DropEvent);
            //btn.AddUnitInformationListener(InformationUnit);
            btn.transform.SetParent(_tr);
            btn.gameObject.SetActive(true);
            btn.SetInteractable(!unitDataArray[i].IsAllDead());

            _list.Add(btn);
        }
    }

    private void DragEvent(UnitCard uCard)
    {
        _dragEvent(uCard);
    }

    private void DropEvent(UIUnitBattleButton uiBtn, UnitCard uCard)
    {
        if (_dropEvent(uCard))
        {
            uiBtn.SetInteractable(false);
        }
    }

    public void Left()
    {
        var pos = _scrollRect.anchoredPosition;
        if (pos.x + _uiButton.GetComponent<RectTransform>().sizeDelta.x < _leftBtn.GetComponent<RectTransform>().sizeDelta.x)
        {
            pos.x += _uiButton.GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            pos.x = _leftBtn.GetComponent<RectTransform>().sizeDelta.x;

        }
        _scrollRect.anchoredPosition = pos;
    }

    public void Right()
    {
        var pos = _scrollRect.anchoredPosition;
        if (pos.x - _uiButton.GetComponent<RectTransform>().sizeDelta.x > -(_scrollRect.sizeDelta.x - Screen.width + _rightBtn.GetComponent<RectTransform>().sizeDelta.x))
        {
            pos.x -= _uiButton.GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            pos.x = -(_scrollRect.sizeDelta.x - Screen.width + _rightBtn.GetComponent<RectTransform>().sizeDelta.x);

        }
        _scrollRect.anchoredPosition = pos;
    }


    public void UpdateUnits()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].UpdateUnit();
        }
    }

    public bool ReturnUnit(UnitActor uActor)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].IsCompareUnitCard(uActor.unitCard))
            {
                _list[i].SetInteractable(true);
                return true;
            }
        }
        return false;
    }

    #region ##### Listener #####

    public System.Func<UnitCard, bool> _dragEvent;
    public System.Func<UnitCard, bool> _dropEvent;

    public void SetOnDragEvent(System.Func<UnitCard, bool> act) => _dragEvent = act;
    public void SetOnDropEvent(System.Func<UnitCard, bool> act) => _dropEvent = act;

    #endregion
}
