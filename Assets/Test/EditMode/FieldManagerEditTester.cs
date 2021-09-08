using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class FieldManagerEditTester
{

    //FieldBlock[][] _fieldBlocks;

    Vector2Int _fieldSize = new Vector2Int(17, 7);

    float _length = 1.25f;

    IUnitActor aliesUnitActor;
    IUnitActor enemyUnitActor;

    private enum TYPE_GRAPHIC_SHAPE { None = 0, Start, Fill, Caster }

    TYPE_TARGET_RANGE _typeTargetRange;
    int _startRangeValue;


    [SetUp]
    public void SetUp()
    {
        CreateBlocks();

        _startRangeValue = 0;

        aliesUnitActor = new Dummy_UnitActor();
        enemyUnitActor = new Dummy_UnitActor();                
    }

    [TearDown]
    public void TearDown()
    {
        FieldManager.CleanUp();
    }

    private void CreateBlocks()
    {
        var fieldBlocks = new Dummy_FieldBlock[_fieldSize.y][];

        for (int y = 0; y < _fieldSize.y; y++)
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
//        Assert.IsTrue(FieldManager.IsTargetBlock(aliesUnitActor, TYPE_TARGET_TEAM.Alies, TYPE_TEAM.Left));
    }

    [Test]
    public void FieldManager_SetUnitActor_5Units()
    {
        for(int i = 0; i < 5; i++)
        {
            var uActor = new Dummy_UnitActor();
            Assert.IsTrue(FieldManager.SetUnitActor(uActor, i, 0));
        }

        PrintFieldManager();
        //        Assert.IsTrue(FieldManager.IsTargetBlock(aliesUnitActor, TYPE_TARGET_TEAM.Alies, TYPE_TEAM.Left));
    }


    private void PrintFieldManager()
    {

        var printCells = new TYPE_GRAPHIC_SHAPE[_fieldSize.y][];
        for (int i = 0; i < printCells.Length; i++)
        {
            printCells[i] = new TYPE_GRAPHIC_SHAPE[_fieldSize.x];
        }


        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            var coordinate = blocks[i].coordinate;

            printCells[coordinate.y][coordinate.x] = (blocks[i].unitActor == null) ? TYPE_GRAPHIC_SHAPE.None : TYPE_GRAPHIC_SHAPE.Fill;
        }

        var str = "";
        str += "¢Ç : Empty\n¢Ã : Unit\n----------\n";
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

        var printCells = new TYPE_GRAPHIC_SHAPE[lengthY][];


        var str = $"minX {minX} ~ maxX {maxX} / minY {minY} ~ maxY { maxY}  / lengthX {lengthX} ~ lengthY {lengthY}\n";

        for (int y = 0; y < lengthY; y++)
        {
            printCells[y] = new TYPE_GRAPHIC_SHAPE[lengthX];
        }


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
        str += "¢Ç : Empty\n¢Ã : Range\n¢Ì : Start\n¢Â : Caster\n----------\n";
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
                        str += "¢Ç";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Fill:
                        str += "¢Ã";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Start:
                        str += "¢Ì";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Caster:
                        str += "¢Â";
                        break;
                }
            }
            str += "\n";
        }
        return str;
    }
}
