using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine.Unity;

[ExecuteAlways]
public class DataStorage
{
    [ExecuteAlways]
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

//    private List<UnitData> _units = new List<UnitData>();

//    private List<CommanderData> _commanders = new List<CommanderData>();

//    private List<BattleFieldData> _battleFields = new List<BattleFieldData>();


#if UNITY_EDITOR
    //public UnitData[] GetAllUnitData() => _units.ToArray();
    //public CommanderData[] GetAllCommanderData() => _commanders.ToArray();
    //public BattleFieldData[] GetAllBattleFieldData() => _battleFields.ToArray();

    public T[] GetAllDatasOrZero<T>() where T : Object
    {
        List<T> list = new List<T>();
        if (IsHasDataType<T>())
        {
            foreach (var data in _dataDic[typeof(T).ToString()].Values)
            {
                list.Add((T)data);
            }
        }
        return list.ToArray();
    }

#endif

    //병사
    //지휘관
    //스킬
    //아이콘
    //사운드
    //상태이상
    //탄환
    //맵
    //필드블록

    UnitData _castleUnit = null;

    private DataStorage()
    {
        InitializeData<UnitData>("Data/Units");
        InitializeData<CommanderData>("Data/Commanders");
        InitializeData<BattleFieldData>("Data/BattleFields");
        InitializeDirectoryInData<SkeletonDataAsset>("Data/Spine");
    }

    public static void Dispose()
    {
        _instance = null;
    }

    //private void InitializeUnits()
    //{
    //    var units = Resources.LoadAll<UnitData>("Units");
    //    if (units != null)
    //    {
    //        for (int i = 0; i < units.Length; i++)
    //        {
    //            if(!IsHasData<UnitData>(units[i].Key))
    //                AddData(units[i].Key, units[i]);
    //        }
    //    }
    //    Debug.Log($"Units : {GetDataCount<UnitData>()}");
    //}

    //private void InitializeCommanders()
    //{
    //    var commanders = Resources.LoadAll<CommanderData>("Commanders");
    //    if (commanders != null)
    //    {
    //        for (int i = 0; i < commanders.Length; i++)
    //        {
    //            if (!IsHasData<CommanderData>(commanders[i].Key))
    //                AddData(commanders[i].Key, commanders[i]);
    //        }
    //    }
    //    Debug.Log($"Commanders : {GetDataCount<CommanderData>()}");
    //}

    //private void InitializeBattleFields()
    //{
    //    var battlefields = Resources.LoadAll<BattleFieldData>("BattleFields");
    //    if (battlefields != null)
    //    {
    //        for (int i = 0; i < battlefields.Length; i++)
    //        {
    //            if (!IsHasData<BattleFieldData>(battlefields[i].Key))
    //                AddData(battlefields[i].Key, battlefields[i]);
    //        }
    //    }

    //    Debug.Log($"BattleFields : {GetDataCount<BattleFieldData>()}");

    //}






    //private void InitializeSkeletonAssets()
    //{
    //    var directories = System.IO.Directory.GetDirectories("Assets/Data/Spine");

    //    for (int i = 0; i < directories.Length; i++)
    //    {
    //        var files = System.IO.Directory.GetFiles(directories[i]);
    //        for(int j = 0; j < files.Length; j++)
    //        {
    //            var data = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(files[j]);
    //            if(data != null)
    //            {
    //                AddData(data.name, data);
    //            }
    //        }
    //    }

    //    Debug.Log($"SkeletonAssets : {GetDataCount<SkeletonDataAsset>()}");
    //}



