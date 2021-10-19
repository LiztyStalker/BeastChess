using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TYPE_BATTLE_ROUND { Morning, Evening, Night}
public enum TYPE_TEAM { None = -1, Left, Right }

public enum TYPE_BATTLEFIELD { Setting, Order, Battle}


public class BattleFieldManager : MonoBehaviour
{
    private UIBattleField _uiGame;

    private FieldGenerator _fieldGenerator;

    private UnitManager _unitManager;

    private static ICommanderActor _leftCommandActor;
    private static ICommanderActor _rightCommandActor;

    private TYPE_BATTLEFIELD _typeBattleField;

    public TYPE_TEAM _firstTypeTeam = TYPE_TEAM.Right;

    private TYPE_TEAM _dropTeam = TYPE_TEAM.Left;

    public TYPE_BATTLE_ROUND _typeBattleRound;

    private Coroutine _battleCoroutine = null;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        _uiGame = FindObjectOfType<UIBattleField>();
        _uiGame?.Initialize(this);
        _uiGame?.SetOnNextTurnListener(NextTurn);

        _typeBattleRound = TYPE_BATTLE_ROUND.Morning;

        if (_fieldGenerator == null) _fieldGenerator = GetComponentInChildren<FieldGenerator>();
        _fieldGenerator.Initialize();

#if UNITY_EDITOR
        if (BattleFieldOutpost.Current == null)
        {
            InitializeTestGame();
        }
        else
        {
            InitializeMockGame();
        }
#else
        InitializeMockGame();
#endif

        _leftCommandActor.AddHealthListener(_uiGame.ShowHealth);
        _rightCommandActor.AddHealthListener(_uiGame.ShowHealth);

        //UI 갱신
        _leftCommandActor.DecreaseHealth(0);
        _rightCommandActor.DecreaseHealth(0);

        _leftCommandActor.AddHealthListener(_uiGame.ShowSupply);
        _leftCommandActor.Supply();

        if (_unitManager == null) _unitManager = GetComponentInChildren<UnitManager>();
        _unitManager.CreateCastleUnit(TYPE_TEAM.Left);
        _unitManager.CreateCastleUnit(TYPE_TEAM.Right);


