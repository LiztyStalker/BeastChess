using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitOutpostBarrack : MonoBehaviour
{

    [SerializeField]
    UIUnitOutpostButton uiButton;

    [SerializeField]
    Transform _tr;

    List<UIUnitOutpostButton> _list = new List<UIUnitOutpostButton>();

    List<UnitCard> units;

    public void Show(TYPE_TEAM typeTeam)
    {
        gameObject.SetActive(true);

        units = (typeTeam == TYPE_TEAM.Left) ? MockGameData.instance.totalUnits_L : MockGameData.instance.totalUnits_R;
        for (int i = 0; i < units.Count; i++)
        {
            var block = (i >= _list.Count) ? Instantiate(uiButton) : _list[i];
            block.transform.SetParent(_tr);
            block.SetData(i, units[i]);
            _list.Add(block);
        }        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AddCard(UIUnitOutpostButton btn)
    {
        _list.Add(btn);
        btn.transform.SetParent(_tr);
        units.Add(btn.unitCard);
    }

    public void RemoveCard(UIUnitOutpostButton btn)
    {
        _list.Remove(btn);
        units.Remove(btn.unitCard);
    }

}
