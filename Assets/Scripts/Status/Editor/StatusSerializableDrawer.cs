#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatusSerializable))]
[CanEditMultipleObjects]
public class StatusSerializableDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = 0f;

        totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_typeStateClass"));

        var typeStatusData = property.FindPropertyRelative("_typeStatusData");

        if (typeStatusData.enumValueIndex == (int)StatusSerializable.TYPE_STATUS_DATA.Value)
        {
            totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_typeValue"));
            totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"));
        }

        //        var typeStatusData = property.FindPropertyRelative("_typeStatusData");
        //        var typeStateHealth = property.FindPropertyRelative("_typeStateHealth");
        //        var turnCount = property.FindPropertyRelative("_turnCount");

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var nameProp = property.FindPropertyRelative("_typeStateClass");
        var stateProp = property.FindPropertyRelative("_typeValue");
        var valueProp = property.FindPropertyRelative("_value");

        var typeStatusData = property.FindPropertyRelative("_typeStatusData");
        var typeStateHealth = property.FindPropertyRelative("_typeStateHealth");
        var turnCount = property.FindPropertyRelative("_turnCount");


        EditorGUI.BeginProperty(position, label, property);
        position.height = EditorGUIUtility.singleLineHeight;


        EditorGUI.LabelField(position, nameProp.stringValue);        


        if (typeStatusData.enumValueIndex == (int)StatusSerializable.TYPE_STATUS_DATA.Value)
        {
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(nameProp));
            EditorGUI.PropertyField(position, stateProp);

            //if (typeStatusData.boolValue)
            //{
            //    position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(nameProp));
            //    EditorGUI.PropertyField(position, typeStateHealth);

            //    if (typeStateHealth.enumValueIndex == (int)StatusValue.TYPE_STATE_HEALTH.Turn)
            //    {
            //        EditorGUI.PropertyField(position, turnCount);
            //    }

            //}

            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(stateProp));
            EditorGUI.PropertyField(position, valueProp);
        }
        else if(typeStatusData.enumValueIndex == (int)StatusSerializable.TYPE_STATUS_DATA.Effect)
        {

        }


        EditorGUI.EndProperty();
    }

  
}
#endif