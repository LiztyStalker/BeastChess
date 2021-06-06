using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStorage
{

    List<UnitData> _units = new List<UnitData>();

    UnitData _castleUnit = null;

    public UnitStorage()
    {
        var units = Resources.LoadAll<UnitData>("Units");
        if(units != null)
        {
            _units.AddRange(units);

            _castleUnit = SearchCastleData();
        }
    }

    UnitData SearchCastleData()
    {
        if (_castleUnit != null) return _castleUnit;

        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].typeUnit == TYPE_UNIT.Castle) return _units[i];
        }
        return null;
    }

    public UnitData GetCastleUnit()
    {
        return SearchCastleData();
    }

    public UnitData[] GetRandomUnits(int count)
    {
        List<UnitData> filterUnits = new List<UnitData>();
        while(count > 0)
        {
            var unit = _units[Random.Range(0, _units.Count)];
            if (unit.typeUnit != TYPE_UNIT.Castle)
            {
                if (!filterUnits.Contains(unit))
                {
                    filterUnits.Add(unit);
                    count--;
                }
            }
        }
        return filterUnits.ToArray();
    }

    public UnitCard[] GetRandomUnitCards(int count)
    {
        var units = GetRandomUnits(count);
        List<UnitCard> filterUnits = new List<UnitCard>();
        for(int i = 0; i < units.Length; i++)
        {
            filterUnits.Add(new UnitCard(units[i]));
        }
        return filterUnits.ToArray();
    }

}
