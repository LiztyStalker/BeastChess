using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TYPE_BATTLE_TURN {None = -1, Forward, Charge, Guard, Backward }

public class UIGame : MonoBehaviour
{


    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    BattleFieldManager gameTestManager;

    [SerializeField]
    Button _upgradeButton;

    [SerializeField]
    UIUnitInformation information;

    [SerializeField]
    UISkillInformation _uiSkillInformation;

    [SerializeField]
    UITextPanel textPanel;

    [SerializeField]
    UIUnitSelector uiUnitSelector;

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
    UIUnitBattleButton _unitButton;

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

    [SerializeField]
    RectTransform scrollRect;

    [SerializeField]
    Button lBtn;

    [SerializeField]
    Button rBtn;

    [SerializeField]
    GameObject battlePanel;

    [SerializeField]
    UIBattleButton[] uiBattleButtons;

    [SerializeField]
    UIBattleButton[] uIBattleShowButtons;

    List<UIUnitBattleButton> buttonList = new List<UIUnitBattleButton>();
    List<TYPE_BATTLE_TURN> typeBattleTurnList = new List<TYPE_BATTLE_TURN>();

    public TYPE_BATTLE_TURN[] GetTypeBattleTurnArray() => typeBattleTurnList.ToArray();

    private void Awake()
    {
        information.Initialize();
        textPanel.Initialize();

        SetUnitData(gameTestManager.GetLeftUnits());

        for(int i = 0; i < uiBattleButtons.Length; i++)
        {
            uiBattleButtons[i].SetOnClickedListener(OnBattleTurnAddClickedEvent);
        }

        for (int i = 0; i < uIBattleShowButtons.Length; i++)
        {
            uIBattleShowButtons[i].SetOnClickedListener(OnBattleTurnRemoveClickedEvent);
        }

        battlePanel.SetActive(false);

        information.gameObject.SetActive(false);

        information.SetOnTextEvent(textPanel.ShowText);

        //        scrollRect.anchoredPosition = Vector2.zero;

        var pos = scrollRect.anchoredPosition;
        pos.x += lBtn.GetComponent<RectTransform>().sizeDelta.x;
        scrollRect.anchoredPosition = pos;

        uiUnitSelector.AddReturnUnitListener(ReturnUnit);
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
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].RemoveUnitDownListener(DragUnit);
            buttonList[i].RemoveUnitUpListener(DropUnit);
            buttonList[i].RemoveUnitInformationListener(InformationUnit);
        }

        buttonList.Clear();

        uiUnitSelector.RemoveReturnUnitListener(ReturnUnit);
    }

    public void SetUnitData(UnitCard[] unitDataArray)
    {
        for (int i = 0; i < unitDataArray.Length; i++)
        {
            var btn = Instantiate(_unitButton);
            btn.SetData(unitDataArray[i]);
            btn.AddUnitDownListener(DragUnit);
            btn.AddUnitUpListener(DropUnit);
            btn.AddUnitInformationListener(InformationUnit);
            btn.transform.SetParent(tr);
            btn.gameObject.SetActive(true);
            btn.SetInteractable(!unitDataArray[i].IsAllDead());

            buttonList.Add(btn);
        }
    }

    public void UpdateUnits()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].UpdateUnit();
        }
    }

    void DragUnit(UnitCard uCard)
    {
        if(gameTestManager.IsSupply(uCard))
            gameTestManager.DragUnit(uCard);
    }

    void InformationUnit(UnitCard uCard)
    {
        information.ShowData(uCard, Input.mousePosition);
        information.SetPosition(Input.mousePosition);
//        information.transform.position = Input.mousePosition;
        CancelUnit();
    }

    void CancelUnit()
    {
        gameTestManager.CancelUnit();
    }

    void DropUnit(UIUnitBattleButton button, UnitCard uCard)
    {
        Debug.Log("DropUnit");
        if (gameTestManager.DropUnit(uCard))
        {
            button.SetInteractable(false);
        }
        //카드 업데이트
    }

    void ReturnUnit(UnitActor uActor)
    {
        for(int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].IsCompareUnitCard(uActor.unitCard))
            {
                buttonList[i].SetInteractable(true);
                gameTestManager.ReturnUnit(uActor.unitCard);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_BATTLE_ROUND), gameTestManager._typeBattleRound.ToString(), "Name");
        deadLText.text = _unitManager.deadL.ToString();
        deadRText.text = _unitManager.deadR.ToString();
        supplyLText.text = gameTestManager.GetSupply(TYPE_TEAM.Left);
        supplyRText.text = gameTestManager.GetSupply(TYPE_TEAM.Right);
        turnL.gameObject.SetActive(true);// GameManager.nowTypeTeam == TYPE_TEAM.Left);
        turnR.gameObject.SetActive(true);//.nowTypeTeam == TYPE_TEAM.Right);
        gameEnd.gameObject.SetActive(gameTestManager.isEnd);

        leftHealthText.text = gameTestManager.GetCastleHealth(TYPE_TEAM.Left).ToString();
        rightHealthText.text = gameTestManager.GetCastleHealth(TYPE_TEAM.Right).ToString();

        leftHealthSlider.value = gameTestManager.GetCastleHealthRate(TYPE_TEAM.Left);
        rightHealthSlider.value = gameTestManager.GetCastleHealthRate(TYPE_TEAM.Right);

        _upgradeButton.interactable = false;
        nextTurnButton.interactable = true;

        addSupplyText.text = "+" + gameTestManager.AddedSupply();

        supplySlider.value = gameTestManager.SupplyRate();

        battlePanel.SetActive(gameTestManager.isReady);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uiSkillInformation.isActiveAndEnabled)
                _uiSkillInformation.Hide();
            else if (information.isActiveAndEnabled)
            {
                information.Hide();
            }
        }
    }

    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_BattleField");
    }

    public void ReturnMockGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_MockGame");
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
        uiUnitSelector.SetActive(isActive);
    }

    public void Left()
    {
        var pos = scrollRect.anchoredPosition;
        if (pos.x + _unitButton.GetComponent<RectTransform>().sizeDelta.x < lBtn.GetComponent<RectTransform>().sizeDelta.x)
        {
            pos.x += _unitButton.GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            pos.x = lBtn.GetComponent<RectTransform>().sizeDelta.x;

        }
        scrollRect.anchoredPosition = pos;
    }

    public void Right()
    {
        var pos = scrollRect.anchoredPosition;
        if (pos.x - _unitButton.GetComponent<RectTransform>().sizeDelta.x > -(scrollRect.sizeDelta.x - Screen.width + rBtn.GetComponent<RectTransform>().sizeDelta.x))
        {
            pos.x -= _unitButton.GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            pos.x = -(scrollRect.sizeDelta.x - Screen.width + rBtn.GetComponent<RectTransform>().sizeDelta.x);

        }
        scrollRect.anchoredPosition = pos;
    }
}

