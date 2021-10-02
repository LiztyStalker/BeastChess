using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderActor : ICommanderActor
{
    private int _castleHealthValue;

    private const int SUPPLY_LEVEL_VALUE = 20;
    private const int SUPPLY_INCREASE_VALUE = 20;
    private const int SUPPLY_VALUE = 500;
    private const int SUPPLY_ADD_VALUE = 5;
    private const int CASTLE_HEALTH_VALUE = 1000;
    private const int CASTLE_HEALTH_INCREASE_VALUE = 100;

    private const int COMMANDER_MASTER_VALUE = 25;


    public const float DEAD_RATE = 0.15f;

    private CommanderCard _commanderCard;

    private int _castleHealthWeight;

    private List<UnitCard> _unitDataArray = new List<UnitCard>();

    private int _supplyLevel;

    private int _nowSupplyValue;

    private int maxSupplyValue => SUPPLY_VALUE + SUPPLY_LEVEL_VALUE * supplyLevel;

    private int _nowCastleHealthValue;

    public int supplyValue => SUPPLY_INCREASE_VALUE + supplyLevel * SUPPLY_ADD_VALUE;

    public UnitCard[] unitDataArray => _unitDataArray.ToArray();

    public bool IsEmptyUnitDataArray() => _unitDataArray.Count == 0;

    public int supplyLevel => _supplyLevel;

    private TYPE_TEAM _typeTeam;

    public TYPE_TEAM typeTeam { get { return _typeTeam; } set { _typeTeam = value; } }

    private TYPE_BATTLE_TURN[] typeBattleTurns;

    public TYPE_COMMANDER_MASTER typeCommanderMaster => _commanderCard.typeCommanderMaster;

    //public int GetBonusCommanderMaster(TYPE_UNIT_GROUP typeUnitGroup)
    //{
    //    switch (typeUnitGroup)
    //    {
    //        case TYPE_UNIT_GROUP.FootSoldier:
    //            if (typeCommanderMaster == TYPE_COMMANDER_MASTER.Infantry)
    //                return COMMANDER_MASTER_VALUE;
    //            break;
    //        case TYPE_UNIT_GROUP.Shooter:
    //            if (typeCommanderMaster == TYPE_COMMANDER_MASTER.Shooter)
    //                return COMMANDER_MASTER_VALUE;
    //            break;
    //        case TYPE_UNIT_GROUP.Charger:
    //            if (typeCommanderMaster == TYPE_COMMANDER_MASTER.Charger)
    //                return COMMANDER_MASTER_VALUE;
    //            break;
    //        case TYPE_UNIT_GROUP.Supporter:
    //            if (typeCommanderMaster == TYPE_COMMANDER_MASTER.Supporter)
    //                return COMMANDER_MASTER_VALUE;
    //            break;

    //    }
    //    return 0;
    //}

    public int castleHealthValue => _castleHealthValue + CASTLE_HEALTH_INCREASE_VALUE * _castleHealthWeight;
    public int nowCastleHealthValue => _nowCastleHealthValue;

    public int maxLeadershipValue => _commanderCard.maxLeadershipValue;
    public int nowLeadershipValue {
        get
        {
            int value = 0;
            for(int i = 0; i < _unitDataArray.Count; i++)
            {
                value += _unitDataArray[i].squadCount;
            }
            return value;
        }
    }

    public SkillData[] skills => _commanderCard.skills;

    public Vector3 position => Vector3.zero;

    public bool IsEnoughLeadership(UnitCard uCard)
    {
        return uCard.squadCount + nowLeadershipValue <= maxLeadershipValue;
    }

    public void AddCard(UnitCard uCard)
    {
        _unitDataArray.Add(uCard);
    }

    public void RemoveCard(UnitCard uCard)
    {
        _unitDataArray.Remove(uCard);
    }

    public void SetCommanderCard(CommanderCard cmdCard)
    {
        _commanderCard = cmdCard;
    }

    public static CommanderActor Create()
    {
        return new CommanderActor();
    }

    public static CommanderActor Create(CommanderCard commanderCard, UnitCard[] unitDataArray, int level = 0)
    {
        return new CommanderActor(commanderCard, unitDataArray, level);
    }

    private CommanderActor()
    {
        _commanderCard = null;

        _supplyLevel = 1;
        _unitDataArray.Clear();
        _nowSupplyValue = SUPPLY_VALUE;
        _castleHealthValue = CASTLE_HEALTH_VALUE;
        _castleHealthWeight = 0;
        _nowCastleHealthValue = castleHealthValue;
    }

    private CommanderActor(CommanderCard commanderCard, UnitCard[] unitDataArray, int level = 0)
    {
        _commanderCard = commanderCard;

        int cycle = level;
        for(int i = 0; i < cycle; i++)
        {
            UpgradeSupply();
        }

        _supplyLevel = level;

        _unitDataArray.Clear();
        _unitDataArray.AddRange(unitDataArray);

        _nowSupplyValue = SUPPLY_VALUE;
        _castleHealthValue = CASTLE_HEALTH_VALUE;
        _castleHealthWeight = 0;
        _nowCastleHealthValue = castleHealthValue;
    }

    public TYPE_BATTLE_TURN[] GetTypeBattleTurns()
    {
        typeBattleTurns = GetRandomTypeBattleTurns();
        return typeBattleTurns;
    }

    private TYPE_BATTLE_TURN[] GetRandomTypeBattleTurns()
    {
        var typeBattleTurns = new TYPE_BATTLE_TURN[Settings.BATTLE_TURN_COUNT];
        for(int i = 0; i < Settings.BATTLE_TURN_COUNT; i++)
        {
            //typeBattleTurns[i] = TYPE_BATTLE_TURN.Charge;
            typeBattleTurns[i] = (TYPE_BATTLE_TURN)Random.Range((int)TYPE_BATTLE_TURN.Forward, (int)TYPE_BATTLE_TURN.Backward);
        }
        return typeBattleTurns;
    }

    //public bool IsUpgradeSupply()
    //{
    //    return _nowSupplyValue == maxSupplyValue;
    //}

    public void UpgradeSupply()
    {
        var rate = (float)_nowCastleHealthValue / (float)castleHealthValue;

        _castleHealthWeight += _supplyLevel;
        _supplyLevel++;

        _nowCastleHealthValue = (int)((float)castleHealthValue * rate);

        _nowSupplyValue = 0;
    }

    public float SupplyRate()
    {
        return (float)_nowSupplyValue / (float)maxSupplyValue;
    }

    public void IncreaseHealth(int damageValue)
    {
        if (damageValue < 0)
            damageValue = 1;

        if (_nowCastleHealthValue - damageValue < 0)
            _nowCastleHealthValue = 0;
        else
            _nowCastleHealthValue -= damageValue;
    }

    public bool IsEmptyCastleHealth()
    {
        return _nowCastleHealthValue == 0;
    }

    public bool IsSupply(UnitCard uCard)
    {
        return (_nowSupplyValue - uCard.AppearCostValue >= 0);
    }

    public void Supply()
    {
        if (_nowSupplyValue + (supplyValue) > maxSupplyValue)
            _nowSupplyValue = maxSupplyValue;
        else
            _nowSupplyValue += supplyValue;
    }

    public void UseSupply(UnitCard uCard)
    {
        _nowSupplyValue -= uCard.AppearCostValue;
    }

    public void ReturnSupply(UnitCard uCard)
    {
        _nowSupplyValue += uCard.AppearCostValue;
    }

    public float GetCastleHealthRate()
    {
        return (float)_nowCastleHealthValue / (float)castleHealthValue;
    }

    public string ToSupplyString()
    {
        return string.Format("{0}/{1}", _nowSupplyValue, maxSupplyValue);
    }

    public void RecoveryUnits()
    {
        for (int i = 0; i < unitDataArray.Length; i++) {
            unitDataArray[i].RecoveryUnit(Settings.RECOVERY_HEALTH_RATE);
        }
    }
}
