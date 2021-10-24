using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICommentInformation : MonoBehaviour, ICanvas, IPointerExitHandler
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private RectTransform _rect;

    [SerializeField]
    private Button _exitBtn;

    public void Initialize ()
    {
        _exitBtn.gameObject.SetActive(false);
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
        _rect.transform.position = screenPosition;
        gameObject.SetActive(true);
        AudioManager.ActivateAudio("BTN_INFOR", AudioManager.TYPE_AUDIO.SFX, false);

    }

    public void Hide(Action callback = null)
    {
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    private void OnExitClicked()
    {
        Hide();
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitClicked();
    }
}
