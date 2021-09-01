#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatusSerializable))]
[CanEditMultipleObjects]
public class StatusSerializableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var nameProp = property.FindPropertyRelative("_typeStateClass");
        var stateProp = property.FindPropertyRelative("_typeValue");
        var valueProp = property.FindPropertyRelative("_value");

        var isExtend = property.FindPropertyRelative("_isExtend");
        var typeStateHealth = property.FindPropertyRelative("_typeStateHealth");
        var turnCount = property.FindPropertyRelative("_turnCount");

        var height = 3f + ((isExtend.boolValue) ? 1f : 0f);
        if (typeStateHealth.enumValueIndex == (int)Status.TYPE_STATE_HEALTH.Turn)
            height += 1f;

        position.height = EditorGUIUtility.singleLineHeight * height;

        EditorGUI.BeginProperty(position, label, property);


        position.height /= height;

        EditorGUI.LabelField(position, nameProp.stringValue);
        
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, stateProp);

        if (isExtend.boolValue)
        {
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, typeStateHealth);

            if (typeStateHealth.enumValueIndex == (int)Status.TYPE_STATE_HEALTH.Turn)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, turnCount);
            }

        }

        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, valueProp);


        EditorGUI.EndProperty();
    }

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return EditorGUIUtility.singleLineHeight * 2f;
    //}
}
#endif