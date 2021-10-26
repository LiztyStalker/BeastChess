using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIComment : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [System.Flags]
    public enum TYPE_COMMENT { 
        Click = 1, 
        OnPointer = 2
    }

    [SerializeField]
    private string _key;

    [SerializeField]
    private TYPE_COMMENT _typeComment;

    [SerializeField]
    private float _commentTime = 0.5f;

    private bool isOn = false;
    private float _nowTime = 0f;

    private UICommentInformation _ui;

    private Vector2 _screenPosition;

    private UICommentInformation ui
    {
        get
        {
            if(_ui == null)
                _ui = UICommon.Current.GetUICommon<UICommentInformation>();
            return _ui;
        }
    }

    private void Update()
    {
        if (isOn)
        {
            _nowTime += Time.deltaTime;
            if(_nowTime > _commentTime)
            {
                ShowTextDescription(_key, _screenPosition);
                isOn = false;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((_typeComment & TYPE_COMMENT.Click) == _typeComment)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _screenPosition = eventData.position;
                ShowTextDescription(_key, _screenPosition);
            }
        }
    }       

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((_typeComment & TYPE_COMMENT.OnPointer) == _typeComment)
        {
            isOn = true;
            _nowTime = 0f;
            _screenPosition = eventData.position;
        }
    }

    private void ShowTextDescription(string key, Vector2 screenPosition)
    {       
        ui.Show(key, screenPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOn) isOn = false;
    }
}
