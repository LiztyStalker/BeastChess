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
        //������ ������ ��ġ�� �����մϴ�
        var targetData = new TargetData();
        targetData.SetTypeTargetRange(TYPE_TARGET_RANGE.Square, true, 0, 2);

        //���� ����
        yield return UnitSettings(new Vector2Int(8, 3), targetData);

        //���� ��ü ü��
        //conscript - 80 * 25 = 2000
        var value = _blocks.Where(block => block.unitActor != null && block.unitActor.typeTeam == TYPE_TEAM.Left && block.unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle).Sum(block => block.unitActor.nowHealthValue);
        Debug.Log(value);
        Assert.IsTrue(value == 2000);

        //�Ʊ� ���� ü�� 50 ���
        var aliesBlocks = _blocks.Where(block => block.unitActor != null).ToArray();
        for(int i = 0; i < aliesBlocks.Length; i++)
        {
            aliesBlocks[i].unitActor.DecreaseHealth(50);
        }        

        //��ų �������� �� ����
        var skillData = DataStorage.Instance.GetDataOrNull<SkillData>("Heal");
        skillData.CastSkillProcess(_caster, skillData.typeSkillCast);

        yield return null;

        //�Ʊ� ü�� ��������
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
        _caster = unitManager.CreateUnit(casterUnitCard, casterUnitCard.UnitKeys[0], block, TYPE_TEAM.Left);

        //���� ���Ͱ� ������
        if (aliesTargetData != null)
        {
            //���� ��� ��������
            var aliesBlocks = FieldManager.GetTargetBlocksInBlankBlock(block, aliesTargetData, TYPE_TEAM.Left);

            if (aliesBlocks != null)
            {
                //���� ��Ͽ� ���� ī�� ���� �� ���� ����
                for (int i = 0; i < aliesBlocks.Length; i++)
                {
                    var uCardL = UnitCard.Create(_uData);
                    unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], aliesBlocks[i], TYPE_TEAM.Left);
                }
            }
        }
        

        yield return null;

        //��� ��� ��������
        _blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < _blocks.Length; i++)
        {
            //�� ���� ����
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