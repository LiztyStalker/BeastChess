using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class UIUnitOutpostBarrack : MonoBehaviour
{
    [SerializeField]
    Transform _tr;

    [SerializeField]
    Toggle _allToggle;

    [SerializeField]
    Toggle[] _toggles;

    private List<UIUnitOutpostButton> _list = new List<UIUnitOutpostButton>();

    private List<UnitCard> _units = new List<UnitCard>();

    TYPE_UNIT_GROUP _typeUnitGroup = TYPE_UNIT_GROUP.All;

    TYPE_TEAM _typeTeam;

    public void Initialize()
    {
        _allToggle.onValueChanged.AddListener(delegate { OnToggleEvent(true); });

        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].onValueChanged.AddListener(delegate { OnToggleEvent(); });
        }
        Hide();
    }

    public void CleanUp()
    {
        _allToggle.onValueChanged.RemoveAllListeners();

        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].onValueChanged.RemoveAllListeners();
        }
    }

    public void Show(TYPE_TEAM typeTeam, List<UnitCard> units)
    {
        gameObject.SetActive(true);
        _typeTeam = typeTeam;
        _typeUnitGroup = TYPE_UNIT_GROUP.All;
        _units = units;
        Show(_typeTeam, _typeUnitGroup);
    }

    private void OnToggleEvent(bool isAll = false)
    {
        if (isAll)
        {
            _typeUnitGroup = TYPE_UNIT_GROUP.All;
        }
        else
        {
            _typeUnitGroup = TYPE_UNIT_GROUP.None;
            for (int i = 0; i < _toggles.Length; i++)
            {
                if (_toggles[i].isOn)
                    _typeUnitGroup |= (TYPE_UNIT_GROUP)(1 << i);
            }
//            Debug.Log(_typeUnitGroup);
        }
        Show(_typeTeam, _typeUnitGroup);
    }

    private void Show(TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeUnitGroup)
    {

        var rectTr = GetComponent<RectTransform>();

        if (typeTeam == TYPE_TEAM.Left)
        {
            rectTr.anchorMin = new Vector2(1f, 0.5f);
            rectTr.anchorMax = new Vector2(1f, 0.5f);
            rectTr.pivot = new Vector2(1f, 0.5f);
        }
        else
        {
            rectTr.anchorMin = new Vector2(0f, 0.5f);
            rectTr.anchorMax = new Vector2(0f, 0.5f);
            rectTr.pivot = new Vector2(0f, 0.5f);
        }

        Clear();

        
        var units = _units.Where(a => (a.typeUnitGroup & typeUnitGroup) == a.typeUnitGroup).OrderBy(a => a.typeUnitClass).ThenBy(a => a.name).ToArray();

        for (int i = 0; i < units.Length; i++)
        {
            var block = GetBlock();
            block.SetData(i, units[i]);
        }

        for (int i = _units.Count; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    private UIUnitOutpostButton GetBlock()
    {
        for(int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].gameObject.activeSelf) return _list[i];
        }
        var block = GameObjectCreater<UIUnitOutpostButton>.Create("UIUnitOutpostButton", _tr);
        block.SetOnUnitInformationListener(InforEvent);
        _list.Add(block);
        return block;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Clear()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].Hide();
        }
    }

    public void AddCard(UIUnitOutpostButton btn)
    {
        _list.Add(btn);
        btn.transform.SetParent(_tr);
        _units.Add(btn.unitCard);
    }

    public void RemoveCard(UIUnitOutpostButton btn)
    {
        _list.Remove(btn);
        _units.Remove(btn.unitCard);
    }

    private void InforEvent(UnitCard uCard)
    {
        _inforEvent?.Invoke(uCard);
    }
    
    public void OnFilterEvent(TYPE_UNIT_GROUP typeUnitGroup)
    {
        _typeUnitGroup = typeUnitGroup;
        Show(_typeTeam, _typeUnitGroup);
    }

    #region ##### Listener #####

    private System.Action<UnitCard> _inforEvent;

    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _inforEvent = act;

    #endregion
}
