using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{

    private static DataStorage _instance = null;

    public static DataStorage Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataStorage();
            return _instance;
        }
    }

    private List<UnitData> _units = new List<UnitData>();
    private List<CommanderData> _commanders = new List<CommanderData>();
    private List<BattleFieldData> _battleFields = new List<BattleFieldData>();

    UnitData _castleUnit = null;

    public DataStorage()
    {
        InitializeUnits();
        InitializeCommanders();
        InitializeBattleFields();
    }

    private void InitializeUnits()
    {
        var units = Resources.LoadAll<UnitData>("Units");
        Debug.Log($"Units : {units.Length}");
        if (units != null)
        {
            _units.AddRange(units);

            _castleUnit = SearchCastleData();
        }
    }

    private void InitializeCommanders()
    {
        var commanders = Resources.LoadAll<CommanderData>("Commanders");
        Debug.Log($"Commanders : {commanders.Length}");
        if (_commanders != null)
        {
            _commanders.AddRange(commanders);
        }
    }

    private void InitializeBattleFields()
    {
        var battlefields = Resources.LoadAll<BattleFieldData>("BattleFields");
        Debug.Log($"BattleFields : {battlefields.Length}");
        if (battlefields != null)
        {
            _battleFields.AddRange(battlefields);
        }
    }



    #region ##### Commander #####

    public CommanderData[] GetCommanders() => _commanders.ToArray();
    
    public CommanderCard GetCommanderCard(string name)
    {
        for(int i = 0; i < _commanders.Count; i++)
        {
            if (_commanders[i].name == name)
                return CommanderCard.Create(_commanders[i]);
        }
        return null;
    }

    public CommanderCard GetRandomCommanderCard()
    {
        return CommanderCard.Create(_commanders[Random.Range(0, _commanders.Count)]);
    }

    #endregion

          
    #region ##### Unit #####

    private UnitData SearchCastleData()
    {
        if (_castleUnit != null) return _castleUnit;

        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].typeUnit == TYPE_UNIT_FORMATION.Castle) return _units[i];
        }
        return null;
    }

    public UnitData GetCastleUnit()
    {
        return SearchCastleData();
    }

    public UnitData[] GetUnits() => _units.ToArray();
    
    public UnitData[] GetUnits(params string[] names)
    {
        List<UnitData> filterUnits = new List<UnitData>();
        for (int i = 0; i < names.Length; i++) { 
            for(int j = 0; j < _units.Count; j++)
            {
                var unit = _units[j];
                if (names[i] == unit.name && !filterUnits.Contains(unit))
                {
                    filterUnits.Add(unit);
                }
            }
        }
        return filterUnits.ToArray();
    }

    public UnitData[] GetRandomUnits(int count)
    {
        List<UnitData> unitList = new List<UnitData>();
        while(count > 0)
        {
            var unit = _units[Random.Range(0, _units.Count)];
            if (unit.typeUnit != TYPE_UNIT_FORMATION.Castle)
            {
                unitList.Add(unit);
                count--;
            }
        }
        return unitList.ToArray();
    }

    public UnitCard[] GetUnitCards(params string[] names)
    {
        var units = GetUnits(names);
        List<UnitCard> filterUnits = new List<UnitCard>();
        for (int i = 0; i < units.Length; i++)
        {
            var uCard = new UnitCard(units[i]);
            uCard.SetFormation(GetRandomFormation(units[i].squadCount));
            filterUnits.Add(uCard);
        }
        return filterUnits.ToArray();
    }

    public UnitCard[] GetRandomUnitCards(int count, bool isTest = false)
    {
        var units = GetRandomUnits(count);
        List<UnitCard> filterUnits = new List<UnitCard>();
        for(int i = 0; i < units.Length; i++)
        {
            var uCard = new UnitCard(units[i], isTest);
            uCard.SetFormation(GetRandomFormation(units[i].squadCount));
            filterUnits.Add(uCard);
        }
        return filterUnits.ToArray();
    }

    public Vector2Int[] GetRandomFormation(int count)
    {
        var formation = CreateFormationArray();
        for (int i = 0; i < count; i++)
        {
            formation = GetRandomFormation(formation);
        }

        return ConvertArrayToListFormation(formation);
    }

    public Vector2Int[] ConvertArrayToListFormation(bool[][] formation)
    {
        List<Vector2Int> formationList = new List<Vector2Int>();
        for (int y = 0; y < formation.Length; y++)
        {
            for(int x = 0; x < formation[y].Length; x++)
            {
                if (formation[y][x])
                    formationList.Add(new Vector2Int(x - 1, y - 1));
            }
        }
        return formationList.ToArray();
    }

    public bool[][] ConvertListToArrayFormation(Vector2Int[] formation)
    {
        bool[][] arr = CreateFormationArray();
        for (int i = 0; i < formation.Length; i++)
        {
            arr[formation[i].y + 1][formation[i].x + 1] = true;
        }
        return arr;
    }

    private bool[][] CreateFormationArray()
    {
        bool[][] formation = new bool[3][];
        for (int i = 0; i < formation.Length; i++)
        {
            formation[i] = new bool[3];
        }
        return formation;
    }

    private bool[][] GetRandomFormation(bool[][] formation, int stackCnt = 100)
    {
        if (stackCnt >= 0)
        {
            stackCnt--;
            var dirX = Random.Range(0, formation.Length);
            var dirY = Random.Range(0, formation.Length);

            if (!formation[dirY][dirX])
                formation[dirY][dirX] = true;
            else
                return GetRandomFormation(formation, stackCnt);
        }
        return formation;
    }

    #endregion


    #region ##### BattleField #####

    public BattleFieldData[] GetBattleFields() => _battleFields.ToArray();

    public BattleFieldData GetBattleFieldData(string name)
    {
        for (int i = 0; i < _battleFields.Count; i++)
        {
            if (_battleFields[i].name == name)
                return _battleFields[i];
        }
        return null;
    }
    #endregion
}
