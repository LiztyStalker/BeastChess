using Spine.Unity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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


   

    private Dictionary<string, Dictionary<string, Object>> _dataDic = new Dictionary<string, Dictionary<string, Object>>();

    private DataStorage()
    {
        InitializeData<UnitData>("Data/Units");
        InitializeData<CommanderData>("Data/Commanders");
        InitializeData<BattleFieldData>("Data/BattleFields");
        InitializeData<BulletData>("Data/Bullets");
        InitializeData<EffectData>("Data/Effects");
        InitializeData<SkillData>("Data/Skills");
        InitializeData<StatusData>("Data/Status");
        InitializeData<TribeData>("Data/Tribes");
        InitializeDirectoryInData<AudioClip>("Sounds");
        InitializeDirectoryInData<SkeletonDataAsset>("Data/Spine");
    }

    public static void Dispose()
    {
        _instance = null;
    }

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    private void InitializeData<T>(string path) where T : Object
    {
        var files = System.IO.Directory.GetFiles($"Assets/{path}");
        for (int j = 0; j < files.Length; j++)
        {
            var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
            //Debug.Log(files[j]);
            if (data != null)
            {
                AddDirectoryInData(data.name, data);
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }


    /// <summary>
    /// 데이터 초기화 - 디렉토리 경유
    /// 현재 depth 1 데이터만 가져옴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    private void InitializeDirectoryInData<T>(string path) where T : Object
    {
        Debug.Log($"Assets/{path}");
        var directories = System.IO.Directory.GetDirectories($"Assets/{path}");

        for (int i = 0; i < directories.Length; i++)
        {

            var childDirs = System.IO.Directory.GetDirectories(directories[i]);

            Debug.Log(directories[i]);
            if (childDirs.Length > 0)
            {
                var paths = directories[i].Split('\\');
                InitializeDirectoryInData<T>($"{path}/{paths[paths.Length - 1]}");
            }

            var files = System.IO.Directory.GetFiles(directories[i]);
            for (int j = 0; j < files.Length; j++)
            {
                var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
                //Debug.Log(files[j]);
                if (data != null)
                {
                    AddDirectoryInData(data.name, data);
                }
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }





#if UNITY_EDITOR

    /// <summary>
    /// 모든 데이터를 가져옵니다
    /// 없으면 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetAllDataArrayOrZero<T>() where T : Object
    {
        List<T> list = new List<T>();
        if (IsHasDataType<T>())
        {
            foreach (var data in _dataDic[ToTypeString<T>()].Values)
            {
                list.Add((T)data);
            }
        }
        return list.ToArray();
    }

#endif

    private string ToTypeString<T>() => typeof(T).ToString();

    /// <summary>
    /// 데이터 가져오기
    /// 없으면 null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetDataOrNull<T>(string key) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[ToTypeString<T>()];
            return GetDataOrNull<T>(dic, key);
        }
        return null;
    }

    /// <summary>
    /// 데이터 리스트 가져오기
    /// 없으면 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public T[] GetDataArrayOrZero<T>(params string[] keys) where T : Object
    {
        List<T> list = new List<T>();
        if (keys != null && keys.Length > 0)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (IsHasDataType<T>())
                {
                    var dic = _dataDic[ToTypeString<T>()];
                    if (dic.ContainsKey(GetConvertKey(keys[i], ToTypeString<T>())))
                    {
                        var data = GetDataOrNull<T>(dic, keys[i]);
                        if (data != null)
                        {
                            list.Add(data);
                        }
                    }
                }
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 데이터 랜덤 가져오기
    /// 없으면 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="count"></param>
    /// <returns></returns>
    public T[] GetRandomDatasOrZero<T>(int count) where T : Object
    {
        if (count <= 0) {
            Debug.LogWarning($"가져올 수량은 0 이하가 될 수 없습니다. 1로 수정된 후 진행합니다");
            count = 1;
        }

        var dataArray = GetAllDataArrayOrZero<T>();
        var list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(list[Random.Range(0, dataArray.Length)]);
        }
        return list.ToArray();
    }

    /// <summary>
    /// 데이터가 있는지 확인
    /// 있으면 true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsHasData<T>(string key) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[ToTypeString<T>()];
            return dic.ContainsKey(GetConvertKey(key, ToTypeString<T>()));
        }
        return false;
    }








    private int GetDataCount<T>() where T : Object
    {
        if (IsHasDataType<T>())
        {
            return _dataDic[ToTypeString<T>()].Count;
        }
        return 0;
    }

    private bool IsHasDataType<T>() where T : Object => _dataDic.ContainsKey(ToTypeString<T>());
    private T GetDataOrNull<T>(Dictionary<string, Object> dic, string key) where T : Object
    {
        var cKey = GetConvertKey(key, ToTypeString<T>());
        if (dic.ContainsKey(cKey))
            return (T)dic[cKey];
        return null;
    }


    private void AddDirectoryInData<T>(string key, T data) where T : Object
    {
        if (!IsHasDataType<T>())
            _dataDic.Add(ToTypeString<T>(), new Dictionary<string, Object>());

        if(!IsHasData<T>(key))
            _dataDic[ToTypeString<T>()].Add(key, data);
    }


    private string GetConvertKey(string key, string frontVerb = null, string backVerb = null)
    {
        if (frontVerb != null) frontVerb += "_";
        if (backVerb != null) backVerb = "_" + backVerb;
        return $"{frontVerb}{key}{backVerb}";
    }
}
