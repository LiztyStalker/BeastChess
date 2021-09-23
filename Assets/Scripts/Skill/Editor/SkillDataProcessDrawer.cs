#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(SkillData.SkillDataProcess))]
[CanEditMultipleObjects]
public class SkillDataProcessDrawer : PropertyDrawer
{

    SerializedProperty castEffectDataProp;
    SerializedProperty typeUsedBulletDataProp;
    SerializedProperty bulletDataProp;
    SerializedProperty bulletTargetDataProp;
    SerializedProperty typeUsedHealthProp;
    SerializedProperty increaseNowHealthTargetDataProp;
    SerializedProperty increaseNowHealthValueProp;
    SerializedProperty typeUsedStatusDataProp;
    SerializedProperty statusTargetDataProp;
    SerializedProperty statusDataProp;


    StatusDataDrawer statusDataDrawer = new StatusDataDrawer();

    SerializedProperty _statusDataProperty;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        var nowProperty = property.FindPropertyRelative("_statusData");
        if (_statusDataProperty != nowProperty)
        {
            if (nowProperty.objectReferenceValue != null)
            {
                var statusData = nowProperty.objectReferenceValue as StatusData;
                statusDataDrawer.Initialize(new SerializedObject(statusData), statusData);
                _statusDataProperty = nowProperty;
            }
            else
            {
                statusDataDrawer.Clear();
            }
        }

        castEffectDataProp = property.FindPropertyRelative("_castEffectData");

        typeUsedBulletDataProp = property.FindPropertyRelative("_typeUsedBulletData");
        bulletDataProp = property.FindPropertyRelative("_bulletData");
        bulletTargetDataProp = property.FindPropertyRelative("_bulletTargetData");

        typeUsedHealthProp = property.FindPropertyRelative("_typeUsedHealth");
        increaseNowHealthTargetDataProp = property.FindPropertyRelative("_increaseNowHealthTargetData");
        increaseNowHealthValueProp = property.FindPropertyRelative("_increaseNowHealthValue");

        typeUsedStatusDataProp = property.FindPropertyRelative("_typeUsedStatusData");
        statusTargetDataProp = property.FindPropertyRelative("_statusTargetData");
        statusDataProp = property.FindPropertyRelative("_statusData");


        var totalHeight = 0f;
        totalHeight += EditorGUI.GetPropertyHeight(castEffectDataProp, true);

        totalHeight += EditorGUI.GetPropertyHeight(typeUsedBulletDataProp, true);
        if (typeUsedBulletDataProp.boolValue)
        {
            totalHeight += EditorGUI.GetPropertyHeight(bulletDataProp, true);
            totalHeight += EditorGUI.GetPropertyHeight(bulletTargetDataProp, true);
        }

        totalHeight += EditorGUI.GetPropertyHeight(typeUsedHealthProp, true);
        if (typeUsedHealthProp.boolValue)
        {
            totalHeight += EditorGUI.GetPropertyHeight(increaseNowHealthTargetDataProp, true);
            totalHeight += EditorGUI.GetPropertyHeight(increaseNowHealthValueProp, true);
        }

        totalHeight += EditorGUI.GetPropertyHeight(typeUsedStatusDataProp, true);
        if (typeUsedStatusDataProp.boolValue)
        {
            totalHeight += EditorGUI.GetPropertyHeight(statusTargetDataProp, true);
            totalHeight += EditorGUI.GetPropertyHeight(statusDataProp, true);
        }
        return totalHeight;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {      

        EditorGUI.BeginProperty(position, label, property);

        position.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(position, castEffectDataProp, true);
        position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(castEffectDataProp));

        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(position, typeUsedBulletDataProp, true);
        position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(typeUsedBulletDataProp));

        if (typeUsedBulletDataProp.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(position, bulletDataProp, true);
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(bulletDataProp));

            EditorGUI.PropertyField(position, bulletTargetDataProp, true);
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(bulletTargetDataProp));

            EditorGUI.indentLevel--;

        }


        EditorGUI.PropertyField(position, typeUsedHealthProp, true);
        position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(typeUsedHealthProp));


        if (typeUsedHealthProp.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(position, increaseNowHealthValueProp, true);
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(increaseNowHealthValueProp));


            EditorGUI.PropertyField(position, increaseNowHealthTargetDataProp, true);
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(increaseNowHealthTargetDataProp));

            EditorGUI.indentLevel--;
        }

        EditorGUI.PropertyField(position, typeUsedStatusDataProp, true);
        position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(typeUsedStatusDataProp));

        if (typeUsedStatusDataProp.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(position, statusDataProp, true);
            position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(statusDataProp));

            EditorGUI.PropertyField(position, statusTargetDataProp, true);
            //position = PropertyDrawerExtend.AddAxisY(position, EditorGUI.GetPropertyHeight(statusTargetDataProp));

            EditorGUI.indentLevel--;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

}

#endif