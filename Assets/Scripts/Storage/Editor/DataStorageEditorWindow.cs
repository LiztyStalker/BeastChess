#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataStorageEditorWindow : EditorWindow
{
    private Vector2 _scrollPos;

    [MenuItem("Window/Show DataStorage")]
    public static void Init()
    {
        var storageWin = (DataStorageEditorWindow)GetWindow(typeof(DataStorageEditorWindow));
        storageWin.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Data Storage", EditorStyles.boldLabel);

        if (GUILayout.Button("Dispose And Refresh"))
        {
            DataStorage.Dispose();
        }
        GUILayout.Space(20f);

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        GUI.enabled = false;

        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<UnitData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<Spine.Unity.SkeletonDataAsset>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<CommanderData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<BattleFieldData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<BulletData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<EffectData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<SkillData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<StatusData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<TribeData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<AudioClip>());

        GUI.enabled = true;

        GUILayout.EndScrollView();



    }


    private void ShowLayout<T>(T[] arr) where T : Object
    {
        GUILayout.Label(typeof(T).ToString(), EditorStyles.boldLabel);
        if (arr != null)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < arr.Length; i++)
            {
                EditorGUILayout.ObjectField(arr[i], typeof(T), true);
            }
            EditorGUI.indentLevel--;
        }
        GUILayout.Space(20f);
    }

}

#endif