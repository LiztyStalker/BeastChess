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

    public UnitCard unitCard { get; private set; }


    private bool isDrag = false;
    private int _index = 0;
    private Transform parent;

    public void SetData(int index, UnitCard uCard)
    {
        _index = index;
        unitCard = uCard;
        _image.sprite = unitCard.icon;
        _text.text = unitCard.employCostValue.ToString();
        _nameText.text = unitCard.name;
        _populationText.text = uCard.Population.ToString();
        _healthSlider.value = uCard.TotalHealthRate();
        _populationImage.fillAmount = (float)uCard.Population / uCard.squadCount;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isDrag)
        {
            transform.position = Input.mousePosition;
        }
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


    UIUnitOutpost parentOutpost;
    UIUnitOutpostBarrack parentBarrack;


    public void OnPointerDown(PointerEventData eventData)
    {
        _downEvent?.Invoke(unitCard);

        if (!isDrag)
        {
            isDrag = true;

            parentOutpost = GetComponentInParent<UIUnitOutpost>();
            parentBarrack = GetComponentInParent<UIUnitOutpostBarrack>();

            parent = transform.parent;
            transform.SetParent(GetComponentInParent<MockGameManager>().dragPanel);
            transform.SetAsLastSibling();
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
                if (parentBarrack != null)
                {
                    var outpost = results[i].gameObject.GetComponent<UIUnitOutpost>();
                    if (outpost != null)
                    {
                        outpost.AddCard(this);
                        parentBarrack.RemoveCard(this);
                        return;
                    }
                }
                else if (parentOutpost != null)
                {
                    var barrack = results[i].gameObject.GetComponent<UIUnitOutpostBarrack>();
                    if (barrack != null)
                    {
                        barrack.AddCard(this);
                        parentOutpost.RemoveCard(this);
                        return;
                    }
                }
            }

            

            transform.SetParent(parent);
            transform.SetSiblingIndex(_index);
            parent = null;

            parentBarrack = null;
            parentOutpost = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
