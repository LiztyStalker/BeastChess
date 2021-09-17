#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Dummy_CommanderActor : ICommanderActor
{
   
    private int _castleHealthWeight;

    private List<UnitCard> _unitDataArray = new List<UnitCard>();

    private int _supplyLevel;

    private int _nowSupplyValue;


    private int _maxSupplyValue;

    private int maxSupplyValue => _maxSupplyValue;

    private int _nowCastleHealthValue;

    private int _supplyValue;
    public int supplyValue => _supplyValue;

    public UnitCard[] unitDataArray => _unitDataArray.ToArray();
    public int supplyLevel => _supplyLevel;

    private TYPE_TEAM _typeTeam;

    public TYPE_TEAM typeTeam { get { return _typeTeam; } set { _typeTeam = value; } }

    private TYPE_BATTLE_TURN[] typeBattleTurns;

    private TYPE_COMMANDER_MASTER _typeCommanderMaster;
    public TYPE_COMMANDER_MASTER typeCommanderMaster => _typeCommanderMaster;

    public Vector3 position => Vector3.zero;

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

    private int _castleHealthValue;

    public int castleHealthValue => _castleHealthValue;
    public int nowCastleHealthValue => _nowCastleHealthValue;

    private int _maxLeadershipValue;

    public int maxLeadershipValue => _maxLeadershipValue;
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

    private List<SkillData> _skills = new List<SkillData>();

    public SkillData[] skills => _skills.ToArray();

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

    public Dummy_CommanderActor()
    {
        _supplyLevel = 1;
        _unitDataArray.Clear();
        _castleHealthWeight = 0;
        _nowCastleHealthValue = castleHealthValue;
    }


    public void AddSkill(SkillData skillData)
    {
        _skills.Add(skillData);
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

    public bool IsUpgradeSupply()
    {
        return _nowSupplyValue == maxSupplyValue;
    }

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
        return (_nowSupplyValue - uCard.employCostValue >= 0);
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
        _nowSupplyValue -= uCard.employCostValue;
    }

    public void ReturnSupply(UnitCard uCard)
    {
        _nowSupplyValue += uCard.employCostValue;
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

    public void SetCommanderCard(CommanderCard cmdCard)
    {
    }
}
#endif