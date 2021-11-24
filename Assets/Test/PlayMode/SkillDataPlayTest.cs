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

    [UnityTest]
    public IEnumerator SkillData_StatusData_Recovery()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Recovery");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Feed()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Feed");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Burn()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Burn");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 8);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Ram()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Ram");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Heist()
    {
        //동맹을 생성할 위치를 지정합니다
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heist");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        yield return CheckUnitInStatusDataCount(skillData, 9);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Storm()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Storm");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 104);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 7800);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_Blizard()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Blizard");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 104);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 7280);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_Freeze()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Freeze");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        yield return CheckUnitInStatusDataCount(skillData, 3);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_IceWall()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("IceWall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);



        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitCount(TYPE_BATTLE_TEAM.Left, 2);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_FireWall()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("FireWall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitCount(TYPE_BATTLE_TEAM.Left, 2);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Fireball()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Fireball");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 8050);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_ChainRockFall()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("ChainRockFall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 8070);
    }

   
    [UnityTest]
    public IEnumerator SkillData_StatusData_Blind()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Blind");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 3);
    }

    /// <summary>
    /// SkinForce 테스트
    /// 아군 및 적군 유닛 생성
    /// 스킬 가져오기 및 시전
    /// 적용된 유닛 수 출력
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Penetrate()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Penetrate");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce 테스트
    /// 아군 및 적군 유닛 생성
    /// 스킬 가져오기 및 시전
    /// 적용된 유닛 수 출력
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Stun()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Stun");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce 테스트
    /// 아군 및 적군 유닛 생성
    /// 스킬 가져오기 및 시전
    /// 적용된 유닛 수 출력
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Parrying()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Parrying");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce 테스트
    /// 아군 및 적군 유닛 생성
    /// 스킬 가져오기 및 시전
    /// 적용된 유닛 수 출력
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Rooted()
    {

        //유닛 생성
        yield return UnitSettings(new Vector2Int(8, 3));

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Rooted");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //스킬 적용된 적 유닛 수 가져오기
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 3);
        yield return CheckUnitInStatusDataCount(skillData, 3);

    }

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
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 9);

        yield return CheckUnitInStatusDataCount(skillData, 9);
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
        var value = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == TYPE_BATTLE_TEAM.Left && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.GetUnitActor().nowHealthValue);
        Debug.Log(value);
        Assert.IsTrue(value == 2000);

        //아군 유닛 체력 50 깎기
        var aliesBlocks = _blocks.Where(block => block.GetUnitActor() != null).ToArray();
        for(int i = 0; i < aliesBlocks.Length; i++)
        {
            aliesBlocks[i].GetUnitActor().DecreaseHealth(50);
        }        

        //스킬 가져오기 및 시전
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heal");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //아군 체력 가져오기
        //25 * 30 = 750
        //30 * 9 = 270
        //750 + 270 = 1020
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Left, 1020);
    }




    [UnityTest]
    public IEnumerator SkillData_UnitSettings()
    {
        yield return UnitSettings(new Vector2Int(8, 3));
        
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Left) == 1, Is.True);
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Right) == 104, Is.True);

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
        _caster = unitManager.CreateUnit(casterUnitCard, casterUnitCard.UnitKeys[0], block, TYPE_BATTLE_TEAM.Left);


        //동맹 액터가 있으면
        if (aliesTargetData != null)
        {
            //동맹 블록 가져오기
            var aliesBlocks = FieldManager.GetTargetBlocksInBlankBlock(block, aliesTargetData, TYPE_BATTLE_TEAM.Left);

            if (aliesBlocks != null)
            {
                //동맹 블록에 동맹 카드 제작 및 액터 생성
                for (int i = 0; i < aliesBlocks.Length; i++)
                {
                    var uCardL = UnitCard.Create(_uData);
                    unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], aliesBlocks[i], TYPE_BATTLE_TEAM.Left);
                }
            }
        }
        

        yield return null;

        //모든 블록 가져오기
        _blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < _blocks.Length; i++)
        {
            //적 액터 생성
            if (_blocks[i].GetUnitActor() == null)
            {
                var uCardR = UnitCard.Create(_uData);
                unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], _blocks[i], TYPE_BATTLE_TEAM.Right);
            }
        }
        Debug.Log($"Blocks Count {_blocks.Length}");
        Debug.Log($"Left UnitActor {_blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == TYPE_BATTLE_TEAM.Left && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Count()}");
        Debug.Log($"Right UnitActor {_blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == TYPE_BATTLE_TEAM.Right && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Count()}");

        yield return null;

    }


    public IEnumerator CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM typeTeam, int targetValue)
    {
        var value = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == typeTeam && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.GetUnitActor().nowHealthValue);
        Debug.Log(value);
        Assert.IsTrue(value == targetValue);
        yield return null;
    }

    public IEnumerator CheckUnitInStatusDataCount(SkillData skillData, int targetValue)
    {
        var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        Debug.Log(count);
        Assert.IsTrue(count == targetValue);
        yield return null;
    }

    public IEnumerator CheckUnitCount(TYPE_BATTLE_TEAM typeTeam, int targetValue)
    {
        var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == typeTeam && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Count();
        Debug.Log(count);
        Assert.IsTrue(count == targetValue);
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