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

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Recovery");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Feed()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Feed");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Burn()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Burn");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 8);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Ram()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Ram");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Heist()
    {
        //������ ������ ��ġ�� �����մϴ�
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heist");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        yield return CheckUnitInStatusDataCount(skillData, 9);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Storm()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Storm");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 104);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 7800);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_Blizard()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Blizard");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 104);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 7280);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_Freeze()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Freeze");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        yield return CheckUnitInStatusDataCount(skillData, 3);
    }


    [UnityTest]
    public IEnumerator SkillData_StatusData_IceWall()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("IceWall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);



        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitCount(TYPE_BATTLE_TEAM.Left, 2);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_FireWall()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("FireWall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitCount(TYPE_BATTLE_TEAM.Left, 2);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_Fireball()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Fireball");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 8050);
    }

    [UnityTest]
    public IEnumerator SkillData_StatusData_ChainRockFall()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("ChainRockFall");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return new WaitForSeconds(2f);

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitTotalNowHealth(TYPE_BATTLE_TEAM.Right, 8070);
    }

   
    [UnityTest]
    public IEnumerator SkillData_StatusData_Blind()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Blind");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 3);
    }

    /// <summary>
    /// SkinForce �׽�Ʈ
    /// �Ʊ� �� ���� ���� ����
    /// ��ų �������� �� ����
    /// ����� ���� �� ���
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Penetrate()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Penetrate");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce �׽�Ʈ
    /// �Ʊ� �� ���� ���� ����
    /// ��ų �������� �� ����
    /// ����� ���� �� ���
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Stun()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Stun");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce �׽�Ʈ
    /// �Ʊ� �� ���� ���� ����
    /// ��ų �������� �� ����
    /// ����� ���� �� ���
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Parrying()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Parrying");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 1);
        yield return CheckUnitInStatusDataCount(skillData, 1);
    }

    /// <summary>
    /// SkinForce �׽�Ʈ
    /// �Ʊ� �� ���� ���� ����
    /// ��ų �������� �� ����
    /// ����� ���� �� ���
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_Rooted()
    {

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3));

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Rooted");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� �� ���� �� ��������
        //var count = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().IsHasStatusData(skillData.GetStatusData())).Count();
        //Debug.Log(count);
        //Assert.IsTrue(count == 3);
        yield return CheckUnitInStatusDataCount(skillData, 3);

    }

    /// <summary>
    /// SkinForce �׽�Ʈ
    /// �Ʊ� �� ���� ���� ����
    /// ��ų �������� �� ����
    /// ����� ���� �� ���
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SkillData_StatusData_SkinForce()
    {
        //������ ������ ��ġ�� �����մϴ�
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("SkinForce");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� ���� �� ��������
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
        //������ ������ ��ġ�� �����մϴ�
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //���� ��ü ü��
        //conscript - 80 * 25 = 2000
        var value = _blocks.Where(block => block.GetUnitActor() != null && block.GetUnitActor().typeTeam == TYPE_BATTLE_TEAM.Left && block.GetUnitActor().typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.GetUnitActor().nowHealthValue);
        Debug.Log(value);
        Assert.IsTrue(value == 2000);

        //�Ʊ� ���� ü�� 50 ���
        var aliesBlocks = _blocks.Where(block => block.GetUnitActor() != null).ToArray();
        for(int i = 0; i < aliesBlocks.Length; i++)
        {
            aliesBlocks[i].GetUnitActor().DecreaseHealth(50);
        }        

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heal");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //�Ʊ� ü�� ��������
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
    /// ������ �����մϴ�
    /// </summary>
    /// <param name="casterBlock">��ų�� ����� �Ʊ� ĳ���� ��ġ</param>
    /// <param name="aliesTargetData">���� �����Դϴ�. null�̸� ���� ������ �������� �ʽ��ϴ�</param>
    /// <returns></returns>
    private IEnumerator UnitSettings(Vector2Int casterBlock, TargetData aliesTargetData = null)
    {
        //���� ��������
        _uData = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");

        //������ ��� ��������
        var block = FieldManager.GetBlock(casterBlock.x, casterBlock.y);

        //������ ī�� ���� �� ���� ����
        var casterUnitCard = UnitCard.Create(_uData);
        _caster = unitManager.CreateUnit(casterUnitCard, casterUnitCard.UnitKeys[0], block, TYPE_BATTLE_TEAM.Left);


        //���� ���Ͱ� ������
        if (aliesTargetData != null)
        {
            //���� ��� ��������
            var aliesBlocks = FieldManager.GetTargetBlocksInBlankBlock(block, aliesTargetData, TYPE_BATTLE_TEAM.Left);

            if (aliesBlocks != null)
            {
                //���� ��Ͽ� ���� ī�� ���� �� ���� ����
                for (int i = 0; i < aliesBlocks.Length; i++)
                {
                    var uCardL = UnitCard.Create(_uData);
                    unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], aliesBlocks[i], TYPE_BATTLE_TEAM.Left);
                }
            }
        }
        

        yield return null;

        //��� ��� ��������
        _blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < _blocks.Length; i++)
        {
            //�� ���� ����
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