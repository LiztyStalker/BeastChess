using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextPanel : MonoBehaviour
{
    [SerializeField]
    Text text;

    [SerializeField]
    RectTransform rect;

    public void Initialize ()
    {
        Close();
    }

    public void ShowText(string str)
    {
        text.text = str;
        gameObject.SetActive(true);
        rect.transform.position = Input.mousePosition;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


}
