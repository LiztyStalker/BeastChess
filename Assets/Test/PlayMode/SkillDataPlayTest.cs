#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDataPlayTest : PlayTest
{
    UnitData _uData;
    ICaster _caster;
    IFieldBlock[] _blocks;


    /// <summary>
    /// SkinForce 테스트
    /// 아군 및 적군 유닛 생성
    /// 스킬 가져오기 및 시전
    /// 적용된 유닛 수 출력
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_SkinForce()
    {
        //동맹을 생성할 위치를 지정합니다
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("SkinForce");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 유닛 수 가져오기
        var count = _blocks.Where(block => block.unitActor != null && block.unitActor.IsHasStatusData(skillData.GetStatusData())).Count();
        Debug.Log(count);
        Assert.IsTrue(count == 9);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Health()
    {
        //동맹을 생성할 위치를 지정합니다
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //유닛 전체 체력
        //conscript - 80 * 25 = 2000
        var value = _blocks.Where(block => block.unitActor != null && block.unitActor.typeTeam == TYPE_TEAM.Left && block.unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.unitActor.nowHealthValue);
        Debug.Log(value);
        Assert.IsTrue(value == 2000);

        //아군 유닛 체력 50 깎기
        var aliesBlocks = _blocks.Where(block => block.unitActor != null).ToArray();
        for(int i = 0; i < aliesBlocks.Length; i++)
        {
            aliesBlocks[i].unitActor.DecreaseHealth(50);
        }        

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heal");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //아군 체력 가져오기
        //25 * 30 = 750
        //30 * 9 = 270
        //750 + 270 = 1020
        value = _blocks.Where(block => block.unitActor != null && block.unitActor.typeTeam == TYPE_TEAM.Left && block.unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.unitActor.nowHealthValue);

        Debug.Log(value);
        Assert.IsTrue(value == 1020);
    }




    [UnityTest]
    public IEnumerator SkillData_UnitSettings()
    {
        yield return UnitSettings(new Vector2Int(8, 3));
        
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Left) == 1, Is.True);
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Right) == 104, Is.True);

    }



    /// <summary>
    /// 유닛을 셋팅합니다
    /// </summary>
    /// <param name="casterBlock">스킬을 사용할 아군 캐스터 위치</param>
    /// <param name="aliesTargetData">동맹 유닛입니다. null이면 동맹 유닛을 생성하지 않습니다</param>
    /// <returns></returns>
    private IEnumerator UnitSettings(Vector2Int casterBlock, TargetData aliesTargetData = null)
    {
        //병사 가져오기
        _uData = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");

        //시전자 블록 가져오기
        var block = FieldManager.GetBlock(casterBlock.x, casterBlock.y);

        //시전자 카드 제작 및 액터 생성
        var casterUnitCard = UnitCard.Create(_uData);
        _caster = unitManager.CreateUnit(casterUnitCard, casterUnitCard.UnitKeys[0], block, TYPE_TEAM.Left);

        //동맹 액터가 있으면
        if (aliesTargetData != null)
        {
            //동맹 블록 가져오기
            var aliesBlocks = FieldManager.GetTargetBlocksInBlankBlock(block, aliesTargetData, TYPE_TEAM.Left);

            if (aliesBlocks != null)
            {
                //동맹 블록에 동맹 카드 제작 및 액터 생성
                for (int i = 0; i < aliesBlocks.Length; i++)
                {
                    var uCardL = UnitCard.Create(_uData);
                    unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], aliesBlocks[i], TYPE_TEAM.Left);
                }
            }
        }
        

        yield return null;

        //모든 블록 가져오기
        _blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < _blocks.Length; i++)
        {
            //적 액터 생성
            if (_blocks[i].unitActor == null)
            {
                var uCardR = UnitCard.Create(_uData);
                unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], _blocks[i], TYPE_TEAM.Right);
            }
        }
        yield return null;

    }

    [TearDown]
    public void TearDown()
    {
        _caster = null;
        _blocks = null;
    }
}
#endif