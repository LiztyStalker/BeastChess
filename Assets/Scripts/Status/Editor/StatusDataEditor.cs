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
        AddMenu(menu, typeof(StatusValueDefensive).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueDefensive))); });
        AddMenu(menu, typeof(StatusValueProficiency).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueProficiency))); });
        AddMenu(menu, typeof(StatusValueCounter).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueCounter))); });
        AddMenu(menu, typeof(StatusValueRevCounter).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueRevCounter))); });
        AddMenu(menu, typeof(StatusValueAttackCount).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueAttackCount))); });
        AddMenu(menu, typeof(StatusValuePriority).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValuePriority))); });
//        AddMenu(menu, typeof(StatusValueHit).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueHit), true)); });
//        AddMenu(menu, typeof(StatusValueRecovery).ToString(), delegate { AddState(new StatusSerializable(typeof(StatusValueRecovery), true)); });
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

            var typeStatusLifeSpanProp = _serializedObject.FindProperty("_typeStatusLifeSpan");          
            EditorGUILayout.PropertyField(typeStatusLifeSpanProp);
            if(typeStatusLifeSpanProp.enumValueIndex == (int)StatusData.TYPE_STATUS_LIFE_SPAN.Turn)
                EditorGUILayout.PropertyField(_serializedObject.FindProperty("_turnCount"));

            var isOverlappedProp = _serializedObject.FindProperty("_isOverlapped");            
            EditorGUILayout.PropertyField(isOverlappedProp);
            if(isOverlappedProp.boolValue)
                EditorGUILayout.PropertyField(_serializedObject.FindProperty("_overlapCount"));

            _list.DoLayoutList();
            _serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel -= addIndentLevel;
        }
    }

    public Object GetObjectReferenceValue()
    {
        return _serializedObject.targetObject;
    }
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
        serializedObject.Update();
        statusDataDrawer.OnDraw();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif