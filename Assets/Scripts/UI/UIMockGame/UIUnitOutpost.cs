using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitOutpost : MonoBehaviour
{
    [SerializeField]
    private TYPE_TEAM _typeTeam;

    [SerializeField]
    private UIUnitOutpostButton btn;

    [SerializeField]
    private Transform _tr;

    [SerializeField]
    private Button _unitBtn;

    private List<UIUnitOutpostButton> _list = new List<UIUnitOutpostButton>();


    public void Initialize()
    {
        _list.Clear();
        _unitBtn.onClick.AddListener(SetOnUnitEvent);
    }

    public void CleanUp()
    {
        _unitBtn.onClick.RemoveListener(SetOnUnitEvent);
    }

    private void SetOnUnitEvent()
    {
        _unitEvent?.Invoke();
    }



    public bool IsEnough(UnitCard uCard)
    {
        return MockGameOutpost.instance.IsEnoughLeadership(uCard, _typeTeam) && MockGameOutpost.instance.IsEnoughCost(uCard, _typeTeam);
    }

    public void AddCard(UIUnitOutpostButton btn)
    {
        _list.Add(btn);
        btn.transform.SetParent(_tr);
        MockGameOutpost.instance.AddCard(btn.unitCard, _typeTeam);
        _refreshEvent?.Invoke();
    }

    public void RemoveCard(UIUnitOutpostButton btn)
    {
        _list.Remove(btn);
        MockGameOutpost.instance.RemoveCard(btn.unitCard, _typeTeam);
        _refreshEvent?.Invoke();

    }

    #region ##### Listener #####

    private System.Action _unitEvent;

    public void SetOnUnitListener(System.Action act) => _unitEvent = act;

    private System.Action _refreshEvent;

    public void SetOnRefreshCostValueListener(System.Action act) => _refreshEvent = act;

    private System.Action<UnitCard> _inforEvent;

    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _inforEvent = act;

    #endregion

}
