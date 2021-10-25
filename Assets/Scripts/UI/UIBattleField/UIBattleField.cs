using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TYPE_BATTLE_TURN {None = -1, Forward, Charge, Guard, Backward }

public enum TYPE_BATTLE_RESULT { Victory, Draw, Defeat}

public class UIBattleField : MonoBehaviour
{
    private BattleFieldManager _battleFieldManager;

    private UIBattleStatusLayout _uiBattieStatusLayout;

    private UIBattleCommand _uiBattleCommandLayout;

    private UIBattleSupply _uiBattleSupplyLayout;

    private UIBattleSquadLayout _uiBattleSquadLayout;

    private UIUnitSelector _uiUnitSelector;

    private UIBattleTurnPanel _uiBattleTurnPanel;

    [SerializeField]
    private GameObject _uiUnitSettingsPanel;

    [SerializeField]
    private Button _nextTurnButton;
  
    public void Initialize(BattleFieldManager battleFieldManager)
    {
        _battleFieldManager = battleFieldManager;

        SetComponent(ref _uiBattieStatusLayout);
        _uiBattieStatusLayout.Initialize();

        SetComponent(ref _uiBattleSquadLayout);
        _uiBattleSquadLayout.Initialize();
        _uiBattleSquadLayout.SetOnDragListener(DragUnit);
        _uiBattleSquadLayout.SetOnDropListener(DropUnit);
        _uiBattleSquadLayout.SetOnUnitInformationListener(ShowUnitInformationEvent);

        SetComponent(ref _uiUnitSelector);

        SetComponent(ref _uiBattieStatusLayout);
        _uiUnitSelector.Initialize();
        _uiUnitSelector.SetOnInformationListener(ShowUnitInformationEvent);
        _uiUnitSelector.SetOnDragListener(OnUnitModifiedClickedEvent);
        _uiUnitSelector.SetOnCancelListener(OnUnitCancelClickedEvent);
        _uiUnitSelector.SetOnReturnListener(OnUnitReturnClickedEvent);

        SetComponent(ref _uiBattleCommandLayout);
        _uiBattleCommandLayout.Initialize();

        SetComponent(ref _uiBattleSupplyLayout);

        _nextTurnButton.onClick.AddListener(NextTurn);

        SetComponent(ref _uiBattleTurnPanel);
        _uiBattleTurnPanel.SetAnimator(false);

        ActivateUnitSetting(false);
    }

    public void CleanUp()
    {
        _uiBattleSquadLayout.CleanUp();
        _uiUnitSelector.CleanUp();
        _uiBattleCommandLayout.CleanUp();

        _nextTurnButton.onClick.RemoveListener(NextTurn);
    }

    /// <summary>
    /// 내부 컴포넌트를 적용합니다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="behaviour"></param>
    /// <returns></returns>
    private bool SetComponent<T>(ref T behaviour) where T : MonoBehaviour
    {
        behaviour = GetComponentInChildren<T>(true);
        return IsNull<T>(behaviour);
    }


    private bool IsNull<T>(MonoBehaviour gameObject)
    {
        if (gameObject == null)
        {
            Debug.LogError($"{typeof(T).Name}를 찾을 수 없습니다");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Supply 수치를 보입니다
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void ShowSupply(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattleSupplyLayout.SetSupply(value, rate);
    }

    /// <summary>
    /// 성에 대한 Health 수치를 보입니다
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void ShowHealth(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattieStatusLayout.SetCastleHealth(typeTeam, value, rate);
    }

    /// <summary>
    /// 자동전투 팝업을 띄웁니다
    /// </summary>
    /// <param name="callback"></param>
    public void ShowAutoBattlePopup(System.Action<bool> callback)
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

    /// <summary>
    /// 병력 Zero 팝업을 띄웁니다
    /// </summary>
    public void ShowUnitSettingIsZeroPopup()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("배치된 병력이 없습니다.\n병력을 1분대 이상 배치해 주세요");
    }

    public void ShowIsNotEnoughCommandPopup()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("3번의 명령을 적용해야 합니다");
    }

    /// <summary>
    /// 유닛 데이터를 업데이트 합니다
    /// </summary>
    public void UpdateUnits()
    {
        _uiBattleSquadLayout.UpdateUnits();
    }

