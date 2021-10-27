using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour, ICanvas
{

    [SerializeField]
    private Button _exitBtn;

    [SerializeField]
    private Button _applyBtn;

    [SerializeField]
    private Button _okBtn;

    [SerializeField]
    private Button _cancelBtn;

    [SerializeField]
    private Text _msgText;

    [SerializeField]
    private Image _backgroundImage;


    public void Initialize()
    {
        _exitBtn.onClick.AddListener(OnClosedEvent);
        _applyBtn.onClick.AddListener(OnApplyClickedEvent);
        _okBtn.onClick.AddListener(OnOkClickedEvent);
        _cancelBtn.onClick.AddListener(OnCancelClickedEvent);
        Hide();
    }

    public void CleanUp()
    {
        _exitBtn.onClick.RemoveListener(OnClosedEvent);
        _applyBtn.onClick.RemoveListener(OnApplyClickedEvent);
        _okBtn.onClick.RemoveListener(OnOkClickedEvent);
        _cancelBtn.onClick.RemoveListener(OnCancelClickedEvent);
    }

    /// <summary>
    /// Apply ÆË¾÷
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="closedCallback"></param>
    public void ShowApplyPopup(string msg, System.Action closedCallback = null, bool isBackground = true)
    {
        ShowApplyPopup(msg, "È®ÀÎ", null, closedCallback, isBackground);
    }

    /// <summary>
    /// Apply ÆË¾÷
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="closedCallback"></param>
    public void ShowApplyPopup(string msg, string applyText, System.Action applyCallback, System.Action closedCallback = null, bool isBackground = true)
    {
        _applyBtn.gameObject.SetActive(true);
        _okBtn.gameObject.SetActive(false);
        _cancelBtn.gameObject.SetActive(false);
        _exitBtn.gameObject.SetActive(false);

        _applyEvent = applyCallback;
        //_closedEvent = applyCallback;

        SetExitEvent(closedCallback);


        SetButtonText(_applyBtn, applyText);
        SetMessage(msg);
        Show();
        SetBackground(isBackground);
    }

    /// <summary>
    /// Ok Cancel ÆË¾÷
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="okCallback"></param>
    /// <param name="cancelCallback"></param>
    /// <param name="closedCallback"></param>
    /// <param name="isBackground"></param>
    public void ShowOkAndCancelPopup(string msg, System.Action okCallback, System.Action cancelCallback, System.Action closedCallback = null, bool isBackground = true)
    {
        ShowOkAndCancelPopup(msg, "È®ÀÎ", "Ãë¼Ò", okCallback, cancelCallback, closedCallback, isBackground);
    }

    /// <summary>
    /// Ok Cancel ÆË¾÷
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="okText"></param>
    /// <param name="cancelText"></param>
    /// <param name="okCallback"></param>
    /// <param name="cancelCallback"></param>
    /// <param name="closedCallback"></param>
    /// <param name="isBackground"></param>

    public void ShowOkAndCancelPopup(string msg, string okText, string cancelText, System.Action okCallback, System.Action cancelCallback, System.Action closedCallback = null, bool isBackground = true)
    {
        _applyBtn.gameObject.SetActive(false);
        _okBtn.gameObject.SetActive(true);
        _cancelBtn.gameObject.SetActive(true);
        _exitBtn.gameObject.SetActive(false);

        _okEvent = okCallback;
        _cancelEvent = cancelCallback;

        SetExitEvent(closedCallback);

        SetButtonText(_okBtn, okText);
        SetButtonText(_cancelBtn, cancelText);
        SetMessage(msg);
        Show();
        SetBackground(isBackground);
    }

    private void SetBackground(bool isBackground)
    {
        _backgroundImage.gameObject.SetActive(isBackground);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void SetMessage(string msg)
    {
        _msgText.text = msg;
    }

    private void SetExitEvent(System.Action closedCallback)
    {
        _closedEvent = closedCallback;
    }

    private void SetButtonText(Button btn, string text)
    {
        btn.GetComponentInChildren<Text>(true).text = text;
    }

    public void Hide(System.Action closedCallback = null)
    {
        gameObject.SetActive(false);
        _msgText.text = null;
        DisposeEvent();
        closedCallback?.Invoke();
    }


    #region ##### Listener #####
    private System.Action _closedEvent;

    private System.Action _applyEvent;

    private System.Action _okEvent;
    private System.Action _cancelEvent;
    private void OnClosedEvent()
    {
        _closedEvent?.Invoke();
        Hide();        
    }

    private void OnApplyClickedEvent()
    {
        AudioManager.ActivateAudio("BTN_NONE", AudioManager.TYPE_AUDIO.SFX, false);
        _applyEvent?.Invoke();
        OnClosedEvent();
    }

    private void OnOkClickedEvent()
    {
        AudioManager.ActivateAudio("BTN_OK", AudioManager.TYPE_AUDIO.SFX, false);
        _okEvent?.Invoke();
        OnClosedEvent();
    }

    private void OnCancelClickedEvent()
    {
        AudioManager.ActivateAudio("BTN_CANCEL", AudioManager.TYPE_AUDIO.SFX, false);
        _cancelEvent?.Invoke();
        OnClosedEvent();
    }

    private void DisposeEvent()
    {
        _closedEvent = null;
        _applyEvent = null;
        _okEvent = null;
        _cancelEvent = null;
    }

    #endregion


}