        _uiGame?.SetUnitData(_leftCommandActor.unitDataArray);
        _uiGame?.SetSupply(_leftCommandActor.nowSupplyValue, _leftCommandActor.GetSupplyRate());
        _uiGame?.ActivateUnitCardPanel();

    }


    private void OnDestroy()
    {
        _leftCommandActor.RemoveHealthListener(_uiGame.ShowHealth);
        _rightCommandActor.RemoveHealthListener(_uiGame.ShowHealth);
        _leftCommandActor.RemoveHealthListener(_uiGame.ShowSupply);
        _fieldGenerator.CleanUp();
    }


    private void Start()
    {
        if (_firstTypeTeam == TYPE_TEAM.Right)
            CreateEnemyUnits();
    }

    void Update()
    {
        if (_unitManager.IsDrag()) return;
        //if (!isAuto && Input.GetKeyDown(KeyCode.Return))
        //{
        //    NextTurn();
        //}
    }



    private void InitializeTestGame()
    {
        Debug.LogWarning("BattleField TestMode");

        var dataArrayL = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();
        var dataArrayR = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();

        dataArrayL = dataArrayL.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3).ToArray();
        dataArrayR = dataArrayR.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3).ToArray();

        var uCardsL = UnitCard.Create(dataArrayL);// _unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");
        var uCardsR = UnitCard.Create(dataArrayR); //_unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");

        _leftCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Raty")), uCardsL, 0);
        _leftCommandActor.typeTeam = TYPE_TEAM.Left;

        _rightCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Raty")), uCardsR, 0);
        _rightCommandActor.typeTeam = TYPE_TEAM.Right;
    }

    private void InitializeMockGame()
    {
        _leftCommandActor = BattleFieldOutpost.Current.regionL.commanderActor;
        _leftCommandActor.typeTeam = TYPE_TEAM.Left;

        _rightCommandActor = BattleFieldOutpost.Current.regionR.commanderActor;
        _rightCommandActor.typeTeam = TYPE_TEAM.Right;    }

    public void SetTypeBattleTurns(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN[] typeBattleTurns)
    {
        if(typeTeam == TYPE_TEAM.Left)
        {
            _leftCommandActor.SetTypeBattleTurns(typeBattleTurns);
        }
        else
        {
            _rightCommandActor.SetTypeBattleTurns(typeBattleTurns);
        }
    }

    private void CreateEnemyUnits()
    {
        for (int i = 0; i < 100; i++)
        {
            CreateUnit(_rightCommandActor);
        }
        UnitManager.CastSkills(_rightCommandActor, TYPE_SKILL_CAST.DeployCast);
    }

    public int GetCastleHealth(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.nowCastleHealthValue;
            case TYPE_TEAM.Right:
                return _rightCommandActor.nowCastleHealthValue;
        }
        return -1;
    }

    public float GetCastleHealthRate(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.GetCastleHealthRate();
            case TYPE_TEAM.Right:
                return _rightCommandActor.GetCastleHealthRate();
        }
        return -1f;
    }

    public void AddSupplyListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.AddSupplyListener(act);
    }
    public void RemoveSupplyListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.RemoveSupplyListener(act);
    }


    public void AddHealthListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.AddHealthListener(act);
        _rightCommandActor.AddHealthListener(act);
    }
    public void RemoveHealthListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.RemoveHealthListener(act);
        _rightCommandActor.RemoveHealthListener(act);
    }

   
    public void NextTurn()
    {







        switch (_typeBattleField)
        {
            case TYPE_BATTLEFIELD.Setting:
                if (_unitManager.IsLivedUnits(TYPE_TEAM.Left) == 0)
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
                var arr = _leftCommandActor.GetTypeBattleTurns();
                if (arr.Length == BattleFieldSettings.BATTLE_TURN_COUNTER)
                {
                    if (_battleCoroutine == null)
                    {
                        _battleCoroutine = StartCoroutine(TurnCoroutine(arr, _rightCommandActor.GetTypeBattleTurns()));
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

    public void NextTurnTester(TYPE_BATTLE_TURN typeBattleTurnL, TYPE_BATTLE_TURN typeBattleTurnR)
    {
        if (_battleCoroutine == null)
        {
            _battleCoroutine = StartCoroutine(TurnTestCoroutine(typeBattleTurnL, typeBattleTurnR));
        }
    }


    public string GetSupply(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.ToSupplyString();
            case TYPE_TEAM.Right:
                return _rightCommandActor.ToSupplyString();
        }
        return null;
    }

    public string AddedSupply()
    {
        return _leftCommandActor.supplyValue.ToString();
    }

    public float SupplyRate()
    {
        return _leftCommandActor.GetSupplyRate();
    }


    private int minimumTurn = BattleFieldSettings.BATTLE_TURN_COUNTER;
    private bool IsBattleEnd()
    {
        if (minimumTurn == 0) return true;

        if (IsGameEnd())
        {
            _isReady = true;
            return true;
        }

        //return false;
        if (_unitManager.IsLiveUnitsEmpty()) 
        {
            _firstTypeTeam = (_unitManager.IsLivedUnits(TYPE_TEAM.Left) == 0) ? TYPE_TEAM.Right : TYPE_TEAM.Left;
            _isReady = true;
            return true;
        }
        return false;
    }

    private bool IsAutoBattleEnd(TYPE_TEAM typeTeam)
    {
        if (IsGameEnd())
        {
            return true;
        }

        return (_unitManager.IsLivedUnits(typeTeam) == 0);
    }

    IEnumerator TurnTestCoroutine(TYPE_BATTLE_TURN battleTurnsLeft, TYPE_BATTLE_TURN battleTurnsRight)
    {
        StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Left, battleTurnsLeft));
        StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Right, battleTurnsRight));

        yield return null;

        while (_unitManager.isRunning)
        {
            yield return null;
        }

        _battleCoroutine = null;

        yield return null;
    }

    IEnumerator TurnCoroutine(TYPE_BATTLE_TURN[] battleTurnsLeft, TYPE_BATTLE_TURN[] battleTurnsRight)
    {
        _uiGame.ActivateBattlePanel();
        
        _isReady = false;
        _isAutoBattle = false;

        minimumTurn = battleTurnsLeft.Length;

        if (!_isReady)
        {
            yield return _unitManager.SetPreActiveActionUnits(_leftCommandActor, _rightCommandActor);

            while (!IsBattleEnd())
            {
                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Left, battleTurnsLeft[battleTurnsLeft.Length - minimumTurn]));
                StartCoroutine(_unitManager.ActionUnits(TYPE_TEAM.Right, battleTurnsRight[battleTurnsRight.Length - minimumTurn]));

                while (_unitManager.isRunning)
                {
                    yield return null;
                }
                minimumTurn--;
            }

            yield return _unitManager.ReleasePreActiveActionUnits();

            //적군 아군이 남아있을때
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
                _isAutoBattle = true;// (Random.Range(0f, 100f) > 50f);
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


    private TYPE_BATTLE_RESULT GameResult()
    {

        if(!_leftCommandActor.IsEmptyCastleHealth() && !_rightCommandActor.IsEmptyCastleHealth())
        {
            return TYPE_BATTLE_RESULT.Draw;
        }
        if (_leftCommandActor.IsSurrender())
        {
            return TYPE_BATTLE_RESULT.Defeat;
        }
        else if (_rightCommandActor.IsSurrender())
        {
            return TYPE_BATTLE_RESULT.Victory;
        }
        else
        {
            return TYPE_BATTLE_RESULT.Draw;
        }
    }

    private void SetAutoBattle(bool isAutoBattle)
    {
        _isAutoBattle = isAutoBattle;
        _isReady = false;

        if (!isAutoBattle)
        {
            //보급반납
            _unitManager.ReturnUnitCards(_leftCommandActor);
        }
    }


    private void UnitActorTurn()
    {
        var blocks = FieldManager.GetAllBlocks();
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                blocks[i].unitActor.Turn();
            }
        }
    }

    [HideInInspector]
    public bool _isReady = false;
    bool _isAutoBattle = false;

    public void AutoBattle()
    {
        _isReady = false;
        _isAutoBattle = true;
    }
