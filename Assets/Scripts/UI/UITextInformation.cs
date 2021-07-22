using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITextInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string str;

    bool isPress = false;
    float pressTime = 0f;

    void Update()
    {
        if (isPress)
        {
            pressTime += Time.deltaTime;
            if(pressTime > 1f)
            {
                showEvent?.Invoke(str);
                pressTime = 0f;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPress = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPress = false;
    }

    System.Action<string> showEvent;

    public void SetOnShowListener(System.Action<string> listener) => showEvent = listener;

}
