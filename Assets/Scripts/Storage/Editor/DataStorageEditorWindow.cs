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

        ShowLayout(DataStorage.Instance.GetAllUnitData());
        ShowLayout(DataStorage.Instance.GetAllCommanderData());
        ShowLayout(DataStorage.Instance.GetAllBattleFieldData());

        GUI.enabled = true;


        if (GUILayout.Button("Dispose And Refresh"))
        {
            DataStorage.Dispose();
        }

    }


    private void ShowLayout<T>(T[] arr) where T : Object
    {
        for (int i = 0; i < arr.Length; i++)
        {
            EditorGUILayout.ObjectField(arr[i], typeof(T), true);
        }
    }

}

#endif