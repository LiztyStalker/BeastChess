#if UNITY_EDITOR
namespace UnityEngine.Test
{
    using UnityEngine;

    public class IMGUIDrawer
    {
        private static Vector2 scrollPosition;
        public static void CreateGUI(float x, float y, float width, float height, System.Action act)
        {
            GUILayout.BeginArea(new Rect(x, y, width, height));
            GUILayout.BeginVertical();
            act?.Invoke();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        public static void CreateScrollVertical(System.Action act)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            act?.Invoke();
            GUILayout.EndScrollView();
        }

        public static void GUISubBlock(System.Action action, float space = 40f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            GUILayout.BeginVertical();
            action?.Invoke();
            GUILayout.EndVertical();
            GUILayout.Space(space);
            GUILayout.EndHorizontal();
        }
        

        //https://docs.unity3d.com/kr/2019.4/Manual/gui-Extending.html
        public static float LabelSlider(float sliderValue, float sliderMaxValue, string labelText)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(labelText, GUILayout.Width(40f));
            sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, sliderMaxValue);
            GUILayout.Label(string.Format("{0:f0}", sliderValue * 255f), GUILayout.Width(60f));
            GUILayout.EndHorizontal();
            return sliderValue;
        }



    }
}
#endif