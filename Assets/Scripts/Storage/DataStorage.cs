using Spine.Unity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

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

        InitializeData<UnitData>("data");
        InitializeData<CommanderData>("data");
        InitializeData<BattleFieldData>("data");
        InitializeData<BulletData>("data");
        InitializeData<EffectData>("data");
        InitializeData<SkillData>("data");
        InitializeData<StatusData>("data");
        InitializeData<TribeData>("data");

        InitializeData<TextAsset>();

        InitializeData<GameObject>("prefab", null);
        InitializeData<Sprite>("unit", "icon");
        InitializeData<AudioClip>("sfx", "sound");
        InitializeData<AudioClip>("bgm", "sound");
        InitializeData<SkeletonDataAsset>("spine", "data");

        //InitializeData<UnitData>("Data");
        //InitializeData<CommanderData>("Data/Commanders");
        //InitializeData<BattleFieldData>("Data/BattleFields");
        //InitializeData<BulletData>("Data/Bullets");
        //InitializeData<EffectData>("Data/Effects");
        //InitializeData<SkillData>("Data/Skills");
        //InitializeData<StatusData>("Data/Status");
        //InitializeData<TribeData>("Data/Tribes");

        //InitializeData<TextAsset>("TextAssets/Translate");
        //InitializeData<TextAsset>("TextAssets/Data");

        ////InitializeDirectoryInData<GameObject>("Prefabs");
        ////InitializeDirectoryInData<Sprite>("Images");
        ////InitializeDirectoryInData<AudioClip>("Sounds");
        ////InitializeDirectoryInData<SkeletonDataAsset>("Data/Spine");

        //InitializeData<GameObject>("Prefabs");
        //InitializeData<Sprite>("Images");
        //InitializeData<AudioClip>("Sounds");
        //InitializeData<SkeletonDataAsset>("Data/Spine");

    }

    public static void Dispose()
    {
        _instance = null;
    }

    private void InitializeData<T>(string path, string directory = null) where T : Object
    {
        string bundlePath = path;
        if (directory != null)
        {
            bundlePath = Path.Combine(directory, path);
        }

        bundlePath = Path.Combine(Application.streamingAssetsPath, bundlePath);

        var assetbundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePath));
        if (assetbundle == null)
        {
            Debug.LogError($"{bundlePath} AssetBundle을 찾을 수 없습니다");
            return;
        }

        var files = assetbundle.LoadAllAssets<T>();
        for (int i = 0; i < files.Length; i++)
        {
            var data = files[i];
            //Debug.Log(files[j]);
            if (data != null)
            {
                AddDirectoryInData(data.name, data);
            }
        }
        assetbundle.Unload(false);
    }


    /// <summary>
    /// 데이터 초기화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    private void InitializeData<T>(string directory = null) where T : Object
    {

        InitializeData<T>(typeof(T).Name.ToLower(), directory);

        //#if UNITY_EDITOR
        ////        var files = System.IO.Directory.GetFiles($"Assets/{path}");
        ////        for (int j = 0; j < files.Length; j++)
        ////        {
        ////            var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
        ////            //Debug.Log(files[j]);
        ////            if (data != null)
        ////            {
        ////                AddDirectoryInData(data.name, data);
        ////            }
        ////        }

        ////        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
        ////#else
        ///
        //string bundlePath = typeof(T).Name.ToLower();
        //if(directory != null)
        //{
        //    bundlePath = Path.Combine(directory, bundlePath);
        //}

        //var assetbundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePath));
        //if (assetbundle == null)
        //{
        //    Debug.LogError($"{typeof(T).Name} AssetBundle을 찾을 수 없습니다");
        //}

        //var files = assetbundle.LoadAllAssets<T>();
        //for (int i = 0; i < files.Length; i++)
        //{
        //    var data = files[i];
        //    //Debug.Log(files[j]);
        //    if (data != null)
        //    {
        //        AddDirectoryInData(data.name, data);
        //    }
        //}
        //#endif
    }


