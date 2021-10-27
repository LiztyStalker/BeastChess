using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHelpInformation : MonoBehaviour, ICanvas
{

    private readonly string BUTTON_KEY = "UIHelpBtn";

    [SerializeField]
    private Transform _pageTr;

    [SerializeField]
    private Transform _buttonTr;

    [SerializeField]
    private Button _exitBtn;

    private UIHelpBtn _uiHelpBtn;

    private UIHelpBtn uiHelpBtn
    {
        get
        {
            if (_uiHelpBtn == null)
            {
                var obj = DataStorage.Instance.GetDataOrNull<GameObject>(BUTTON_KEY, null, null);
                _uiHelpBtn = obj.GetComponent<UIHelpBtn>();
                //Debug.Log(_uiHelpBtn);
            }
            return _uiHelpBtn;
        }
    }

    private GameObject[] _helpArr;

    private string[] _pageNames = { "게임", "게임방식", "야전막사", "전장", "전투", "배치", "병사" };

    public void Initialize()
    {
        _helpArr = new GameObject[_pageTr.childCount];
        //Debug.Log(_helpArr.Length);
        for(int i = 0; i < _helpArr.Length; i++)
        {
            _helpArr[i] = _pageTr.GetChild(i).gameObject;
            _helpArr[i].SetActive(false);
        }

        for(int i = 0; i < _helpArr.Length; i++)
        {
            var btn = Instantiate(uiHelpBtn);
            btn.Initialize();
            btn.transform.SetParent(_buttonTr);
            btn.transform.localScale = Vector2.one;
            btn.transform.eulerAngles = Vector3.zero;
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 80f);
            btn.SetData(i);
            btn.SetData(_pageNames[i]);
            btn.SetOnClickedListener(ShowEvent);
        }

        _exitBtn.onClick.AddListener(OnClosedEvent);
        Hide();
    }

    public void CleanUp()
    {
        _exitBtn.onClick.RemoveListener(OnClosedEvent);
    }

    public void Show(int index = 0, System.Action callback = null)
    {
        AudioManager.ActivateAudio("BTN_OPEN", AudioManager.TYPE_AUDIO.SFX, false);
        ShowEvent(index);
        gameObject.SetActive(true);
        callback?.Invoke();
    }

    private void ShowEvent(int index)
    {
        AudioManager.ActivateAudio("BTN_FLIP", AudioManager.TYPE_AUDIO.SFX, false);
        for(int i = 0; i < _helpArr.Length; i++)
        {
            _helpArr[i].SetActive(i == index);
        }
    }

    public void Hide(Action callback = null)
    {
        AudioManager.ActivateAudio("BTN_CLOSE", AudioManager.TYPE_AUDIO.SFX, false);
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    private void OnClosedEvent()
    {
        Hide();
    }


}
