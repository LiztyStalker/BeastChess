#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public class SkillDataEditTester
{

    Dummy_CommanderActor _cActor;
    SkillData _skillData;

    [SetUp]
    public void SetUp()
    {
        FieldManagerEditTester.DefaultSetUp();
        _skillData = new SkillData();
        _cActor = new Dummy_CommanderActor();        
    }

    [TearDown]
    public void TearDown()
    {
        FieldManagerEditTester.DefaultTearDown();
        _skillData = null;
        _cActor = null;
    }

    [Test]
    public void SkillData_Passive_All_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Passive, TYPE_TARGET_TEAM.All, 17 * 7);
    }   

    [Test]
    public void SkillData_Passive_Alies_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Passive, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_Passive_Enemy_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Passive, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }

    [Test]
    public void SkillData_PreActive_All_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.PreActive, TYPE_TARGET_TEAM.All, 17 * 7);
    }

    [Test]
    public void SkillData_PreActive_Alies_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.PreActive, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_PreActive_Enemy_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.PreActive, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }


    [Test]
    public void SkillData_Active_All_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.PreActive, TYPE_TARGET_TEAM.All, 17 * 7);
    }

    [Test]
    public void SkillData_Active_Alies_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Active, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_Active_Enemy_Test()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Active, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }

    [Test]
    public void SkillData_Active_All_IncreaseHealth()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Active, TYPE_TARGET_TEAM.All, 10, 17 * 7 * 110);
    }

    [Test]
    public void SkillData_Active_All_DecreaseHealth()
    {
        SetTest(TYPE_SKILL_ACTIVATE.Active, TYPE_TARGET_TEAM.All, -10, 17 * 7 * 90);
    }

    private void SetTest(TYPE_SKILL_ACTIVATE typeSkillActivate, TYPE_TARGET_TEAM typeTargetTeam, int healthValue, int assertCount)
    {
        FieldManagerEditTester.CreateGridUnitActors();

        _skillData.SetData(typeSkillActivate);
        _skillData.SetData(true, healthValue);

        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        targetData.SetTypeTeam(typeTargetTeam);

        _skillData.SetData(targetData);
        _cActor.AddSkill(_skillData);

        UnitManager.CastSkill(_cActor, _skillData, TYPE_TEAM.Left);
        var blocks = FieldManager.GetAllBlocks();
        var totalHealth = FieldManagerEditTester.PrintHealthBlocks(blocks);
        Debug.Log(totalHealth);
        Assert.IsTrue(assertCount == totalHealth);
    }
    private void SetTest(TYPE_SKILL_ACTIVATE typeSkillActivate, TYPE_TARGET_TEAM typeTargetTeam, int assertCount)
    {
        FieldManagerEditTester.CreateGridUnitActors();
        _skillData.SetData(typeSkillActivate);
        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        targetData.SetTypeTeam(typeTargetTeam);
        _skillData.SetData(targetData);
        _cActor.AddSkill(_skillData);
        UnitManager.CastSkill(_cActor, _skillData, TYPE_TEAM.Left);
        var blocks = FieldManager.GetAllBlocks();
        var count = FieldManagerEditTester.PrintSkillBlocks(blocks, typeSkillActivate);
        Debug.Log(count);
        Assert.IsTrue(assertCount == count);
    }
}
#endif