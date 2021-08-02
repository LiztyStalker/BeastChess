using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIUnitOutpostButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
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

    UnitCard _uCard;

    public void SetData(UnitCard uCard)
    {
        _uCard = uCard;
        _image.sprite = _uCard.icon;
        _text.text = _uCard.employCostValue.ToString();
        _nameText.text = _uCard.name;
        _populationText.text = uCard.Population.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.Population / uCard.squadCount;
        gameObject.SetActive(true);
    }

    void Update()
    {
    }

    #region ##### Listener #####

    System.Action<UnitCard> _downEvent;
    System.Action<UIUnitOutpostButton, UnitCard> _upEvent;
    System.Action<UnitCard> _inforEvent;

    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UIUnitOutpostButton, UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UIUnitOutpostButton, UnitCard> listener) => _upEvent -= listener;

    public void AddUnitInformationListener(System.Action<UnitCard> listener) => _inforEvent += listener;
    public void RemoveUnitInformationListener(System.Action<UnitCard> listener) => _inforEvent -= listener;

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        _downEvent?.Invoke(_uCard);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _upEvent?.Invoke(this, _uCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
