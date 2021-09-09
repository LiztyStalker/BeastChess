#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class FieldManagerEditTester
{

    //FieldBlock[][] _fieldBlocks;

    Vector2Int _fieldSize = new Vector2Int(17, 7);

    private enum TYPE_GRAPHIC_SHAPE { None = 0, Start, Fill, Caster, Alies, Enemy }

    TYPE_TARGET_RANGE _typeTargetRange;
    int _startRangeValue;


    [SetUp]
    public void SetUp()
    {
        CreateBlocks();

        _startRangeValue = 0;
    }

    [TearDown]
    public void TearDown()
    {
        FieldManager.CleanUp();
    }

    private void CreateBlocks()
    {
        var fieldBlocks = new Dummy_FieldBlock[_fieldSize.y][];

        for (int y = _fieldSize.y - 1; y >= 0; y--)
        {
            fieldBlocks[y] = new Dummy_FieldBlock[_fieldSize.x];

            for (int x = 0; x < _fieldSize.x; x++)
            {
                var block = new Dummy_FieldBlock();
                block.SetCoordinate(new Vector2Int(x, y));
                fieldBlocks[y][x] = block;
            }
        }

        FieldManager.Initialize(fieldBlocks, _fieldSize);


    }


    // A Test behaves as an ordinary method
    [Test]
    public void FieldManager_GetCellsNormal()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsNormal_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsTriangle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Triangle;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsTriangle_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Triangle;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsVertical()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Vertical;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsVertical_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Vertical;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsSquare()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Square;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsSquare_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Square;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsRhombus()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Rhombus;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }
    [Test]
    public void FieldManager_GetCellsRhombus_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Rhombus;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCross()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Cross;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCross_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Cross;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCircle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Circle;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCircle_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Circle;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        Print(_typeTargetRange, _startRangeValue, cells);
    }


    [Test]
    public void FieldManager_FieldBlocks()
    {
        PrintFieldManager();
    }

    [Test]
    public void FieldManager_SetUnitActor_All_Half()
    {
        CreateHalfUnitActors();
        PrintFieldManager();
    }

    [Test]
    public void FieldManager_SetUnitActor_All_Grid()
    {
        CreateGridUnitActors();
        PrintFieldManager();
    }

    [Test]
    public void FieldManager_SetUnitActor_All_Grid_Priority()
    {
        CreateGridUnitActors();
        PrintPriorityBlocks(FieldManager.GetAllBlocks());
    }

    private void CreateHalfUnitActors()
    {
        var blocks_L = FieldManager.GetTeamUnitBlocks(TYPE_TEAM.Left);
        for (int i = 0; i < blocks_L.Length; i++)
        {
            var uActor = new Dummy_UnitActor();
            uActor.SetTypeTeam(TYPE_TEAM.Left);
            blocks_L[i].SetUnitActor(uActor);
        }

        var blocks_R = FieldManager.GetTeamUnitBlocks(TYPE_TEAM.Right);
        for (int i = 0; i < blocks_R.Length; i++)
        {
            var uActor = new Dummy_UnitActor();
            uActor.SetTypeTeam(TYPE_TEAM.Right);
            blocks_R[i].SetUnitActor(uActor);
        }
    }

    private void CreateGridUnitActors()
    {
        var blocks = FieldManager.GetAllBlocks();

        for(int i = 0; i < blocks.Length; i++)
        {
            var x = Mathf.Abs(blocks[i].coordinate.x - (_fieldSize.x / 2)) + Mathf.Abs(blocks[i].coordinate.x - (_fieldSize.x / 2));
            var y = Mathf.Abs(blocks[i].coordinate.y - (_fieldSize.y / 2)) + Mathf.Abs(blocks[i].coordinate.y - (_fieldSize.y / 2));
            var uActor = new Dummy_UnitActor(x * y + x + y);
            
            uActor.SetTypeTeam((blocks[i].coordinate.x % 2 == 0) ? TYPE_TEAM.Left : TYPE_TEAM.Right);
            blocks[i].SetUnitActor(uActor);
        }
    }


    /// <summary>
    /// 자신을 참조하지 않음
    /// </summary>
    [Test]
    public void FieldManager_TargetData_All_NotMyself()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, false, TYPE_TARGET_RANGE.Square, 0, 2, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }


    #region ##### Normal #####

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Normal_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Normal, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range5_TargetCnt1_None()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.None, true, 1);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range5_TargetCnt2_None()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.None, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range5_TargetCnt2_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.High, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range5_TargetCnt2_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Normal_All_Start0_Range5_TargetCnt2_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Normal, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 2);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Triangle #####


    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range5_TargetCnt1_None()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.None, true, 1);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range5_TargetCnt5_None()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.None, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range5_TargetCnt5_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.High, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range5_TargetCnt5_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range5_TargetCnt5_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 5);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }


    [Test]
    public void FieldManager_TargetData_Triangle_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Triangle_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Triangle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Square #####


    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range5_TargetCnt3()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range5_TargetCnt3_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range5_TargetCnt3_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start0_Range5_TargetCnt3_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 0, 5, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }


    [Test]
    public void FieldManager_TargetData_Square_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Square_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Square, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Vertical #####


    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range2_TargetCnt3()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range2_TargetCnt3_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range2_TargetCnt3_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start0_Range2_TargetCnt3_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 0, 2, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Vertical_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Vertical, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Cross #####


    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range3_TargetCnt3()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range3_TargetCnt3_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range3_TargetCnt3_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start0_Range3_TargetCnt3_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 0, 3, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_All_Start1_Range20_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }


    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Cross_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Cross, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Rhombus #####


    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range2_TargetCnt3()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range2_TargetCnt3_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range2_TargetCnt3_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start0_Range2_TargetCnt3_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 0, 2, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range3_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Rhombus_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Rhombus, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Circle #####


    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }


    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range3_TargetCnt3()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range3_TargetCnt3_High()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.High, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range3_TargetCnt3_Low()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.Low, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_All_Start0_Range3_TargetCnt3_Random()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.Random, true, 3);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }




    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }


    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range0_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range1_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range3_AllTargetCnt_Reverse()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Right);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_All_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.All, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Alies_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Alies, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start0_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start0_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start0_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start0_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 0, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start1_Range0_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 0, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start1_Range1_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 1, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start1_Range3_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 3, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }
    [Test]
    public void FieldManager_TargetData_Circle_Enemy_Start1_Range20_AllTargetCnt()
    {
        CreateGridUnitActors();
        //PrintFieldManager();
        var targetData = new TargetData(TYPE_TARGET_TEAM.Enemy, true, TYPE_TARGET_RANGE.Circle, 1, 20, TYPE_TARGET_PRIORITY.None, false, 0);
        var block = FieldManager.GetBlock(_fieldSize.x / 2, _fieldSize.y / 2);
        var blocks = FieldManager.GetTargetBlocks(block.unitActor, targetData, TYPE_TEAM.Left);
        PrintTargetBlocks(blocks);
    }

    #endregion



    #region ##### Print #####

    private TYPE_GRAPHIC_SHAPE[][] CreatePrintCells(int lengthX, int lengthY)
    {
        var printCells = new TYPE_GRAPHIC_SHAPE[lengthY][];
        for (int i = 0; i < printCells.Length; i++)
        {
            printCells[i] = new TYPE_GRAPHIC_SHAPE[lengthX];
        }
        return printCells;
    }

    private void PrintPriorityBlocks(IFieldBlock[] fieldBlocks)
    {
        var printCells = new int[_fieldSize.y][];
        for (int i = 0; i < printCells.Length; i++)
        {
            printCells[i] = new int[_fieldSize.x];
        }

        for(int i = 0; i < fieldBlocks.Length; i++)
        {
            var block = fieldBlocks[i];
            var coordinate = block.coordinate;
            if(block.unitActor != null) printCells[coordinate.y][coordinate.x] = block.unitActor.priorityValue;
        }

        var str = "";
        for (int y = 0; y < printCells.Length; y++)
        {
            for (int x = 0; x < printCells[y].Length; x++)
            {
                str += printCells[y][x] + "\t";
            }
            str += "\n";
        }
        Assert.Pass(str);
    }
    private void PrintTargetBlocks(IFieldBlock[] fieldBlocks)
    {
        var printCells = CreatePrintCells(_fieldSize.x, _fieldSize.y);
        for(int i = 0; i < fieldBlocks.Length; i++)
        {
            var block = fieldBlocks[i];
            var coordinate = block.coordinate;
            if (block.unitActor.typeTeam == TYPE_TEAM.Left)
                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Alies;
            else if (block.unitActor.typeTeam == TYPE_TEAM.Right)
                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Enemy;
        }

        var str = "";
        str += "▤ : Empty\n▷ : Alies\n◁ : Enemy\n----------\n";
        str += PrintCells(printCells);
        Assert.Pass(str);
    }

    private void PrintFieldManager()
    {
        var printCells = CreatePrintCells(_fieldSize.x, _fieldSize.y);

        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            var coordinate = blocks[i].coordinate;

            if (blocks[i].unitActor == null)
                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.None;
            else if(blocks[i].unitActor.typeTeam == TYPE_TEAM.Left)
                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Alies;
            else if (blocks[i].unitActor.typeTeam == TYPE_TEAM.Right)
                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Enemy;
        }

        var str = "";
        str += "▤ : Empty\n▷ : Alies\n◁ : Enemy\n----------\n";
        str += PrintCells(printCells);
        Assert.Pass(str);

    }

    private void Print(TYPE_TARGET_RANGE typeTargetRange, int startRangeValue, Vector2Int[] cells)
    {

        var minX = cells.Min(cell => cell.x);
        var minY = cells.Min(cell => cell.y);
        var maxX = cells.Max(cell => cell.x);
        var maxY = cells.Max(cell => cell.y);


        var offset = Vector2Int.zero;

        switch (typeTargetRange)
        {
            case TYPE_TARGET_RANGE.Circle:
            case TYPE_TARGET_RANGE.Cross:
            case TYPE_TARGET_RANGE.Rhombus:
            case TYPE_TARGET_RANGE.Square:
                offset.x = -minX + startRangeValue;
                break;
            default:
                minX = (minX != 0) ? minX : 0;
                minY = (minY != 0) ? minY : 0;
                maxX = (maxX > 4) ? maxX : 4;
                maxY = (maxY > 4) ? maxY : 4;
                break;
        }

        var lengthY = maxY - minY + 1;
        var lengthX = maxX - minX + 1 + startRangeValue;

        var printCells = CreatePrintCells(lengthX, lengthY);
        var str = $"minX {minX} ~ maxX {maxX} / minY {minY} ~ maxY { maxY}  / lengthX {lengthX} ~ lengthY {lengthY}\n";


        foreach (var cell in cells)
        {
            //Debug.Log((cell.y + lengthY / 2) + " " + cell.x);
            if (cell.y == 0 && cell.x == 0)
                printCells[cell.y + lengthY / 2][cell.x + offset.x] = TYPE_GRAPHIC_SHAPE.Caster;
            else if (cell.y == 0 && cell.x == _startRangeValue)
                printCells[cell.y + lengthY / 2][cell.x + offset.x] = TYPE_GRAPHIC_SHAPE.Start;
            else
                printCells[cell.y + lengthY / 2][cell.x + offset.x] = TYPE_GRAPHIC_SHAPE.Fill;
            
        }
        str += "▤ : Empty\n▣ : Range\n▩ : Start\n◈ : Caster\n----------\n";
        str += PrintCells(printCells);
        Assert.Pass(str);
    }

    private string PrintCells(TYPE_GRAPHIC_SHAPE[][] printCells)
    {
        string str = "";
        for (int y = 0; y < printCells.Length; y++)
        {
            for (int x = 0; x < printCells[y].Length; x++)
            {
                switch (printCells[y][x])
                {
                    case TYPE_GRAPHIC_SHAPE.None:
                        str += "▤";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Fill:
                        str += "▣";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Start:
                        str += "▩";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Caster:
                        str += "◈";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Alies:
                        str += "▷";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Enemy:
                        str += "◁";
                        break;
                }
            }
            str += "\n";
        }
        return str;
    }

    #endregion
}
#endif