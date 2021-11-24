#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class TargetDataEditTester
{

    private Vector2Int _fieldSize;

    [SetUp]
    public void SetUp()
    {
        FieldManagerEditTester.DefaultSetUp();
        _fieldSize = FieldManagerEditTester.fieldSize;
    }

    [TearDown]
    public void TearDown()
    {
        FieldManagerEditTester.DefaultTearDown();
    }
    #region ##### AllTarget #####
    [Test]
    public void TargetData_AllTarget()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_AllTarget_Alies()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        targetData.SetTypeTeam(TYPE_TARGET_TEAM.Alies);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_AllTarget_Enemy()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData();
        targetData.SetIsAllTargetRange(true);
        targetData.SetTypeTeam(TYPE_TARGET_TEAM.Enemy);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    #endregion

    #region ##### Normal #####

    [Test]
    public void TargetData_Normal_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Normal_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range5_TargetCnt1_None()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.None, true, 1);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range5_TargetCnt2_None()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.None, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range5_TargetCnt2_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.High, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range5_TargetCnt2_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Normal_All_Start0_Range5_TargetCnt2_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Triangle #####


    [Test]
    public void TargetData_Triangle_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start0_Range5_TargetCnt1_None()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.None, true, 1);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start0_Range5_TargetCnt5_None()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.None, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start0_Range5_TargetCnt5_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.High, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start0_Range5_TargetCnt5_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_All_Start0_Range5_TargetCnt5_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }


    [Test]
    public void TargetData_Triangle_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Triangle_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Triangle_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Square #####


    [Test]
    public void TargetData_Square_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range5_TargetCnt3()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range5_TargetCnt3_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range5_TargetCnt3_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start0_Range5_TargetCnt3_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }


    [Test]
    public void TargetData_Square_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Square_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Square_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Vertical #####


    [Test]
    public void TargetData_Vertical_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range2_TargetCnt3()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range2_TargetCnt3_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range2_TargetCnt3_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start0_Range2_TargetCnt3_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Vertical_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Vertical_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Cross #####


    [Test]
    public void TargetData_Cross_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range3_TargetCnt3()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range3_TargetCnt3_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range3_TargetCnt3_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start0_Range3_TargetCnt3_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }


    [Test]
    public void TargetData_Cross_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Cross_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Cross_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Rhombus #####


    [Test]
    public void TargetData_Rhombus_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range2_TargetCnt3()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range2_TargetCnt3_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range2_TargetCnt3_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start0_Range2_TargetCnt3_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_All_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_All_Start1_Range3_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Alies_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Enemy_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Rhombus_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_Enemy_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Rhombus_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Circle #####


    [Test]
    public void TargetData_Circle_All_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }


    [Test]
    public void TargetData_Circle_All_Start0_Range3_TargetCnt3()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range3_TargetCnt3_High()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range3_TargetCnt3_Low()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_All_Start0_Range3_TargetCnt3_Random()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }




    [Test]
    public void TargetData_Circle_All_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_All_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_All_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }


    [Test]
    public void TargetData_Circle_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_All_Start1_Range3_AllTargetCnt_Reverse()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Right);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_All_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Alies_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Alies_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Alies_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Alies_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Alies_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Alies_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Alies_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Alies_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Enemy_Start0_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Enemy_Start0_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Enemy_Start0_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Enemy_Start0_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    [Test]
    public void TargetData_Circle_Enemy_Start1_Range0_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Enemy_Start1_Range1_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Enemy_Start1_Range3_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }
    [Test]
    public void TargetData_Circle_Enemy_Start1_Range20_AllTargetCnt()
    {
        FieldManagerEditTester.CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.GetUnitActor(), targetData, TYPE_BATTLE_TEAM.Left);
        FieldManagerEditTester.PrintTargetBlocks(blocks);
    }

    #endregion

}
#endif