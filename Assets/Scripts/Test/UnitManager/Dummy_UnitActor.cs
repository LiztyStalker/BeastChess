#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

using UnityEngine;

public class Dummy_UnitActor : IUnitActor
{
    public TYPE_UNIT_FORMATION typeUnit;

    private int _uKey;

    public int uKey => _uKey;

    public UnitCard unitCard => null;

    public int damageValue => 0;

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

    public Vector2Int[] attackCells => null;

    public Vector2Int[] movementCells => null;

    public Vector2Int[] chargeCells => null;

    public SkillData[] skills => null;

    public bool isRunning => false;

    private TYPE_BATTLE_TURN _typeBattleTurn;

    public TYPE_BATTLE_TURN typeBattleTurn => _typeBattleTurn;

    private Vector2 _position;

    public Vector2 position => _position;

    TYPE_UNIT_FORMATION IUnitActor.typeUnit => TYPE_UNIT_FORMATION.Ground;

    public Dummy_UnitActor() { }

    public Dummy_UnitActor(int priorityValue)
    {
        _priorityValue = priorityValue;
    }
    public void ActionAttack(GameManager gameTestManager)
    {
    }

    public void ActionChargeAttack(GameManager gameTestManager)
    {
    }

    public void ActionChargeReady(GameManager gameTestManager)
    {
    }

    public void ActionGuard(GameManager gameTestManager)
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

    public bool DirectAttack(GameManager gameTestManager)
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
    }

    public bool IsDead()
    {
        return false;
    }

    public void RemovePreActiveSkill()
    {
    }

    public void RemoveSkill(ICaster caster)
    {
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

    public void SetSkill(ICaster caster, SkillData skillData, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
    }

    public void SetSkill(ICaster caster, SkillData[] skills, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
    }

    public void SetStatePreActive(FieldManager fieldManager)
    {
    }

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        _typeTeam = typeTeam;
    }

    public void Turn()
    {
    }
}

#endif