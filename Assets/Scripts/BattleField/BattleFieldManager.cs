using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public enum TYPE_BATTLE_ROUND { Morning, Evening, Night}
public enum TYPE_TEAM { None = -1, Left, Right }

public enum TYPE_BATTLEFIELD { Setting, Order, Battle}

/// <summary>
/// �������� �� ��� ����
/// </summary>
public class BattleFieldManager : MonoBehaviour
{
    private readonly string CANVAS_BATTLEFIELD_KEY = "Canvas@BattleField";

    private UIBattleField _uiGame;

    private FieldGenerator _fieldGenerator;

    private UnitManager _unitManager;

    private static CommanderCamp _commanderCamp = null;

    private TYPE_BATTLEFIELD _typeBattleField;

    public TYPE_TEAM _firstTypeTeam = TYPE_TEAM.Right;

    private TYPE_TEAM _dropTeam = TYPE_TEAM.Left;

    public TYPE_BATTLE_ROUND _typeBattleRound;

    private Coroutine _battleCoroutine = null;
    public bool isRunning => _battleCoroutine != null;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        _uiGame = FindObjectOfType<UIBattleField>();
        if(_uiGame == null)
        {
            var obj = DataStorage.Instance.GetDataOrNull<GameObject>(CANVAS_BATTLEFIELD_KEY, null, null);
            var instance = Instantiate(obj);
            _uiGame = instance.GetComponent<UIBattleField>();
        }


        //Canvas Event ���
        _uiGame?.Initialize(this);
        _uiGame?.SetOnNextTurnListener(NextTurn);
        _uiGame?.SetOnDragUnitListener(DragUnit);
        _uiGame?.SetOnDropUnitListener(DropUnit);
        _uiGame?.AddBattleTurnListener(SetTypeBattleTurns);
        _uiGame?.AddCancelUnitListener(CancelChangeUnit);
        _uiGame?.AddModifiedUnitListener(DragUnit);
        _uiGame?.AddReturnUnitListener(ReturnUnit);



        _typeBattleRound = TYPE_BATTLE_ROUND.Morning;

        if (_fieldGenerator == null) _fieldGenerator = GetComponentInChildren<FieldGenerator>();
        _fieldGenerator.Initialize();

#if UNITY_EDITOR
        if (BattleFieldOutpost.Current == null)
        {
            _commanderCamp = InitializeTestGame();
        }
        else
        {
            _commanderCamp = InitializeMockGame();
        }
#else
        _commanderCamp = InitializeMockGame();
#endif

        _commanderCamp.AddAllHealthListener(_uiGame.ShowHealth);

        //UI ����
        _commanderCamp.DecreaseHealth(TYPE_TEAM.Left, 0);
        _commanderCamp.DecreaseHealth(TYPE_TEAM.Right, 0);

        _commanderCamp.AddSupplyListener(TYPE_TEAM.Left, _uiGame.ShowSupply);
        _commanderCamp.Supply(TYPE_TEAM.Left);


        if (_unitManager == null) _unitManager = GetComponentInChildren<UnitManager>();
        _unitManager.CreateCastleUnit(TYPE_TEAM.Left);
        _unitManager.CreateCastleUnit(TYPE_TEAM.Right);

        



