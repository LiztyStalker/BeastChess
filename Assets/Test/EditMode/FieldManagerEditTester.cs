#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;
public enum TYPE_GRAPHIC_SHAPE { None = 0, Start, Fill, Caster, Alies, Enemy, Passive, PreActive, Active }

public class FieldManagerEditTester
{

    //FieldBlock[][] _fieldBlocks;

    private static Vector2Int _fieldSize = new Vector2Int(17, 7);
    public static Vector2Int fieldSize => _fieldSize;

    private static TYPE_TARGET_RANGE _typeTargetRange;
    private static int _startRangeValue;


    public static void DefaultSetUp()
    {
        CreateBlocks();
        _startRangeValue = 0;
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
    }

    public static void DefaultTearDown()
    {
        FieldManager.CleanUp();
        UnitManager.CleanUp();
    }

    [SetUp]
    public void SetUp()
    {
        DefaultSetUp();
    }

    [TearDown]
    public void TearDown()
    {
        DefaultTearDown();
    }

    private static void CreateBlocks()
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
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsNormal_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsTriangle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Triangle;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsTriangle_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Triangle;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 5);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsVertical()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Vertical;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsVertical_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Vertical;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsSquare()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Square;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsSquare_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Square;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsRhombus()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Rhombus;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }
    [Test]
    public void FieldManager_GetCellsRhombus_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Rhombus;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCross()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Cross;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCross_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Cross;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCircle()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Circle;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }

    [Test]
    public void FieldManager_GetCellsCircle_StartRangeValue()
    {
        _typeTargetRange = TYPE_TARGET_RANGE.Circle;
        _startRangeValue = 1;
        var cells = FieldManager.GetCells(_typeTargetRange, _startRangeValue, 3);
        PrintTarget(_typeTargetRange, _startRangeValue, cells);
    }


    [Test]
    public void FieldManager_FieldBlocks()
    {
        PrintFieldManagerInUnitActor();
    }

    [Test]
    public void FieldManager_SetUnitActor_All_Half()
    {
        CreateHalfUnitActors();
        PrintFieldManagerInUnitActor();
    }

    [Test]
    public void FieldManager_SetUnitActor_All_Grid()
    {
        CreateGridUnitActors();
        PrintFieldManagerInUnitActor();
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

    public static void CreateGridUnitActors()
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


    #region ##### Print #####

    /// <summary>
    /// 출력할 블록을 생성합니다
    /// </summary>
    /// <param name="lengthX"></param>
    /// <param name="lengthY"></param>
    /// <returns></returns>
    public static TYPE_GRAPHIC_SHAPE[][] CreatePrintCells(int lengthX, int lengthY)
    {
        var printCells = new TYPE_GRAPHIC_SHAPE[lengthY][];
        for (int i = 0; i < printCells.Length; i++)
        {
            printCells[i] = new TYPE_GRAPHIC_SHAPE[lengthX];
        }
        return printCells;
    }


    /// <summary>
    /// 우선순위 블록으로 출력합니다
    /// </summary>
    /// <param name="fieldBlocks"></param>
    public static void PrintPriorityBlocks(IFieldBlock[] fieldBlocks)
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

    /// <summary>
    /// 체력 블록으로 출력합니다
    /// </summary>
    /// <param name="fieldBlocks"></param>
    public static int PrintHealthBlocks(IFieldBlock[] fieldBlocks)
    {
        int totalHealth = 0;
        var printCells = new int[_fieldSize.y][];
        for (int i = 0; i < printCells.Length; i++)
        {
            printCells[i] = new int[_fieldSize.x];
        }

        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var block = fieldBlocks[i];
            var coordinate = block.coordinate;
            if (block.unitActor != null)
            {
                printCells[coordinate.y][coordinate.x] = block.unitActor.nowHealthValue;
                totalHealth += block.unitActor.nowHealthValue;
            }
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
        Debug.Log(str);
        return totalHealth;
    }

    /// <summary>
    /// Target된 블록을 TYPE_GRAPHIC_SHAPE로 가져옵니다
    /// </summary>
    /// <param name="fieldBlocks"></param>
    public static void PrintTargetBlocks(IFieldBlock[] fieldBlocks)
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

    /// <summary>
    /// SkillData에 적용된 FieldBlocks을 TYPE_GRAPHIC_SHAPE로 출력합니다
    /// </summary>
    /// <param name="fieldBlocks"></param>
    /// <param name="typeSkillActivate"></param>
    /// <returns></returns>
    public static int PrintSkillBlocks(IFieldBlock[] fieldBlocks, TYPE_SKILL_CAST typeSkillActivate)
    {
        var printCells = CreatePrintCells(_fieldSize.x, _fieldSize.y);
        int count = 0;
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var block = fieldBlocks[i];
            if (block.unitActor != null)
            {
                var skills = block.unitActor.skills;
                for (int j = 0; j < skills.Length; j++)
                {
                    if (skills[j].typeSkillCast == typeSkillActivate)
                    {
                        var coordinate = block.coordinate;
                        switch (typeSkillActivate)
                        {
                            case TYPE_SKILL_CAST.DeployCast:
                                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Passive;
                                break;
                            case TYPE_SKILL_CAST.AttackCast:
                                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.Active;
                                break;
                            case TYPE_SKILL_CAST.PreCast:
                                printCells[coordinate.y][coordinate.x] = TYPE_GRAPHIC_SHAPE.PreActive;
                                break;
                        }
                        count++;
                    }
                }
            }
        }

        var str = "";
        str += "▤ : Empty\nPA : Passive\nPR : PreActive\nAC : Active\n----------\n";
        str += PrintCells(printCells);
        Debug.Log(str);
        return count;
    }

    /// <summary>
    /// FieldManager내에 상주하는 UnitActor를 TYPE_GRAPHIC_SHAPE로 출력합니다
    /// </summary>
    public static void PrintFieldManagerInUnitActor()
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

    /// <summary>
    /// Target을 TYPE_GRAPHIC_SHAPE로 출력합니다
    /// </summary>
    /// <param name="typeTargetRange"></param>
    /// <param name="startRangeValue"></param>
    /// <param name="cells"></param>
    public static void PrintTarget(TYPE_TARGET_RANGE typeTargetRange, int startRangeValue, Vector2Int[] cells)
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

    /// <summary>
    /// TYPE_GRAPHIC_SHAPE를 출력합니다
    /// </summary>
    /// <param name="printCells"></param>
    /// <returns></returns>
    public static string PrintCells(TYPE_GRAPHIC_SHAPE[][] printCells)
    {
        string str = "";
        for (int y = 0; y < printCells.Length; y++)
        {
            for (int x = 0; x < printCells[y].Length; x++)
            {
                switch (printCells[y][x])
                {
                    case TYPE_GRAPHIC_SHAPE.None:
                        str += "▤ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Fill:
                        str += "▣ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Start:
                        str += "▩ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Caster:
                        str += "◈ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Alies:
                        str += "▷ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Enemy:
                        str += "◁ ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Passive:
                        str += "PA ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.PreActive:
                        str += "PR ";
                        break;
                    case TYPE_GRAPHIC_SHAPE.Active:
                        str += "AC ";
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