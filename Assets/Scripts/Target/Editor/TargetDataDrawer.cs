#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TargetData))]
[CanEditMultipleObjects]
public class TargetDataDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeTargetTeamProp = property.FindPropertyRelative("_typeTargetTeam");
        var isMyselfProp = property.FindPropertyRelative("_isMyself");
        var typeTargetRangeProp = property.FindPropertyRelative("_typeTargetRange");
        var targetStartRangeProp = property.FindPropertyRelative("_targetStartRange");
        var targetRangeProp = property.FindPropertyRelative("_targetRange");
        var typeTargetPriority = property.FindPropertyRelative("_typeTargetPriority");
        var isTargetCountProp = property.FindPropertyRelative("_isTargetCount");
        var targetCountProp = property.FindPropertyRelative("_targetCount");

        var isEnemy = (typeTargetTeamProp.enumValueIndex == (int)TYPE_TARGET_TEAM.Enemy);
        var isTargetCount = (isTargetCountProp.boolValue);

        var count = 6f;
        count += (isEnemy) ? 0 : 1;
        count += (isTargetCount) ? 1 : 0;

        return count * EditorGUIUtility.singleLineHeight;
    }





    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var typeTargetTeamProp = property.FindPropertyRelative("_typeTargetTeam");        
        var isMyselfProp = property.FindPropertyRelative("_isMyself");
        var typeTargetRangeProp = property.FindPropertyRelative("_typeTargetRange");
        var targetStartRangeProp = property.FindPropertyRelative("_targetStartRange");
        var targetRangeProp = property.FindPropertyRelative("_targetRange");
        var typeTargetPriority = property.FindPropertyRelative("_typeTargetPriority");
        var isTargetCountProp = property.FindPropertyRelative("_isTargetCount");
        var targetCountProp = property.FindPropertyRelative("_targetCount");

        var isEnemy = (typeTargetTeamProp.enumValueIndex == (int)TYPE_TARGET_TEAM.Enemy);
        var isTargetCount = (isTargetCountProp.boolValue);

        var count = 6f;
        count += (isEnemy) ? 0 : 1;
        count += (isTargetCount) ? 1 : 0;

        position.height = count * EditorGUIUtility.singleLineHeight;

        using (new EditorGUI.PropertyScope(position, label, property))
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);
            

            position.height /= count;



            EditorGUI.PropertyField(position, typeTargetTeamProp, new GUIContent("목표"));

            if (!isEnemy)
            {
                position = AddHeight(position);
                EditorGUI.PropertyField(position, isMyselfProp, new GUIContent("자신포함"));
            }

            position = AddHeight(position);
            EditorGUI.PropertyField(position, typeTargetRangeProp, new GUIContent("목표범위"));

            position = AddHeight(position);
            EditorGUI.PropertyField(position, targetStartRangeProp, new GUIContent("목표시작지점"));

            position = AddHeight(position);
            EditorGUI.PropertyField(position, targetRangeProp, new GUIContent("목표범위값"));

            position = AddHeight(position);
            EditorGUI.PropertyField(position, typeTargetPriority, new GUIContent("우선순위"));

            position = AddHeight(position);
            EditorGUI.PropertyField(position, isTargetCountProp, new GUIContent("목표 수 여부"));

            if (isTargetCount)
            {
                position = AddHeight(position);
                EditorGUI.PropertyField(position, targetCountProp, new GUIContent("목표 수"));
            }

            EditorGUI.EndProperty();
            EditorGUI.indentLevel--;
        }
    }

    private Rect AddHeight(Rect position) { 
        position.y += EditorGUIUtility.singleLineHeight;
        return position;
    }
}
#endif