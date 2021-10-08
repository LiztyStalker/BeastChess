using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Settings
{
    public const float RECOVERY_HEALTH_RATE = 0.3f;

    public const int BATTLE_TURN_COUNT = 3;
    public const float FRAME_TIME = 0.01f;
    public const float FRAME_END_TIME = 0.25f;
    public const float BULLET_MOVEMENT = 0.8f;
    public const float MAX_UNIT_MOVEMENT = 0.04f;
    public const float MIN_UNIT_MOVEMENT = 0.06f;

    public static bool Invincible = false;
    public static bool SingleFormation = false;

}

public enum TYPE_BATTLE_ROUND { Morning, Evening, Night}
public enum TYPE_TEAM { None = -1, Left, Right }


public class BattleFieldManager : MonoBehaviour
{
    [SerializeField]
    UIGame _uiGame;

    FieldGenerator _fieldGenerator;

    UnitManager _unitManager;

    [SerializeField]
    int count = 4;

    [SerializeField]
    bool isAuto = false;
    
    static ICommanderActor _leftCommandActor;
    static ICommanderActor _rightCommandActor;

    Coroutine co;

    //public static TYPE_TEAM nowTypeTeam = TYPE_TEAM.Right;

    public TYPE_TEAM _firstTypeTeam = TYPE_TEAM.Right;

    private TYPE_TEAM _dropTeam = TYPE_TEAM.Left;

    public bool isEnd = false;

    public TYPE_BATTLE_ROUND _typeBattleRound;

    // Start is called before the first frame update
    void Awake()
    {

        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        _typeBattleRound = TYPE_BATTLE_ROUND.Morning;

        if (_fieldGenerator == null) _fieldGenerator = GetComponentInChildren<FieldGenerator>();
        _fieldGenerator.Initialize();

        if (MockGameOutpost.instance == null)
        {
            Debug.LogWarning("BattleField TestMode");

            var dataArrayL = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();
            var dataArrayR = DataStorage.Instance.GetAllDataArrayOrZero<UnitData>();

//            var dataArrayL = DataStorage.Instance.GetRandomDatasOrZero<UnitData>(100);
//            var dataArrayR = DataStorage.Instance.GetRandomDatasOrZero<UnitData>(100);

            dataArrayL = dataArrayL.Where(data => data.SkeletonDataAsset != null && data.Icon != null).ToArray();
            dataArrayR = dataArrayL.Where(data => data.SkeletonDataAsset != null && data.Icon != null).ToArray();


            var uCardsL = UnitCard.Create(dataArrayL);// _unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");
            var uCardsR = UnitCard.Create(dataArrayR); //_unitManager.GetRandomUnitCards(20);//_unitManager.GetUnitCards("UnitData_SpearSoldier", "UnitData_Archer", "UnitData_Assaulter");

            _leftCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Raty")), uCardsL, 0);
            _leftCommandActor.typeTeam = TYPE_TEAM.Left;

            _rightCommandActor = CommanderActor.Create(CommanderCard.Create(DataStorage.Instance.GetDataOrNull<CommanderData>("Dummy")), uCardsR, 0);
            _rightCommandActor.typeTeam = TYPE_TEAM.Right;

        }
        else
        {
            _leftCommandActor = MockGameOutpost.instance.commander_L.commanderActor;
            _leftCommandActor.typeTeam = TYPE_TEAM.Left;

            _rightCommandActor = MockGameOutpost.instance.commander_R.commanderActor;
            _rightCommandActor.typeTeam = TYPE_TEAM.Right;
        }

        if (_unitManager == null) _unitManager = GetComponentInChildren<UnitManager>();
        _unitManager.CreateCastleUnit(TYPE_TEAM.Left);
        _unitManager.CreateCastleUnit(TYPE_TEAM.Right);

        _uiGame?.SetBattleTurn(false);

    }

    private void OnDestroy()
    {
        _fieldGenerator.CleanUp();
    }


    private void Start()
    {
        if (_firstTypeTeam == TYPE_TEAM.Right)
            CreateEnemyUnits();
    }