//    /// <summary>
//    /// 데이터 초기화 - 디렉토리 경유
//    /// 현재 depth 1 데이터만 가져옴
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="path"></param>
//    private void InitializeDirectoryInData<T>(string path) where T : Object
//    {
//#if UNITY_EDITOR

//        //Debug.Log($"Assets/{path}");
//        var directories = System.IO.Directory.GetDirectories($"Assets/{path}");

//        for (int i = 0; i < directories.Length; i++)
//        {

//            var childDirs = System.IO.Directory.GetDirectories(directories[i]);

//            //Debug.Log(directories[i]);
//            if (childDirs.Length > 0)
//            {
//                var paths = directories[i].Split('\\');
//                InitializeDirectoryInData<T>($"{path}/{paths[paths.Length - 1]}");
//            }

//            var files = System.IO.Directory.GetFiles(directories[i]);
//            for (int j = 0; j < files.Length; j++)
//            {
//                var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
//                //Debug.Log(files[j]);
//                if (data != null)
//                {
//                    AddDirectoryInData(data.name, data);
//                }
//            }
//        }

//        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
//#endif


//    }


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

    public static string ToTypeString<T>() => typeof(T).Name.ToString();


    /// <summary>
    /// 데이터 가져오기 
    /// ex) GetDataOrNull<UnitData>() => return [UnitData_Data]
    /// 없으면 null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetDataOrNull<T>(string key) where T : Object => GetDataOrNull<T>(key, ToTypeString<T>(), null);

    /// <summary>
    /// 데이터 가져오기
    /// 없으면 null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetDataOrNull<T>(string key, string firstVerb, string lastVerb) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[ToTypeString<T>()];
            var cKey = GetConvertKey(key, firstVerb, lastVerb);
            //Debug.Log(cKey);
            return GetDataOrNull<T>(dic, cKey);
        }
        return null;
    }

    /// <summary>
    /// 첫 데이터 가져오기
    /// 없으면 null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="firstVerb"></param>
    /// <param name="lastVerb"></param>
    /// <returns></returns>
    public T GetFirstDataOrNull<T>() where T : Object
    {
        var arr = GetAllDataArrayOrZero<T>();
        if (arr.Length > 0)
            return arr[0];
        return null;
    }

    /// <summary>
    /// 마지막 데이터 가져오기
    /// 없으면 null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="firstVerb"></param>
    /// <param name="lastVerb"></param>
    /// <returns></returns>
    public T GetLastDataOrNull<T>() where T : Object
    {
        var arr = GetAllDataArrayOrZero<T>();
        if (arr.Length > 0)
            return arr[arr.Length - 1];
        return null;
    }



    /// <summary>
    /// 데이터리스트 가져오기 
    /// ex) GetDataArrayOrZero<UnitData>() => return [UnitData_Data]
    /// 없으면 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public T[] GetDataArrayOrZero<T>(string[] keys) where T : Object => GetDataArrayOrZero<T>(keys, ToTypeString<T>(), null);

    /// <summary>
    /// 데이터 리스트 가져오기
    /// 없으면 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public T[] GetDataArrayOrZero<T>(string[] keys, string firstVerb, string lastVerb) where T : Object
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
                        var cKey = GetConvertKey(keys[i], firstVerb, lastVerb);
                        var data = GetDataOrNull<T>(dic, cKey);
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
            list.Add(dataArray[Random.Range(0, dataArray.Length)]);
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
    public bool IsHasData<T>(string key) where T : Object => IsHasData<T>(key, ToTypeString<T>(), null);
    

    /// <summary>
    /// 데이터가 있는지 확인
    /// 있으면 true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsHasData<T>(string key, string frontVerb, string lastVerb) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[ToTypeString<T>()];
            var cKey = GetConvertKey(key, frontVerb, lastVerb);
            return dic.ContainsKey(cKey);
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
        if (dic.ContainsKey(key))
        {
            //Debug.Log("GetDataOrNull " + key);
            //Debug.Log("GetDataOrNull " + (T)dic[key]);
            return (T)dic[key];
        }
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
