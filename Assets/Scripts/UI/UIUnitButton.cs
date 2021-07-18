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
    UnitCard _uCard;



    public void SetData(UnitCard uCard)
    {
        _uCard = uCard;
        _image.sprite = _uCard.icon;
        _text.text = _uCard.costValue.ToString();
        _nameText.text = _uCard.name;
        _populationText.text = uCard.Population.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.Population / uCard.squadCount;
        gameObject.SetActive(true);
    }

    public void UpdateUnit()
    {
        SetInteractable(!_uCard.IsAllDead());
        SetData(_uCard);
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }


    #region ##### Listener #####

    System.Action<UnitCard> _downEvent;
    System.Action<UIUnitButton, UnitCard> _upEvent;

    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UIUnitButton, UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UIUnitButton, UnitCard> listener) => _upEvent -= listener;

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _downEvent?.Invoke(_uCard);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _upEvent?.Invoke(this, _uCard);
        }
    }
}
