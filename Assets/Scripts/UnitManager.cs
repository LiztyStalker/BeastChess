using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    GameTestManager gameTestManager;

    UnitStorage unitStorage;

    [SerializeField]
    FieldManager _fieldManager;

    [SerializeField]
    UnitActor _unitActor;

    //[SerializeField]
    //UnitData[] _unitDataArray;

    [SerializeField]
    UIBar _uiBar;


    List<UnitActor> list = new List<UnitActor>();

    [HideInInspector]
    public int deadL;

    [HideInInspector]
    public int deadR;

    UnitActor _dragUnitActor;
    FieldBlock _dragFieldBlock;


    public UnitData[] GetRandomUnit(int count)
    {
        if (unitStorage == null)
            unitStorage = new UnitStorage();
        return unitStorage.GetRandomUnit(count);
    }

    public void CreateCastleUnit(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var sideBlocks = fieldManager.GetSideBlocks(typeTeam);
        var castleData = unitStorage.GetCastleUnit();
        for(int i = 0; i < sideBlocks.Length; i++)
        {
            CreateUnit(castleData, sideBlocks[i], typeTeam);
        }
    }


    public void CreateUnit(UnitData unitData, FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var unit = Instantiate(_unitActor);

        unit.SetData(unitData);

        fieldBlock.SetUnitActor(unit);
        unit.AddBar(Instantiate(_uiBar));
        unit.SetTypeTeam(typeTeam);
        unit.gameObject.SetActive(true);
        list.Add(unit);

        _dragUnitActor = null;
    }


    public void CreateUnit(FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var unit = _dragUnitActor;

        fieldBlock.SetUnitActor(unit);
        unit.AddBar(Instantiate(_uiBar));
        unit.SetTypeTeam(typeTeam);
        unit.gameObject.SetActive(true);
        list.Add(unit);

        _dragUnitActor = null;
        _dragFieldBlock = null;
    }

    public void DragUnit(UnitData uData)
    {
        var unitActor = Instantiate(_unitActor);
        unitActor.gameObject.SetActive(true);
        unitActor.SetData(uData);
        _dragUnitActor = unitActor;
    }

    public bool DropUnit(UnitData uData, TYPE_TEAM typeTeam)
    {
        ClearCell();
        if (_dragUnitActor != null)
        {
            if(_dragFieldBlock != null)
            {
                if (_dragFieldBlock.unitActor == null)
                {
                    CreateUnit(_dragFieldBlock, typeTeam);
                    return true;
                }
                else
                {
                    ClearCell();
                    Debug.LogWarning("Create Canceled");
                    DestroyImmediate(_dragUnitActor.gameObject);
                    _dragFieldBlock = null;
                }
            }
            else
            {
                ClearCell();
                Debug.LogWarning("Create Canceled");
                DestroyImmediate(_dragUnitActor.gameObject);
                _dragFieldBlock = null;
            }
        }
        return false;
    }

    private void Update()
    {
        if(_dragUnitActor != null)
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 100f);

            bool isCheck = false;

            for(int i = 0; i < hits.Length; i++)
            {
                var block = hits[i].collider.GetComponent<FieldBlock>();

                if (block != null && _fieldManager.IsTeamUnitBlock(block, TYPE_TEAM.Left))
                 {
                    _dragFieldBlock = block;
                    _dragUnitActor.transform.position = _dragFieldBlock.transform.position;
                    MovementCell(_dragFieldBlock, _dragUnitActor.movementCells);
                    RangeCell(_dragFieldBlock, _dragUnitActor.attackCells);
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                ClearCell();
                _dragUnitActor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _dragFieldBlock = null;
            }

        }
    }
       

    private void ClearCell()
    {
        _fieldManager.ClearMovements();
        _fieldManager.ClearRanges();
    }

    private void MovementCell(FieldBlock block, Vector2Int[] cells)
    {
        //블록을 기준으로 셀값 movement 
        //이외의 블록은 false
        _fieldManager.ClearMovements();
        _fieldManager.SetMovementBlocks(block, cells);
    }

    private void RangeCell(FieldBlock block, Vector2Int[] cells)
    {
        //블록을 기준으로 셀값 movement 
        //이외의 블록은 false
        _fieldManager.ClearRanges();
        _fieldManager.SetRangeBlocks(block, cells);
    }

    public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        List<UnitActor> deadList = new List<UnitActor>();

        for(int i = 0; i < list.Count; i++)
        {
            var unit = list[i];
            if (unit.typeTeam == typeTeam)
            {
                var nowBlock = fieldManager.FindActorBlock(unit);

                //var attackDirectionX = (typeTeam == TYPE_TEAM.Left) ? unit.rangeValue : -unit.rangeValue;
                //var movementDirectionX = (typeTeam == TYPE_TEAM.Left) ? unit.movementValue : -unit.movementValue;

                //공격방위
                var attackDirection = unit.attackCells;

                //이동방위 
                var movementDirection = unit.movementCells;

                FieldBlock[] attackBlocks = new FieldBlock[1];

                for (int firstAttackCount = unit.attackCount; firstAttackCount > 0; firstAttackCount--)
                {

                    switch (unit.typeUnitAttack)
                    {
                        case TYPE_UNIT_ATTACK.Normal:
                            attackBlocks[0] = fieldManager.GetAttackNearBlock(nowBlock.coordinate, attackDirection, typeTeam);
                            break;
                        case TYPE_UNIT_ATTACK.RandomRange:
                            attackBlocks[0] = fieldManager.GetAttackRandomBlock(nowBlock.coordinate, attackDirection, typeTeam);
                            break;
                        case TYPE_UNIT_ATTACK.Range:
                            attackBlocks = fieldManager.GetAttackAllBlocks(nowBlock.coordinate, attackDirection, typeTeam);
                            break;
                        case TYPE_UNIT_ATTACK.SingleRange:
                            if (attackBlocks[0] == null || attackBlocks[0].unitActor == null)
                                attackBlocks[0] = fieldManager.GetAttackRandomBlock(nowBlock.coordinate, attackDirection, typeTeam);
                            break;
                    }



                    //공격 가능 블록
                    //var attackBlock = fieldManager.GetAttackBlock(nowBlock.coordinate, attackDirection, typeTeam);

                    for (int b = 0; b < attackBlocks.Length; b++)
                    {
                        var attackBlock = attackBlocks[b];
                        //횟수만큼 공격
                        if (attackBlock != null)
                        {
                            if (attackBlock.unitActor.typeUnit == TYPE_UNIT.Castle)
                                gameTestManager.IncreaseHealth(unit.damageValue, typeTeam);
                            else
                            {
                                attackBlock.unitActor.IncreaseHealth(unit.damageValue);
                                if (attackBlock.unitActor.IsDead())
                                {
                                    var deadUnit = attackBlock.unitActor;
                                    deadList.Add(deadUnit);
                                    attackBlock.ResetUnitActor();
                                }
                            }
                        }
                    }

                    yield return new WaitForSeconds(Setting.FREAM_TIME);
                }

                //이동 가능 블록
                var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, movementDirection, typeTeam);

                //1회 이동
                if (movementBlock != null) {
                    nowBlock.ResetUnitActor();
                    movementBlock.SetUnitActor(unit);
                }

                yield return new WaitForSeconds(Setting.FREAM_TIME);




                if (unit.typeUnitAttack == TYPE_UNIT_ATTACK.Normal)
                {
                    //횟수만큼 추가 공격
                    for (int secondAttackCount = unit.attackCount; secondAttackCount > 0; secondAttackCount--)
                    {

                        switch (unit.typeUnitAttack)
                        {
                            case TYPE_UNIT_ATTACK.Normal:
                                if (attackBlocks[0] == null || attackBlocks[0].unitActor == null)
                                    attackBlocks[0] = fieldManager.GetAttackNearBlock(nowBlock.coordinate, attackDirection, typeTeam);
                                break;
                            //case TYPE_UNIT_ATTACK.RandomRange:
                            //    attackBlocks[0] = fieldManager.GetAttackRandomBlock(nowBlock.coordinate, attackDirection, typeTeam);
                            //    break;
                            //case TYPE_UNIT_ATTACK.Range:
                            //    attackBlocks = fieldManager.GetAttackAllBlocks(nowBlock.coordinate, attackDirection, typeTeam);
                            //    break;
                            //case TYPE_UNIT_ATTACK.SingleRange:
                            //    if (attackBlocks[0] == null || attackBlocks[0].unitActor == null)
                            //        attackBlocks[0] = fieldManager.GetAttackRandomBlock(nowBlock.coordinate, attackDirection, typeTeam);
                            //    break;
                        }

                        for (int b = 0; b < attackBlocks.Length; b++)
                        {
                            var attackBlock = attackBlocks[b];
                            //횟수만큼 공격
                            if (attackBlock != null)
                            {
                                if (attackBlock.unitActor.typeUnit == TYPE_UNIT.Castle)
                                    gameTestManager.IncreaseHealth(unit.damageValue, typeTeam);
                                else
                                {
                                    attackBlock.unitActor.IncreaseHealth(unit.damageValue);
                                    if (attackBlock.unitActor.IsDead())
                                    {
                                        var deadUnit = attackBlock.unitActor;
                                        deadList.Add(deadUnit);
                                        attackBlock.ResetUnitActor();
                                    }
                                }
                            }
                        }
                        yield return new WaitForSeconds(Setting.FREAM_TIME);
                    }
                }

            }
            yield return new WaitForSeconds(Setting.FREAM_TIME);
        }

        var arr = deadList.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            switch (arr[i].typeTeam)
            {
                case TYPE_TEAM.Left:
                    deadL++;
                    break;
                case TYPE_TEAM.Right:
                    deadR++;
                    break;
            }
            list.Remove(arr[i]);
            DestroyImmediate(arr[i].gameObject);

        }

    }

}
