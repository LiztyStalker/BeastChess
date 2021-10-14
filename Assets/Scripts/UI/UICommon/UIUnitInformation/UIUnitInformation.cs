using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformation : MonoBehaviour, ICanvas
{

    [SerializeField]
    private UIUnitInformationCommon _uiUnitInformationCommon;

    [SerializeField]
    private UIUnitInformationSlider _uiUnitInformationSlider;

    [SerializeField]
    private UIUnitInformationAbility _uiUnitInformationAbility;

    [SerializeField]
    private UIUnitInformationCost _uiUnitInformationCost;

    [SerializeField]
    RectTransform _tr;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Button _exitBtn;

    public void Initialize()
    {
        _exitBtn.onClick.AddListener(OnExitClickedEvent);
        _uiUnitInformationCommon.Initialize();
        Hide();
    }

    public void CleanUp()
    {
        _exitBtn.onClick.RemoveListener(OnExitClickedEvent);
        _uiUnitInformationCommon.CleanUp();
    }

    public void ShowActor(UnitActor uActor)
    {
        ShowData(uActor.unitCard, uActor.position);        
    }

    public void ShowData(UnitCard _uCard, Vector2 screenPosition)
    {
        _uiUnitInformationCommon.SetData(_uCard);
        _uiUnitInformationSlider.SetData(_uCard);
        _uiUnitInformationAbility.SetData(_uCard);
        _uiUnitInformationCost.SetData(_uCard);       

        descriptionText.text = _uCard.Description;

        gameObject.SetActive(true);

        SetPosition(screenPosition);

    }

    //public void SetOnTextEvent(System.Action<string> listener)
    //{
    //    var arr = transform.GetComponentsInChildren<UITextInformation>(true);
    //    for(int i = 0; i < arr.Length; i++)
    //    {
    //        arr[i].SetOnShowListener(listener);
    //    }
    //}

    public void SetPosition(Vector2 screenPosition)
    {
        _tr.position = screenPosition;
    }

    public void Hide(Action callback = null)
    {
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    private void OnExitClickedEvent()
    {        
        Hide();
    }
   

}
