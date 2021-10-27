using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleFieldMenu : MonoBehaviour, ICanvas
{
    [SerializeField]
    private Button _returnBtn;

    [SerializeField]
    private Button _retryBtn;

    [SerializeField]
    private Button _surrenderBtn;

    public void Initialize()
    {
        _returnBtn.onClick.AddListener(OnReturnClickEvent);
        _retryBtn.onClick.AddListener(OnRetryClickEvent);
        _surrenderBtn.onClick.AddListener(OnSurrenderClickEvent);

        Hide();
    }

    public void CleanUp()
    {
        _returnBtn.onClick.RemoveListener(OnReturnClickEvent);
        _retryBtn.onClick.RemoveListener(OnRetryClickEvent);
        _surrenderBtn.onClick.RemoveListener(OnSurrenderClickEvent);
    }

    public void Show(Action callback = null)
    {
        gameObject.SetActive(true);
        callback?.Invoke();
    }
    public void Hide(Action callback = null)
    {
        gameObject.SetActive(false);
        _closedEvent?.Invoke();
        callback?.Invoke();
    }

    private void OnReturnClickEvent()
    {
        _returnEvent?.Invoke();
        Hide();
    }

    private void OnRetryClickEvent()
    {
        _retryEvent?.Invoke();
        Hide();
    }

    private void OnSurrenderClickEvent()
    {
        _surrenderEvent?.Invoke();
        Hide();
    }

    #region ##### Listener #####

    private System.Action _closedEvent;
    private System.Action _returnEvent;
    private System.Action _retryEvent;
    private System.Action _surrenderEvent;

    public void SetOnReturnListener(System.Action act) => _returnEvent = act;
    public void SetOnRetryListener(System.Action act) => _retryEvent = act;
    public void SetOnSurrenderListener(System.Action act) => _surrenderEvent = act;

    public void AddOnClosedListener(System.Action act) => _closedEvent += act;
    public void RemoveOnClosedListener(System.Action act) => _closedEvent -= act;



    #endregion

}
