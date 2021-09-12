using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitActor : ICaster
{
    public int uKey { get; }

    public UnitCard unitCard { get; }

    void SetActive(bool isActive);
    void Destroy();
    float HealthRate();

    bool IsDead();

    int damageValue { get; }

    int attackCount { get; }

    void SetKey(int key);

    int priorityValue { get; }

    TYPE_UNIT_FORMATION typeUnit { get; }

    TYPE_UNIT_GROUP typeUnitGroup { get; }

    TYPE_UNIT_CLASS typeUnitClass { get; }

    TYPE_BATTLE_TURN typeBattleTurn { get; }
    TYPE_MOVEMENT typeMovement { get; }

    Vector2Int[] movementCells { get; }
    Vector2Int[] chargeCells { get; }

    TargetData TargetData { get; }

    void SetTypeTeam(TYPE_TEAM typeTeam);
    void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn);

    void SetData(UnitCard uCard);

    void SetLayer();

    void AddBar(UIBar uiBar);

    void SetPosition(Vector2 pos);

    Vector2 position { get; }

    bool isRunning { get; }

    void IncreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1);


    void Dead();
    void Turn();


    IFieldBlock[] GatheringStatePreActive(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam);
    bool DirectAttack(BattleFieldManager gameTestManager);

    void ActionAttack(BattleFieldManager gameTestManager);
    void ActionChargeReady(BattleFieldManager gameTestManager);
    void ActionChargeAttack(BattleFieldManager gameTestManager);
    void ActionGuard(BattleFieldManager gameTestManager);


    void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void SetOnDeadListener(System.Action<ICaster> act);


    void SetStatePreActive(FieldManager fieldManager);
    void SetSkill(ICaster caster, SkillData skillData, TYPE_SKILL_ACTIVATE typeSkillActivate);
    void SetSkill(ICaster caster, SkillData[] skills, TYPE_SKILL_ACTIVATE typeSkillActivate);
    void RemovePreActiveSkill();
    void RemoveSkill(ICaster caster);
}

