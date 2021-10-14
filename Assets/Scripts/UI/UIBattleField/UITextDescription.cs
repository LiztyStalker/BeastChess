using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITextDescription : MonoBehaviour, ICanvas, IPointerClickHandler
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private RectTransform _rect;

    [SerializeField]
    private Button _exitBtn;

    public void Initialize ()
    {
        _exitBtn.onClick.AddListener(OnExitClicked);
        Hide();
    }

    public void CleanUp()
    {
        _exitBtn.onClick.RemoveListener(OnExitClicked);
    }

    public void Show(string key, Vector2 screenPosition)
    {
        _text.text = TranslatorStorage.Instance.GetTranslator("CommentData", key, "Description");
        gameObject.SetActive(true);
        _rect.transform.position = screenPosition;

        //UITextDescription에 등록하기
    }

    public void Hide(Action callback = null)
    {
        gameObject.SetActive(false);
        callback?.Invoke();

        //UITextDescription에 해제하기
    }

    private void OnExitClicked()
    {
        Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {

        }
    }
}
