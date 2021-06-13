using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting
{
    public const float FRAME_TIME = 0.01f;
    public const float FRAME_END_TIME = 0.25f;
    public const float BULLET_MOVEMENT = 0.8f;
    public const float MAX_UNIT_MOVEMENT = 0.04f;
    public const float MIN_UNIT_MOVEMENT = 0.06f;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    UIGame _uiGame;

    [SerializeField]
    FieldManager _fieldManager;

    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    bool isTest = false;

    [SerializeField]
    int count = 4;

    [SerializeField]
    bool isAuto = false;

    [HideInInspector]
    public int turnCount;

    static CommanderActor _leftCommandActor;
    static CommanderActor _rightCommandActor;

    Coroutine co;

    public TYPE_TEAM _typeTeam;

    public bool isEnd = false;

    // Start is called before the first frame update
    void Awake()
    {

        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;


        _fieldManager.Initialize();

        var uCards = _unitManager.GetRandomUnitCards(4);

        _leftCommandActor = new CommanderActor(uCards, 0);
        _rightCommandActor = new CommanderActor(uCards, 0);

        _unitManager.CreateCastleUnit(_fieldManager, TYPE_TEAM.Left);
        _unitManager.CreateCastleUnit(_fieldManager, TYPE_TEAM.Right);

        _uiGame.SetBattleTurn(false);

    }

    private void Start()
    {
        if (isTest)
            CreateFieldUnit(_rightCommandActor, TYPE_TEAM.Right);
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

    // Update is called once per frame
    void Update()
    {
        if (_unitManager.IsDrag()) return;
        if (!isAuto && Input.GetKeyDown(KeyCode.Return))
        {
            NextTurn();
        }
        //else
        //{
        //    if (isAuto || _typeTeam == TYPE_TEAM.Right)
        //    {
        //        if (co == null)
        //            co = StartCoroutine(TurnCoroutine());
        //    }           
        //}
    }

    public void NextTurn()
    {

        if (isSquad)
        {
            isSquad = false;
            _uiGame.SetSquad(false);
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

    public bool IsUpgradeSupply(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.IsUpgradeSupply();
            case TYPE_TEAM.Right:
                return _rightCommandActor.IsUpgradeSupply();
        }
        return false;
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
        return _unitManager.IsLiveUnitsEmpty();
    }


    private class GameManagerAction : CustomYieldInstruction
    {
        private bool isRunning = false;
        private MonoBehaviour mono;
        private IEnumerator enumerator;
        private Coroutine coroutine;

        public override bool keepWaiting
        {
            get
            {
                //Debug.Log("keepwaiting");
                return isRunning;
            }
        }

        public GameManagerAction(MonoBehaviour mono, IEnumerator enumerator)
        {
            this.mono = mono;
            this.enumerator = enumerator;
            coroutine = mono.StartCoroutine(ActionCoroutine());
        }

        private IEnumerator ActionCoroutine()
        {
            isRunning = true;
            yield return mono.StartCoroutine(enumerator);
            isRunning = false;
        }
    }

    IEnumerator TurnCoroutine(TYPE_BATTLE_TURN[] battleTurnsLeft, TYPE_BATTLE_TURN[] battleTurnsRight)
    {
        _uiGame.SetBattleTurn(false);

        _typeTeam = TYPE_TEAM.Right;
        if (!isTest)
        {
            if (_typeTeam == TYPE_TEAM.Right)
            {
                for (int i = 0; i < count; i++)
                {
                    CreateUnit(_rightCommandActor);
                    yield return new WaitForSeconds(Setting.FRAME_TIME);
                }
            }

            if (isAuto && _typeTeam == TYPE_TEAM.Left)
            {
                for (int i = 0; i < count; i++)
                {
                    CreateUnit(_leftCommandActor);
                    yield return new WaitForSeconds(Setting.FRAME_TIME);
                }
            }
            yield return null;
        }



        minimumTurn = battleTurnsLeft.Length;
               
        while (!IsBattleEnd())
        {
            StartCoroutine(_unitManager.ActionUnits(_fieldManager, TYPE_TEAM.Left, battleTurnsLeft[battleTurnsLeft.Length - minimumTurn]));
            StartCoroutine(_unitManager.ActionUnits(_fieldManager, TYPE_TEAM.Right, battleTurnsRight[battleTurnsRight.Length - minimumTurn]));

            while (_unitManager.isRunning)
            {
                yield return null;
            }
//            yield return _unitManager.ActionUnits(_fieldManager, battleTurns[battleTurns.Length - minimumTurn]);
            minimumTurn--;
            //Debug.Log("minimumTurn " + minimumTurn);
        }

        //yield return _unitManager.ClearUnits();

        _typeTeam = TYPE_TEAM.Left;
        //_leftCommandActor.Supply();
        turnCount++;
        co = null;

        _uiGame.SetBattleTurn(true);
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

    public bool IsSupply(UnitCard uCard)
    {
        return (_typeTeam == TYPE_TEAM.Left) ? _leftCommandActor.IsSupply(uCard) : _rightCommandActor.IsSupply(uCard);
    }

    public void CreateUnit(CommanderActor cActor)
    {
        var block = _fieldManager.GetRandomBlock(_typeTeam);

        if (block != null && block.unitActor == null)
        {
            var unit = cActor.unitDataArray[Random.Range(0, cActor.unitDataArray.Length)];

            var blocks = _fieldManager.GetFormationBlocks(block.coordinate, unit.formationCells, _typeTeam);

            if (blocks.Length == unit.formationCells.Length)
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

                if (!isCheck && cActor.IsSupply(unit))
                {
                    cActor.UseSupply(unit);
                    _unitManager.CreateUnits(unit, blocks, _typeTeam);
//                    _unitManager.CreateUnit(unit, block, _typeTeam);
                }
            }
        }
    }

    public void CreateFieldUnit(CommanderActor cActor, TYPE_TEAM typeTeam)
    {
        var blocks = _fieldManager.GetTeamUnitBlocks(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && block.unitActor == null)
            {
                var unit = cActor.unitDataArray[0];// Random.Range(0, cActor.unitDataArray.Length)];
                if (unit != null)
                {
                    _unitManager.CreateUnit(unit, block, typeTeam);
                }
            }
        }
    }

    //public void DragUnit(UnitData uData)
    //{
    //    _unitManager.DragUnit(uData);
    //}

    //public void DropUnit(UnitData uData)
    //{
    //    if (_unitManager.DropUnit(uData, _typeTeam))
    //    {
    //        switch (_typeTeam)
    //        {
    //            case TYPE_TEAM.Left:
    //                _leftCommandActor.UseSupply(uData);
    //                break;
    //            case TYPE_TEAM.Right:
    //                _rightCommandActor.UseSupply(uData);
    //                break;
    //        }
    //    }
    //}

    public void DragUnit(UnitCard uCard)
    {
        _unitManager.DragUnit(uCard);
    }

    public void DropUnit(UnitCard uCard)
    {
        if (_unitManager.DropUnit(uCard, _typeTeam))
        {
            switch (_typeTeam)
            {
                case TYPE_TEAM.Left:
                    _leftCommandActor.UseSupply(uCard);
                    break;
                case TYPE_TEAM.Right:
                    _rightCommandActor.UseSupply(uCard);
                    break;
            }
        }
    }

    private bool IsGameEnd()
    {
        return _leftCommandActor.IsEmptyCastleHealth() || _rightCommandActor.IsEmptyCastleHealth();
    }
}
