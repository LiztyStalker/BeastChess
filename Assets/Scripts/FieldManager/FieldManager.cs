using System.Collections.Generic;
using UnityEngine;


public class FieldManager : MonoBehaviour
{

    [SerializeField]
    FieldBlock _block;

    [SerializeField]
    Vector2Int _fieldSize;

    [SerializeField]
    float _length;

    FieldBlock[][] _fieldBlocks;

    List<FieldBlock> _blockList = new List<FieldBlock>();
    List<FieldBlock> _blockListUnitL = new List<FieldBlock>();
    List<FieldBlock> _blockListUnitR = new List<FieldBlock>();
    List<FieldBlock> _blockListSideL = new List<FieldBlock>();
    List<FieldBlock> _blockListSideR = new List<FieldBlock>();

    public void Initialize()
    {
        _fieldBlocks = new FieldBlock[_fieldSize.y][];

        var startX = -((float)_fieldSize.x) * _length * 0.5f + _length * 0.5f;
        var startY = -((float)_fieldSize.y) * _length * 0.5f + _length * 0.5f;

        for (int y = 0; y < _fieldSize.y; y++)
        {
            _fieldBlocks[y] = new FieldBlock[_fieldSize.x];

            for (int x = 0; x < _fieldSize.x; x++)
            {
                var block = Instantiate(_block);
                block.transform.SetParent(transform);
                block.SetCoordinate(new Vector2Int(x, y));
                block.transform.localPosition = new Vector3(startX + ((float)x) * _length, startY + ((float)y) * _length, 0f);
                block.gameObject.SetActive(true);

                _fieldBlocks[y][x] = block;
                _blockList.Add(block);

                if (x == 0)
                    _blockListSideL.Add(block);
                else if (x == _fieldSize.x - 1)
                    _blockListSideR.Add(block);

                if (x < _fieldSize.x / 2 - 2)
                    _blockListUnitL.Add(block);
                else if(x > _fieldSize.x / 2 + 2)
                    _blockListUnitR.Add(block);
            }
        }
    }

