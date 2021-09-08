#if UNITY_EDITOR

using UnityEngine;

public class Dummy_UnitActor : IUnitActor
{
    public TYPE_UNIT_FORMATION typeUnit;

    public int uKey => throw new System.NotImplementedException();

    public UnitCard unitCard => throw new System.NotImplementedException();

    public int damageValue => throw new System.NotImplementedException();

    public int attackCount => throw new System.NotImplementedException();

    public TYPE_TEAM typeTeam => throw new System.NotImplementedException();

    public int minRangeValue => throw new System.NotImplementedException();

    public int priorityValue => throw new System.NotImplementedException();

    public TYPE_UNIT_GROUP typeUnitGroup => throw new System.NotImplementedException();

    public TYPE_UNIT_CLASS typeUnitClass => throw new System.NotImplementedException();

    public TYPE_UNIT_ATTACK typeUnitAttack => throw new System.NotImplementedException();

    public TYPE_MOVEMENT typeMovement => throw new System.NotImplementedException();

    public Vector2Int[] attackCells => throw new System.NotImplementedException();

    public Vector2Int[] movementCells => throw new System.NotImplementedException();

    public Vector2Int[] chargeCells => throw new System.NotImplementedException();

    public SkillData[] skills => throw new System.NotImplementedException();

    public bool isRunning => throw new System.NotImplementedException();

    public TYPE_BATTLE_TURN typeBattleTurn => throw new System.NotImplementedException();

    public Vector2 position => throw new System.NotImplementedException();

    TYPE_UNIT_FORMATION IUnitActor.typeUnit => throw new System.NotImplementedException();

    public void ActionAttack(GameManager gameTestManager)
    {
        throw new System.NotImplementedException();
    }

    public void ActionChargeAttack(GameManager gameTestManager)
    {
        throw new System.NotImplementedException();
    }

    public void ActionChargeReady(GameManager gameTestManager)
    {
        throw new System.NotImplementedException();
    }

    public void ActionGuard(GameManager gameTestManager)
    {
        throw new System.NotImplementedException();
    }

    public void AddBar(UIBar uiBar)
    {
        throw new System.NotImplementedException();
    }

    public void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        throw new System.NotImplementedException();
    }

    public void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        throw new System.NotImplementedException();
    }

    public void Dead()
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }

    public bool DirectAttack(GameManager gameTestManager)
    {
        throw new System.NotImplementedException();
    }

    public void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        throw new System.NotImplementedException();
    }

    public IFieldBlock[] GatheringStatePreActive(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    {
        throw new System.NotImplementedException();
    }

    public float HealthRate()
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }

    public void RemovePreActiveSkill()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveSkill(ICaster caster)
    {
        throw new System.NotImplementedException();
    }

    public void SetActive(bool isActive)
    {
        throw new System.NotImplementedException();
    }



    public void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn)
    {
        throw new System.NotImplementedException();
    }

    public void SetData(UnitCard uCard)
    {
        throw new System.NotImplementedException();
    }

    public void SetKey(int key)
    {
        throw new System.NotImplementedException();
    }

    public void SetLayer()
    {
        throw new System.NotImplementedException();
    }

    public void SetOnDeadListener(System.Action<ICaster> act)
    {
        throw new System.NotImplementedException();
    }

    public void SetPosition(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public void SetSkill(ICaster caster, SkillData skillData, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        throw new System.NotImplementedException();
    }

    public void SetSkill(ICaster caster, SkillData[] skills, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        throw new System.NotImplementedException();
    }

    public void SetStatePreActive(FieldManager fieldManager)
    {
        throw new System.NotImplementedException();
    }

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        throw new System.NotImplementedException();
    }

    public void Turn()
    {
        throw new System.NotImplementedException();
    }
}

#endif