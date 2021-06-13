using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TYPE_BATTLE_TURN {None = -1, Forward, Shoot, Charge, Guard, Backward }

public class UIGame : MonoBehaviour
{


    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    GameManager gameTestManager;

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

    [SerializeField]
    Text addSupplyText;

    [SerializeField]
    Slider supplySlider;

    [SerializeField]
    Button nextTurnButton;

    [SerializeField]
    GameObject commandPanel;

    [SerializeField]
    GameObject squadPanel;

    //[SerializeField]
    //UnitData[] _unitDataArray;

    [SerializeField]
    UIBattleButton[] uiBattleButtons;

    [SerializeField]
    UIBattleButton[] uIBattleShowButtons;

    List<UIUnitButton> list = new List<UIUnitButton>();
    List<TYPE_BATTLE_TURN> typeBattleTurnList = new List<TYPE_BATTLE_TURN>();

    public TYPE_BATTLE_TURN[] GetTypeBattleTurnArray() => typeBattleTurnList.ToArray();

    private void Awake()
    {
        SetUnitData(gameTestManager.GetLeftUnits());
        _upgradeButton.onClick.AddListener(delegate
        {
            gameTestManager.IncreaseUpgrade(TYPE_TEAM.Left);
        });

        for(int i = 0; i < uiBattleButtons.Length; i++)
        {
            uiBattleButtons[i].SetOnClickedListener(OnBattleTurnAddClickedEvent);
        }

        for (int i = 0; i < uIBattleShowButtons.Length; i++)
        {
            uIBattleShowButtons[i].SetOnClickedListener(OnBattleTurnRemoveClickedEvent);
        }
    }

    void OnBattleTurnAddClickedEvent(TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (typeBattleTurnList.Count < uIBattleShowButtons.Length)
        {
            typeBattleTurnList.Add(typeBattleTurn);
            ShowBattleTurn();
        }
    }

    void OnBattleTurnRemoveClickedEvent(TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (0 < typeBattleTurnList.Count)
        {
            typeBattleTurnList.Remove(typeBattleTurn);
            ShowBattleTurn();
        }
    }

    void ShowBattleTurn()
    {
        for(int i = 0; i < uIBattleShowButtons.Length; i++)
        {
            if (i < typeBattleTurnList.Count)
                uIBattleShowButtons[i].SetTurn(typeBattleTurnList[i]);
            else
                uIBattleShowButtons[i].SetTurn(TYPE_BATTLE_TURN.None);
        }
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

    public void SetUnitData(UnitCard[] unitDataArray)
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

    void DragUnit(UnitCard uCard)
    {
        if(gameTestManager.IsSupply(uCard))
            gameTestManager.DragUnit(uCard);
    }

    void DropUnit(UnitCard uCard)
    {
        gameTestManager.DropUnit(uCard);
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = gameTestManager._typeBattleRound.ToString();
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

        _upgradeButton.interactable = gameTestManager.IsUpgradeSupply(TYPE_TEAM.Left) && gameTestManager._typeTeam == TYPE_TEAM.Left;
        nextTurnButton.interactable = gameTestManager._typeTeam == TYPE_TEAM.Left;

        addSupplyText.text = "+" + gameTestManager.AddedSupply();

        supplySlider.value = gameTestManager.SupplyRate();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetInteractable(gameTestManager._typeTeam == TYPE_TEAM.Left);
        }
    }

    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_Game");
    }

    public void SetBattleTurn(bool isActive)
    {
        if (!isActive)
        {
            typeBattleTurnList.Clear();
            ShowBattleTurn();
        }
        commandPanel.SetActive(isActive);
    }

    public void SetSquad(bool isActive)
    {
        squadPanel.SetActive(isActive);
    }
}
