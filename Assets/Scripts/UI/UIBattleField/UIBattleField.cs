using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TYPE_BATTLE_TURN {None = -1, Forward, Charge, Guard, Backward }

public class UIBattleField : MonoBehaviour
{
    private BattleFieldManager _battleFieldManager;

    [SerializeField]
    private UIBattleStatusLayout _uiBattieStatusLayout;

    [SerializeField]
    private UIBattleCommand _uiBattleCommandLayout;

    [SerializeField]
    private UIBattleSupply _uiBattleSupplyLayout;

    [SerializeField]
    private UIBattleSquadLayout _uiBattleSquadLayout;

    [SerializeField]
    private UIUnitSelector _uiUnitSelector;

    [SerializeField]
    private Button _nextTurnButton;

    [SerializeField]
    private GameObject gameEnd;

    public void Initialize(BattleFieldManager battleFieldManager)
    {
        _battleFieldManager = battleFieldManager;


        _uiBattleSquadLayout.Initialize();
        _uiBattleSquadLayout.SetOnDragEvent(DragUnit);
        _uiBattleSquadLayout.SetOnDropEvent(DropUnit);

        _uiUnitSelector.Initialize();
        _uiUnitSelector.SetOnInformationListener(ShowUnitInformationEvent);
        _uiUnitSelector.SetOnDragListener(OnUnitModifiedClickedEvent);
        _uiUnitSelector.SetOnCancelListener(OnUnitCancelClickedEvent);
        _uiUnitSelector.SetOnReturnListener(OnUnitReturnClickedEvent);

        _uiBattleCommandLayout.Initialize();

        _nextTurnButton.onClick.AddListener(NextTurn);

    }

    public void CleanUp()
    {
        _uiBattleSquadLayout.CleanUp();
        _uiUnitSelector.CleanUp();
        _uiBattleCommandLayout.CleanUp();

        _nextTurnButton.onClick.RemoveListener(NextTurn);
    }

    public void ShowSupply(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattleSupplyLayout.SetSupply(value, rate);
    }

    public void ShowHealth(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattieStatusLayout.SetCastleHealth(typeTeam, value, rate);
    }

    public void ShowAutoBattle(System.Action<bool> callback)
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("전장의 적을 모두 섬멸하였습니다.\n적의 성을 향해 공격하시겠습니까?\n아니면 전력을 보존하기 위해 후퇴하시겠습니까?",
            "전진", 
            "후퇴", 
            delegate { callback?.Invoke(true); },
            delegate { callback?.Invoke(false); }, 
            null,
            true);
    }

    private void ShowUnitInformationEvent(UnitActor uActor, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UIUnitInformation>();
        ui.Show(uActor);
        ui.SetPosition(screenPosition);
    }

    public void ShowUnitSettingIsZeroPopup()
    {       
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("배치된 병력이 없습니다.\n병력을 1분대 이상 배치해 주세요");
    }

    public void OnUnitModifiedClickedEvent(UnitActor uActor)
    {
        _battleFieldManager.DragUnit(uActor);
    }

    public void OnUnitReturnClickedEvent(UnitActor uActor)
    {
        if(_uiBattleSquadLayout.ReturnUnit(uActor))
            _battleFieldManager.ReturnUnit(uActor);
    }
    public void OnUnitCancelClickedEvent()
    {
        _battleFieldManager.CancelChangeUnit();
    }


    public void SetUnitData(UnitCard[] unitDataArray)
    {
        _uiBattleSquadLayout.SetUnitData(unitDataArray);
    }

    public void UpdateUnits()
    {
        _uiBattleSquadLayout.UpdateUnits();
    }

    private bool DragUnit(UnitCard uCard)
    {
        if(_battleFieldManager.IsSupply(uCard)){
            _battleFieldManager.DragUnit(uCard);
            return true;
        }
        return false;
    }

    private bool DropUnit(UnitCard uCard) => _battleFieldManager.DropUnit(uCard);

  
    void ReturnUnit(UnitActor uActor)
    {
        if (_uiBattleSquadLayout.ReturnUnit(uActor))
        {
            _battleFieldManager.ReturnUnit(uActor.unitCard);
        }
    }

    public void SetCastleHealth(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattieStatusLayout.SetCastleHealth(typeTeam, value, rate);
    }

    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound)
    {
        _uiBattieStatusLayout.SetBattleRound(typeBattleRound);
    }



    public void SetSupply(int value, int rate)
    {
        _uiBattleSupplyLayout.SetSupply(value, rate);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { 
                var screenPosition = Input.mousePosition;
                if (_battleFieldManager.IsOrder())
                    _uiUnitSelector.ShowSelectorMenu(TYPE_TEAM.Left, screenPosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiUnitSelector.CloseMenu();

            if (UICommon.Current.IsCanvasActivated())
                UICommon.Current.NowCanvasHide();
        }
    }

    public void Replay()
    {
        LoadManager.SetNextSceneName("Test_BattleField");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    public void ReturnMockGame()
    {
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    public void ActivateBattleRound()
    {
        _uiBattleSquadLayout.gameObject.SetActive(false);
        _nextTurnButton.gameObject.SetActive(true);
        _uiBattleSupplyLayout.gameObject.SetActive(false);
        _uiBattleCommandLayout.Show();
    }

    public void ActivateUnitSquad()
    {
        _uiBattleSquadLayout.gameObject.SetActive(true);
        _nextTurnButton.gameObject.SetActive(true);
        _uiBattleSupplyLayout.gameObject.SetActive(true);
        _uiBattleCommandLayout.Hide();
    }

    public void ActivateBattle()
    {
        _uiBattleSquadLayout.gameObject.SetActive(false);
        _nextTurnButton.gameObject.SetActive(false);
        _uiBattleSupplyLayout.gameObject.SetActive(false);
        _uiBattleCommandLayout.Hide();
    }

    public void GameEnd(bool isVictory)
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        if (isVictory)
        {
            MockGameOutpost.Current.AddChallengeLevel();
            ui.ShowOkAndCancelPopup("승리", "재시작", "메인", Replay, ReturnMockGame);
        }
        else
        {
            ui.ShowOkAndCancelPopup("패배", "재시작", "메인", Replay, ReturnMockGame);
        }
    }

    public void SetSquadPanel(bool isActive)
    {
        _uiBattleSquadLayout.gameObject.SetActive(isActive);
        _uiUnitSelector.SetActive(isActive);
    }


    private void NextTurn()
    {
        if(_uiBattleCommandLayout.isActiveAndEnabled)
            _battleFieldManager.SetTypeBattleTurns(TYPE_TEAM.Left, _uiBattleCommandLayout.GetTypeBattleTurnArray());

        _nextTurnEvent?.Invoke();
    }


    #region ##### Listener #####
    private System.Action _nextTurnEvent;

    public void SetOnNextTurnListener(System.Action act) => _nextTurnEvent = act;
    #endregion
}

