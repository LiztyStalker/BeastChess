using System.Collections;
using UnityEngine;

public class UnitActionData
{
    public IFieldBlock[] attackFieldBlocks;
    public int nowAttackCount;
    public IFieldBlock nowBlock;
    public IFieldBlock movementBlock;
    public int chargeRange;
}


public interface IUnitActionState
{
    IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback);
}



public class UnitActionAttack : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        var isCast = castSkillsCallback(TYPE_SKILL_CAST.AttackCast);

        if (!isCast)
        {
            if (unitActor.IsHasAnimation("Attack"))
            {
                //공격방위
                var _attackFieldBlocks = FieldManager.GetTargetBlocks(unitActor, unitActor.AttackTargetData, unitActor.typeTeam);
                unitActionData.attackFieldBlocks = _attackFieldBlocks;

                //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
                if (_attackFieldBlocks.Length > 0)
                {
                    for (int i = 0; i < _attackFieldBlocks.Length; i++)
                    {
                        if (_attackFieldBlocks[i].IsHasUnitActor())
                        {
                            var unitActors = _attackFieldBlocks[i].unitActors;
                            for (int j = 0; j < unitActors.Length; j++)
                            {
                                var uActor = unitActors[j];
                                if (uActor.typeTeam != unitActor.typeTeam && !uActor.IsDead())
                                {
                                    //Debug.Log(attackCount + " " + name);
                                    if (unitActor.attackCount > 0)
                                    {
                                        unitActor.SetAnimation("Attack", false);
                                        unitActionData.nowAttackCount = unitActor.attackCount;
                                    }
                                    yield break;
                                }
                            }
                        }
                    }
                }
            }
        }

        actionCtrler.isRunning = false;
        yield return null;
    }
}


public class UnitActionGuard : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        if (unitActor.IsHasAnimation("Guard"))
            unitActor.SetAnimation("Guard", false);
        else
            unitActor.DefaultAnimation(false);


        unitActionData.nowAttackCount = unitActor.attackCount;
        actionCtrler.isRunning = false;
        //        actor._nowAttackCount = attackCount;
        //        _unitAction.isRunning = false;
        yield return null;
    }
}

public class UnitActionForward : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        if (unitActor.IsHasAnimation("Forward"))
            unitActor.SetAnimation("Forward", true);
        else if (unitActor.IsHasAnimation("Move"))
            unitActor.SetAnimation("Move", true);

        unitActionData.nowBlock.LeaveUnitActor(unitActor);
        unitActionData.movementBlock.SetUnitActor(unitActor, false);

        while (Vector2.Distance(unitActor.transform.position, unitActionData.movementBlock.position) > 0.1f)
        {
            unitActor.transform.position = Vector2.MoveTowards(unitActor.transform.position, unitActionData.movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        unitActor.DefaultAnimation(true);
        actionCtrler.isRunning = false;
        yield return null;
    }
}

public class UnitActionBackward : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        if (unitActor.IsHasAnimation("Backward"))
            unitActor.SetAnimation("Backward", true);
        else if (unitActor.IsHasAnimation("Move"))
            unitActor.SetAnimation("Move", true);

        unitActionData.nowBlock.LeaveUnitActor(unitActor);
        unitActionData.movementBlock.SetUnitActor(unitActor, false);

        while (Vector2.Distance(unitActor.transform.position, unitActionData.movementBlock.position) > 0.1f)
        {
            unitActor.transform.position = Vector2.MoveTowards(unitActor.transform.position, unitActionData.movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        unitActor.DefaultAnimation(true);
        actionCtrler.isRunning = false;
        yield return null;
    }
}

public class UnitActionChargeReady : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        if (unitActor.IsHasAnimation("Charge_Ready"))
            unitActor.SetAnimation("Charge_Ready", false);
        else
            unitActor.DefaultAnimation(false);

        unitActionData.nowAttackCount = unitActor.attackCount;
        actionCtrler.isRunning = false;
        yield break;
    }
}

public class UnitActionCharge : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        unitActionData.chargeRange = (unitActor.typeTeam == TYPE_BATTLE_TEAM.Left) ? unitActionData.movementBlock.coordinate.x - unitActionData.nowBlock.coordinate.x : unitActionData.nowBlock.coordinate.x - unitActionData.movementBlock.coordinate.x;

        if (unitActor.IsHasAnimation("Charge"))
            unitActor.SetAnimation("Charge", true);
        else if (unitActor.IsHasAnimation("Forward"))
            unitActor.SetAnimation("Forward", true);

        unitActionData.nowBlock.LeaveUnitActor(unitActor);
        unitActionData.movementBlock.SetUnitActor(unitActor, false);

        while (Vector2.Distance(unitActor.transform.position, unitActionData.movementBlock.position) > 0.1f)
        {
            unitActor.transform.position = Vector2.MoveTowards(unitActor.transform.position, unitActionData.movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        unitActor.DefaultAnimation(true);
        actionCtrler.isRunning = false;
        yield return null;
    }
}

public class UnitActionChargeAttack : IUnitActionState
{
    public IEnumerator ActionCoroutine(UnitActor unitActor, UnitActionController actionCtrler, UnitActionData unitActionData, System.Func<TYPE_SKILL_CAST, bool> castSkillsCallback)
    {
        if (unitActor.IsHasAnimation("Charge_Attack") || unitActor.IsHasAnimation("Attack"))
        {
            //공격방위
            unitActionData.attackFieldBlocks = FieldManager.GetTargetBlocks(unitActor, unitActor.AttackTargetData, unitActor.typeTeam);
            var _attackFieldBlocks = unitActionData.attackFieldBlocks;
            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (_attackFieldBlocks.Length > 0)
            {
                for (int i = 0; i < _attackFieldBlocks.Length; i++)
                {
                    if (_attackFieldBlocks[i].IsHasUnitActor())
                    {
                        var block = _attackFieldBlocks[i];
                        for (int j = 0; j < block.unitActors.Length; j++)
                        {
                            var uActor = block.unitActors[j];
                            if (uActor.typeTeam != unitActor.typeTeam && !uActor.IsDead())
                            {
                                if (unitActor.attackCount > 0)
                                {
                                    if (unitActor.IsHasAnimation("Charge_Attack"))
                                        unitActor.SetAnimation("Charge_Attack", false);
                                    else if (unitActor.IsHasAnimation("Attack"))
                                        unitActor.SetAnimation("Attack", false);

                                    unitActionData.nowAttackCount = unitActor.attackCount;
                                }
                                yield break;
                            }
                        }
                    }
                }

                unitActor.DefaultAnimation(false);

            }
        }
        actionCtrler.isRunning = false;
        yield break;
    }
}


