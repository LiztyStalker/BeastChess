using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextDescriptionManager
{
    private static List<UITextDescription> _list = new List<UITextDescription>();

    public static void Add(UITextDescription ui)
    {
        if (!_list.Contains(ui))
            _list.Add(ui);
    }

    public static void Remove(UITextDescription ui)
    {
        if (_list.Contains(ui))
            _list.Remove(ui);
    }
}

public class UITextDescription : MonoBehaviour, ICanvas
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

    public void Show(string text, Vector2 screenPosition)
    {
        _text.text = text;
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

}
