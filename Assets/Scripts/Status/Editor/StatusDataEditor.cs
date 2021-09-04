#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


public class StatusDataDrawer
{
    private ReorderableList _list;

    private SerializedProperty _property;

    private SerializedObject _serializedObject;

    private StatusData _statusData;

    public void Initialize(SerializedObject serializedObject, StatusData statusData)
    {
        _statusData = statusData;
        if (_serializedObject != serializedObject)
        {
            _serializedObject = serializedObject;
            _property = _serializedObject.FindProperty("_editorStatusList");
            _list = new ReorderableList(serializedObject, _property, true, false, true, true);
            _list.drawElementCallback = DrawElement;
            _list.onAddCallback = AddElement;
            _list.onRemoveCallback = RemoveElement;
            _list.elementHeightCallback = ElementHeight;
        }
    }
    
    public void Clear()
    {
        _serializedObject = null;
        _property = null;
        _statusData = null;
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
        var element = _property.GetArrayElementAtIndex(index);
        var isExtend = element.FindPropertyRelative("_isExtend");
        return EditorGUIUtility.singleLineHeight * (3f + ((isExtend.boolValue) ? 2f : 0f));
    }

    private void AddElement(ReorderableList list)
    {
        GenericMenu menu = new GenericMenu();
        AddMenu(menu, typeof(StatusValueAttack).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueAttack))); });
        AddMenu(menu, typeof(StatusValueHit).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueHit), true)); });
        AddMenu(menu, typeof(StatusValueRecovery).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueRecovery), true)); });
        menu.AddSeparator("");
        //        AddMenu(menu, typeof(StateValueAttack).ToString());
        menu.ShowAsContext();
    }


    private void AddState(StatusSerializable state)
    {
        _statusData.AddState(state);
    }

    private void RemoveState(int index)
    {
        _statusData.RemoveAt(index);
    }

    private void RemoveElement(ReorderableList list)
    {
        RemoveState(list.index);
    }

    public void OnDraw(int addIndentLevel = 0)
    {
        if (_serializedObject != null && _list != null)
        {
            EditorGUI.indentLevel += addIndentLevel;
            _serializedObject.Update();
            EditorGUILayout.PropertyField(_serializedObject.FindProperty("_icon"));
            _list.DoLayoutList();
            _serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel -= addIndentLevel;
        }
    }

    public Object GetObjectReferenceValue()
    {
        return _serializedObject.targetObject;
    }

    #region ##### Listener #####

    #endregion
}


[CustomEditor(typeof(StatusData))]
[CanEditMultipleObjects]
public class StatusDataEditor : Editor
{
    StatusDataDrawer statusDataDrawer = new StatusDataDrawer();

    private void OnEnable()
    {
        statusDataDrawer.Initialize(serializedObject, target as StatusData);
    }
    public override void OnInspectorGUI()
    {
        statusDataDrawer.OnDraw();
    }
}

#endif