    /// <summary>
    /// 병사카드 드래그
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DragUnit(UnitCard uCard)
    {
        if(_battleFieldManager.IsSupply(uCard)){
            _battleFieldManager.DragUnit(uCard);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 병사카드 드롭
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DropUnit(UnitCard uCard)
    {
        var boolean = _battleFieldManager.DropUnit(uCard);
        
        return boolean;
    }

    /// <summary>
    /// 현재 전투 라운드를 보입니다
    /// </summary>
    /// <param name="typeBattleRound"></param>
    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound)
    {
        _uiBattieStatusLayout.SetBattleRound(typeBattleRound);
    }

    /// <summary>
    /// Supply 수치를 적용합니다
    /// </summary>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void SetSupply(int value, float rate)
    {
        _uiBattleSupplyLayout.SetSupply(value, rate);
    }

    /// <summary>
    /// 총 분대 수치를 적용합니다
    /// </summary>
    /// <param name="isActive"></param>
    public void SetTotalSquadCount(bool isActive)
    {
        _uiBattleSquadLayout.gameObject.SetActive(isActive);
        _uiUnitSelector.SetActive(isActive);
    }

    /// <summary>
    /// UnitCard[]를 적용합니다
    /// </summary>
    /// <param name="unitDataArray"></param>
    public void SetUnitData(UnitCard[] unitDataArray)
    {
        _uiBattleSquadLayout.SetUnitData(unitDataArray);
    }


    //InputManager 통합 필요
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

    /// <summary>
    /// 모의전으로 돌아갑니다
    /// </summary>
    public void ReturnMockGame()
    {
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    /// <summary>
    /// 메인으로 돌아갑니다
    /// </summary>
    public void ReturnMainTitle()
    {
        LoadManager.SetNextSceneName("Test_MainTitle");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    /// <summary>
    /// 병사카드창을 활성화 합니다
    /// </summary>
    public void ActivateUnitCardPanel()
    {
        _uiBattleSquadLayout.gameObject.SetActive(true);
        _nextTurnButton.gameObject.SetActive(true);
        _uiBattleSupplyLayout.gameObject.SetActive(true);
        _uiBattleCommandLayout.Hide();
        _uiBattleTurnPanel.SetAnimator(false);
    }

    /// <summary>
    /// 명령창을 활성화 합니다
    /// </summary>
    public void ActivateBattleRoundPanel()
    {
        _uiBattleSquadLayout.gameObject.SetActive(false);
        _nextTurnButton.gameObject.SetActive(true);
        _uiBattleSupplyLayout.gameObject.SetActive(false);
        _uiBattleCommandLayout.Show();
        _uiBattleTurnPanel.SetAnimator(false);
    }

    /// <summary>
    /// 전투창을 활성화 합니다
    /// </summary>
    public void ActivateBattlePanel()
    {
        _uiBattleSquadLayout.gameObject.SetActive(false);
        _nextTurnButton.gameObject.SetActive(false);
        _uiBattleSupplyLayout.gameObject.SetActive(false);
        _uiBattleCommandLayout.Hide();
        _uiBattleTurnPanel.SetAnimator(true);
    }   

    /// <summary>
    /// 다음 턴으로 진행합니다
    /// </summary>
    private void NextTurn()
    {
        if(_uiBattleCommandLayout.isActiveAndEnabled)
            _battleFieldManager.SetTypeBattleTurns(TYPE_TEAM.Left, _uiBattleCommandLayout.GetTypeBattleTurnArray());

        _nextTurnEvent?.Invoke();
        
    }

    /// <summary>
    /// 게임을 마칩니다
    /// </summary>
    /// <param name="typeBattleResult"></param>
    public void GameEnd(TYPE_BATTLE_RESULT typeBattleResult)
    {
        BattleFieldOutpost.Current.AllRecovery();

        var ui = UICommon.Current.GetUICommon<UIPopup>();
        switch (typeBattleResult)
        {
            case TYPE_BATTLE_RESULT.Defeat:
                AudioManager.ActivateAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM, false);
                ui.ShowOkAndCancelPopup("패배", "재시작", "메인", ReturnMockGame, delegate {
                    AudioManager.InactiveAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM);
                    ReturnMainTitle();
                });
                break;
            case TYPE_BATTLE_RESULT.Victory:
                AudioManager.ActivateAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM, false);
                if (BattleFieldOutpost.Current.IsChallengeEnd())
                {
                    ui.ShowApplyPopup("챌린지에서 승리하였습니다.\n플레이해주셔서 감사합니다", "메인", delegate
                    {
                        AudioManager.InactiveAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM);
                        ReturnMainTitle();
                    });
                }
                else
                {
                    BattleFieldOutpost.Current.AddChallengeLevel();
                    ui.ShowApplyPopup("승리", "다음단계로", delegate
                    {
                        AudioManager.InactiveAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM);
                        ReturnMockGame();
                    });
                }
                break;
            case TYPE_BATTLE_RESULT.Draw:
                AudioManager.ActivateAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM, false);
                ui.ShowOkAndCancelPopup("무승부", "재시작", "메인", delegate {
                    AudioManager.InactiveAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM);
                    ReturnMockGame();
                }, delegate {
                    AudioManager.InactiveAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM);
                    ReturnMainTitle(); 
                });
                break;
        }
    }

    public void SetBattleTurnOrder(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        _uiBattleTurnPanel.SetBattleTurnOrderText(typeTeam, typeBattleTurn);
    }

    public void ActivateUnitSetting(bool isActive)
    {
        _uiUnitSettingsPanel.SetActive(isActive);
    }

    #region ##### Listener #####

    /// <summary>
    /// 다음턴 진행 이벤트
    /// </summary>
    private System.Action _nextTurnEvent;

    public void SetOnNextTurnListener(System.Action act) => _nextTurnEvent = act;



    /// <summary>
    /// 유닛 정보 이벤트
    /// </summary>
    /// <param name="uActor"></param>
    /// <param name="screenPosition"></param>
    private void ShowUnitInformationEvent(UnitActor uActor, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UIUnitInformation>();
        ui.Show(uActor);
        ui.SetPosition(screenPosition);
    }

    /// <summary>
    /// 유닛 정보 이벤트
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="screenPosition"></param>
    private void ShowUnitInformationEvent(UnitCard uCard, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UIUnitInformation>();
        ui.Show(uCard, screenPosition);
    }

    /// <summary>
    /// 유닛 배치 수정 이벤트
    /// </summary>
    /// <param name="uActor"></param>
    public void OnUnitModifiedClickedEvent(UnitActor uActor)
    {
        _battleFieldManager.DragUnit(uActor);
    }

    /// <summary>
    /// 유닛 반납 이벤트
    /// </summary>
    /// <param name="uActor"></param>
    public void OnUnitReturnClickedEvent(UnitActor uActor)
    {
        if (_uiBattleSquadLayout.ReturnUnit(uActor))
            _battleFieldManager.ReturnUnit(uActor);
    }

    /// <summary>
    /// 유닛 배치 취소 이벤트
    /// </summary>
    public void OnUnitCancelClickedEvent()
    {
        _battleFieldManager.CancelChangeUnit();
    }


   
    #endregion
}

