using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitActor : ICaster
{
    public int uKey { get; }

    public UnitCard unitCard { get; }

    int nowHealthValue { get; }
    int maxHealthValue { get; }


    int damageValue { get; }

    int defensiveValue { get; }
    int attackCount { get; }
    int priorityValue { get; }


    TYPE_UNIT_FORMATION typeUnit { get; }
    TYPE_UNIT_GROUP typeUnitGroup { get; }
    TYPE_UNIT_CLASS typeUnitClass { get; }

    TYPE_BATTLE_TURN TypeBattleTurn { get; }
    TYPE_MOVEMENT typeMovement { get; }

    Vector2Int[] movementCells { get; }
    Vector2Int[] chargeCells { get; }


    TargetData AttackTargetData { get; }




    void SetActive(bool isActive);
    void Destroy();
    float HealthRate();
    bool IsDead();
    void SetKey(int key);


   

    void SetTypeTeam(TYPE_TEAM typeTeam);
    void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn);

    void SetData(UnitCard uCard);

    void SetLayer();

    void AddBar(UIBar uiBar);

    void SetPosition(Vector2 pos);


    bool isRunning { get; }

    void IncreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1);

    void IncreaseHealth(int value);

    void Dead();
    void Turn();


    bool DirectAttack(BattleFieldManager gameTestManager);

    void ActionAttack(BattleFieldManager gameTestManager);
    void ActionChargeReady(BattleFieldManager gameTestManager);
    void ActionChargeAttack(BattleFieldManager gameTestManager);
    void ActionGuard(BattleFieldManager gameTestManager);


    void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock);
    void SetOnDeadListener(System.Action<ICaster> act);


    //void SetStatePreActive(FieldManager fieldManager);
    //void ReceiveSkill(ICaster caster, SkillData skillData, TYPE_SKILL_ACTIVATE typeSkillActivate);
    //void ReceiveSkills(ICaster caster, SkillData[] skills, TYPE_SKILL_ACTIVATE typeSkillActivate);
    void ReceiveStatusData(ICaster caster, StatusData statusData);
    void RemoveStatusData();
    void RemoveStatusData(ICaster caster);

    void CleanUp();
}

