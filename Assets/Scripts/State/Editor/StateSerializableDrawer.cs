#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StateSerializable))]
[CanEditMultipleObjects]
public class StateSerializableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight * 3f;

        var nameProp = property.FindPropertyRelative("_typeStateClass");
        var stateProp = property.FindPropertyRelative("_typeValue");
        var valueProp = property.FindPropertyRelative("_value");


        EditorGUI.BeginProperty(position, label, property);


        position.height /= 3f;

        EditorGUI.LabelField(position, nameProp.stringValue);
        
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, stateProp);

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