#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public class SkillDataEditTester
{

    Dummy_CommanderActor _cActor;
    ICaster _caster;
    Dummy_UnitActor _uActor;
    SkillData _skillData;
    SkillData.SkillDataProcess _skillDataProcess;

    [SetUp]
    public void SetUp()
    {
        FieldManagerEditTester.DefaultSetUp();
        _caster = new Dummy_CommanderActor();
        _skillData = new SkillData();
        _cActor = new Dummy_CommanderActor();
        _uActor = new Dummy_UnitActor();
        _skillDataProcess = new SkillData.SkillDataProcess();
    }

    [TearDown]
    public void TearDown()
    {
        FieldManagerEditTester.DefaultTearDown();
        _skillData = null;
        _cActor = null;
        _skillDataProcess = null;
    }


    #region ##### SkillData #####
    [Test]
    public void SkillData_Passive_All_Test()
    {
        SetTest(TYPE_SKILL_CAST.DeployCast, TYPE_TARGET_TEAM.All, 17 * 7);
    }   

    [Test]
    public void SkillData_Passive_Alies_Test()
    {
        SetTest(TYPE_SKILL_CAST.DeployCast, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_Passive_Enemy_Test()
    {
        SetTest(TYPE_SKILL_CAST.DeployCast, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }

    [Test]
    public void SkillData_PreActive_All_Test()
    {
        SetTest(TYPE_SKILL_CAST.PreCast, TYPE_TARGET_TEAM.All, 17 * 7);
    }

    [Test]
    public void SkillData_PreActive_Alies_Test()
    {
        SetTest(TYPE_SKILL_CAST.PreCast, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_PreActive_Enemy_Test()
    {
        SetTest(TYPE_SKILL_CAST.PreCast, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }


    [Test]
    public void SkillData_Active_All_Test()
    {
        SetTest(TYPE_SKILL_CAST.PreCast, TYPE_TARGET_TEAM.All, 17 * 7);
    }

    [Test]
    public void SkillData_Active_Alies_Test()
    {
        SetTest(TYPE_SKILL_CAST.AttackCast, TYPE_TARGET_TEAM.Alies, 9 * 7);
    }

    [Test]
    public void SkillData_Active_Enemy_Test()
    {
        SetTest(TYPE_SKILL_CAST.AttackCast, TYPE_TARGET_TEAM.Enemy, 8 * 7);
    }

    #endregion


    #region ##### SkillDataProcess #####

    [Test]
    public void SkillDataProcess_Test()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());
        
        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        process.SetTypeUsedHealth(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetIncreaseNowHealthTargetData(new TargetData());
        process.SetIncreaseNowHealthValue(0);

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_StatusData()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_HealthData()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedHealth(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetIncreaseNowHealthTargetData(new TargetData());
        process.SetIncreaseNowHealthValue(10);

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_BulletData()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_BulletData_Callback()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetStatusData(new StatusData());
        process.SetStatusTargetData(new TargetData());


        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);

        Assert.Pass();
    }



    [Test]
    public void SkillDataProcess_EffectData_Callback()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetStatusData(new StatusData());
        process.SetStatusTargetData(new TargetData());

        _skillData.SetSkillDataProcess(process);
        _skillData.CastSkillProcess(_caster, TYPE_SKILL_CAST.DeployCast);


        Assert.Pass();
    }


    [Test]
    public void SkillData_DeployCast_Test()
    {
        _skillData.SetData(TYPE_SKILL_CAST.DeployCast);
        Assert.IsTrue(_skillData.IsTypeSkillCast(TYPE_SKILL_CAST.DeployCast));
    }

    [Test]
    public void SkillData_PreCast_Test()
    {
        _skillData.SetData(TYPE_SKILL_CAST.PreCast);
        Assert.IsTrue(_skillData.IsTypeSkillCast(TYPE_SKILL_CAST.PreCast));
    }


    [Test]
    public void SkillData_AttackCast_Test()
    {
        _skillData.SetData(TYPE_SKILL_CAST.AttackCast);
        Assert.IsTrue(_skillData.IsTypeSkillCast(TYPE_SKILL_CAST.AttackCast));
    }


    [Test]
    public void SkillData_AttackedCast_Test()
    {
        _skillData.SetData(TYPE_SKILL_CAST.AttackedCast);
        Assert.IsTrue(_skillData.IsTypeSkillCast(TYPE_SKILL_CAST.AttackedCast));
    }



    [Test]
    public void SkillData_Condition_None()
    {
        _skillData.SetSkillDataCondition(TYPE_SKILL_CAST_CONDITION.None);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_Condition_Health()
    {
        _skillData.SetSkillDataCondition(TYPE_SKILL_CAST_CONDITION.Health);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_ConditionCompare_Value_GreaterThan()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.GreaterThan);
        _skillData.SetSkillDataConditionValue(49);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_ConditionCompare_Value_LessThan()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.LessThan);
        _skillData.SetSkillDataConditionValue(51);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_ConditionCompare_Value_Equal()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.Equal);
        _skillData.SetSkillDataConditionValue(50);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }



    [Test]
    public void SkillData_ConditionCompare_Value_NotEqual()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.NotEqual);
        _skillData.SetSkillDataConditionValue(49);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }



    [Test]
    public void SkillData_ConditionCompare_Rate_GreaterThan()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.GreaterThan);
        _skillData.SetSkillDataConditionCompareValue(TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE.Rate);
        _skillData.SetSkillDataConditionValue(0.4f);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_ConditionCompare_Rate_LessThan()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataCondition(TYPE_SKILL_CAST_CONDITION.Health);
        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.LessThan);
        _skillData.SetSkillDataConditionCompareValue(TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE.Rate);
        _skillData.SetSkillDataConditionValue(0.6f);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }

    [Test]
    public void SkillData_ConditionCompare_Rate_Equal()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.Equal);
        _skillData.SetSkillDataConditionCompareValue(TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE.Rate);
        _skillData.SetSkillDataConditionValue(0.5f);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }



    [Test]
    public void SkillData_ConditionCompare_Rate_NotEqual()
    {
        _uActor.SetNowHealth(50);

        _skillData.SetSkillDataConditionCompare(TYPE_SKILL_CAST_CONDITION_COMPARE.NotEqual);
        _skillData.SetSkillDataConditionCompareValue(TYPE_SKILL_CAST_CONDITION_COMPARE_VALUE.Rate);
        _skillData.SetSkillDataConditionValue(0.4f);
        Assert.IsTrue(_skillData.IsSkillCondition(_uActor));
    }




    #endregion

    private void SetTest(TYPE_SKILL_CAST typeSkillActivate, TYPE_TARGET_TEAM typeTargetTeam, int assertCount)
    {
        FieldManagerEditTester.CreateGridUnitActors();
        _skillData.SetData(typeSkillActivate);
        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        targetData.SetTypeTeam(typeTargetTeam);
        _skillData.SetData(targetData);
        _cActor.AddSkill(_skillData);
        UnitManager.ReceiveSkill(_cActor, _skillData, TYPE_TEAM.Left);
        var blocks = FieldManager.GetAllBlocks();
        var count = FieldManagerEditTester.PrintSkillBlocks(blocks, typeSkillActivate);
        Debug.Log(count);
        Assert.IsTrue(assertCount == count);
    }




}
#endif