        _uiGame?.SetUnitData(_commanderCamp.GetUnitDataArray(TYPE_TEAM.Left));
        _uiGame?.SetSupply(_commanderCamp.NowSupplyValue(TYPE_TEAM.Left), _commanderCamp.GetSupplyRate(TYPE_TEAM.Left));
        _uiGame?.ActivateUnitCardPanel();

    }


    private void OnDestroy()
    {
        _commanderCamp.RemoveAllHealthListener(_uiGame.ShowHealth);
        _commanderCamp.RemoveSupplyListener(TYPE_TEAM.Left, _uiGame.ShowSupply);

        //_uiGame?.SetOnNextTurnListener(NextTurn);
        //_uiGame?.SetOnDragUnitListener(DragUnit);
        //_uiGame?.SetOnDropUnitListener(DropUnit);
        _uiGame?.RemoveBattleTurnListener(SetTypeBattleTurns);
        _uiGame?.RemoveCancelUnitListener(CancelChangeUnit);
        _uiGame?.RemoveModifiedUnitListener(DragUnit);
        _uiGame?.RemoveReturnUnitListener(ReturnUnit);

        _fieldGenerator.CleanUp();
    }


    private void Start()
    {
        if (_firstTypeTeam == TYPE_TEAM.Right)
            CreateEnemyUnits();

    }

    void Update()
    {

        /// <summary>
        /// �Է� ��üȭ - ���� �ý���
        /// �Է� �ý��� �ʿ�
        /// EventSystems�� �����?
        /// </summary>

        var screenPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (_unitManager.IsDrag())
            {
                _unitManager.ClickedAction(screenPosition);
            }
            else {
                if (IsOrder())
                {
                    _uiGame.ClickAction(screenPosition);
                }
            }          
        }

        if (_unitManager.IsDrag())
        {
            _unitManager.ClickAction(screenPosition);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiGame.EscapeAction();
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// �׽�Ʈ ���� ��ġ
    /// </summary>
    private CommanderCamp InitializeTestGame()
    {

        BattleFieldOutpost.InitializeBattleFieldOutpost();

        Debug.LogWarning("BattleField TestMode");

        var dataArrayL = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();
        var dataArrayR = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();

        dataArrayL = dataArrayL.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3).ToArray();
        dataArrayR = dataArrayR.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3).ToArray();

        var uCardsL = UnitCard.Create(dataArrayL);// _unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");
        var uCardsR = UnitCard.Create(dataArrayR); //_unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");
        

        var leftCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Raty")), uCardsL, 0);
        leftCommandActor.SetTeam(TYPE_TEAM.Left);

        var rightCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Raty")), uCardsR, 0);
        rightCommandActor.SetTeam(TYPE_TEAM.Right);

        var commanderCamp = CommanderCamp.Create(leftCommandActor, rightCommandActor);
        return commanderCamp;

    }

