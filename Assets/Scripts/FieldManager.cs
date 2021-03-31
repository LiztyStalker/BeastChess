using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBlock
{
    public Vector2Int coordinate { get; private set; }
    public FieldBlock fieldBlock { get; private set; }
    public UnitActor unitActor { get; private set; }

    public void SetUnitActor(UnitActor unitActor)
    {
        this.unitActor = unitActor;
        unitActor.transform.position = fieldBlock.transform.position;

    }

    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }

    public void SetFieldBlock(FieldBlock fieldBlock)
    {
        this.fieldBlock = fieldBlock;
    }

    public void ResetUnitActor()
    {
        unitActor = null;
    }
}

public class FieldManager : MonoBehaviour
{
      
    [SerializeField]
    FieldBlock _block;

    [SerializeField]
    Vector2Int _fieldSize;

    [SerializeField]
    float _length;

    ActorBlock[][] _fieldBlocks;

    List<ActorBlock> _blockList = new List<ActorBlock>();
    List<ActorBlock> _blockListL = new List<ActorBlock>();
    List<ActorBlock> _blockListR = new List<ActorBlock>();

    public void Initialize()
    {
        _fieldBlocks = new ActorBlock[_fieldSize.y][];

        var startX = -((float)_fieldSize.x) * _length * 0.5f + _length * 0.5f;
        var startY = -((float)_fieldSize.y) * _length * 0.5f + _length * 0.5f;

        for(int y = 0; y <_fieldSize.y; y++)
        {
            _fieldBlocks[y] = new ActorBlock[_fieldSize.x];

            for (int x = 0; x < _fieldSize.x; x++)
            {
                var block = Instantiate(_block);
                block.transform.SetParent(transform);
                block.transform.localPosition = new Vector3(startX + ((float)x) * _length, startY + ((float)y) * _length, 0f);
                block.gameObject.SetActive(true);

                var actorBlock = new ActorBlock();
                actorBlock.SetCoordinate(new Vector2Int(x, y));
                actorBlock.SetFieldBlock(block);

                _fieldBlocks[y][x] = actorBlock;
                _blockList.Add(actorBlock);

                if (x == 0)
                    _blockListL.Add(actorBlock);
                else if(x == _fieldSize.x - 1)
                    _blockListR.Add(actorBlock);
            }
        }
    }

    public ActorBlock GetRandomBlock(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _blockListL[Random.Range(0, _blockListL.Count)];
            case TYPE_TEAM.Right:
                return _blockListR[Random.Range(0, _blockListR.Count)];
        }
        return null;
    }

    public ActorBlock GetBlock(int x, int y)
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
            return _fieldBlocks[y][x].fieldBlock == null;
        return true;
    }

    public ActorBlock FindActorBlock(UnitActor unitActor)
    {
        for(int i = 0; i < _blockList.Count; i++)
        {
            if (_blockList[i].unitActor != null && _blockList[i].unitActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
        }
        return null;
    }

    public bool IsGameEnd(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                for (int i = 0; i < _blockListR.Count; i++)
                    if (_blockListR[i].unitActor != null && _blockListR[i].unitActor.typeTeam == TYPE_TEAM.Left) return true;
                break;
            case TYPE_TEAM.Right:
                for (int i = 0; i < _blockListL.Count; i++)
                    if (_blockListL[i].unitActor != null && _blockListL[i].unitActor.typeTeam == TYPE_TEAM.Right) return true;

                break;
        }
        return false;
    }
}
