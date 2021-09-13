#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public class StatusDataEditTester
{

    private ICaster _caster;
    private Dummy_UnitActor _unitActor;
    private StatusData _statusData;

    [SetUp]
    public void SetUp()
    {
        _statusData = new StatusData();
        _unitActor = new Dummy_UnitActor();
        _caster = new Dummy_UnitActor();
    }

    [TearDown]
    public void TearDown()
    {
        _statusData = null;
        _unitActor = null;
        _caster = null;
    }


    [Test]
    public void StatusData_LifeSpan_Caster() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _unitActor.SetStatus(_caster, _statusData);
        Assert.IsTrue(_unitActor.IsHasStatusData(StatusData.TYPE_STATUS_LIFE_SPAN.Caster));
    }

    [Test]
    public void StatusData_LifeSpan_Caster_Dead() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.RemoveStatusData(_caster);
        Assert.IsFalse(_unitActor.IsHasStatusData(_statusData));
    }

    [Test]
    public void StatusData_LifeSpan_Turn_Start()
    {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn);
        _statusData.SetTurnCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.TurnCount);
        Assert.IsTrue(statusElement.TurnCount == 2);
    }

    [Test]
    public void StatusData_LifeSpan_Turn_Next() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn);
        _statusData.SetTurnCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.Turn();
        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.TurnCount);
        Assert.IsTrue(statusElement.TurnCount == 1);
    }

    [Test]
    public void StatusData_LifeSpan_Turn_End() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn);
        _statusData.SetTurnCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.Turn();
        _unitActor.Turn();
        Assert.IsFalse(_unitActor.IsHasStatusData(_statusData));
    }

    [Test]
    public void StatusData_LifeSpan_Always() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Always);
        _unitActor.SetStatus(_caster, _statusData);
        Assert.IsTrue(_unitActor.IsHasStatusData(StatusData.TYPE_STATUS_LIFE_SPAN.Always));
    }

    [Test]
    public void StatusData_Overlap_Count2_Set1() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _statusData.SetIsOverlap(true);
        _statusData.SetOverlapCount(2);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.OverlapCount);
        Assert.IsTrue(statusElement.OverlapCount == 1);

    }

    [Test]
    public void StatusData_Overlap_Count2_Set2() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _statusData.SetIsOverlap(true);
        _statusData.SetOverlapCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.OverlapCount);
        Assert.IsTrue(statusElement.OverlapCount == 2);


    }
    [Test]
    public void StatusData_Overlap_Count2_Set3() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _statusData.SetIsOverlap(true);
        _statusData.SetOverlapCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.OverlapCount);
        Assert.IsFalse(statusElement.OverlapCount == 3);
    }

    [Test]
    public void StatusData_NotOverlap_Set1() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _statusData.SetIsOverlap(false);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.OverlapCount);
        Assert.IsTrue(statusElement.OverlapCount == 1);
    }
    [Test]
    public void StatusData_NotOverlap_Set2() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Caster);
        _statusData.SetIsOverlap(false);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.OverlapCount);
        Assert.IsFalse(statusElement.OverlapCount == 2);
    }

    [Test]
    public void StatusData_LifeSpan_Turn_Count2_NotOverlap_Set1() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn);
        _statusData.SetTurnCount(2);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.TurnCount + " " + statusElement.OverlapCount);
        Assert.IsTrue(statusElement.TurnCount == 2 && statusElement.OverlapCount == 1);
    }

    [Test]
    public void StatusData_LifeSpan_Turn_Count2_NotOverlap_Set2() {
        _statusData.SetTypeStatusLifeSpan(StatusData.TYPE_STATUS_LIFE_SPAN.Turn);
        _statusData.SetTurnCount(2);
        _unitActor.SetStatus(_caster, _statusData);
        _unitActor.SetStatus(_caster, _statusData);

        var statusElement = _unitActor.StatusActor.GetStatusElement(_statusData);
        Debug.Log(statusElement.TurnCount + " " + statusElement.OverlapCount);
        Assert.IsTrue(statusElement.TurnCount == 2 && statusElement.OverlapCount == 1);
    }
}
#endif