#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataStorageEditorWindow : EditorWindow
{
    [MenuItem("Window/Show DataStorage")]
    public static void Init()
    {
        var storageWin = (DataStorageEditorWindow)GetWindow(typeof(DataStorageEditorWindow));
        storageWin.Show();
    }

    private void OnGUI()
    {
        //����
        //��������Ʈ
        //UI
        //����
        //����
        //��ų
        //�����̻�        


        GUI.enabled = false;

        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<UnitData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<CommanderData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<BattleFieldData>());
        ShowLayout(DataStorage.Instance.GetAllDatasOrZero<Spine.Unity.SkeletonDataAsset>());

        GUI.enabled = true;


        if (GUILayout.Button("Dispose And Refresh"))
        {
            DataStorage.Dispose();
        }

    }


    private void ShowLayout<T>(T[] arr) where T : Object
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                EditorGUILayout.ObjectField(arr[i], typeof(T), true);
            }
        }
    }

}

#endif