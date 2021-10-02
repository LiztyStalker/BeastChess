using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIUnitBattleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
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

    bool isPress = false;
    float pressTime = 0f;

    public void SetData(UnitCard uCard)
    {
        _uCard = uCard;
        _image.sprite = _uCard.Icon;
        _text.text = _uCard.employCostValue.ToString();
        _nameText.text = _uCard.name;
        _populationText.text = uCard.LiveSquadCount.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.LiveSquadCount / uCard.squadCount;
        gameObject.SetActive(true);
    }

    public bool IsCompareUnitCard(UnitCard uCard)
    {
        return _uCard == uCard;
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

    void Update()
    {
        if (isPress)
        {
            pressTime += Time.deltaTime;
            if (pressTime > 1f)
            {
                _inforEvent?.Invoke(_uCard);
                pressTime = 0;
            }
        }
        else
        {
            pressTime = 0f;
        }
    }

    #region ##### Listener #####

    System.Action<UnitCard> _downEvent;
    System.Action<UIUnitBattleButton, UnitCard> _upEvent;
    System.Action<UnitCard> _inforEvent;

    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UIUnitBattleButton, UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UIUnitBattleButton, UnitCard> listener) => _upEvent -= listener;

    public void AddUnitInformationListener(System.Action<UnitCard> listener) => _inforEvent += listener;
    public void RemoveUnitInformationListener(System.Action<UnitCard> listener) => _inforEvent -= listener;

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            isPress = true;
            _downEvent?.Invoke(_uCard);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            Debug.Log(_upEvent.Method);
            isPress = false;
            _upEvent?.Invoke(this, _uCard);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPress = false;
    }
}