#endif

    /// <summary>
    /// ���翡�� �������� (�Ϲ�)
    /// </summary>
    private CommanderCamp InitializeMockGame()
    {
        var leftCommandActor = BattleFieldOutpost.Current.regionL.commanderActor;
        leftCommandActor.SetTeam(TYPE_TEAM.Left);

        var rightCommandActor = BattleFieldOutpost.Current.regionR.commanderActor;
        rightCommandActor.SetTeam(TYPE_TEAM.Right);

        var commanderCamp = CommanderCamp.Create(leftCommandActor, rightCommandActor);

        return commanderCamp;
    }




    /// <summary>
    /// ���� ��� ����
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="typeBattleTurns"></param>
    public void SetTypeBattleTurns(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN[] typeBattleTurns)
    {
        _commanderCamp.SetTypeBattleTurns(typeTeam, typeBattleTurns);
    }


    //��ġ AI ���� �ʿ�
    private void CreateEnemyUnits()
    {
        //�⺻
        //������ ������ �����켱
        //����� ������ ��ݿ켱


        //���� �Ⱙ �տ��� ���� (������ ���� ������ �Ǵ��ϰ� �׿� ���� ��ġ)
        //���� �տ��� ���� (������ ���� ������ �Ǵ��ϰ� �׿� ���� ��ġ)
        //��� �ڿ��� ���� (������ ���� ������ �Ǵ��ϰ� �׿� ���� ��ġ)
        //���� ������� ����ϸ鼭 ����
        //���� ���� �˰��� ����
        //��� �켱 - ��ݺ� ���� ��ġ
        //���� �켱 - ���� ���� ��ġ
        //���� �켱 - ���� �� �Ⱙ ���� ��ġ
        //���� - �� ������ ���� ���� ���� ��ġ

        //���� - ������ ������
        //������ - ���� �� ����
        //������ - ��� �� ����


        var rightCommanderActor = _commanderCamp.GetCommanderActor(TYPE_TEAM.Right);
        CreateUnitActorForAI(rightCommanderActor);
        UnitManager.CastSkills(rightCommanderActor, TYPE_SKILL_CAST.DeployCast);
    }

    //public int GetCastleHealth(TYPE_TEAM typeTeam)
    //{
    //    switch (typeTeam)
    //    {
    //        case TYPE_TEAM.Left:
    //            return _leftCommandActor.nowCastleHealthValue;
    //        case TYPE_TEAM.Right:
    //            return _rightCommandActor.nowCastleHealthValue;
    //    }
    //    return -1;
    //}

    //public float GetCastleHealthRate(TYPE_TEAM typeTeam)
    //{
    //    switch (typeTeam)
    //    {
    //        case TYPE_TEAM.Left:
    //            return _leftCommandActor.GetCastleHealthRate();
    //        case TYPE_TEAM.Right:
    //            return _rightCommandActor.GetCastleHealthRate();
    //    }
    //    return -1f;
    //}

    //public void AddSupplyListener(System.Action<TYPE_TEAM, int, float> act)
    //{
    //    _leftCommandActor.AddSupplyListener(act);
    //}
    //public void RemoveSupplyListener(System.Action<TYPE_TEAM, int, float> act)
    //{
    //    _leftCommandActor.RemoveSupplyListener(act);
    //}


    //public void AddHealthListener(System.Action<TYPE_TEAM, int, float> act)
    //{
    //    _leftCommandActor.AddHealthListener(act);
    //    _rightCommandActor.AddHealthListener(act);
    //}
    //public void RemoveHealthListener(System.Action<TYPE_TEAM, int, float> act)
    //{
    //    _leftCommandActor.RemoveHealthListener(act);
    //    _rightCommandActor.RemoveHealthListener(act);
    //}

   
    /// <summary>
    /// ������ ����
    /// </summary>
    public void NextTurn()
    {
        switch (_typeBattleField)
        {
            case TYPE_BATTLEFIELD.Setting:
                if (_unitManager.IsLivedUnitCount(TYPE_TEAM.Left) == 0)
                {
                    _uiGame.ShowUnitSettingIsZeroPopup();
                }
                else
                {
                    _uiGame.ActivateBattleRoundPanel();
                    if (_firstTypeTeam == TYPE_TEAM.Left)
                        CreateEnemyUnits();

                    _typeBattleField++;
                }

                break;
            case TYPE_BATTLEFIELD.Order:

                var arr = _commanderCamp.GetTypeBattleTurns(TYPE_TEAM.Left);
                if (arr.Length == BattleFieldSettings.BATTLE_TURN_COUNTER)
                {
                    if (_battleCoroutine == null)
                    {
                        AudioManager.ActivateAudio("Warhorn", AudioManager.TYPE_AUDIO.SFX);
                        _battleCoroutine = StartCoroutine(TurnCoroutine(arr, _commanderCamp.GetTypeBattleTurns(TYPE_TEAM.Right)));
                        _typeBattleField++;
                    }
                }
                else
                {
                    _uiGame.ActivateBattleRoundPanel();
                    _uiGame.ShowIsNotEnoughCommandPopup();
                }
                break;
        }
    }

    
    /// <summary>
    /// ������ �׽�Ʈ
    /// </summary>
    /// <param name="typeBattleTurnL"></param>
    /// <param name="typeBattleTurnR"></param>
    public void NextTurnTester(TYPE_BATTLE_TURN typeBattleTurnL, TYPE_BATTLE_TURN typeBattleTurnR)
    {
        if (_battleCoroutine == null)
        {
            _battleCoroutine = StartCoroutine(TurnTestCoroutine(typeBattleTurnL, typeBattleTurnR));
        }
    }

    //BattleFieldController �ʿ�
    //���� ���� �ڷ�ƾ

    //public string GetSupply(TYPE_TEAM typeTeam)
    //{
    //    switch (typeTeam)
    //    {
    //        case TYPE_TEAM.Left:
    //            return _leftCommandActor.ToSupplyString();
    //        case TYPE_TEAM.Right:
    //            return _rightCommandActor.ToSupplyString();
    //    }
    //    return null;
    //}

    //public string AddedSupply()
    //{
    //    return _leftCommandActor.supplyValue.ToString();
    //}

    //public float SupplyRate()
    //{
    //    return _leftCommandActor.GetSupplyRate();
    //}


    /// <summary>
    /// ��
    /// </summary>
    private int _minimumTurn = BattleFieldSettings.BATTLE_TURN_COUNTER;

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <returns></returns>
    private bool IsBattleEnd()
    {
        if (_minimumTurn == 0) return true;

        if (IsGameEnd())
        {
            _isReady = true;
            return true;
        }

        //return false;
        if (_unitManager.IsLiveUnitsEmpty()) 
        {
            _firstTypeTeam = (_unitManager.IsLivedUnitCount(TYPE_TEAM.Left) == 0) ? TYPE_TEAM.Right : TYPE_TEAM.Left;
            _isReady = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// �ڵ� ���� ����
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    private bool IsAutoBattleEnd(TYPE_TEAM typeTeam)
    {
        if (IsGameEnd())
        {
            return true;
        }

        return (_unitManager.IsLivedUnitCount(typeTeam) == 0);
    }




    /// <summary>
    /// �׽�Ʈ�� �� �ڷ�ƾ
    /// </summary>
    /// <param name="battleTurnsLeft"></param>
    /// <param name="battleTurnsRight"></param>
    /// <returns></returns>
    IEnumerator TurnTestCoroutine(TYPE_BATTLE_TURN battleTurnsLeft, TYPE_BATTLE_TURN battleTurnsRight)
    {
        _uiGame.ActivateBattlePanel();

        StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Left, battleTurnsLeft));
        StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Right, battleTurnsRight));

        yield return null;

        while (_unitManager.isRunning)
        {
            yield return null;
        }

        UnitActorTurn();

        _battleCoroutine = null;

        yield return null;
    }



    /// <summary>
    /// �� �ڷ�ƾ
    /// </summary>
    /// <param name="battleTurnsLeft"></param>
    /// <param name="battleTurnsRight"></param>
    /// <returns></returns>
    IEnumerator TurnCoroutine(TYPE_BATTLE_TURN[] battleTurnsLeft, TYPE_BATTLE_TURN[] battleTurnsRight)
    {
        _uiGame.ActivateBattlePanel();
        
        _isReady = false;
        _isAutoBattle = false;

        _minimumTurn = battleTurnsLeft.Length;

        if (!_isReady)
        {
            yield return _unitManager.SetPreActiveActionUnits(_commanderCamp.GetCommanderActor(TYPE_TEAM.Left), _commanderCamp.GetCommanderActor(TYPE_TEAM.Right));

            while (!IsBattleEnd())
            {
                var leftOrder = battleTurnsLeft[battleTurnsLeft.Length - _minimumTurn];
                var rightOrder = battleTurnsRight[battleTurnsRight.Length - _minimumTurn];

                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Left, leftOrder));
                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Right, rightOrder));

                _uiGame.SetBattleTurnOrder(TYPE_TEAM.Left, leftOrder);
                _uiGame.SetBattleTurnOrder(TYPE_TEAM.Right, rightOrder);

                while (_unitManager.isRunning)
                {
                    yield return null;
                }
                _minimumTurn--;
            }

            yield return _unitManager.ReleasePreActiveActionUnits();

            //���� �Ʊ��� ����������
            if (!_isReady)
            {
                Debug.Log("ActivateBattleRound");
                UnitActorTurn();
                _typeBattleField = TYPE_BATTLEFIELD.Order;
                _uiGame.ActivateBattleRoundPanel();
                _battleCoroutine = null;
                yield break;
            }
        }

        UnitActorTurn();

        if (IsGameEnd())
        {
            _uiGame.GameEnd(GameResult());
            yield break;
        }


        if (_firstTypeTeam == TYPE_TEAM.Right)
        {
            while (_isReady)
            {
                yield return null;
                _isReady = false;
                _isAutoBattle = true;
            }
        }
        else
        {
            while (_isReady)
            {
                _uiGame.ShowAutoBattlePopup(SetAutoBattle);
                yield return null;
            }
        }


        while (_isReady)
        {
            yield return null;
        }

        if (_isAutoBattle)
        {
            int cnt = 3;
            while (!IsAutoBattleEnd(_firstTypeTeam))
            {
                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Left, TYPE_BATTLE_TURN.Forward));
                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Right, TYPE_BATTLE_TURN.Forward));

                while (_unitManager.isRunning)
                {
                    yield return null;
                }

                cnt--;
                if(cnt == 0)
                {
                    UnitActorTurn();
                    cnt = 3;
                }
            }
        }

        if (IsGameEnd())
        {
            _uiGame.GameEnd(GameResult());
            yield break;
        }


        NextRound();
        _battleCoroutine = null;

        yield return null;
    }



    /// <summary>
    /// ���� ���
    /// </summary>
    /// <returns></returns>
    private TYPE_BATTLE_RESULT GameResult()
    {
        return _commanderCamp.GameResult();        
    }


    /// <summary>
    /// �ڵ����� ����
    /// </summary>
    /// <param name="isAutoBattle"></param>
    private void SetAutoBattle(bool isAutoBattle)
    {
        _isAutoBattle = isAutoBattle;
        _isReady = false;

        if (!isAutoBattle)
        {
            //���޹ݳ�
            _unitManager.ReturnUnitCards(_commanderCamp.GetCommanderActor(TYPE_TEAM.Left));
        }
    }

    /// <summary>
    /// ���� ��
    /// </summary>
    private void UnitActorTurn()
    {
        var blocks = FieldManager.GetAllBlocks();
        for (int i = 0; i < blocks.Length; i++)
        {
            if (!blocks[i].IsHasUnitActor())
            {
                blocks[i].Turn();
            }
        }
    }



    [HideInInspector]
    public bool _isReady = false;
    bool _isAutoBattle = false;



    //    public void AutoBattle()
    //    {
    //        _isReady = false;
    //        _isAutoBattle = true;
    //    }
