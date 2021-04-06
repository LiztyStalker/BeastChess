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
        if(_dragUnitActor != null)
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
                    Debug.LogWarning("Create Canceled");
                    DestroyImmediate(_dragUnitActor.gameObject);
                    _dragFieldBlock = null;
                }
            }
            else
            {
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
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                _dragUnitActor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _dragFieldBlock = null;
            }

        }
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

                var attackDirection = unit.attackCells;
                var movementDirection = unit.movementCells;


                var attactBlock = fieldManager.GetAttackBlock(nowBlock.coordinate, attackDirection, typeTeam);
                var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, movementDirection, typeTeam);

                //1회 공격
                if (attactBlock != null)
                {

                    if (attactBlock.unitActor.typeUnit == TYPE_UNIT.Castle)
                        gameTestManager.IncreaseHealth(unit.damageValue, typeTeam);
                    else
                    {
                        attactBlock.unitActor.IncreaseHealth(unit.damageValue);
                        if (attactBlock.unitActor.IsDead())
                        {
                            var deadUnit = attactBlock.unitActor;
                            deadList.Add(deadUnit);
                            attactBlock.ResetUnitActor();
                        }
                    }
                }

                yield return new WaitForSeconds(Setting.FREAM_TIME);

                //1회 이동
                if (movementBlock != null) { 
                    nowBlock.ResetUnitActor();
                    movementBlock.SetUnitActor(unit);
                }

                yield return new WaitForSeconds(Setting.FREAM_TIME);

                attactBlock = fieldManager.GetAttackBlock(nowBlock.coordinate, attackDirection, typeTeam);

                //1회 추가 공격
                if (attactBlock != null)
                {


                    if (attactBlock.unitActor.typeUnit == TYPE_UNIT.Castle)
                        gameTestManager.IncreaseHealth(unit.damageValue, typeTeam);
                    else
                    {
                        attactBlock.unitActor.IncreaseHealth(unit.damageValue);
                        if (attactBlock.unitActor.IsDead())
                        {
                            var deadUnit = attactBlock.unitActor;
                            deadList.Add(deadUnit);
                            attactBlock.ResetUnitActor();
                        }
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
