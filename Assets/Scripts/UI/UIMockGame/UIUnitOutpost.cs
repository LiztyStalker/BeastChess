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
        _unitBtn.gameObject.SetActive(true);
        _unitBtn.onClick.AddListener(SetOnUnitEvent);
    }

    public void CleanUp()
    {
        _unitBtn.onClick.RemoveListener(SetOnUnitEvent);
    }

    public void SetUnitCards(UnitCard[] unitCards)
    {
        for (int i = 0; i < unitCards.Length; i++)
        {
            var block = GetBlock();
            block.SetData(i, unitCards[i]);
        }

        //Debug.Log(units.Count);

        for (int i = _list.Count; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    private UIUnitOutpostButton GetBlock()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].gameObject.activeSelf) return _list[i];
        }

        var block = GameObjectCreater<UIUnitOutpostButton>.Create("UIUnitOutpostButton", _tr);
        block.SetOnUnitInformationListener(InforEvent);
        _list.Add(block);
        return block;
    }

    public void SetChallenge(bool isChallenge, int challengeLevel)
    {
        _unitBtn.gameObject.SetActive(!isChallenge);
    }

    private void SetOnUnitEvent()
    {
        _unitEvent?.Invoke();
    }

    private void InforEvent(UnitCard uCard)
    {
        _inforEvent?.Invoke(uCard);
    }

    public bool IsEnough(UnitCard uCard)
    {
        return MockGameOutpost.Current.IsEnoughLeadership(uCard, _typeTeam) && MockGameOutpost.Current.IsEnoughEmployCost(uCard, _typeTeam);
    }

    public void AddCard(UIUnitOutpostButton btn)
    {
        _list.Add(btn);
        btn.transform.SetParent(_tr);
        MockGameOutpost.Current.AddCard(btn.unitCard, _typeTeam);
        _refreshEvent?.Invoke();
    }

    public void RemoveCard(UIUnitOutpostButton btn)
    {
        _list.Remove(btn);
        MockGameOutpost.Current.RemoveCard(btn.unitCard, _typeTeam);
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
