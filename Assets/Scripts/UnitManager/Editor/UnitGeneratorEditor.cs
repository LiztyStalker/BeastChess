#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class UnitGeneratorEditor : EditorWindow
{
    private TextAsset _textAsset;
    private string _path = "TextAssets/UnitData.json";

    private Vector2 _scrollPos;

    [MenuItem("Window/Generator/Unit Generator")]
    private static void Init()
    {
        UnitGeneratorEditor gen = (UnitGeneratorEditor)GetWindow(typeof(UnitGeneratorEditor));
        gen.Show();
    }

    private void OnGUI()
    {

        GUILayout.Label("UnitData Asset", EditorStyles.boldLabel);
        _textAsset = (TextAsset)EditorGUILayout.ObjectField(_textAsset, typeof(TextAsset), true);
       
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
        var units = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();

        if (units.Length > 0)
        {
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

    private void UnitGenerator()
    {

        if (_textAsset != null)
        {
            var jsonData = JsonMapper.ToObject(_textAsset.ToString())["data"];

            if (jsonData.IsArray)
            {
                for (int i = 0; i < jsonData.Count; i++)
                {
                    var jData = jsonData[i];
                    var key = jData["Key"].ToString();
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (!DataStorage.Instance.IsHasData<UnitData>(key))
                        {
                            var data = UnitData.Create(jData);
                            Debug.Log($"CreateData {data.Key}");
                        }
                        else
                        {
                            var data = DataStorage.Instance.GetDataOrNull<UnitData>(key);
                            if (data != null)
                            {
                                data.SetData(jData);
                                Debug.Log($"RefreshData {data.Key}");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"UnitData TextAsset을 찾을 수 없습니다 : {_path}");
        }
    }
}

#endif