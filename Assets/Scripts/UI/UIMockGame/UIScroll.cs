using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScroll : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private Button _lBtn;

    [SerializeField]
    private Button _rBtn;


    private void Awake()
    {
        _lBtn.onClick.AddListener(OnLeftClickedEvent);
        _rBtn.onClick.AddListener(OnRightClickedEvent);
    }

    private void OnDestroy()
    {
        _lBtn.onClick.RemoveListener(OnLeftClickedEvent);
        _rBtn.onClick.RemoveListener(OnRightClickedEvent);
    }

    public void SetText(string str)
    {
        _text.text = str;
    }

    private void OnLeftClickedEvent()
    {
        _leftEvent?.Invoke();
    }

    private void OnRightClickedEvent()
    {
        _rightEvent?.Invoke();
    }

    private event System.Action _leftEvent;
    private event System.Action _rightEvent;

    public void AddOnLeftBtnClickListener(System.Action act) => _leftEvent += act;
    public void RemoveOnLeftBtnClickListener(System.Action act) => _leftEvent -= act;
    public void AddOnRightBtnClickListener(System.Action act) => _rightEvent += act;
    public void RemoveOnRightBtnClickListener(System.Action act) => _rightEvent -= act;

}