#if UNITY_EDITOR
    /// <summary>
    /// ���� �� �׽�Ʈ
    /// </summary>
    public void NextRoundTest()
    {
        NextRound();
    }
#endif

    /// <summary>
    /// ���� ����
    /// </summary>
    private void NextRound()
    {
        if(_typeBattleRound == TYPE_BATTLE_ROUND.Night)
        {
            _uiGame.GameEnd(GameResult());
            return;
        }

        _unitManager.ClearUnitCards();
        _unitManager.ClearDeadUnits();

        //��ģ ���� ȸ��
        _commanderCamp.RecoveryAllUnits();

        _typeBattleField = TYPE_BATTLEFIELD.Setting;

        //���� ����
        _typeBattleRound++;

        _isReady = false;
        _isAutoBattle = false;

        //UI ����
        _uiGame.UpdateUnits();
        _uiGame.ActivateUnitCardPanel();
        _uiGame.SetBattleRound(_typeBattleRound);

        if (_firstTypeTeam == TYPE_TEAM.Right)
            CreateEnemyUnits();
    }

    /// <summary>
    /// �ʵ忡 �ִ� ��� ������ �����մϴ�
    /// </summary>
    public void ClearAllUnits(bool isIncludeCastle = false)
    {
        _unitManager.ClearAllUnits(isIncludeCastle);
    }


    /// <summary>
    /// ����
    /// </summary>
    //public void Retreat()
    //{
    //    _isReady = false;
    //    //���� ���� ���� �����ϱ�
    //    //_unitManager.RetreatUnits(_leftCommandActor);
    //    //_unitManager.RetreatUnits(_rightCommandActor);
    //}

    /// <summary>
    /// ü�� ����
    /// </summary>
    public static void IncreaseHealth(int damageValue, TYPE_TEAM typeTeam)
    {
        _commanderCamp.DecreaseHealth(typeTeam, damageValue);
    }

    /// <summary>
    /// ���� ��
    /// </summary>
    /// <returns></returns>
    public TYPE_TEAM GetNowTeam()
    {
        return _dropTeam;
    }

    /// <summary>
    /// ���� �� ���ϱ�
    /// </summary>
    /// <param name="typeTeam"></param>
    public void SetNowTeam(TYPE_TEAM typeTeam)
    {
        _dropTeam = typeTeam;
    }

    /// <summary>
    /// ��� �ܰ����� Ȯ��
    /// </summary>
    /// <returns></returns>
    public bool IsOrder() => _typeBattleField == TYPE_BATTLEFIELD.Setting;

    /// <summary>
    /// ���޷� �������� Ȯ��
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="nowTypeTeam"></param>
    /// <returns></returns>
    public bool IsSupply(UnitCard uCard, TYPE_TEAM typeTeam = TYPE_TEAM.Left)
    {
        return _commanderCamp.IsSupply(uCard, typeTeam);
    }




    /// <summary>
    /// AI ��ġ
    /// </summary>
    /// <param name="cActor"></param>
    public void CreateUnitActorForAI(ICommanderActor cActor)
    {
        var blocks = FieldManager.GetTeamUnitBlocksFromVertical(cActor.typeTeam);
        var list = cActor.unitDataArray.ToArray();//.OrderBy(uCard => uCard.typeUnitClass).ToArray();


        //List<UnitCard> cards = new List<UnitCard>();
        //for (int i = 0; i < list.Length; i++)
        //{
        //    if (list[i].typeUnitClass == TYPE_UNIT_CLASS.Charger || list[i].typeUnitClass == TYPE_UNIT_CLASS.HeavySoldier)
        //        cards.Insert(0, list[i]);
        //    else
        //        cards.Add(list[i]);
        //}

        //list = cards.ToArray();

        for (int i = 0; i < list.Length; i++)
        {
            for(int j = 0; j < blocks.Length; j++)
            {
                var block = blocks[j];
                var uCard = list[i];

                //����� ���� �����̼��� ����
                var formationCells = new List<Vector2Int>();
                var uKeys = new List<int>();

                for (int k = 0; k < uCard.UnitKeys.Length; k++)
                {
                    if (!uCard.IsDead(uCard.UnitKeys[k]))
                    {
                        formationCells.Add(uCard.formationCells[k]);
                        uKeys.Add(uCard.UnitKeys[k]);
                    }
                }

                //�����̼� ��� ��������
                var formationBlocks = FieldManager.GetFormationBlocks(block.coordinate, formationCells.ToArray(), cActor.typeTeam);

                if (uKeys.Count == formationBlocks.Length)
                {
                    bool isCheck = false;
                    for (int k = 0; k < formationBlocks.Length; k++)
                    {
                        if (formationBlocks[k].IsHasGroundUnitActor())
                        {
                            isCheck = true;
                            break;
                        }
                    }

                    if (!isCheck && cActor.IsSupply(uCard))
                    {
                        cActor.UseSupply(uCard);
                        _unitManager.CreateUnits(uCard, uKeys.ToArray(), formationBlocks, cActor.typeTeam);
                        break;
                    }
                }
                
            }
        }
    }

    /// <summary>
    /// ���� ��ġ
    /// </summary>
    /// <param name="cActor"></param>
    //public void CreateUnit(ICommanderActor cActor)
    //{
    //    var block = FieldManager.GetRandomBlock(cActor.typeTeam);

    //    if (block != null && !block.IsHasUnitActor())
    //    {
    //        if (cActor.unitDataArray.Length > 0)
    //        {
    //            //���� ī�� ��������
    //            var uCard = cActor.unitDataArray[Random.Range(0, cActor.unitDataArray.Length)];

    //            //�̹� ī�� �����
    //            if (_unitManager.IsUsedCard(uCard)) return;

    //            //����� ���� �����̼��� ����
    //            var formationCells = new List<Vector2Int>();
    //            var uKeys = new List<int>();

    //            for (int i = 0; i < uCard.UnitKeys.Length; i++)
    //            {
    //                if (!uCard.IsDead(uCard.UnitKeys[i]))
    //                {
    //                    formationCells.Add(uCard.formationCells[i]);
    //                    uKeys.Add(uCard.UnitKeys[i]);
    //                }
    //            }

    //            //�����̼� ��� ��������
    //            var blocks = FieldManager.GetFormationBlocks(block.coordinate, formationCells.ToArray(), cActor.typeTeam);

    //            if (uKeys.Count == blocks.Length)
    //            {

    //                bool isCheck = false;
    //                for (int i = 0; i < blocks.Length; i++)
    //                {
    //                    if (blocks[i].IsHasGroundUnitActor())
    //                    {
    //                        isCheck = true;
    //                        break;
    //                    }
    //                }

    //                if (!isCheck && cActor.IsSupply(uCard))
    //                {
    //                    cActor.UseSupply(uCard);
    //                    _unitManager.CreateUnits(uCard, uKeys.ToArray(), blocks, cActor.typeTeam);
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// �ʵ忡 ���� ��ġ
    /// </summary>
    public void CreateFieldUnit(TYPE_TEAM typeTeam)
    {
        var cActor = _commanderCamp.GetCommanderActor(typeTeam);

        var blocks = FieldManager.GetTeamUnitBlocksFromHorizental(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && !block.IsHasUnitActor())
            {
                var uCard = cActor.unitDataArray[Random.Range(0, cActor.unitDataArray.Length)];

                if (uCard != null)
                {
                    var uCardTmp = UnitCard.Create(uCard.UnitData);
                    var uKey = uCardTmp.UnitKeys[0];
                    _unitManager.CreateUnit(uCardTmp, uKey, block, typeTeam);
                }                
            }
        }
    }


