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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"), new GUIContent("설명"));
        EditorGUILayout.EndVertical();



        EditorGUILayout.BeginVertical("GroupBox");

        var typeSkillActivate = serializedObject.FindProperty("_typeSkillCast");
        EditorGUILayout.PropertyField(typeSkillActivate, new GUIContent("발동 조건"));
        if (typeSkillActivate.enumValueIndex == (int)TYPE_SKILL_CAST.AttackCast)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillCastRate"));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillDataProcess"));
        EditorGUILayout.EndVertical();
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetData"));

        //var typeSkillLifeSpanProp = serializedObject.FindProperty("_typeSkillLifeSpan");
        //EditorGUILayout.PropertyField(typeSkillLifeSpanProp);

        //if(typeSkillLifeSpanProp.enumValueIndex == (int)TYPE_STATUS_LIFE_SPAN.Turn)
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("_turnCount"));


        //var isOverlapProp = serializedObject.FindProperty("_isOverlapped");
        //EditorGUILayout.PropertyField(isOverlapProp);
        //if(isOverlapProp.boolValue)
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("_overlapCount"));

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeTargetTeam"));

        //var rangeProp = serializedObject.FindProperty("_typeSkillRange");
        //EditorGUILayout.PropertyField(rangeProp);


        //if (rangeProp.enumValueIndex != (int)TYPE_SKILL_RANGE.All)
        //{
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("_skillRangeValue"));
        //    if (rangeProp.enumValueIndex == (int)TYPE_SKILL_RANGE.MyselfRange)
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isMyself"));
        //    else if (rangeProp.enumValueIndex == (int)TYPE_SKILL_RANGE.UnitGroupRange)
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeUnitGroup"));
        //    else if (rangeProp.enumValueIndex == (int)TYPE_SKILL_RANGE.UnitClassRange)
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeUnitClass"));
        //}


        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeTargetSkillRange"));
        //EditorGUILayout.Space();

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_effectData"), new GUIContent("이펙트데이터"));


        //var isIncreaseNowHealthValue = serializedObject.FindProperty("_isIncreaseNowHealthValue");
        //EditorGUILayout.PropertyField(isIncreaseNowHealthValue, new GUIContent("체력증감여부"));
        //if(isIncreaseNowHealthValue.boolValue)
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("_increaseNowHealthValue"), new GUIContent("체력증감"));

        //EditorGUILayout.EndVertical();

        //EditorGUILayout.BeginVertical("GroupBox");
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_statusData"), true);
        //statusDataDrawer.OnDraw(1);
        //EditorGUILayout.EndVertical();

    }

}

#endif