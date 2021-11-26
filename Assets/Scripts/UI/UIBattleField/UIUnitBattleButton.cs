using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIUnitBattleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Image _classImage;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Text _populationText;

    [SerializeField]
    private Image _populationImage;

    [SerializeField]
    private GameObject _appearedPanel;

    [SerializeField]
    private UIUnitFormation _uiUnitFormation;

    private UnitCard _uCard;

    private bool isPress = false;

    public void Initialize()
    {
        _uiUnitFormation.Initialize();
    }

    public void CleanUp()
    {
        _uiUnitFormation.CleanUp();
    }

    public void SetData(UnitCard uCard)
    {
        _uCard = uCard;
        _image.sprite = _uCard.Icon;
        _text.text = _uCard.AppearCostValue.ToString();
        _nameText.text = _uCard.UnitName;
        _populationText.text = uCard.LiveSquadCount.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.LiveSquadCount / uCard.SquadCount;
        _classImage.sprite = DataStorage.Instance.GetDataOrNull<Sprite>(uCard.TypeUnitClass.ToString(), "Icon_Class", null);
        _uiUnitFormation.ShowFormation(uCard);
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
        _appearedPanel.SetActive(!interactable);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_button.interactable)
            {
                isPress = true;
                _downEvent?.Invoke(_uCard);
                AudioManager.ActivateAudio("BTN_DN", AudioManager.TYPE_AUDIO.SFX, false);
            }
        }        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.interactable && isPress)
        {
            //Debug.Log(_upEvent.Method);
            isPress = false;
            _upEvent?.Invoke(this, _uCard);
            AudioManager.ActivateAudio("BTN_UP", AudioManager.TYPE_AUDIO.SFX, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //isPress = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _inforEvent?.Invoke(_uCard, eventData.position);
        }
    }

    #region ##### Listener #####

    System.Action<UnitCard> _downEvent;
    System.Action<UIUnitBattleButton, UnitCard> _upEvent;
    System.Action<UnitCard, Vector2> _inforEvent;

    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;

    public void AddUnitUpListener(System.Action<UIUnitBattleButton, UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UIUnitBattleButton, UnitCard> listener) => _upEvent -= listener;

    public void AddUnitInformationListener(System.Action<UnitCard, Vector2> listener) => _inforEvent += listener;
    public void RemoveUnitInformationListener(System.Action<UnitCard, Vector2> listener) => _inforEvent -= listener;

    #endregion

}