#if UNITY_EDITOR
    /// <summary>
    /// �׽�Ʈ�� ���� ������
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="unit"></param>
    public void CreateFieldUnitInTest(TYPE_TEAM typeTeam, UnitData unit)
    {
        var blocks = FieldManager.GetTeamUnitBlocksFromHorizental(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && !block.IsHasGroundUnitActor())
            {
                var uCardTmp = UnitCard.CreateTest(unit);
                var uKey = uCardTmp.UnitKeys[0];
                _unitManager.CreateUnit(uCardTmp, uKey, block, typeTeam);
            }
        }
    }
#endif

    /// <summary>
    /// ���� �巡��
    /// </summary>
    /// <param name="uCard"></param>
    public bool DragUnit(UnitCard uCard)
    {
        if (IsSupply(uCard))
        {
            if (BattleFieldSettings.SingleFormation)
            {
                var card = UnitCard.Create(uCard.UnitData);
                card.SetFormation(new Vector2Int[] { new Vector2Int(0, 0) });
                _unitManager.DragUnitActor(card, _dropTeam);
            }
            else
            {
                _unitManager.DragUnitActor(uCard, _dropTeam);
            }
            _uiGame?.ActivateUnitSetting(true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// ���� �巡��
    /// </summary>
    /// <param name="uActor"></param>
    public void DragUnit(IUnitActor uActor)
    {
        _unitManager.DragUnitActor(uActor);
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    /// <param name="uCard"></param>
    /// <returns></returns>
    public bool DropUnit(UnitCard uCard)
    {
        _uiGame?.ActivateUnitSetting(false);

        var leftCommanderActor = _commanderCamp.GetCommanderActor(TYPE_TEAM.Left);
        if (_unitManager.DropUnitActor(leftCommanderActor, uCard))
        {
            _commanderCamp.UseSupply(TYPE_TEAM.Left, uCard);
            UnitManager.CastSkills(leftCommanderActor, TYPE_SKILL_CAST.DeployCast);
            _uiGame.SetSupply(leftCommanderActor.nowSupplyValue, leftCommanderActor.GetSupplyRate());
            return true;
        }
        return false;
    }

    /// <summary>
    /// ���� �ݳ�
    /// </summary>
    /// <param name="uActor"></param>
    public void ReturnUnit(IUnitActor uActor)
    {
        _unitManager.ReturnUnitActor(uActor);
    }

    /// <summary>
    /// �����ϴ� ���� ���
    /// </summary>
    public void CancelChangeUnit()
    {
        _unitManager.CancelChangeUnitActor();
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <returns></returns>
    private bool IsGameEnd()
    {
        return _commanderCamp.IsGameEnd();
    }
}
