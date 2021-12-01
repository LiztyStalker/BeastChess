#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class PlayTest
{
    protected Camera camera;
    protected BattleFieldManager battleFieldManager;
    protected FieldGenerator fieldGenerator;
    protected UnitManager unitManager;

    [UnitySetUp]
    public virtual IEnumerator UnitySetUp()
    {
        Debug.Log("UnitySetUp");

        var cameraObject = new GameObject();
        cameraObject.AddComponent<AudioListener>();
        camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 7f;
        camera.transform.position = Vector3.back * 10f;


        //¸ÊÇÊµå Á¦ÀÛ
        var gameObject = new GameObject();
        fieldGenerator = gameObject.AddComponent<FieldGenerator>();
        unitManager = gameObject.AddComponent<UnitManager>();
        battleFieldManager = gameObject.AddComponent<BattleFieldManager>();

        yield return null;
        
        battleFieldManager.ClearAllUnits();

        Assert.NotNull(battleFieldManager);
    }

    [UnityTearDown]
    public virtual IEnumerator UnityTearDown()
    {
        Debug.Log("TearDown");
        //¸ÊÇÊµå ¹× À¯´Ö Á¦°Å
        battleFieldManager.ClearAllUnits(true);

        UnitManager.CleanUpTest();
        FieldManager.CleanUp();

        yield return null;
        Object.DestroyImmediate(battleFieldManager.gameObject);
        Object.DestroyImmediate(camera.gameObject);
        yield return null;
        BattleFieldOutpost.Dispose();
    }

    [UnityTest]
    public IEnumerator BattleField_Created()
    {
        yield return null;
        Assert.NotNull(battleFieldManager);
    }

    [UnityTest]
    public IEnumerator BattleField_AllCommandTest()
    {
        yield return BattleCommandTest(true);
    }

    [UnityTest]
    public IEnumerator BattleField_RandomCommandTest()
    {
        yield return BattleCommandTest(false);
    }

    public IEnumerator BattleCommandTest(bool isLinear)
    {
        var leftData = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");
        var rightData = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");

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

        //Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Left) == 42, Is.True);
        //Assert.That(FieldManager.IsHasTeamUnitActorCount(TYPE_BATTLE_TEAM.Right) == 42, Is.True);
        yield return null;

        int turn = 10;
        TYPE_BATTLE_TURN[] _turns = new TYPE_BATTLE_TURN[turn];

        if (isLinear)
        {
            for(int i = 0; i < turn; i++)
            {
                _turns[i] = (TYPE_BATTLE_TURN)(i % System.Enum.GetValues(typeof(TYPE_BATTLE_TURN)).Length);
            }
        }
        else
        {
            for (int i = 0; i < turn; i++)
            {
                _turns[i] = (TYPE_BATTLE_TURN)UnityEngine.Random.Range(0, (int)TYPE_BATTLE_TURN.Backward + 1);
            }
        }


        while (!unitManager.IsLiveUnitsEmpty())
        {
            Debug.Log(_turns[turn - 1]);
            battleFieldManager.NextTurnTester(_turns[turn - 1], _turns[turn - 1]);

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

}
#endif