#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class UnitGeneratorEditor : EditorWindow
{

    [MenuItem("Window/Generator/Unit Generator")]
    private static void Init()
    {
        UnitGeneratorEditor gen = (UnitGeneratorEditor)GetWindow(typeof(UnitGeneratorEditor));
        gen.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("UnitList", EditorStyles.boldLabel);

        ShowUnits();
        
        GUILayout.Space(20f);

        

        if (GUILayout.Button("Unit Generator"))
        {
            UnitGenerator();
        }
    }


    private void ShowUnits()
    {
        var _units = Resources.LoadAll<UnitData>("Units");

        if (_units.Length > 0)
        {
            EditorGUI.indentLevel++;
            GUI.enabled = false;
            for (int i = 0; i < _units.Length; i++)
            {
                _units[i] = (UnitData)EditorGUILayout.ObjectField(_units[i], typeof(UnitData), true);
            }
            GUI.enabled = true;
            EditorGUI.indentLevel--;
        }
    }

    private void UnitGenerator()
    {
        var textAsset = System.IO.File.ReadAllText($"{ Application.dataPath}/TextAssets/UnitData.json");
        var jsonData = JsonMapper.ToObject(textAsset);

        if (jsonData.IsArray) {
            for (int i = 0; i < jsonData.Count; i++)
            {
                var jData = jsonData[i];
                var key = jData["Key"].ToString();
                if (!DataStorage.Instance.IsHasData<UnitData>(key))
                {
                    UnitData.Create(jData);
                }
                else
                {
                    var data = DataStorage.Instance.GetDataOrNull<UnitData>(key);
                    data.SetData(jData);
                }
            }
        }
    }
}

#endif