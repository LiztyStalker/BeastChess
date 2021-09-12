#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public class SkillDataEditTester
{


    SkillData _skillData;

    [SetUp]
    public void SetUp()
    {
        FieldManagerEditTester.DefaultSetUp();
        _skillData = new SkillData();
    }

    [TearDown]
    public void TearDown()
    {
        FieldManagerEditTester.DefaultTearDown();
        _skillData = null;
    }

    [Test]
    public void SkillData_Passive_Test()
    {
        _skillData.SetData(TYPE_SKILL_ACTIVATE.Passive);
        Assert.IsTrue(_skillData.typeSkillActivate == TYPE_SKILL_ACTIVATE.Passive);


    }

    [Test]
    public void SkillData_PreActive_Test()
    {
        _skillData.SetData(TYPE_SKILL_ACTIVATE.PreActive);
        Assert.IsTrue(_skillData.typeSkillActivate == TYPE_SKILL_ACTIVATE.PreActive);
    }

    [Test]
    public void SkillData_Active_Test()
    {
        _skillData.SetData(TYPE_SKILL_ACTIVATE.Active);
        Assert.IsTrue(_skillData.typeSkillActivate == TYPE_SKILL_ACTIVATE.Active);
    }


}
#endif