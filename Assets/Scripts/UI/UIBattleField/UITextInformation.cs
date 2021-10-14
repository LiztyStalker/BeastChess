using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UITextInformation : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField]
    private string _key;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ShowTextDescription(_key, eventData.position);
        }       
    }

    private void ShowTextDescription(string key, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UITextDescription>();
        ui.Show(key, screenPosition);
    }  

}
