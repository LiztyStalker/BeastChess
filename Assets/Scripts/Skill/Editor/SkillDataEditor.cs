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

    //StatusDataDrawer statusDataDrawer = new StatusDataDrawer();

    //SerializedProperty _statusDataProperty;
        
    //private void Initialize()
    //{
    //    var property = serializedObject.FindProperty("_statusData");
    //    if (_statusDataProperty != property)
    //    {
    //        if (property.objectReferenceValue != null)
    //        {
    //            var statusData = property.objectReferenceValue as StatusData;
    //            statusDataDrawer.Initialize(new SerializedObject(statusData), statusData);
    //            _statusDataProperty = property;
    //        }
    //        else
    //        {
    //            statusDataDrawer.Clear();
    //        }
    //    }
    //}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //Initialize();
        DrawSkillData();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSkillData()
    {

        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_icon"), new GUIContent("아이콘"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"), new GUIContent("설명"));
        EditorGUILayout.EndVertical();



        EditorGUILayout.BeginVertical("GroupBox");

        var typeSkillActivate = serializedObject.FindProperty("_typeSkillCast");
        EditorGUILayout.PropertyField(typeSkillActivate, new GUIContent("발동 조건"));
        if (typeSkillActivate.enumValueIndex == (int)TYPE_SKILL_CAST.AttackCast || typeSkillActivate.enumValueIndex == (int)TYPE_SKILL_CAST.AttackedCast)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillCastRate"));


        var typeSkillCondition = serializedObject.FindProperty("_typeSkillCastCondition");
        EditorGUILayout.PropertyField(typeSkillCondition, new GUIContent("스킬 조건"));
        if(typeSkillCondition.enumValueIndex != (int)TYPE_SKILL_CAST_CONDITION.None)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillCastConditionCompare"), new GUIContent("비교타입"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillCastConditionCompareValue"), new GUIContent("비교값타입"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_conditionValue"), new GUIContent("비교값"));
            EditorGUI.indentLevel--;

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillDataProcess"));
        EditorGUILayout.EndVertical();
       

    }

}

#endif