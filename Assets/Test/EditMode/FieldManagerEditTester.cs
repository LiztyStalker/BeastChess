using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class FieldManagerEditTester
{
    private enum TYPE_GRAPHIC_SHAPE { None, Center, Fill }

    TYPE_TARGET_RANGE _typeTargetRange;

    // A Test behaves as an ordinary method
    [Test]
    public void FieldManager_GetCellsNormal()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 5);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsTriangle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Triangle;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 5);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsVertical()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Vertical;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 3);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsSquare()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Square;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 3);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsRhombus()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Rhombus;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 3);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsCross()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Cross;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 3);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }

    [Test]
    public void FieldManager_GetCellsCircle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Circle;
        var cells = FieldManager.GetCells(_typeTargetRange, 0, 3);
        Print(_typeTargetRange, cells);
        Assert.Pass();
    }



    private void Print(TYPE_TARGET_RANGE typeTargetRange, Vector2Int[] cells)
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
                offset.x = -minX;
                break;
            default:
                minX = (minX != 0) ? minX : 0;
                minY = (minY != 0) ? minY : 0;
                maxX = (maxX > 4) ? maxX : 4;
                maxY = (maxY > 4) ? maxY : 4;
                break;
        }

        var lengthY = maxY - minY + 1;
        var lengthX = maxX - minX + 1;

        var printCell = new TYPE_GRAPHIC_SHAPE[lengthY][];

        Debug.Log($"minY {minY} maxY { maxY} / minX {minX} maxX {maxX} / lengthX {lengthX} lengthY {lengthY}");

        for (int y = 0; y < lengthY; y++)
        {
            printCell[y] = new TYPE_GRAPHIC_SHAPE[lengthX];
        }


        foreach (var cell in cells)
        {
            //Debug.Log((cell.y + lengthY / 2) + " " + cell.x);
            if(cell.y == 0 && cell.x == 0)
                printCell[cell.y + lengthY / 2][cell.x + offset.x] = TYPE_GRAPHIC_SHAPE.Center;
            else
                printCell[cell.y + lengthY / 2][cell.x + offset.x] = TYPE_GRAPHIC_SHAPE.Fill;
            
        }

        var str = "";
        for (int y = 0; y < lengthY; y++)
        {
            for (int x = 0; x < lengthX; x++)
            {
                switch (printCell[y][x])
                {
                    case TYPE_GRAPHIC_SHAPE.None:
                        str += "¢Ç";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Fill:
                        str += "¢Ã";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Center:
                        str += "¢Ì";
                        break;
                }
            }
            str += "\n";
        }

        Debug.Log(str);
    }
}
