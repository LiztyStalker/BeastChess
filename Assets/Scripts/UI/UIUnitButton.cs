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

    [SerializeField]
    Slider _healthSlider;

    [SerializeField]
    Text _populationText;

    [SerializeField]
    Image _populationImage;

    //UnitData _unitData;
    UnitCard _unitCard;


    System.Action<UnitCard> _downEvent;
    System.Action<UnitCard> _upEvent;

    private void Awake()
    {
        //_button.GetComponent<Button>();
        //_button.onClick.AddListener(OnClickEvent);
    }

    private void OnDestroy()
    {
        //_button.onClick.RemoveListener(OnClickEvent);
    }

    //public void SetData(UnitData unitData)
    //{
    //    _unitData = unitData;
    //    _image.sprite = _unitData.icon;
    //    _text.text = _unitData.costValue.ToString();
    //    _nameText.text = _unitData.name;
    //    gameObject.SetActive(true);
    //}

    public void SetData(UnitCard uCard)
    {
        _unitCard = uCard;
        _image.sprite = _unitCard.icon;
        _text.text = _unitCard.costValue.ToString();
        _nameText.text = _unitCard.name;
        _populationText.text = uCard.Population.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.Population / uCard.squadCount;
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

    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UnitCard> listener) => _upEvent -= listener;

    public void OnPointerDown(PointerEventData eventData)
    {
        _downEvent?.Invoke(_unitCard);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _upEvent?.Invoke(_unitCard);
    }
}
