using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIUnitOutpostButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    Image _image;

    [SerializeField]
    Text _text;

    [SerializeField]
    Text _nameText;

    [SerializeField]
    Image _classImage;

    [SerializeField]
    Slider _healthSlider;

    [SerializeField]
    Text _populationText;

    [SerializeField]
    Image _populationImage;

    [SerializeField]
    private UIUnitFormation _uiUnitFormation;

    public UnitCard unitCard { get; private set; }


    private bool isDrag = false;
    private int _index = 0;
    private Transform parent;

    public void Initialize()
    {
        _uiUnitFormation.Initialize();
    }

    public void CleanUp()
    {
        _uiUnitFormation.CleanUp();
    }

    public void SetData(int index, UnitCard uCard)
    {
        _index = index;
        unitCard = uCard;
        _image.sprite = unitCard.Icon;
        _text.text = unitCard.employCostValue.ToString();
        _nameText.text = unitCard.UnitName;
        _populationText.text = uCard.LiveSquadCount.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.LiveSquadCount / uCard.squadCount;
        _uiUnitFormation.ShowFormation(uCard);
        _classImage.sprite = DataStorage.Instance.GetDataOrNull<Sprite>(uCard.typeUnitClass.ToString(), "Icon_Class", null);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        unitCard = null;
        _index = -1;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDrag)
        {
            transform.position = Input.mousePosition;
        }
    }



    private UIUnitOutpost _parentOutpost;
    private UIUnitOutpostBarrack _parentBarrack;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _downEvent?.Invoke(unitCard);

            if (!isDrag)
            {
                isDrag = true;

                _parentOutpost = GetComponentInParent<UIUnitOutpost>();
                _parentBarrack = GetComponentInParent<UIUnitOutpostBarrack>();

                parent = transform.parent;
                transform.SetParent(GetComponentInParent<UIMockGame>().dragPanel);
                transform.SetAsLastSibling();

                AudioManager.ActivateAudio("BTN_UP", AudioManager.TYPE_AUDIO.SFX, false);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _upEvent?.Invoke(this, unitCard);

        if (isDrag)
        {
            isDrag = false;


            var raycaster = GetComponentInParent<GraphicRaycaster>();

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            for(int i = 0; i < results.Count; i++)
            {
                if (_parentBarrack != null)
                {

                    var outpost = results[i].gameObject.GetComponent<UIUnitOutpost>();
                    if (outpost != null)
                    {
                        if (outpost.IsEnough(unitCard))
                        {
                            outpost.ChangeCard(unitCard);
                            break;
                        }
                    }
                }
                if (_parentOutpost != null)
                {

                    var barrack = results[i].gameObject.GetComponent<UIUnitOutpostBarrack>();
                    if (barrack != null)
                    {
                        //계정에 적용
                        barrack.ChangeCard(unitCard);
                        break;
                    }
                }
            }

            

            transform.SetParent(parent);
            transform.SetSiblingIndex(_index);
            parent = null;

            _parentBarrack = null;
            _parentOutpost = null;

            AudioManager.ActivateAudio("BTN_DN", AudioManager.TYPE_AUDIO.SFX, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
            _inforEvent?.Invoke(unitCard);
    }

    #region ##### Listener #####

    System.Action<UnitCard> _downEvent;
    public void AddUnitDownListener(System.Action<UnitCard> listener) => _downEvent += listener;
    public void RemoveUnitDownListener(System.Action<UnitCard> listener) => _downEvent -= listener;



    System.Action<UIUnitOutpostButton, UnitCard> _upEvent;
    public void AddUnitUpListener(System.Action<UIUnitOutpostButton, UnitCard> listener) => _upEvent += listener;
    public void RemoveUnitUpListener(System.Action<UIUnitOutpostButton, UnitCard> listener) => _upEvent -= listener;



    System.Action<UnitCard> _inforEvent;
    public void SetOnUnitInformationListener(System.Action<UnitCard> listener) => _inforEvent = listener;




    #endregion

}
