#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TargetData))]
[CanEditMultipleObjects]
public class TargetDataDrawer : PropertyDrawer
{
    private const float HEIGHT_LENGTH = 7f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeTargetTeamProp = property.FindPropertyRelative("_typeTargetTeam");
        var isTargetCountProp = property.FindPropertyRelative("_isTargetCount");
        var isAllTargetRangeProp = property.FindPropertyRelative("_isAllTargetRange");

        var isEnemy = (typeTargetTeamProp.enumValueIndex == (int)TYPE_TARGET_TEAM.Enemy);
        var isTargetCount = (isTargetCountProp.boolValue);

        var count = HEIGHT_LENGTH;
        count += (isEnemy) ? 0 : 1;
        count += (isTargetCount) ? 1 : 0;

        return count * EditorGUIUtility.singleLineHeight;
    }





    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var typeTargetTeamProp = property.FindPropertyRelative("_typeTargetTeam");
        var isAlwaysTargetEnemyProp = property.FindPropertyRelative("_isAlwaysTargetEnemy");
        var isAllTargetRangeProp = property.FindPropertyRelative("_isAllTargetRange");
        var typeTargetRangeProp = property.FindPropertyRelative("_typeTargetRange");
        var isMyselfProp = property.FindPropertyRelative("_isMyself");
        var targetStartRangeProp = property.FindPropertyRelative("_targetStartRange");
        var targetRangeProp = property.FindPropertyRelative("_targetRange");
        var typeTargetPriority = property.FindPropertyRelative("_typeTargetPriority");
        var isTargetCountProp = property.FindPropertyRelative("_isTargetCount");
        var targetCountProp = property.FindPropertyRelative("_targetCount");

        var isEnemy = (typeTargetTeamProp.enumValueIndex == (int)TYPE_TARGET_TEAM.Enemy);
        var isTargetCount = (isTargetCountProp.boolValue);

        var count = HEIGHT_LENGTH;
        count += (isEnemy) ? 0 : 1;
        count += (isTargetCount) ? 1 : 0;

        position.height = count * EditorGUIUtility.singleLineHeight;

        using (new EditorGUI.PropertyScope(position, label, property))
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);


            position.height /= count;

            EditorGUI.PropertyField(position, isAllTargetRangeProp, new GUIContent("����ü"));

            if (isAlwaysTargetEnemyProp.boolValue)
            {
                GUI.enabled = false;
                position = PropertyDrawerExtend.AddAxisY(position);
                EditorGUI.PropertyField(position, typeTargetTeamProp, new GUIContent("��ǥ��"));
                GUI.enabled = true;
            }
            else
            {
                position = PropertyDrawerExtend.AddAxisY(position);
                EditorGUI.PropertyField(position, typeTargetTeamProp, new GUIContent("��ǥ��"));
            }

            if (!isEnemy)
            {
                position = PropertyDrawerExtend.AddAxisY(position);
                EditorGUI.PropertyField(position, isMyselfProp, new GUIContent("�ڽ�����"));
            }

            position = PropertyDrawerExtend.AddAxisY(position);
            EditorGUI.PropertyField(position, typeTargetRangeProp, new GUIContent("��ǥ����"));

            position = PropertyDrawerExtend.AddAxisY(position);
            EditorGUI.PropertyField(position, targetStartRangeProp, new GUIContent("��ǥ��������"));

            position = PropertyDrawerExtend.AddAxisY(position);
            EditorGUI.PropertyField(position, targetRangeProp, new GUIContent("��ǥ������"));

            position = PropertyDrawerExtend.AddAxisY(position);
            EditorGUI.PropertyField(position, typeTargetPriority, new GUIContent("�켱����"));

            position = PropertyDrawerExtend.AddAxisY(position);
            EditorGUI.PropertyField(position, isTargetCountProp, new GUIContent("��ǥ �� ����"));

            if (isTargetCount)
            {
                position = PropertyDrawerExtend.AddAxisY(position);
                EditorGUI.PropertyField(position, targetCountProp, new GUIContent("��ǥ ��"));
            }

            EditorGUI.EndProperty();
            EditorGUI.indentLevel--;
        }
    }
}
#endif