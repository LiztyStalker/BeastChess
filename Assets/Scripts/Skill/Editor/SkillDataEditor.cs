#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SkillData))]
[CanEditMultipleObjects]
public class SkillDataEditor : Editor
{

    SkillData data;

    ReorderableList _list;

    SerializedProperty _property;

    private void OnEnable()
    {

        data = target as SkillData;

        if (_list == null)
        {
            _property = serializedObject.FindProperty("_editorStateList");
            _list = new ReorderableList(serializedObject, _property, true, false, true, true);
            _list.drawElementCallback += DrawElement;
            _list.onAddCallback += AddElement;
            _list.onRemoveCallback += RemoveElement;
            _list.elementHeightCallback += ElementHeight;
            //_list.drawNoneElementCallback = (rect) =>
            //{
            //    EditorGUI.LabelField(rect, "Empty");
            //};
        }
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, _property.GetArrayElementAtIndex(index), true);
    }

    private void AddMenu(GenericMenu menu, string path, GenericMenu.MenuFunction act)
    {
        menu.AddItem(new GUIContent(path), false, act);
    }

    private float ElementHeight(int index)
    {
        return EditorGUIUtility.singleLineHeight * 3f;
    }

    private void AddElement(ReorderableList list)
    {
        GenericMenu menu = new GenericMenu();
        AddMenu(menu, typeof(StateValueAttack).ToString(), delegate { AddState(new StateSerializable(typeof(StateValueAttack))); });
        menu.AddSeparator("");
        //AddMenu(menu, typeof(StateValueAttack).ToString());
        menu.ShowAsContext();
    }


    private void AddState(StateSerializable state)
    {
        data.AddState(state);
    }

    private void RemoveState(int index)
    {
        data.RemoveAt(index);
    }


    private void RemoveElement(ReorderableList list)
    {
        RemoveState(list.index);
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawSkillData();


        //_property = serializedObject.FindProperty("_stateList");

        //for(int i = 0; i < _property.arraySize; i++)
        //{
        //    var prop = _property.GetArrayElementAtIndex(i);
        //    EditorGUILayout.PropertyField(prop, true);
        //}
        

        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(data);
    }

    private void DrawSkillData()
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillRangeValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeMine"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeTargetTeam"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillActivateRate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_turnCount"));
    }

}

#endif