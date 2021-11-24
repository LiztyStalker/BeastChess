#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BalancePlayTest : PlayTest
{

    static string[] values = {"SpearShield",
                            "Pike",
                            "SwordShield",
                            "TwoHandedSword",
                            "Mace",
                            "ClubShield",
                            "AxeShield",
                            "Outlaw",
                            "Archer",
                            "Crossbow",
                            "Blowgun",
                            "Skirmisher",
                            "Assaulter",
                            "Claw",
                            "Sneak",
                            "Shield",
                            "Armor"
    };

    private List<BalancePlayTestAsset> list = new List<BalancePlayTestAsset>();

    private struct BalancePlayTestAsset
    {
        public UnitData unitData;
        public TYPE_BATTLE_TEAM typeTeam;
        public Vector2Int[] positions;
    }

    [SetUp]
    public void SetUp()
    {
        list.Clear();
    }

    [TearDown]
    public void TearDown()
    {
        list.Clear();
    }

    [UnityTest]
    public IEnumerator BalanceTest_Total([ValueSource("values")] string value1, [ValueSource("values")] string value2)
    {
        Time.timeScale = 10f;
        battleFieldManager.ClearAllUnits();
        if (value1 == value2)
        {
            yield break;
        }
        else
        {
            var dataL = DataStorage.Instance.GetDataOrNull<UnitData>(value1);
            var dataR = DataStorage.Instance.GetDataOrNull<UnitData>(value2);
            yield return BattleTest(dataL, dataR);
        }
        Time.timeScale = 1f;
    }


    [UnityTest]
    public IEnumerator BalanceTest_Mace_Outlaw()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Mace");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Outlaw");

        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_ClubShield_AxeShield()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("ClubShield");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("AxeShield");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_SwordShield_SpearShield()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("SwordShield");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("SpearShield");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Pike_TwoHandedSword()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Pike");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("TwoHandedSword");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Blowgun_Skirmisher()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Blowgun");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Skirmisher");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Archer_Crossbow()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Archer");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Crossbow");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Assaulter_Claw()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Assaulter");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Claw");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Sneak_Claw()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Sneak");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Claw");
        yield return BattleTest(dataL, dataR);
    }

    [UnityTest]
    public IEnumerator BalanceTest_Assaulter_Sneak()
    {
        var dataL = DataStorage.Instance.GetDataOrNull<UnitData>("Sneak");
        var dataR = DataStorage.Instance.GetDataOrNull<UnitData>("Assaulter");
        yield return BattleTest(dataL, dataR);
    }

    private IEnumerator BattleTest(UnitData leftData, UnitData rightData)
    {
        Debug.Log($"Test Start {leftData.name} - {rightData.name}");

        var blocksL = FieldManager.GetTeamUnitBlocksFromVertical(TYPE_BATTLE_TEAM.Left);
        var blocksR = FieldManager.GetTeamUnitBlocksFromVertical(TYPE_BATTLE_TEAM.Right);

        for (int i = 0; i < blocksL.Length; i++)
        {
            var uCardL = UnitCard.Create(leftData);
            unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], blocksL[i], TYPE_BATTLE_TEAM.Left);
        }
        yield return null;

        for (int i = 0; i < blocksR.Length; i++)
        {
            var uCardR = UnitCard.Create(rightData);
            unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], blocksR[i], TYPE_BATTLE_TEAM.Right);
        }
        yield return null;

        var costRateL = (42f / (float)leftData.SquadCount) * (float)leftData.AppearCostValue;
        var costRateR = (42f / (float)rightData.SquadCount) * (float)rightData.AppearCostValue;

        Debug.Log($"[CountL {FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Left)}  Rate {costRateL}] - [CountR {FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Right)}  Rate {costRateR}]");

        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Left) == 42, Is.True);
        Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Right) == 42, Is.True);
        yield return null;

        int turn = 10;

        while (!unitManager.IsLiveUnitsEmpty()) { 

            battleFieldManager.NextTurnTester(TYPE_BATTLE_TURN.Forward, TYPE_BATTLE_TURN.Forward);

            while (battleFieldManager.isRunning)
            {
                yield return null;
            }
            turn--;
            if (turn == 0)
                break;
        }

        Debug.Log(unitManager.BattleResultToString());
        Debug.Log("TestEnd");
    }


    //private IEnumerator BattleTest(List<BalancePlayTest> list)
    //{
    //    var blocksL = FieldManager.GetTeamUnitBlocks(TYPE_TEAM.Left);
    //    var blocksR = FieldManager.GetTeamUnitBlocks(TYPE_TEAM.Right);

    //    for (int i = 0; i < blocksL.Length; i++)
    //    {
    //        var uCardL = UnitCard.Create(leftData);
    //        unitManager.CreateUnit(uCardL, uCardL.UnitKeys[0], blocksL[i], TYPE_TEAM.Left);
    //    }
    //    yield return null;

    //    for (int i = 0; i < blocksR.Length; i++)
    //    {
    //        var uCardR = UnitCard.Create(rightData);
    //        unitManager.CreateUnit(uCardR, uCardR.UnitKeys[0], blocksR[i], TYPE_TEAM.Right);
    //    }
    //    yield return null;

    //    Debug.Log(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Left));
    //    Debug.Log(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Right));

    //    Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Left) == 42, Is.True);
    //    Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_TEAM.Right) == 42, Is.True);
    //    yield return null;

    //    int turn = 10;

    //    while (!unitManager.IsLiveUnitsEmpty())
    //    {

    //        battleFieldManager.NextTurnTester(TYPE_BATTLE_TURN.Forward, TYPE_BATTLE_TURN.Forward);

    //        while (battleFieldManager.isRunning)
    //        {
    //            yield return null;
    //        }
    //        turn--;
    //        if (turn == 0)
    //            break;
    //    }

    //    Debug.Log(unitManager.BattleResultToString());

    //}
}
#endif