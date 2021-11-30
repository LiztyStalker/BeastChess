using System.Collections;
using UnityEngine;


public class UnitActionData
{
    public IFieldBlock[] attackFieldBlocks;
    public int nowAttackCount;
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


//public class UnitActionGuard : IUnitActionState
//{
//    public IEnumerator ActionCoroutine()
//    {
//        //if (actor.IsHasAnimation.("Guard"))
//        //    actor.SetAnimation("Guard", false);
//        //else
//        //    actor.DefaultAnimation(false);

//        //actor._nowAttackCount = attackCount;
//        //_unitAction.isRunning = false;
//        //yield break;
//        yield return null;
//    }
//}




