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

        //����
        //���ְ�
        //��ų
        //������
        //����
        //�����̻�
        //źȯ
        //��
        //�ʵ���

        InitializeData<UnitData>("Data/Units");
        InitializeData<CommanderData>("Data/Commanders");
        InitializeData<BattleFieldData>("Data/BattleFields");
        InitializeDirectoryInData<SkeletonDataAsset>("Data/Spine");
    }

    public static void Dispose()
    {
        _instance = null;
    }

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    private void InitializeData<T>(string path) where T : Object
    {
        var files = System.IO.Directory.GetFiles($"Assets/{path}");
        for (int j = 0; j < files.Length; j++)
        {
            var data = AssetDatabase.LoadAssetAtPath<T>(files[j]);
            Debug.Log(files[j]);
            if (data != null)
            {
                AddDirectoryInData(data.name, data);
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }


    /// <summary>
    /// ������ �ʱ�ȭ - ���丮 ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
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
                    AddDirectoryInData(data.name, data);
                }
            }
        }

        Debug.Log($"{typeof(T)} : {GetDataCount<T>()}");
    }





#if UNITY_EDITOR

    /// <summary>
    /// ��� �����͸� �����ɴϴ�
    /// ������ 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetAllDatasOrZero<T>() where T : Object
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
    /// ������ ��������
    /// ������ null
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
    /// ������ ����Ʈ ��������
    /// ������ 0
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
                    if (IsHasData<T>(dic, keys[i]))
                    {
                        list.Add(GetDataOrNull<T>(dic, keys[i]));
                    }
                }
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// ������ ���� ��������
    /// ������ 0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="count"></param>
    /// <returns></returns>
    public T[] GetRandomDatasOrZero<T>(int count) where T : Object
    {
        if (count <= 0) {
            Debug.LogWarning($"������ ������ 0 ���ϰ� �� �� �����ϴ�. 1�� ������ �� �����մϴ�");
            count = 1;
        }

        var dataArray = GetAllDatasOrZero<T>();
        var list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(list[Random.Range(0, dataArray.Length)]);
        }
        return list.ToArray();
    }

    /// <summary>
    /// �����Ͱ� �ִ��� Ȯ��
    /// ������ true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsHasData<T>(string key) where T : Object
    {
        if (IsHasDataType<T>())
        {
            var dic = _dataDic[ToTypeString<T>()];
            return IsHasData<T>(dic, key);
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
    private bool IsHasData<T>(Dictionary<string, Object> dic, string key) where T : Object => dic.ContainsKey(key);
    private T GetDataOrNull<T>(Dictionary<string, Object> dic, string key) where T : Object => (T)dic[key];


    private void AddDirectoryInData<T>(string key, T data) where T : Object
    {
        if (!IsHasDataType<T>())
            _dataDic.Add(ToTypeString<T>(), new Dictionary<string, Object>());

        if(!IsHasData<T>(key))
            _dataDic[ToTypeString<T>()].Add(key, data);
    }
}
