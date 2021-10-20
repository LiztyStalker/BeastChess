#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class UnitGeneratorEditor : EditorWindow
{
    private TextAsset _textAsset;

    private Vector2 _scrollPos;

    Dictionary<string, UnitData> _dic = new Dictionary<string, UnitData>();
    

    [MenuItem("Window/Generator/Unit Generator")]
    private static void Init()
    {
        UnitGeneratorEditor gen = (UnitGeneratorEditor)GetWindow(typeof(UnitGeneratorEditor));
        gen.Show(); 
    }

    private void OnGUI()
    {
        
        GUILayout.Label("UnitData Asset", EditorStyles.boldLabel);

        _textAsset = DataStorage.Instance.GetDataFromAssetDatabase<TextAsset>("TextAssets/Data/UnitData.json");
            
            //DataStorage.Instance.GetDataOrNull<TextAsset>("UnitData", null, null);
        GUI.enabled = false;
        _textAsset = (TextAsset)EditorGUILayout.ObjectField(_textAsset, typeof(TextAsset), true);
        GUI.enabled = true;

        GUILayout.Space(20f);

        GUILayout.Label("UnitList", EditorStyles.boldLabel);

        ShowUnits();
        
        GUILayout.Space(20f);

        

        if (GUILayout.Button("Unit Generator"))
        {
            UnitGenerator();
            DataStorage.Dispose();
        }
    }


    private void ShowUnits()
    {
        var units = DataStorage.Instance.GetDataArrayFromAssetDatabase<UnitData>("Data/Units");

        if (units.Length > 0)
        {
            RefreshDictionary(units);
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            EditorGUI.indentLevel++;
            GUI.enabled = false;
            for (int i = 0; i < units.Length; i++)
            {
                units[i] = (UnitData)EditorGUILayout.ObjectField(units[i], typeof(UnitData), true);
            }
            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.EndScrollView();
        }
    }

    private void RefreshDictionary(UnitData[] units)
    {
        var checkList = new List<string>();
        foreach(var key in _dic.Keys)
        {
            checkList.Add(key);
        }

        for(int i = 0; i < units.Length; i++)
        {
            var unit = units[i];
            if (!_dic.ContainsKey(unit.Key))
            {
                _dic.Add(unit.Key, unit);
                checkList.Remove(unit.Key);
            }
        }

        for(int i = 0; i < checkList.Count; i++)
        {
            _dic.Remove(checkList[i]);
        }
    }

    private UnitData GetUnitData(string key)
    {
        return _dic[key];
    }

    private bool IsHasData(string key)
    {
        return _dic.ContainsKey(key);
    }

    private void UnitGenerator()
    {

        if (_textAsset != null)
        {
            var jsonData = JsonMapper.ToObject(_textAsset.text);

            foreach(var key in jsonData.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var jData = jsonData[key];
                    if (!IsHasData(key))
                    //if (!DataStorage.Instance.IsHasData<UnitData>(key))
                    {
                        var data = UnitData.Create(key, jData);
                        Debug.Log($"CreateData {data.Key}");
                    }
                    else
                    {
                        var data = GetUnitData(key);
                        //var data = DataStorage.Instance.GetDataOrNull<UnitData>(key);
                        if (data != null)
                        {
                            data.SetData(key, jData);
                            Debug.Log($"RefreshData {data.Key}");
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"UnitData TextAsset을 찾을 수 없습니다");
        }
    }
}

#endif