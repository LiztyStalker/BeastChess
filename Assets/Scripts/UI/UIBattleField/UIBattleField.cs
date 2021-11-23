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
    /// ���� ������Ʈ�� �����մϴ�
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
            Debug.LogError($"{typeof(T).Name}�� ã�� �� �����ϴ�");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Supply ��ġ�� ���Դϴ�
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void ShowSupply(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattleSupplyLayout.SetSupply(value, rate);
    }

    /// <summary>
    /// ���� ���� Health ��ġ�� ���Դϴ�
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void ShowHealth(TYPE_TEAM typeTeam, int value, float rate)
    {
        _uiBattieStatusLayout.SetCastleHealth(typeTeam, value, rate);
    }

    /// <summary>
    /// �ڵ����� �˾��� ���ϴ�
    /// </summary>
    /// <param name="callback"></param>
    public void ShowAutoBattlePopup(System.Action<bool> callback)
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("������ ���� ��� �����Ͽ����ϴ�.\n���� ���� ���� �����Ͻðڽ��ϱ�?\n�ƴϸ� ������ �����ϱ� ���� �����Ͻðڽ��ϱ�?",
            "����", 
            "����", 
            delegate { callback?.Invoke(true); },
            delegate { callback?.Invoke(false); }, 
            null,
            true);
    }

    /// <summary>
    /// ���� Zero �˾��� ���ϴ�
    /// </summary>
    public void ShowUnitSettingIsZeroPopup()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("��ġ�� ������ �����ϴ�.\n������ 1�д� �̻� ��ġ�� �ּ���");
    }

    public void ShowIsNotEnoughCommandPopup()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("3���� ����� �����ؾ� �մϴ�.");
    }

    /// <summary>
    /// ���� �����͸� ������Ʈ �մϴ�
    /// </summary>
    public void UpdateUnits() => _uiBattleSquadLayout.UpdateUnits();

    /// <summary>
    /// �� ����ī�� �巡��
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DragUnitEvent(UnitCard uCard) => _dragUnitEvent(uCard);
    

    /// <summary>
    /// �� ����ī�� ���
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    private bool DropUnitEvent(UnitCard uCard) => _dropUnitEvent(uCard);

    /// <summary>
    /// ���� ���� ���带 ���Դϴ�
    /// </summary>
    /// <param name="typeBattleRound"></param>
    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound) => _uiBattieStatusLayout.SetBattleRound(typeBattleRound);

    /// <summary>
    /// Supply ��ġ�� �����մϴ�
    /// </summary>
    /// <param name="value"></param>
    /// <param name="rate"></param>
    public void SetSupply(int value, float rate) => _uiBattleSupplyLayout.SetSupply(value, rate);

    /// <summary>
    /// UnitCard[]�� �����մϴ�
    /// </summary>
    /// <param name="unitDataArray"></param>
    public void SetUnitData(UnitCard[] unitDataArray) => _uiBattleSquadLayout.SetUnitData(unitDataArray);

    #region ##### Ŀ�ǵ� ���� �߰� �ʿ� #####

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
    /// ������ ������մϴ�
    /// </summary>
    private void RetryGame()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
        LoadManager.SetNextSceneName("Test_BattleField");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
        BattleFieldOutpost.Current.AllRecovery();
    }

    /// <summary>
    /// ���������� ���ư��ϴ�
    /// </summary>
    private void ReturnMockGame()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
        LoadManager.SetNextSceneName("Test_MockGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    /// <summary>
    /// �������� ���ư��ϴ�
    /// </summary>
    private void ReturnMainTitle()
    {
        AudioManager.InactiveAudio("BGMGrass", AudioManager.TYPE_AUDIO.BGM);
        LoadManager.SetNextSceneName("Test_MainTitle");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    /// <summary>
    /// ����ī��â�� Ȱ��ȭ �մϴ�
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
    /// ���â�� Ȱ��ȭ �մϴ�
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
    /// ����â�� Ȱ��ȭ �մϴ�
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
    /// ���� ������ �����մϴ�
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
        Debug.Log("�޴� ����");
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
        ui.ShowOkAndCancelPopup("������ ������ �ٽ� �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�", delegate
        {
            RetryGame();
        }, null, closedCallback: GameReturnTimeScale);
    }

    private void GameSurrenderEvent()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowOkAndCancelPopup("������ �׺��ϰ� ����� ���ư��ðڽ��ϱ�?", "��", "�ƴϿ�", delegate
        {
            ReturnMockGame();
        }, null, closedCallback: GameReturnTimeScale);
    }



    /// <summary>
    /// ������ ��Ĩ�ϴ�
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
                ui.ShowOkAndCancelPopup("�й�", "�����", "����", ReturnMockGame, delegate {
                    AudioManager.InactiveAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM);
                    ReturnMainTitle();
                });
                break;
            case TYPE_BATTLE_RESULT.Victory:
                AudioManager.ActivateAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM, false);
                if (BattleFieldOutpost.Current.IsChallengeEnd())
                {
                    ui.ShowApplyPopup("ç�������� �¸��Ͽ����ϴ�.\n�÷������ּż� �����մϴ�", "����", delegate
                    {
                        AudioManager.InactiveAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM);
                        ReturnMainTitle();
                    });
                }
                else
                {
                    BattleFieldOutpost.Current.AddChallengeLevel();
                    ui.ShowApplyPopup("�¸�", "�����ܰ��", delegate
                    {
                        AudioManager.InactiveAudio("BGMVictory", AudioManager.TYPE_AUDIO.BGM);
                        ReturnMockGame();
                    });
                }
                break;
            case TYPE_BATTLE_RESULT.Draw:
                AudioManager.ActivateAudio("BGMDefeat", AudioManager.TYPE_AUDIO.BGM, false);
                ui.ShowOkAndCancelPopup("���º�", "�����", "����", delegate {
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
    /// ��� �г�
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="typeBattleTurn"></param>
    public void SetBattleTurnOrder(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        _uiBattleTurnPanel.SetBattleTurnOrderText(typeTeam, typeBattleTurn);
    }

    /// <summary>
    /// ���� ��ġ �г�
    /// </summary>
    /// <param name="isActive"></param>
    public void ActivateUnitSetting(bool isActive)
    {
        _uiUnitSettingsPanel.SetActive(isActive);
    }







    #region ##### Listener #####

    /// <summary>
    /// ������ ���� �̺�Ʈ
    /// </summary>
    private System.Action _nextTurnEvent;
    public void SetOnNextTurnListener(System.Action act) => _nextTurnEvent = act;



    /// <summary>
    /// ���� ���� �̺�Ʈ
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
    /// ���� ���� �̺�Ʈ
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="screenPosition"></param>
    private void ShowUnitInformationEvent(UnitCard uCard, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UIUnitInformation>();
        ui.Show(uCard, screenPosition);
    }


    /// <summary>
    /// ���� ��ġ ���� �̺�Ʈ
    /// </summary>
    /// <param name="uActor"></param>
    public void OnModifiedUnitClickedEvent(UnitActor uActor)
    {
        _modifiedUnitEvent?.Invoke(uActor);
    }

    /// <summary>
    /// ���� �ݳ� �̺�Ʈ
    /// </summary>
    /// <param name="uActor"></param>
    public void OnReturnUnitClickedEvent(UnitActor uActor)
    {
        if (_uiBattleSquadLayout.ReturnUnit(uActor))
            _returnUnitEvent?.Invoke(uActor);
    }

    /// <summary>
    /// ���� ��ġ ��� �̺�Ʈ
    /// </summary>
    public void OnCancelUnitClickedEvent()
    {
        _cancelUnitEvent?.Invoke();
    }

    /// <summary>
    /// ���� �巡�� �̺�Ʈ
    /// </summary>
    private System.Func<UnitCard, bool> _dragUnitEvent;
    public void SetOnDragUnitListener(System.Func<UnitCard, bool> act) => _dragUnitEvent = act;


    /// <summary>
    /// ���� ���ġ �̺�Ʈ
    /// </summary>
    private System.Action<UnitActor> _modifiedUnitEvent;
    public void AddModifiedUnitListener(System.Action<UnitActor> act) => _modifiedUnitEvent += act;
    public void RemoveModifiedUnitListener(System.Action<UnitActor> act) => _modifiedUnitEvent -= act;


    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    private System.Func<UnitCard, bool> _dropUnitEvent;
    public void SetOnDropUnitListener(System.Func<UnitCard, bool> act) => _dropUnitEvent = act;


    /// <summary>
    /// ���� �ݳ� �̺�Ʈ
    /// </summary>
    private System.Action<UnitActor> _returnUnitEvent;
    public void AddReturnUnitListener(System.Action<UnitActor> act) => _returnUnitEvent += act;
    public void RemoveReturnUnitListener(System.Action<UnitActor> act) => _returnUnitEvent -= act;

    /// <summary>
    /// ���� ��ġ ��� �̺�Ʈ
    /// </summary>
    private System.Action _cancelUnitEvent;
    public void AddCancelUnitListener(System.Action act) => _cancelUnitEvent += act;
    public void RemoveCancelUnitListener(System.Action act) => _cancelUnitEvent -= act;


    /// <summary>
    /// �÷��̾� ���� �� �̺�Ʈ
    /// </summary>
    private System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> _battleTurnEvent;
    public void AddBattleTurnListener(System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> act) => _battleTurnEvent += act;
    public void RemoveBattleTurnListener(System.Action<TYPE_TEAM, TYPE_BATTLE_TURN[]> act) => _battleTurnEvent -= act;

    #endregion
}

