#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

using System.Collections.Generic;
using UnityEngine;

public class Dummy_UnitActor : IUnitActor
{
    public TYPE_UNIT_FORMATION typeUnit;

    private int _uKey;

    public int uKey => _uKey;

    public UnitCard unitCard => null;

    private int _nowHealthValue = 100;

    public int nowHealthValue => _nowHealthValue;//_unitCard.GetUnitNowHealth(uKey);

    private int _maxHealthValue = 200;
    public int maxHealthValue => _maxHealthValue;//_unitCard.GetUnitMaxHealth(uKey);
    public int damageValue => 0;
    public int defensiveValue => 0;
    public int attackCount => 0;

    private TYPE_TEAM _typeTeam;

    public TYPE_TEAM typeTeam => _typeTeam;

    public int minRangeValue => 0;

    private int _priorityValue = 0;

    public int priorityValue => _priorityValue;

    public TYPE_UNIT_GROUP typeUnitGroup => TYPE_UNIT_GROUP.FootSoldier;

    public TYPE_UNIT_CLASS typeUnitClass => TYPE_UNIT_CLASS.LightSoldier;

    public TYPE_UNIT_ATTACK typeUnitAttack => TYPE_UNIT_ATTACK.Normal;

    public TYPE_MOVEMENT typeMovement => TYPE_MOVEMENT.Normal;

    public int movementValue => 1;

    public int chargeMovementValue => 2;


    private List<SkillData> _skills = new List<SkillData>();

    public SkillData[] skills => _skills.ToArray();



    public bool isRunning => false;

    private TYPE_BATTLE_TURN _typeBattleTurn;

    public TYPE_BATTLE_TURN TypeBattleTurn => _typeBattleTurn;

    private Vector3 _position;

    public Vector3 position => _position;

    TYPE_UNIT_FORMATION IUnitActor.typeUnit => TYPE_UNIT_FORMATION.Ground;

    public TargetData AttackTargetData => null;

    private StatusActor _statusActor = new StatusActor();
    public Dummy_UnitActor() { 

    }

    public Dummy_UnitActor(int priorityValue)
    {
        _priorityValue = priorityValue;
    }
    public void ActionAttack(BattleFieldManager gameTestManager)
    {
    }

    public void ActionChargeAttack(BattleFieldManager gameTestManager)
    {
    }

    public void ActionChargeReady(BattleFieldManager gameTestManager)
    {
    }

    public void ActionGuard(BattleFieldManager gameTestManager)
    {
    }

    public void AddBar(UIBar uiBar)
    {
    }

    public void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
    }

    public void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
    }

    public void Dead()
    {
    }

    public void Destroy()
    {
    }

    public bool DirectAttack(BattleFieldManager gameTestManager)
    {
        return false;
    }

    public void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
    }

    public IFieldBlock[] GatheringStatePreActive(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    {
        return null;
    }

    public float HealthRate()
    {
        return 0;
    }

    public void IncreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1)
    {
        value *= additiveRate;
        IncreaseHealth(value);
    }

    public void IncreaseHealth(int value)
    {
        _nowHealthValue += value;
    }


    public bool IsDead()
    {
        return _nowHealthValue <= 0;
    }

    public void RemoveStatusData()
    {
    }

    public void RemoveStatusData(ICaster caster)
    {
        _statusActor.RemoveStatusData(caster);
    }

    public void SetActive(bool isActive)
    {
    }



    public void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn)
    {
    }

    public void SetData(UnitCard uCard)
    {
    }

    public void SetKey(int key)
    {
        _uKey = key;
    }

    public void SetLayer()
    {
    }

    public void SetOnDeadListener(System.Action<ICaster> act)
    {
    }

    public void SetPosition(Vector2 pos)
    {
    }

    public void ReceiveSkill(ICaster caster, SkillData skillData, TYPE_SKILL_CAST typeSkillActivate)
    {
        _skills.Add(skillData);
    }

    public void ReceiveSkills(ICaster caster, SkillData[] skills, TYPE_SKILL_CAST typeSkillActivate)
    {
        _skills.AddRange(skills);
    }

    public void SetStatusData(ICaster caster, StatusData status)
    {
        _statusActor.AddStatusData(caster, status);
    }


#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

    public StatusActor StatusActor => _statusActor;


    public bool IsHasStatusData(StatusData.TYPE_STATUS_LIFE_SPAN typeStatusLifeSpan)
    {
        return _statusActor.IsHasStatusData(typeStatusLifeSpan);
    }

    public bool IsHasStatusData(StatusData statusData)
    {
        return _statusActor.IsHasStatusData(statusData);
    }

#endif


    public void SetStatePreActive(FieldManager fieldManager)
    {
    }

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        _typeTeam = typeTeam;
    }

    public void SetNowHealth(int value)
    {
        _nowHealthValue = value;
    }

    public void Turn()
    {
        _statusActor.Turn(null);
    }

    public void CleanUp()
    {
        _skills.Clear();
    }

    public void ReceiveStatusData(ICaster caster, StatusData statusData)
    {
    }
}

#endif