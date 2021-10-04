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

    [MenuItem("Window/Generator/Unit Generator")]
    private static void Init()
    {
        UnitGeneratorEditor gen = (UnitGeneratorEditor)GetWindow(typeof(UnitGeneratorEditor));
        gen.Show();
        


    }

    private void OnGUI()
    {
        
        GUILayout.Label("UnitData Asset", EditorStyles.boldLabel);

        _textAsset = DataStorage.Instance.GetDataOrNull<TextAsset>("UnitData", null, null);
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
            var jsonData = JsonMapper.ToObject(_textAsset.text);

            foreach(var key in jsonData.Keys)
            {
                var jData = jsonData[key];
                if (!DataStorage.Instance.IsHasData<UnitData>(key))
                {
                    var data = UnitData.Create(key, jData);
                    //Debug.Log($"CreateData {data.Key}");
                }
                else
                {
                    var data = DataStorage.Instance.GetDataOrNull<UnitData>(key);
                    if (data != null)
                    {
                        data.SetData(key, jData);
                        //Debug.Log($"RefreshData {data.Key}");
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