#if UNITY_EDITOR
    public void NextRoundTest()
    {
        NextRound();
    }
#endif

    private void NextRound()
    {
        if(_typeBattleRound == TYPE_BATTLE_ROUND.Night)
        {
            _uiGame.GameEnd(GameResult());
            return;
        }

        _unitManager.ClearUnitCards();
        _unitManager.ClearDeadUnits();

        //다친 유닛 회복
        _leftCommandActor.RecoveryUnits();
        _rightCommandActor.RecoveryUnits();

        _typeBattleField = TYPE_BATTLEFIELD.Setting;

        //라운드 증가
        _typeBattleRound++;

        _isReady = false;
        _isAutoBattle = false;

        //UI 갱신
        _uiGame.UpdateUnits();
        _uiGame.ActivateUnitCardPanel();
        _uiGame.SetBattleRound(_typeBattleRound);

        if (_firstTypeTeam == TYPE_TEAM.Right)
            CreateEnemyUnits();
    }

    /// <summary>
    /// 필드에 있는 모든 유닛을 제거합니다
    /// </summary>
    public void ClearAllUnits()
    {
        _unitManager.ClearAllUnits();
    }

    public void Retreat()
    {
        _isReady = false;

        //현재 남은 병력 보존하기
        //_unitManager.RetreatUnits(_leftCommandActor);
        //_unitManager.RetreatUnits(_rightCommandActor);


    }


    public static void IncreaseHealth(int damageValue, TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                _leftCommandActor.DecreaseHealth(damageValue);
                break;
            case TYPE_TEAM.Right:
                _rightCommandActor.DecreaseHealth(damageValue);
                break;
        }
    }

    public TYPE_TEAM GetNowTeam()
    {
        return _dropTeam;
    }

    public void SetNowTeam(TYPE_TEAM typeTeam)
    {
        _dropTeam = typeTeam;
    }

    public bool IsOrder() => _typeBattleField == TYPE_BATTLEFIELD.Setting;

    public bool IsSupply(UnitCard uCard, TYPE_TEAM nowTypeTeam = TYPE_TEAM.Left)
    {
        return (nowTypeTeam == TYPE_TEAM.Left) ? _leftCommandActor.IsSupply(uCard) : _rightCommandActor.IsSupply(uCard);
    }

    public void CreateUnit(ICommanderActor cActor)
    {
        var block = FieldManager.GetRandomBlock(cActor.typeTeam);

        if (block != null && block.unitActor == null)
        {
            //유닛 카드 가져오기
            var uCard = cActor.unitDataArray[Random.Range(0, cActor.unitDataArray.Length)];

            //이미 카드 사용중
            if (_unitManager.IsUsedCard(uCard)) return;

            //사망한 병사 포메이션은 무시
            var formationCells = new List<Vector2Int>();
            var uKeys = new List<int>();

            for(int i = 0; i < uCard.UnitKeys.Length; i++)
            {
                if (!uCard.IsDead(uCard.UnitKeys[i]))
                {
                    formationCells.Add(uCard.formationCells[i]);
                    uKeys.Add(uCard.UnitKeys[i]);
                }
            }
            
            //포메이션 블록 가져오기
            var blocks = FieldManager.GetFormationBlocks(block.coordinate, formationCells.ToArray(), cActor.typeTeam);

            if (uKeys.Count == blocks.Length)
            {

                bool isCheck = false;
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].unitActor != null)
                    {
                        isCheck = true;
                        break;
                    }
                }

                if (!isCheck && cActor.IsSupply(uCard))
                {
                    cActor.UseSupply(uCard);
                    _unitManager.CreateUnits(uCard, uKeys.ToArray(), blocks, cActor.typeTeam);
                }
            }
        }
    }

    public void CreateFieldUnit(TYPE_TEAM typeTeam)
    {
        var cActor = (typeTeam == TYPE_TEAM.Left) ? _leftCommandActor : _rightCommandActor;

        var blocks = FieldManager.GetTeamUnitBlocks(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && block.unitActor == null)
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
    /// 테스트용 유닛 생성기
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="unit"></param>
    public void CreateFieldUnitInTest(TYPE_TEAM typeTeam, UnitData unit)
    {
        var blocks = FieldManager.GetTeamUnitBlocks(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && block.unitActor == null)
            {
                var uCardTmp = UnitCard.CreateTest(unit);
                var uKey = uCardTmp.UnitKeys[0];
                _unitManager.CreateUnit(uCardTmp, uKey, block, typeTeam);
            }
        }
    }
#endif


    public void DragUnit(UnitCard uCard)
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
    }

    public void DragUnit(IUnitActor uActor)
    {
        _unitManager.DragUnitActor(uActor);
    }

    public bool DropUnit(UnitCard uCard)
    {
        if (_unitManager.DropUnitActor(_leftCommandActor, uCard))
        {
            _leftCommandActor.UseSupply(uCard);
            UnitManager.CastSkills(_leftCommandActor, TYPE_SKILL_CAST.DeployCast);
            _uiGame.SetSupply(_leftCommandActor.nowSupplyValue, _leftCommandActor.GetSupplyRate());
            return true;
        }
        return false;
    }

    public void ReturnUnit(UnitCard uCard)
    {
        _leftCommandActor.ReturnSupply(uCard);
    }

    public void ReturnUnit(IUnitActor uActor)
    {
        _unitManager.ReturnUnitActor(uActor);
    }

    public void CancelUnit()
    {
        _unitManager.CancelUnitActor();
    }

    public void CancelChangeUnit()
    {
        _unitManager.CancelChangeUnitActor();
    }

    private bool IsGameEnd()
    {
        return _leftCommandActor.IsEmptyCastleHealth() || _rightCommandActor.IsEmptyCastleHealth();
    }
}
