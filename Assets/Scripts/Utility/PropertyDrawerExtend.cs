#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PropertyDrawerExtend
{

    public static Rect AddAxisY(Rect position)
    {
        position.y += EditorGUIUtility.singleLineHeight;
        return position;
    }

    public static Rect AddAxisY(Rect position, float height)
    {
        position.y += height;
        return position;
    }
}
#endif