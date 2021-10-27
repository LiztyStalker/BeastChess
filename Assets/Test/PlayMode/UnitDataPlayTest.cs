using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitDataPlayTest : PlayTest
{


    [UnityTest]
    public IEnumerator UnitData_SettingUnits()
    {
        var data = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");

        var blocksL = FieldManager.GetTeamUnitBlocksFromVertical(TYPE_TEAM.Left);
        var blocksR = FieldManager.GetTeamUnitBlocksFromVertical(TYPE_TEAM.Right);

        for (int i = 0; i < blocksL.Length; i++)
        {
            var uCardL = UnitCard.Create(data);
            unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], blocksL[i], TYPE_TEAM.Left);
        }
        yield return null;

        for (int i = 0; i < blocksR.Length; i++)
        {
            var uCardR = UnitCard.Create(data);
            unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], blocksR[i], TYPE_TEAM.Right);
        }
        yield return null;

        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Left) == 35, Is.True);
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Right) == 35, Is.True);
        yield return null;
    }


}
