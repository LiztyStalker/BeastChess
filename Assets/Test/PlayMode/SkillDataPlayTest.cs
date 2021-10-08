#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDataPlayTest : PlayTest
{

    ICaster caster;
    IFieldBlock[] blocks;


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
        skillData.CastSkillProcess(caster, skillData.typeSkillCast);

        yield return null;

        //��ų ����� ���� �� ��������
        var count = blocks.Where(block => block.unitActor != null && block.unitActor.IsHasStatusData(skillData.GetStatusData())).Count();
        Debug.Log(count);
        Assert.IsTrue(count == 9);
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
        var data = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");

        //������ ��� ��������
        var block = FieldManager.GetBlock(casterBlock.x, casterBlock.y);

        //������ ī�� ���� �� ���� ����
        var casterUnitCard = UnitCard.Create(data);
        caster = unitManager.CreateUnit(casterUnitCard, casterUnitCard.UnitKeys[0], block, TYPE_TEAM.Left);

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
                    var uCardL = UnitCard.Create(data);
                    unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], aliesBlocks[i], TYPE_TEAM.Left);
                }
            }
        }
        

        yield return null;

        //��� ��� ��������
        blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            //�� ���� ����
            if (blocks[i].unitActor == null)
            {
                var uCardR = UnitCard.Create(data);
                unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], blocks[i], TYPE_TEAM.Right);
            }
        }
        yield return null;

    }

    [TearDown]
    public void TearDown()
    {
        caster = null;
        blocks = null;
    }
}
#endif