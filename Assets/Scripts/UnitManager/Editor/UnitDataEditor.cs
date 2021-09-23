#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitData))]
public class UnitDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeUnit"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeUnitGroup"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeUnitClass"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skeletonDataAsset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_squadCount"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_healthValue"));

        var isAttackProp = serializedObject.FindProperty("_isAttack");
        EditorGUILayout.PropertyField(isAttackProp);
        if (isAttackProp.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_bulletData"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_damageValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackCount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetData"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackClip"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_proficiencyValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_movementValue"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("_typeMovement"));


        EditorGUILayout.PropertyField(serializedObject.FindProperty("_skills"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_priorityValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_employCostValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_maintenanceCostValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_deadClip"));

        serializedObject.ApplyModifiedProperties();
    }
}

#endif