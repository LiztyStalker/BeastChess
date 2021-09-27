#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    }
}

#endif