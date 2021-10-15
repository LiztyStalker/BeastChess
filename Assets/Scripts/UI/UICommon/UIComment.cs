using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIComment : MonoBehaviour, IPointerClickHandler 
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
        var ui = UICommon.Current.GetUICommon<UICommentInformation>();
        ui.Show(key, screenPosition);
    }  

}
