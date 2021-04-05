using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    GameTestManager gameTestManager;

    [SerializeField]
    Button _upgradeButton;

    [SerializeField]
    Text turnText;
    [SerializeField]
    Text deadLText;
    [SerializeField]
    Text deadRText;

    [SerializeField]
    Text supplyLText;
    [SerializeField]
    Text supplyRText;

    [SerializeField]
    Text leftHealthText;
    [SerializeField]
    Text rightHealthText;

    [SerializeField]
    Slider leftHealthSlider;
    [SerializeField]
    Slider rightHealthSlider;

    [SerializeField]
    GameObject turnL;

    [SerializeField]
    GameObject turnR;

    [SerializeField]
    GameObject gameEnd;

    [SerializeField]
    Transform tr;

    [SerializeField]
    UIUnitButton _unitButton;

    //[SerializeField]
    //UnitData[] _unitDataArray;

    List<UIUnitButton> list = new List<UIUnitButton>();


    private void Awake()
    {
        SetUnitData(gameTestManager.GetLeftUnits());
        _upgradeButton.onClick.AddListener(delegate
        {
            gameTestManager.IncreaseUpgrade(TYPE_TEAM.Left);
        });
    }

    private void OnDestroy()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].RemoveUnitDownListener(DragUnit);
            list[i].RemoveUnitUpListener(DragUnit);
        }

        list.Clear();
    }

    public void SetUnitData(UnitData[] unitDataArray)
    {
        for (int i = 0; i < unitDataArray.Length; i++)
        {
            var btn = Instantiate(_unitButton);
            btn.SetData(unitDataArray[i]);
            btn.AddUnitDownListener(DragUnit);
            btn.AddUnitUpListener(DropUnit);
            btn.transform.SetParent(tr);
            btn.gameObject.SetActive(true);
            list.Add(btn);
        }
    }

    void DragUnit(UnitData uData)
    {
        if(gameTestManager.IsSupply(uData))
            gameTestManager.DragUnit(uData);
    }

    void DropUnit(UnitData uData)
    {
        gameTestManager.DropUnit(uData);
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = "Turn : " + gameTestManager.turnCount.ToString();
        deadLText.text = _unitManager.deadL.ToString();
        deadRText.text = _unitManager.deadR.ToString();
        supplyLText.text = gameTestManager.GetSupply(TYPE_TEAM.Left);
        supplyRText.text = gameTestManager.GetSupply(TYPE_TEAM.Right);
        turnL.gameObject.SetActive(gameTestManager._typeTeam == TYPE_TEAM.Left);
        turnR.gameObject.SetActive(gameTestManager._typeTeam == TYPE_TEAM.Right);
        gameEnd.gameObject.SetActive(gameTestManager.isEnd);

        leftHealthText.text = gameTestManager.GetCastleHealth(TYPE_TEAM.Left).ToString();
        rightHealthText.text = gameTestManager.GetCastleHealth(TYPE_TEAM.Right).ToString();

        leftHealthSlider.value = gameTestManager.GetCastleHealthRate(TYPE_TEAM.Left);
        rightHealthSlider.value = gameTestManager.GetCastleHealthRate(TYPE_TEAM.Right);

        _upgradeButton.interactable = gameTestManager.IsUpgradeSupply(TYPE_TEAM.Left);
    }
}
