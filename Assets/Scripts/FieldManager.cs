using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class ActorBlock
//{
//    public Vector2Int coordinate { get; private set; }
//    public FieldBlock fieldBlock { get; private set; }
//    public UnitActor unitActor { get; private set; }

//    public void SetUnitActor(UnitActor unitActor)
//    {
//        this.unitActor = unitActor;
//        unitActor.transform.position = fieldBlock.transform.position;

//    }

//    public void SetCoordinate(Vector2Int coor)
//    {
//        coordinate = coor;
//    }

//    public void SetFieldBlock(FieldBlock fieldBlock)
//    {
//        this.fieldBlock = fieldBlock;
//    }

//    public void ResetUnitActor()
//    {
//        unitActor = null;
//    }
//}

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

        for(int y = 0; y <_fieldSize.y; y++)
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
                else if(x == _fieldSize.x - 1)
                    _blockListSideR.Add(block);

                if (x == 1)
                    _blockListUnitL.Add(block);
                else if(x == _fieldSize.x - 2)
                    _blockListUnitR.Add(block);
            }
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
        for(int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == fieldBlock) return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="range"></param>
    /// <param name="isReverse"></param>
    /// <returns></returns>
    public FieldBlock GetAttackBlock(Vector2Int nowCoordinate, int range, TYPE_TEAM typeTeam, bool isReverse = false)
    {
        //L -> R
        if (range > 0)
        {
            if (isReverse)
            {
                for (int x = range; x >= 0; x--)
                {
                    var block = GetBlock(nowCoordinate.x + x, nowCoordinate.y);
                    if (block != null)
                    {
                        if (block.unitActor != null)
                        {
                            if (typeTeam != block.unitActor.typeTeam)
                            {
                                return block;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int x = 1; x <= range; x++)
                {
                    var block = GetBlock(nowCoordinate.x + x, nowCoordinate.y);
                    if (block != null)
                    {
                        if (block.unitActor != null)
                        {
                            if (typeTeam != block.unitActor.typeTeam)
                            {
                                return block;
                            }
                        }
                    }
                }
            }
        }

        //R -> L
        else if (range < 0)
        {
            if (isReverse)
            {
                for (int x = Mathf.Abs(range); x >= 0; x--)
                {
                    var block = GetBlock(nowCoordinate.x - x, nowCoordinate.y);
                    if (block != null)
                    {
                        if (block.unitActor != null)
                        {
                            if (typeTeam != block.unitActor.typeTeam)
                            {
                                return block;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int x = 1; x <= Mathf.Abs(range); x++)
                {
                    var block = GetBlock(nowCoordinate.x - x, nowCoordinate.y);
                    if (block != null)
                    {
                        if (block.unitActor != null)
                        {
                            if (typeTeam != block.unitActor.typeTeam)
                            {
                                return block;
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public FieldBlock GetMovementBlock(Vector2Int nowCoordinate, int range)
    {
        FieldBlock tmpBlock = null;

        //L -> R
        if (range > 0)
        {
            for (int x = 1; x <= range; x++)
            {
                var block = GetBlock(nowCoordinate.x + x, nowCoordinate.y);
                if (block != null)
                {
                    if (block.unitActor == null)
                    {
                        tmpBlock = block;
                    }
                    else if (tmpBlock != null || block.unitActor != null)
                    {
                        break;
                    }
                }
            }
        }

        //R -> L
        else if (range < 0)
        {
            for (int x = 1; x <= Mathf.Abs(range); x++)
            {
                var block = GetBlock(nowCoordinate.x - x, nowCoordinate.y);
                if (block != null)
                {
                    if (block.unitActor == null)
                    {
                        tmpBlock = block;
                    }
                    else if (tmpBlock != null || block.unitActor != null)
                    {
                        break;
                    }
                }
            }
        }
        return tmpBlock;
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
        for(int i = 0; i < _blockList.Count; i++)
        {
            if (_blockList[i].unitActor != null && _blockList[i].unitActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
        }
        return null;
    }

    public bool IsGameEnd(TYPE_TEAM typeTeam)
    {
        //switch (typeTeam)
        //{
        //    case TYPE_TEAM.Left:
        //        for (int i = 0; i < _blockListSideR.Count; i++)
        //            if (_blockListUnitR[i].unitActor != null && _blockListUnitR[i].unitActor.typeTeam == TYPE_TEAM.Left) return true;
        //        break;
        //    case TYPE_TEAM.Right:
        //        for (int i = 0; i < _blockListSideL.Count; i++)
        //            if (_blockListUnitL[i].unitActor != null && _blockListUnitL[i].unitActor.typeTeam == TYPE_TEAM.Right) return true;

        //        break;
        //}
        return false;
    }
}
