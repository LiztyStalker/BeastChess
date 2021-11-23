using UnityEngine;
using UnityEngine.UI;

public enum TYPE_BATTLE_TURN {None = -1, Forward, Charge, Guard, Backward }

public enum TYPE_BATTLE_RESULT { Victory, Draw, Defeat}

public class UIBattleField : MonoBehaviour
{
    //private BattleFieldManager _battleFieldManager;

    private UIBattleStatusLayout _uiBattieStatusLayout;

    private UIBattleCommand _uiBattleCommandLayout;

    private UIBattleSupply _uiBattleSupplyLayout;

    private UIBattleSquadLayout _uiBattleSquadLayout;

    private UIUnitSelector _uiUnitSelector;

    private UIBattleTurnPanel _uiBattleTurnPanel;

    private UIBattleFieldMenu _uiBattleFieldMenu;

    [SerializeField]
    private GameObject _uiUnitSettingsPanel;

    [SerializeField]
    private Button _nextTurnButton;

    [SerializeField]
    private Button _helpButton;

    [SerializeField]
    private Button _menuButton;

    public void Initialize()
    {
        SetComponent(ref _uiBattieStatusLayout);
        _uiBattieStatusLayout.Initialize();

        SetComponent(ref _uiBattleSquadLayout);
        _uiBattleSquadLayout.Initialize();
        _uiBattleSquadLayout.SetOnDragListener(DragUnitEvent);
        _uiBattleSquadLayout.SetOnDropListener(DropUnitEvent);
        _uiBattleSquadLayout.SetOnUnitInformationListener(ShowUnitInformationEvent);

        SetComponent(ref _uiUnitSelector);

        SetComponent(ref _uiBattieStatusLayout);
        _uiUnitSelector.Initialize();
        _uiUnitSelector.SetOnInformationListener(ShowUnitInformationEvent);
        _uiUnitSelector.SetOnDragListener(OnModifiedUnitClickedEvent);
        _uiUnitSelector.SetOnCancelListener(OnCancelUnitClickedEvent);
        _uiUnitSelector.SetOnReturnListener(OnReturnUnitClickedEvent);

        SetComponent(ref _uiBattleCommandLayout);
        _uiBattleCommandLayout.Initialize();

        SetComponent(ref _uiBattleSupplyLayout);

        SetComponent(ref _uiBattleTurnPanel);
        _uiBattleTurnPanel.SetAnimator(false);

        SetComponent(ref _uiBattleFieldMenu);
        _uiBattleFieldMenu.Initialize();
        _uiBattleFieldMenu.AddOnClosedListener(ClosedMenuEvent);
        _uiBattleFieldMenu.SetOnRetryListener(GameRetryEvent);
        _uiBattleFieldMenu.SetOnReturnListener(GameReturnEvent);
        _uiBattleFieldMenu.SetOnSurrenderListener(GameSurrenderEvent);

        ActivateUnitSetting(false);

        _nextTurnButton.onClick.AddListener(NextTurnEvent);
        _helpButton.onClick.AddListener(ShowHelpEvent);
        _menuButton.onClick.AddListener(ShowMenuEvent);

        AudioManager.ActivateAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM, true);
    }

    public void CleanUp()
    {
        _uiBattleFieldMenu.RemoveOnClosedListener(ClosedMenuEvent);
        _uiBattleFieldMenu.CleanUp();

        _uiBattleSquadLayout.CleanUp();
        _uiUnitSelector.CleanUp();
        _uiBattleCommandLayout.CleanUp();

        _nextTurnButton.onClick.RemoveListener(NextTurnEvent);
        _helpButton.onClick.RemoveListener(ShowHelpEvent);
        _menuButton.onClick.RemoveListener(ShowMenuEvent);
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
        ui.ShowApplyPopup("3번의 명령을 적용해야 합니다.");
    }

    /// <summary>
    /// 유닛 데이터를 업데이트 합니다
    /// </summary>
    public void UpdateUnits() => _uiBattleSquadLayout.UpdateUnits();

    /// <summary>
    /// 새 병사카드 드래그
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DragUnitEvent(UnitCard uCard) => _dragUnitEvent(uCard);
    

    /// <summary>
    /// 새 병사카드 드롭
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DropUnitEvent(UnitCard uCard) => _dropUnitEvent(uCard);

    /// <summary>
    /// 현재 전투 라운드를 보입니다
    /// </summary>
    /// <param name="typeBattleRound"></param>
    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound) => _uiBattieStatusLayout.SetBattleRound(typeBattleRound);

    /// <summary>
    /// Supply 수치를 적용합니다
    /// </summary>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void SetSupply(int value, float rate) => _uiBattleSupplyLayout.SetSupply(value, rate);

    /// <summary>
    /// UnitCard[]를 적용합니다
    /// </summary>
    /// <param name="unitDataArray"></param>
    public void SetUnitData(UnitCard[] unitDataArray) => _uiBattleSquadLayout.SetUnitData(unitDataArray);

    #region ##### 커맨드 패턴 추가 필요 #####

    public void ClickAction(Vector2 screenPosition)
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            _uiUnitSelector.ShowSelectorMenu(TYPE_TEAM.Left, screenPosition);
        }
    }

    public void EscapeAction()
    {
        _uiUnitSelector.CloseMenu();

        if (UICommon.Current.IsCanvasActivated())
            UICommon.Current.NowCanvasHide();
    }

    #endregion


    /// <summary>
    /// 게임을 재시작합니다
    /// </summary>
    private void RetryGame()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
        LoadManager.SetNextSceneName("Test_BattleField");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
        BattleFieldOutpost.Current.AllRecovery();
    }

    /// <summary>
    /// 모의전으로 돌아갑니다
    /// </summary>
    private void ReturnMockGame()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    /// <summary>
    /// 메인으로 돌아갑니다
    /// </summary>
    private void ReturnMainTitle()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
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
    private void NextTurnEvent()
    {
        if (_uiBattleCommandLayout.isActiveAndEnabled)
        {
            _battleTurnEvent?.Invoke(TYPE_TEAM.Left, _uiBattleCommandLayout.GetTypeBattleTurnArray());
        }

        _nextTurnEvent?.Invoke();
        
    }

    private void ShowMenuEvent()
    {
        Time.timeScale = 0f;
        _uiBattleFieldMenu.Show();
    }

    private void ClosedMenuEvent()
    {
        Debug.Log("메뉴 닫힘");
    }

    private void ShowHelpEvent()
    {
        Time.timeScale = 0f;
        var ui = UICommon.Current.GetUICommon<UIHelpInformation>();
        ui.Show(4);
        ui.SetOnClosedListener(delegate
        {
            GameReturnEvent();
            ui.SetOnClosedListener(null);
        }
        );
    }

    private void GameReturnEvent()
    {
        GameReturnTimeScale();
    }

    private void GameReturnTimeScale()
    {
        Time.timeScale = 1f;
    }

    private void GameRetryEvent()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("정말로 전투를 다시 시작하시겠습니까?", "예", "아니오", delegate
        {
            RetryGame();
        }, null, closedCallback: GameReturnTimeScale);
    }

    private void GameSurrenderEvent()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("정말로 항복하고 막사로 돌아가시겠습니까?", "예", "아니오", delegate
        {
            ReturnMockGame();
        }, null, closedCallback: GameReturnTimeScale);
    }



    /// <summary>
    /// 게임을 마칩니다
    /// </summary>
    /// <param name="typeBattleResult"></param>
    public void GameEnd(TYPE_BATTLE_RESULT typeBattleResult)
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);

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

    /// <summary>
    /// 명령 패널
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="typeBattleTurn"></param>
    public void SetBattleTurnOrder(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        _uiBattleTurnPanel.SetBattleTurnOrderText(typeTeam, typeBattleTurn);
    }

    /// <summary>
    /// 유닛 배치 패널
    /// </summary>
    /// <param name="isActive"></param>
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
    public void OnModifiedUnitClickedEvent(UnitActor uActor)
    {
        _modifiedUnitEvent?.Invoke(uActor);
    }

    /// <summary>
    /// 유닛 반납 이벤트
    /// </summary>
    /// <param name="uActor"></param>
    public void OnReturnUnitClickedEvent(UnitActor uActor)
    {
        if (_uiBattleSquadLayout.ReturnUnit(uActor))
            _returnUnitEvent?.Invoke(uActor);
    }

    /// <summary>
    /// 유닛 배치 취소 이벤트
    /// </summary>
    public void OnCancelUnitClickedEvent()
    {
        _cancelUnitEvent?.Invoke();
    }

    /// <summary>
    /// 유닛 드래그 이벤트
    /// </summary>
    private System.Func<UnitCard, bool> _dragUnitEvent;
    public void SetOnDragUnitListener(System.Func<UnitCard, bool> act) => _dragUnitEvent = act;


    /// <summary>
    /// 유닛 재배치 이벤트
    /// </summary>
    private System.Action<UnitActor> _modifiedUnitEvent;
    public void AddModifiedUnitListener(System.Action<UnitActor> act) => _modifiedUnitEvent += act;
    public void RemoveModifiedUnitListener(System.Action<UnitActor> act) => _modifiedUnitEvent -= act;


    /// <summary>
    /// 유닛 놓기 이벤트
    /// </summary>
    private System.Func<UnitCard, bool> _dropUnitEvent;
    public void SetOnDropUnitListener(System.Func<UnitCard, bool> act) => _dropUnitEvent = act;


    /// <summary>
    /// 유닛 반납 이벤트
    /// </summary>
    private System.Action<UnitActor> _returnUnitEvent;
    public void AddReturnUnitListener(System.Action<UnitActor> act) => _returnUnitEvent += act;
    public void RemoveReturnUnitListener(System.Action<UnitActor> act) => _returnUnitEvent -= act;

    /// <summary>
    /// 유닛 배치 취소 이벤트
    /// </summary>
    private System.Action _cancelUnitEvent;
    public void AddCancelUnitListener(System.Action act) => _cancelUnitEvent += act;
    public void RemoveCancelUnitListener(System.Action act) => _cancelUnitEvent -= act;


    /// <summary>
    /// 플레이어 전투 턴 이벤트
    /// </summary>
    private System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> _battleTurnEvent;
    public void AddBattleTurnListener(System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> act) => _battleTurnEvent += act;
    public void RemoveBattleTurnListener(System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> act) => _battleTurnEvent -= act;

    #endregion
}