    public void ClearMovements()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetMovement();
    }

    public void ClearRanges()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetRange();

    }

    public void ClearFormations()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetFormation();
    }

    public void SetRangeBlocks(FieldBlock block, Vector2Int[] cells, int minRangeValue)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = GetBlock(block.coordinate.x + cells[i].x + minRangeValue, block.coordinate.y + cells[i].y);
            if (cell != null)
                cell.SetRange();
        }
    }

    public void SetFormation(FieldBlock block)
    {
        block.SetFormation();
    }

    public void SetMovementBlocks(FieldBlock block, Vector2Int[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = GetBlock(block.coordinate.x + cells[i].x, block.coordinate.y + cells[i].y);
            if (cell != null)
                cell.SetMovement();
        }
    }

    public FieldBlock GetRandomBlock(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _blockListUnitL[Random.Range(0, _blockListUnitL.Count)];
            case TYPE_TEAM.Right:
                return _blockListUnitR[Random.Range(0, _blockListUnitR.Count)];
        }
        return null;
    }

    public FieldBlock[] GetSideBlocks(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? _blockListSideL.ToArray() : _blockListSideR.ToArray();
    }

    public bool IsTeamUnitBlock(FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var blocks = (typeTeam == TYPE_TEAM.Left) ? _blockListUnitL : _blockListUnitR;
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == fieldBlock) return true;
        }
        return false;
    }

    public FieldBlock[] GetAttackBlocks(Vector2Int nowCoordinate, Vector2Int[] cells, int minRangeValue, TYPE_TEAM typeTeam)
    {
        List<FieldBlock> blocks = new List<FieldBlock>();

        for (int i = 0; i < cells.Length; i++)
        {
            var block = GetBlock(nowCoordinate.x + ((typeTeam == TYPE_TEAM.Left) ? cells[i].x + minRangeValue : -(cells[i].x + minRangeValue)), nowCoordinate.y + cells[i].y);
            if (block != null)
            {
                if (block.unitActor != null && typeTeam != block.unitActor.typeTeam)
                {
                    blocks.Add(block);
                }
                else if (block.castleActor != null && typeTeam != block.castleActor.typeTeam)
                {
                    blocks.Add(block);
                }
            }
        }
        return blocks.ToArray();
    }

    public FieldBlock GetMovementBlock(Vector2Int nowCoordinate, Vector2Int[] movementCells, TYPE_TEAM typeTeam, TYPE_MOVEMENT typeMovement, bool isCharge = false)
    {
        List<Vector2Int> movementBlocks = new List<Vector2Int>(movementCells);

        
        switch (typeMovement)
        {
            case TYPE_MOVEMENT.Penetration:
            case TYPE_MOVEMENT.Normal:

                movementBlocks.Sort((a, b) => { return a.x - b.x; });

                FieldBlock tmpBlock = null;

                for (int i = 0; i < movementBlocks.Count; i++)
                {
                    var block = GetBlock(nowCoordinate.x + ((typeTeam == TYPE_TEAM.Left) ? movementBlocks[i].x : -movementBlocks[i].x), nowCoordinate.y + movementBlocks[i].y);
                    if (block != null)
                    {
                        if (block.unitActor == null && block.castleActor == null)
                        {
                            tmpBlock = block;
                        }
                        else if(block.unitActor != null || block.castleActor != null)
                        {
                            break;
                        }
                    }
                }
                return tmpBlock;

            case TYPE_MOVEMENT.Rush:

                movementBlocks.Sort((a, b) => { return b.x - a.x; });

                for (int i = 0; i < movementCells.Length; i++)
                {
                    //경로 안에 적이나 아군이 있으면 멈추기
                    var block = GetBlock(nowCoordinate.x + ((typeTeam == TYPE_TEAM.Left) ? movementCells[i].x : -movementCells[i].x), nowCoordinate.y + movementCells[i].y);
                    if (block != null)
                    {
                        if (block.unitActor == null && block.castleActor == null)
                        {
                            return block;
                        }
                    }
                }
                break;
        }
        return null;
    }

   

    public FieldBlock GetBlock(FieldBlock fieldBlock, Vector2Int offset)
    {
        ///GetBlock 사용하기
        var x = fieldBlock.coordinate.x + offset.x;
        var y = fieldBlock.coordinate.y + offset.y;
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x];
        return null;
    }

    #region ##### SkillData API #####



    private bool IsTargetBlock(UnitActor uActor, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, TYPE_UNIT_CLASS typeUnitClass, TYPE_UNIT_GROUP typeUnitGroup = TYPE_UNIT_GROUP.All)
    {
        if (uActor != null && uActor.typeUnitClass == typeUnitClass)
        {
            return IsTargetBlock(uActor, typeTargetTeam, typeTeam, typeUnitGroup);
        }        
        return false;
    }

    private bool IsTargetBlock(UnitActor uActor, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeUnitGroup = TYPE_UNIT_GROUP.All)
    {
        if (uActor != null)
        {

            if (typeUnitGroup != TYPE_UNIT_GROUP.All)
            {
                if ((uActor.typeUnitGroup & typeUnitGroup) != uActor.typeUnitGroup)
                    return false;
            }

            switch (typeTargetTeam)
            {
                case TYPE_TARGET_TEAM.Alies:
                    return (uActor.typeTeam == typeTeam);
                case TYPE_TARGET_TEAM.Enemy:
                    return (uActor.typeTeam != typeTeam);
                default:
                    return true;
            }
        }
        return false;
    }



    /// <summary>
    /// SkillData
    /// 현재 블록을 중심으로 range만큼 [사각형] UnitActor가 들어있는 블록을 가져옵니다
    /// </summary>
    /// <param name="fieldBlock"></param>
    /// <param name="range"></param>
    /// <param name="isMyself"></param>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public FieldBlock[] GetBlocksOnUnitActor(FieldBlock fieldBlock, int range, bool isMyself, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam)
    {
        List<FieldBlock> blocks = new List<FieldBlock>();

        if (isMyself) blocks.Add(fieldBlock);
        
        for (int y = 1; y < range; y++)
        {
            for(int x = 1; x < range; x++)
            {
                //양수
                var block = GetBlock(fieldBlock.coordinate.x + x, fieldBlock.coordinate.y + y);
                if(block != null)
                {
                    if(IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam))
                    {
                        if(!blocks.Contains(block))
                            blocks.Add(block);
                    }
                }

                //음수
                block = GetBlock(fieldBlock.coordinate.x - x, fieldBlock.coordinate.y - y);
                if (block != null)
                {
                    if (IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam))
                    {
                        if (!blocks.Contains(block))
                            blocks.Add(block);
                    }
                }
            }
        }

        return blocks.ToArray();
    }

    /// <summary>
    /// SkillData
    /// UnitActor가 있는 모든 블록을 가져옵니다
    /// </summary>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public FieldBlock[] GetBlocksOnUnitActor(TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam)
    {
        List<FieldBlock> blocks = new List<FieldBlock>();

        for (int i = 0; i < _blockList.Count; i++)
        {
            if (IsTargetBlock(_blockList[i].unitActor, typeTargetTeam, typeTeam))
            {
                blocks.Add(_blockList[i]);
            }
        }
        return blocks.ToArray();
    }

    #endregion


    public FieldBlock[] GetFormationBlocks(Vector2Int nowCoordinate, Vector2Int[] cells, TYPE_TEAM typeTeam)
    {
        List<FieldBlock> blocks = new List<FieldBlock>();

        for (int i = 0; i < cells.Length; i++)
        {
            var block = GetBlock(nowCoordinate.x + ((typeTeam == TYPE_TEAM.Left) ? cells[i].x : -cells[i].x), nowCoordinate.y + cells[i].y);
            if (block != null)
            {
                blocks.Add(block);
            }
        }
        return blocks.ToArray();
    }

    private FieldBlock GetBlock(int x, int y)
    {
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x];
        return null;
    }

    public bool IsEmptyUnitInActionBlock(int x, int y)
    {
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x].unitActor == null;
        return true;
    }

    public bool IsEmptyFieldBlockInActionBlock(int x, int y)
    {
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x] == null;
        return true;
    }

    public FieldBlock FindActorBlock(UnitActor unitActor)
    {
        for (int i = 0; i < _blockList.Count; i++)
        {
            //Debug.LogError($"UnitActor is Not Found  {unitActor.GetInstanceID()} {((_blockList[i].unitActor != null) ? _blockList[i].unitActor.GetInstanceID() : 0)} {((_blockList[i].castleActor != null) ? _blockList[i].castleActor.GetInstanceID() : 0)}");

            if (_blockList[i].unitActor != null && _blockList[i].unitActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
            else if (_blockList[i].castleActor != null && _blockList[i].castleActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
        }
        Debug.LogError($"UnitActor is Not Found  {unitActor.GetInstanceID()}");
        return null;
    }

    public FieldBlock FindActorBlock(FieldBlock nowBlock, TYPE_UNIT_CLASS typeUnitClass, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, bool isFarStart = false)
    {
        var blocks = (typeTeam == TYPE_TEAM.Left) ? GetAllBlockRightToLeft() : GetAllBlockLeftToRight();
        FieldBlock targetBlock = null;

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if(IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam, typeUnitClass))
            {

                if (targetBlock == null) targetBlock = block;
                else
                {
                    if (isFarStart)
                    {
                        if (Vector2.Distance(targetBlock.coordinate, nowBlock.coordinate) < Vector2.Distance(block.coordinate, nowBlock.coordinate))
                            targetBlock = block;
                    }
                    else
                    {
                        if(Vector2.Distance(targetBlock.coordinate, nowBlock.coordinate) > Vector2.Distance(block.coordinate, nowBlock.coordinate))
                            targetBlock = block;
                    }
                }
            }
        }
        return targetBlock;
    }

    public FieldBlock FindActorBlock(FieldBlock nowBlock, TYPE_UNIT_GROUP typeUnitGroup, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, bool isFarStart = false)
    {
        var blocks = (typeTeam == TYPE_TEAM.Left) ? GetAllBlockRightToLeft() : GetAllBlockLeftToRight();
        FieldBlock targetBlock = null;

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam, typeUnitGroup))
            {

                if (targetBlock == null) targetBlock = block;
                else
                {
                    if (isFarStart)
                    {
                        if (Vector2.Distance(targetBlock.coordinate, nowBlock.coordinate) < Vector2.Distance(block.coordinate, nowBlock.coordinate))
                            targetBlock = block;
                    }
                    else
                    {
                        if (Vector2.Distance(targetBlock.coordinate, nowBlock.coordinate) > Vector2.Distance(block.coordinate, nowBlock.coordinate))
                            targetBlock = block;
                    }
                }
            }
        }
        return targetBlock;
    }

    public FieldBlock FindActorBlock(TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, bool isAscendingPriority)
    {
        var blocks = (typeTeam == TYPE_TEAM.Left) ? GetAllBlockRightToLeft() : GetAllBlockLeftToRight();
        FieldBlock targetBlock = null;

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam))
            {

                if (targetBlock == null) targetBlock = block;
                else
                {
                    if (isAscendingPriority)
                    {
                        if (targetBlock.unitActor.priorityValue < block.unitActor.priorityValue)
                            targetBlock = block;
                    }
                    else
                    {
                        if (targetBlock.unitActor.priorityValue > block.unitActor.priorityValue)
                            targetBlock = block;
                    }
                }
            }
        }
        return targetBlock;
    }

    public bool IsGameEnd(TYPE_TEAM typeTeam)
    {
        return false;
    }


    private FieldBlock[] GetAllBlockLeftToRight()
    {
        List<FieldBlock> blocks = new List<FieldBlock>();
        for (int x = _fieldSize.x - 1; x >= 0; x--)
        {
            for (int y = _fieldSize.y - 1; y >= 0; y--)
            {
                blocks.Add(_fieldBlocks[y][x]);
            }
        }
        return blocks.ToArray();
    }

    private FieldBlock[] GetAllBlockRightToLeft()
    {
        List<FieldBlock> blocks = new List<FieldBlock>();
        for (int x = 0; x < _fieldSize.x; x++)
        {
            for (int y = _fieldSize.y - 1; y >= 0; y--)
            {
                blocks.Add(_fieldBlocks[y][x]);
            }
        }
        return blocks.ToArray();
    }

    public FieldBlock[] GetAllBlocks(TYPE_TEAM typeTeam, bool isReverse = false)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {
            return (!isReverse) ? GetAllBlockLeftToRight() : GetAllBlockRightToLeft();
        }
        else
        {
            return (!isReverse) ? GetAllBlockRightToLeft() : GetAllBlockLeftToRight();
        }
    }

    public FieldBlock[] GetAllBlocks()
    {
        return _blockList.ToArray();
    }

    public FieldBlock[] GetTeamUnitBlocks(TYPE_TEAM typeTeam)
    {
        List<FieldBlock> blocks = new List<FieldBlock>();
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                blocks.AddRange(_blockListUnitL);
                break;
                //for(int i = 0; i < _blockList.Count; i++)
                //{
                //    if (_blockListUnitR.Contains(_blockList[i])) continue;// || _blockListUnitR.Contains(_blockList[i])) continue;
                //    blocks.Add(_blockList[i]);
                //}
                //break;
            case TYPE_TEAM.Right:
                blocks.AddRange(_blockListUnitR);
                break;
                //for (int i = 0; i < _blockList.Count; i++)
                //{
                //    if (_blockListUnitL.Contains(_blockList[i])) continue;// || _blockListUnitL.Contains(_blockList[i])) continue;
                //    blocks.Add(_blockList[i]);
                //}
                //break;
        }
        return blocks.ToArray();
    }
}
