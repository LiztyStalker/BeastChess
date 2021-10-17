using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitOutpost : MonoBehaviour
{
    [SerializeField]
    private TYPE_TEAM _typeTeam;

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
        Clear();
        for (int i = 0; i < unitCards.Length; i++)
        {
            var block = GetBlock();
            block.SetData(i, unitCards[i]);
        }

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

    private void Clear()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    public void SetChallenge(bool isChallenge)
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

    public void ChangeCard(UnitCard uCard)
    {
        _changeEvent?.Invoke(_typeTeam, uCard);
        _refreshEvent?.Invoke(_typeTeam);
    }


    #region ##### Listener #####

    private System.Action _unitEvent;
    public void SetOnUnitListener(System.Action act) => _unitEvent = act;


    private System.Action<TYPE_TEAM> _refreshEvent;
    public void AddOnRefreshListener(System.Action<TYPE_TEAM> act) => _refreshEvent += act;
    public void RemoveOnRefreshListener(System.Action<TYPE_TEAM> act) => _refreshEvent -= act;


    private System.Action<UnitCard> _inforEvent;
    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _inforEvent = act;


    private System.Action<TYPE_TEAM, UnitCard> _changeEvent;
    public void SetOnUnitChangeListener(System.Action<TYPE_TEAM, UnitCard> act) => _changeEvent = act;


    #endregion

}