    private void CreateEnemyUnits()
    {
        for (int i = 0; i < count; i++)
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

    public UnitCard[] GetLeftUnits()
    {
        return _leftCommandActor.unitDataArray;
    }

    private bool isSquad = true;

    void Update()
    {
        if (_unitManager.IsDrag()) return;
        if (!isAuto && Input.GetKeyDown(KeyCode.Return))
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {

        if (isSquad)
        {
            isSquad = false;
            _uiGame.SetSquad(false);
            if (_firstTypeTeam == TYPE_TEAM.Left)
                CreateEnemyUnits();
        }

        var arr = _uiGame.GetTypeBattleTurnArray();
        if (arr.Length == 3)
        {
            if (co == null)
                co = StartCoroutine(TurnCoroutine(_uiGame.GetTypeBattleTurnArray(), _rightCommandActor.GetTypeBattleTurns()));
        }
        else
        {
            _uiGame.SetBattleTurn(true);
            Debug.Log("Not BattleTurn 3");
        }
    }

    public void NextTurnTester(TYPE_BATTLE_TURN typeBattleTurnL, TYPE_BATTLE_TURN typeBattleTurnR)
    {
        if (co == null)
        {
            co = StartCoroutine(TurnTestCoroutine(typeBattleTurnL, typeBattleTurnR));
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
        return _leftCommandActor.SupplyRate();
    }


    private int minimumTurn = 3;
    private bool IsBattleEnd()
    {
        if (minimumTurn == 0) return true;

        if (IsGameEnd())
        {
            isEnd = true;
            return true;
        }

        //return false;
        if (_unitManager.IsLiveUnitsEmpty()) {

            _firstTypeTeam = (_unitManager.IsLivedUnits(TYPE_TEAM.Left) == 0) ? TYPE_TEAM.Right : TYPE_TEAM.Left;
            isReady = true;
            return true;
        }
        return false;
    }

    private bool IsAutoBattleEnd(TYPE_TEAM typeTeam)
    {
        if (IsGameEnd())
        {
            isEnd = true;
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

        co = null;

        yield return null;
    }

    IEnumerator TurnCoroutine(TYPE_BATTLE_TURN[] battleTurnsLeft, TYPE_BATTLE_TURN[] battleTurnsRight)
    {
        _uiGame.SetBattleTurn(false);
        isReady = false;
        isAutoBattle = false;
        minimumTurn = battleTurnsLeft.Length;
        if (!isReady)
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
            if (!isReady)
            {
                UnitActorTurn();
                _uiGame.SetBattleTurn(true);
                co = null;
                yield break;
            }
        }

        UnitActorTurn();

        if (IsGameEnd())
        {
            isEnd = true;
            yield break;
        }


        while (isReady)
        {
            yield return null;
            if(_firstTypeTeam == TYPE_TEAM.Right)
            {
                isReady = false;
                isAutoBattle = true;// (Random.Range(0f, 100f) > 50f);
                Debug.Log("IsAutoBattle " +  _firstTypeTeam + " " + isAutoBattle);
            }
        }
        if (isAutoBattle)
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
            isEnd = true;
            yield break;
        }


        NextRound();
        co = null;

        yield return null;
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
    public bool isReady = false;
    bool isAutoBattle = false;

    public void AutoBattle()
    {
        isReady = false;
        isAutoBattle = true;
    }


    private void NextRound()
    {
        
        _unitManager.ClearUnitCards();
        _unitManager.ClearDeadUnits();

        //다친 유닛 회복
        _leftCommandActor.RecoveryUnits();
        _rightCommandActor.RecoveryUnits();

        //라운드 증가
        _typeBattleRound++;

        isSquad = true;
        isReady = false;
        isAutoBattle = false;

        //UI 갱신
        _uiGame.UpdateUnits();
        _uiGame.SetSquad(isSquad);
        _uiGame.SetBattleTurn(false);
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
        isReady = false;

        //현재 남은 병력 보존하기
        //_unitManager.RetreatUnits(_leftCommandActor);
        //_unitManager.RetreatUnits(_rightCommandActor);


    }


    public static void IncreaseHealth(int damageValue, TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                _leftCommandActor.IncreaseHealth(damageValue);
                break;
            case TYPE_TEAM.Right:
                _rightCommandActor.IncreaseHealth(damageValue);
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


    [System.Obsolete("사용하지 않음")]
    public void IncreaseUpgrade(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                _leftCommandActor.UpgradeSupply();
                break;
            case TYPE_TEAM.Right:
                _rightCommandActor.UpgradeSupply();
                break;
        }

        //if (co == null)
        //    co = StartCoroutine(TurnCoroutine());
    }

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
        if (Settings.SingleFormation)
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

    public bool DropUnit(UnitCard uCard)
    {
        if (_unitManager.DropUnitActor(_leftCommandActor, uCard))
        {
            _leftCommandActor.UseSupply(uCard);
            UnitManager.CastSkills(_leftCommandActor, TYPE_SKILL_CAST.DeployCast);
            return true;
        }
        return false;
    }

    public void ReturnUnit(UnitCard uCard)
    {
        _leftCommandActor.ReturnSupply(uCard);
    }

    public void CancelUnit()
    {
        _unitManager.CancelUnitActor();
    }

    private bool IsGameEnd()
    {
        return _leftCommandActor.IsEmptyCastleHealth() || _rightCommandActor.IsEmptyCastleHealth();
    }
}