    private void InitializeData<T>(string path) where T : Object
    {
        var files = System.IO.Directory.GetFiles($"Assets/{path}");
        for (int j = 0; j < files.Length; j++)
        {
            var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
            Debug.Log(files[j]);
            if (data != null)
            {
                AddData(data.name, data);
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }


    private void InitializeDirectoryInData<T>(string path) where T : Object
    {
        var directories = System.IO.Directory.GetDirectories($"Assets/{path}");

        for (int i = 0; i < directories.Length; i++)
        {
            var files = System.IO.Directory.GetFiles(directories[i]);
            Debug.Log(directories[i]);
            for (int j = 0; j < files.Length; j++)
            {
                var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
                Debug.Log(files[j]);
                if (data != null)
                {
                    AddData(data.name, data);
                }
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }




    //#region ##### Commander #####

    //public CommanderData[] GetCommanders() => _commanders.ToArray();

    //public CommanderCard GetCommanderCard(string name)
    //{
    //    for(int i = 0; i < _commanders.Count; i++)
    //    {
    //        if (_commanders[i].name == name)
    //            return CommanderCard.Create(_commanders[i]);
    //    }
    //    Debug.LogError($"{name} 을 찾지 못했습니다");
    //    return null;
    //}

    //public CommanderCard GetRandomCommanderCard()
    //{
    //    return CommanderCard.Create(_commanders[Random.Range(0, _commanders.Count)]);
    //}

    //#endregion


    //#region ##### Unit #####

    //private UnitData SearchCastleData()
    //{
    //    if (_castleUnit != null) return _castleUnit;

    //    for (int i = 0; i < _units.Count; i++)
    //    {
    //        if (_units[i].TypeUnit == TYPE_UNIT_FORMATION.Castle) return _units[i];
    //    }
    //    return null;
    //}

    //public UnitData GetCastleUnit()
    //{
    //    return SearchCastleData();
    //}

    //public UnitData[] GetUnits() => _units.ToArray();

    //public UnitData[] GetUnits(params string[] names)
    //{
    //    List<UnitData> filterUnits = new List<UnitData>();
    //    for (int i = 0; i < names.Length; i++) { 
    //        for(int j = 0; j < _units.Count; j++)
    //        {
    //            var unit = _units[j];
    //            if (names[i] == unit.name && !filterUnits.Contains(unit))
    //            {
    //                filterUnits.Add(unit);
    //            }
    //        }
    //    }
    //    return filterUnits.ToArray();
    //}

    //public UnitData[] GetRandomUnits(int count)
    //{
    //    List<UnitData> unitList = new List<UnitData>();
    //    while(count > 0)
    //    {
    //        var unit = _units[Random.Range(0, _units.Count)];
    //        if (unit.TypeUnit != TYPE_UNIT_FORMATION.Castle)
    //        {
    //            unitList.Add(unit);
    //            count--;
    //        }
    //    }
    //    return unitList.ToArray();
    //}

    //public UnitCard[] GetUnitCards(params string[] names)
    //{
    //    var units = GetUnits(names);
    //    List<UnitCard> filterUnits = new List<UnitCard>();
    //    for (int i = 0; i < units.Length; i++)
    //    {
    //        var uCard = new UnitCard(units[i]);
    //        uCard.SetFormation(GetRandomFormation(units[i].SquadCount));
    //        filterUnits.Add(uCard);
    //    }
    //    return filterUnits.ToArray();
    //}

    //public UnitCard[] GetRandomUnitCards(int count, bool isTest = false)
    //{
    //    var units = GetRandomUnits(count);
    //    List<UnitCard> filterUnits = new List<UnitCard>();
    //    for(int i = 0; i < units.Length; i++)
    //    {
    //        var uCard = new UnitCard(units[i], isTest);
    //        uCard.SetFormation(GetRandomFormation(units[i].SquadCount));
    //        filterUnits.Add(uCard);
    //    }
    //    return filterUnits.ToArray();
    //}

    //public Vector2Int[] GetRandomFormation(int count)
    //{
    //    var formation = CreateFormationArray();
    //    for (int i = 0; i < count; i++)
    //    {
    //        formation = GetRandomFormation(formation);
    //    }

    //    return ConvertArrayToListFormation(formation);
    //}

    //public Vector2Int[] ConvertArrayToListFormation(bool[][] formation)
    //{
    //    List<Vector2Int> formationList = new List<Vector2Int>();
    //    for (int y = 0; y < formation.Length; y++)
    //    {
    //        for(int x = 0; x < formation[y].Length; x++)
    //        {
    //            if (formation[y][x])
    //                formationList.Add(new Vector2Int(x - 1, y - 1));
    //        }
    //    }
    //    return formationList.ToArray();
    //}

    //public bool[][] ConvertListToArrayFormation(Vector2Int[] formation)
    //{
    //    bool[][] arr = CreateFormationArray();
    //    for (int i = 0; i < formation.Length; i++)
    //    {
    //        arr[formation[i].y + 1][formation[i].x + 1] = true;
    //    }
    //    return arr;
    //}

    //private bool[][] CreateFormationArray()
    //{
    //    bool[][] formation = new bool[3][];
    //    for (int i = 0; i < formation.Length; i++)
    //    {
    //        formation[i] = new bool[3];
    //    }
    //    return formation;
    //}

    //private bool[][] GetRandomFormation(bool[][] formation, int stackCnt = 100)
    //{
    //    if (stackCnt >= 0)
    //    {
    //        stackCnt--;
    //        var dirX = Random.Range(0, formation.Length);
    //        var dirY = Random.Range(0, formation.Length);

    //        if (!formation[dirY][dirX])
    //            formation[dirY][dirX] = true;
    //        else
    //            return GetRandomFormation(formation, stackCnt);
    //    }
    //    return formation;
    //}

    //#endregion


    //#region ##### BattleField #####

    //public BattleFieldData[] GetBattleFields() => _battleFields.ToArray();

    //public BattleFieldData GetBattleFieldData(string name)
    //{
    //    for (int i = 0; i < _battleFields.Count; i++)
    //    {
    //        if (_battleFields[i].name == name)
    //            return _battleFields[i];
    //    }
    //    return null;
    //}
    //#endregion



    Dictionary<string, Dictionary<string, Object>> _dataDic = new Dictionary<string, Dictionary<string, Object>>();


    public T GetDataOrNull<T>(string key) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[typeof(T).ToString()];
            return GetDataOrNull<T>(dic, key);
        }
        return null;
    }

    public T[] GetDatasOrNull<T>(params string[] keys) where T : Object
    {
        if (keys != null && keys.Length > 0)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < keys.Length; i++)
            {
                if (IsHasDataType<T>())
                {
                    var dic = _dataDic[typeof(T).ToString()];
                    if (IsHasData<T>(dic, keys[i]))
                    {
                        list.Add(GetDataOrNull<T>(dic, keys[i]));
                    }
                }
            }
            return list.ToArray();
        }
        return null;
    }

    public T[] GetRandomDatasOrZero<T>(int count) where T : Object
    {
        var datas = GetAllDatasOrZero<T>();
        var list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(list[Random.Range(0, datas.Length)]);
        }
        return list.ToArray();
    }

    public bool IsHasData<T>(string key) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[typeof(T).ToString()];
            return IsHasData<T>(dic, key);
        }
        return false;
    }

    private int GetDataCount<T>() where T : Object
    {
        if (IsHasDataType<T>())
        {
            return _dataDic[typeof(T).ToString()].Count;
        }
        return 0;
    }

    private bool IsHasDataType<T>() where T : Object => _dataDic.ContainsKey(typeof(T).ToString());
    private bool IsHasData<T>(Dictionary<string, Object> dic, string key) where T : Object => dic.ContainsKey(key);
    private T GetDataOrNull<T>(Dictionary<string, Object> dic, string key) where T : Object => (T)dic[key];



    private void AddData<T>(string key, T data) where T : Object
    {
        if (!IsHasDataType<T>())
        {
            _dataDic.Add(typeof(T).ToString(), new Dictionary<string, Object>());
        }

        if(!IsHasData<T>(key))
            _dataDic[typeof(T).ToString()].Add(key, data);
    }
}
