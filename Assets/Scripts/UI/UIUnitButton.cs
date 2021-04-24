using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIUnitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    Image _image;

    [SerializeField]
    Text _text;

    [SerializeField]
    Text _nameText;

    [SerializeField]
    Button _button;

    UnitData _unitData;


    System.Action<UnitData> _downEvent;
    System.Action<UnitData> _upEvent;

    private void Awake()
    {
        //_button.GetComponent<Button>();
        //_button.onClick.AddListener(OnClickEvent);
    }

    private void OnDestroy()
    {
        //_button.onClick.RemoveListener(OnClickEvent);
    }

    public void SetData(UnitData unitData)
    {
        _unitData = unitData;
        _image.sprite = _unitData.icon;
        _text.text = _unitData.costValue.ToString();
        _nameText.text = _unitData.name;
        gameObject.SetActive(true);
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    //private void OnClickEvent()
    //{
    //    _clickEvent?.Invoke(_unitData);
    //}

    public void AddUnitDownListener(System.Action<UnitData> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitData> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UnitData> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UnitData> listener) => _upEvent -= listener;

    public void OnPointerDown(PointerEventData eventData)
    {
        _downEvent?.Invoke(_unitData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _upEvent?.Invoke(_unitData);
    }
}
