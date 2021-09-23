#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public class SkillDataEditTester
{

    Dummy_CommanderActor _cActor;
    ICaster _caster;
    SkillData _skillData;
    SkillData.SkillDataProcess _skillDataProcess;

    [SetUp]
    public void SetUp()
    {
        FieldManagerEditTester.DefaultSetUp();
        _caster = new Dummy_CommanderActor();
        _skillData = new SkillData();
        _cActor = new Dummy_CommanderActor();
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
        _skillData.ActivateSkillProcess(_caster);

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
        _skillData.ActivateSkillProcess(_caster);

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
        _skillData.ActivateSkillProcess(_caster);

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
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_BulletData_And_StatusData()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_BulletData_And_StatusData_Callback()
    {
        var process = new SkillData.SkillDataProcess();

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }



    [Test]
    public void SkillDataProcess_EffectData_StatusData()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);


        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData_StatusData_Callback()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData_BulletData()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData_BulletData_Callback()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }

    [Test]
    public void SkillDataProcess_EffectData_BulletData_StatusData_All_Callback()
    {
        var process = new SkillData.SkillDataProcess();
        process.SetCastEffectData(new EffectData());

        process.SetTypeUsedBulletData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetBulletData(new BulletData());
        process.SetBulletTargetData(new TargetData());

        process.SetTypeUsedStatusData(SkillData.SkillDataProcess.TYPE_USED_DATA.Used_Callback);
        process.SetStatusTargetData(new TargetData());
        process.SetStatusData(new StatusData());

        _skillData.SetSkillDataProcess(process);
        _skillData.ActivateSkillProcess(_caster);

        Assert.Pass();
    }
    #endregion

    private void SetTest(TYPE_SKILL_ACTIVATE typeSkillActivate, TYPE_TARGET_TEAM typeTargetTeam, int assertCount)
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