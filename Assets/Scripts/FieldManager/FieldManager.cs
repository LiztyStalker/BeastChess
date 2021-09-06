using System.Collections.Generic;
using UnityEngine;


public class FieldManager : MonoBehaviour
{

    [SerializeField]
    private FieldBlock _block;

    [SerializeField]
    private Vector2Int _fieldSize;

    [SerializeField]
    private float _length;

    private FieldBlock[][] _fieldBlocks;

    private List<FieldBlock> _blockList = new List<FieldBlock>();
    private List<FieldBlock> _blockListUnitL = new List<FieldBlock>();
    private List<FieldBlock> _blockListUnitR = new List<FieldBlock>();
    private List<FieldBlock> _blockListSideL = new List<FieldBlock>();
    private List<FieldBlock> _blockListSideR = new List<FieldBlock>();


#if UNITY_EDITOR

    private Vector2 scrollPos;
    public void DrawDebug(GUISkin guiSkin)
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();

        for (int i = 0; i < _blockList.Count; i++)
        {
            if (_blockList[i].unitActor != null)
            {
                var uActor = _blockList[i].unitActor;
                var style = guiSkin.FindStyle((uActor.isRunning) ? "unitActor_Run" : "unitActor_Idle");
                GUILayout.Label($"{uActor.unitCard.name} - {_blockList[i].coordinate} | {uActor.typeTeam} - {uActor.isRunning}", style);
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
#endif

    /// <summary>
    /// FieldManager를 초기화 합니다
    /// </summary>
    public void Initialize()
    {
        CreateBlocks();
    }

    /// <summary>
    /// 맵 상에 모든 블록을 배치합니다
    /// </summary>
    private void CreateBlocks()
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
                else if (x > _fieldSize.x / 2 + 2)
                    _blockListUnitR.Add(block);
            }
        }
    }

    /// <summary>
    /// 블록 이동 색상을 초기화 합니다.
    /// </summary>
    public void ClearMovements()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetMovement();
    }

    /// <summary>
    /// 블록 사거리 색상을 초기화 합니다.
    /// </summary>
    public void ClearRanges()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetRange();

    }

    /// <summary>
    /// 블록 포메이션 색상을 초기화 합니다.
    /// </summary>
    public void ClearFormations()
    {
        for (int i = 0; i < _blockList.Count; i++)
            _blockList[i].ResetFormation();
    }




    /// <summary>
    /// 이동 블록 색상을 지정합니다
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    /// <param name="minRangeValue"></param>
    public void SetRangeBlocksColor(FieldBlock block, Vector2Int[] cells, int minRangeValue)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = GetBlock(block.coordinate.x + cells[i].x + minRangeValue, block.coordinate.y + cells[i].y);
            if (cell != null)
                cell.SetRange();
        }
    }

    /// <summary>
    /// 진형 블록 색상을 지정합니다
    /// </summary>
    /// <param name="block"></param>
    public void SetFormationColor(FieldBlock block)
    {
        block.SetFormation();
    }
    
    /// <summary>
    /// 이동 블록 색상을 지정합니다
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    public void SetMovementBlocksColor(FieldBlock block, Vector2Int[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = GetBlock(block.coordinate.x + cells[i].x, block.coordinate.y + cells[i].y);
            if (cell != null)
                cell.SetMovement();
        }
    }




    /// <summary>
    /// 팀 진형의 블록을 하나 랜덤으로 가져옵니다
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 팀 진형의 사이드 블록을 모두 가져옵니다
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public FieldBlock[] GetSideBlocks(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? _blockListSideL.ToArray() : _blockListSideR.ToArray();
    }


    /// <summary>
    /// 해당 블록이 팀 진형의 블록인지 여부
    /// </summary>
    /// <param name="fieldBlock"></param>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public bool IsTeamUnitBlock(FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var blocks = (typeTeam == TYPE_TEAM.Left) ? _blockListUnitL : _blockListUnitR;
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == fieldBlock) return true;
        }
        return false;
    }

    /// <summary>
    /// 공격 가능한 블록을 모두 가져옵니다
    /// 블록이 없으면 빈 배열을 가져옵니다
    /// </summary>
    /// <param name="nowCoordinate"></param>
    /// <param name="cells"></param>
    /// <param name="minRangeValue"></param>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 이동 가능한 블록을 가져옵니다
    /// 블록이 없으면 null을 가져옵니다
    /// </summary>
    /// <param name="nowCoordinate"></param>
    /// <param name="movementCells"></param>
    /// <param name="typeTeam"></param>
    /// <param name="typeMovement"></param>
    /// <param name="isCharge"></param>
    /// <returns></returns>
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

   
    /// <summary>
    /// 해당 블록을 가져옵니다
    /// 범위 밖으로 벗어나면 null을 반환합니다
    /// </summary>
    /// <param name="fieldBlock"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 특정 타겟의 유닛인지 가져옵니다
    /// </summary>
    /// <param name="uActor"></param>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public static bool IsTargetBlock(UnitActor uActor,TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam)
    {
        if (uActor != null)
        {
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

    ///
    public static bool IsTargetBlock(UnitActor uActor, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, TYPE_UNIT_CLASS typeUnitClass, TYPE_UNIT_GROUP typeUnitGroup = TYPE_UNIT_GROUP.All)
    {
        if (uActor != null && uActor.typeUnitClass == typeUnitClass)
        {
            return IsTargetBlock(uActor, typeTargetTeam, typeTeam, typeUnitGroup);
        }        
        return false;
    }

    public static bool IsTargetBlock(UnitActor uActor, TYPE_TARGET_TEAM typeTargetTeam, TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeUnitGroup)
    {
        if (uActor != null)
        {

            if (typeUnitGroup != TYPE_UNIT_GROUP.All)
            {
                if ((uActor.typeUnitGroup & typeUnitGroup) != uActor.typeUnitGroup)
                    return false;
            }

            return IsTargetBlock(uActor, typeTargetTeam, typeTeam);
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
        
        //-x +y
        //-y +x쪽 안됨

        for (int y = -range; y <= range; y++)
        {
            for(int x = -range; x <= range; x++)
            {
                var block = GetBlock(fieldBlock.coordinate.x + x, fieldBlock.coordinate.y + y);
                if(block != null)
                {
                    if(IsTargetBlock(block.unitActor, typeTargetTeam, typeTeam))
                    {
                        if(!blocks.Contains(block))
                            blocks.Add(block);
                    }
                }
            }
        }
        //Debug.Log("Center " + fieldBlock.coordinate + " | blockArr " + blocks.Count);
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

    /// <summary>
    /// 블록을 가져옵니다
    /// 블록 밖으로 범위가 넘어가면 null을 반환합니다
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private FieldBlock GetBlock(int x, int y)
    {
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x];
        return null;
    }

    public bool IsEmptyBlockInUnitActor(int x, int y)
    {
        if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
            return _fieldBlocks[y][x].unitActor == null;
        return true;
    }

    //public bool IsEmptyFieldBlock(int x, int y)
    //{
    //    if (x >= 0 && x < _fieldSize.x && y >= 0 && y < _fieldSize.y)
    //        return _fieldBlocks[y][x] == null;
    //    return true;
    //}

    /// <summary>
    /// unitActor가 있는 FieldBlock을 가져옵니다
    /// </summary>
    /// <param name="unitActor"></param>
    /// <returns></returns>
    public FieldBlock FindActorBlock(UnitActor unitActor)
    {
        for (int i = 0; i < _blockList.Count; i++)
        {
            if (_blockList[i].unitActor != null && _blockList[i].unitActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
            else if (_blockList[i].castleActor != null && _blockList[i].castleActor.GetInstanceID() == unitActor.GetInstanceID()) return _blockList[i];
        }
        Debug.LogError($"UnitActor is Not Found  {unitActor.GetInstanceID()}");
        return null;
    }

    /// <summary>
    /// 특정 블록을 찾습니다
    /// </summary>
    /// <param name="nowBlock"></param>
    /// <param name="typeUnitClass"></param>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <param name="isFarStart"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 특정 블록을 찾습니다
    /// </summary>
    /// <param name="nowBlock"></param>
    /// <param name="typeUnitGroup"></param>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <param name="isFarStart"></param>
    /// <returns></returns>
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

//        if (targetBlock != null)
//            Debug.Log("FIndActorBlock " + targetBlock.coordinate);

        return targetBlock;
    }

    /// <summary>
    /// 특정 블록을 찾습니다
    /// </summary>
    /// <param name="typeTargetTeam"></param>
    /// <param name="typeTeam"></param>
    /// <param name="isAscendingPriority"></param>
    /// <returns></returns>
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

    //public bool IsGameEnd(TYPE_TEAM typeTeam)
    //{
    //    return false;
    //}


    /// <summary>
    /// Y축으로 왼쪽부터 오른쪽으로 정리된 블록을 가져옵니다
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Y축으로 오른쪽부터 왼쪽으로 정리된 블록을 가져옵니다
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <param name="isReverse"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 블록리스트를 가져옵니다
    /// </summary>
    /// <returns></returns>
    public FieldBlock[] GetAllBlocks()
    {
        return _blockList.ToArray();
    }

    /// <summary>
    /// 팀 블록 리스트를 가져옵니다
    /// </summary>
    /// <param name="typeTeam"></param>
    /// <returns></returns>
    public FieldBlock[] GetTeamUnitBlocks(TYPE_TEAM typeTeam) => (typeTeam == TYPE_TEAM.Left) ? _blockListUnitL.ToArray() : _blockListUnitR.ToArray();


    public FieldBlock[] GetheringSkillPreActive(UnitActor uActor, SkillData skillData, TYPE_TEAM typeTeam)
    {
        List<FieldBlock> gatherBlocks = new List<FieldBlock>();
        //switch (skillData.typeSkillRange)
        //{
        //    //완료
        //    case TYPE_SKILL_RANGE.All:
        //        {
        //            gatherBlocks.AddRange(GetBlocksOnUnitActor(skillData.typeTargetTeam, typeTeam));
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.MyselfRange:
        //        {
        //            var nowBlock = FindActorBlock(uActor);
        //            if (nowBlock != null)
        //            {
        //                var blocks = GetBlocksOnUnitActor(nowBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                gatherBlocks.AddRange(blocks);
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitClassRange:
        //        {
        //            var nowBlock = FindActorBlock(uActor);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = FindActorBlock(nowBlock, skillData.typeUnitClass, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    gatherBlocks.AddRange(blocks);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitGroupRange:
        //        {
        //            //자신을 찾기
        //            var nowBlock = FindActorBlock(uActor);
        //            if (nowBlock != null)
        //            {

        //                var fieldBlock = FindActorBlock(nowBlock, skillData.typeUnitGroup, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    gatherBlocks.AddRange(blocks);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.AscendingPriorityRange:
        //        {
        //            var nowBlock = FindActorBlock(uActor);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = FindActorBlock(skillData.typeTargetTeam, typeTeam, true);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    gatherBlocks.AddRange(blocks);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.DecendingPriorityRange:
        //        {
        //            var nowBlock = FindActorBlock(uActor);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = FindActorBlock(skillData.typeTargetTeam, typeTeam, false);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    gatherBlocks.AddRange(blocks);
        //                }
        //            }
        //        }
        //        break;
        //}
        return gatherBlocks.ToArray();
    }



    public FieldBlock[] GetTargetBlocks(UnitActor uActor, TargetData targetData, TYPE_TEAM typeTeam)
    {

        var cells = GetCells(targetData.TypeTargetRange, targetData.TargetStartRange, targetData.TargetRange);
        
        var list = new List<FieldBlock>();

        var nowBlock = FindActorBlock(uActor);
        if (nowBlock != null)
        {
            

            //targetData.TargetRange
            //targetData.TargetStartRange
        }

        return list.ToArray();
    }

    public static Vector2Int[] GetCells(TYPE_TARGET_RANGE typeTargetRange, int startRangeValue, int rangeValue)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        switch (typeTargetRange)
        {
            case TYPE_TARGET_RANGE.Normal:
                for (int x = 0; x <= rangeValue; x++)
                {
                    cells.Add(new Vector2Int(x + startRangeValue, 0));
                }
                break;
            case TYPE_TARGET_RANGE.Vertical:
                for (int y = 0; y <= rangeValue; y++)
                {
                    if (y == 0)
                    {
                        cells.Add(new Vector2Int(0 + startRangeValue, y));
                    }
                    else
                    {
                        cells.Add(new Vector2Int(0 + startRangeValue, y));
                        cells.Add(new Vector2Int(0 + startRangeValue, -y));
                    }
                }

                break;
            case TYPE_TARGET_RANGE.Triangle:
                for (int x = 0; x <= rangeValue; x++)
                {
                    cells.Add(new Vector2Int(x + startRangeValue, 0));

                    for (int y = 0; y < x; y++)
                    {
                        cells.Add(new Vector2Int(x + startRangeValue, y));
                        cells.Add(new Vector2Int(x + startRangeValue, -y));
                    }
                }
                break;
            case TYPE_TARGET_RANGE.Square:
                for (int x = -rangeValue; x <= rangeValue; x++)
                {
                    var posX = x + startRangeValue;
                    cells.Add(new Vector2Int(posX, 0));

                    if (rangeValue > 0)
                    {
                        for (int y = 1; y <= rangeValue; y++)
                        {
                            cells.Add(new Vector2Int(posX, y));
                            cells.Add(new Vector2Int(posX, -y));
                        }
                    }
                }
                break;
            case TYPE_TARGET_RANGE.Rhombus:
                for (int x = -rangeValue; x <= rangeValue; x++)
                {
                    cells.Add(new Vector2Int(x, 0));

                    for (int y = 0; y <= rangeValue - Mathf.Abs(x); y++)
                    {
                        cells.Add(new Vector2Int(x, y));
                        cells.Add(new Vector2Int(x, -y));
                    }
                }
                break;
            case TYPE_TARGET_RANGE.Cross:
                for (int x = -rangeValue; x <= rangeValue; x++)
                {
                    cells.Add(new Vector2Int(x + startRangeValue, 0));

                    if (x == 0)
                    {
                        for (int y = 0; y <= rangeValue; y++)
                        {
                            cells.Add(new Vector2Int(x + startRangeValue, y));
                            cells.Add(new Vector2Int(x + startRangeValue, -y));
                        }
                    }
                }
                break;
            case TYPE_TARGET_RANGE.Circle:

                var basePos = new Vector2Int(startRangeValue, 0);
                for (int x = -rangeValue; x <= rangeValue; x++)
                {
                    cells.Add(new Vector2Int(x + startRangeValue, 0));

                    if (rangeValue > 0)
                    {
                        for (int y = 1; y <= rangeValue; y++)
                        {
                            var pos = new Vector2Int(x + startRangeValue, y);
                            if (Vector2Int.Distance(basePos, pos) <= rangeValue)
                            {
                                cells.Add(pos);
                                cells.Add(new Vector2Int(x + startRangeValue, -y));
                            }
                        }
                    }
                }
                break;
            default:
                Debug.LogError($"해당 타입이 적용되지 않았습니다 {typeTargetRange}");
                break;
        }
        return cells.ToArray();
    }


}
