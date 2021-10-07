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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_icon"), new GUIContent("������"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"), new GUIContent("����"));
        EditorGUILayout.EndVertical();



        EditorGUILayout.BeginVertical("GroupBox");

        var typeSkillActivate = serializedObject.FindProperty("_typeSkillCast");
        EditorGUILayout.PropertyField(typeSkillActivate, new GUIContent("�ߵ� ����"));
        if (typeSkillActivate.enumValueIndex == (int)TYPE_SKILL_CAST.AttackCast || typeSkillActivate.enumValueIndex == (int)TYPE_SKILL_CAST.AttackedCast)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillCastRate"));


        var typeSkillCondition = serializedObject.FindProperty("_typeSkillCastCondition");
        EditorGUILayout.PropertyField(typeSkillCondition, new GUIContent("��ų ����"));
        if(typeSkillCondition.enumValueIndex != (int)TYPE_SKILL_CAST_CONDITION.None)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillCastConditionCompare"), new GUIContent("��Ÿ��"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeSkillCastConditionCompareValue"), new GUIContent("�񱳰�Ÿ��"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_conditionValue"), new GUIContent("�񱳰�"));
            EditorGUI.indentLevel--;

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillDataProcess"));
        EditorGUILayout.EndVertical();
       

    }

}

#endif