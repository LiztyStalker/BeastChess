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

    [SerializeField]
    UIBar _uiBar;


    [HideInInspector]
    public int deadL;

    [HideInInspector]
    public int deadR;

    UnitActor _dragUnitActor;
    FieldBlock _dragFieldBlock;

    List<UnitActor> unitActorList = new List<UnitActor>();

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
        unit.gameObject.SetActive(true);

        unit.SetData(unitData);

        fieldBlock.SetUnitActor(unit);
        unit.AddBar(Instantiate(_uiBar));
        unit.SetTypeTeam(typeTeam);
        unitActorList.Add(unit);

        _dragUnitActor = null;
    }

    public void CreateUnit(FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var unit = _dragUnitActor;

        fieldBlock.SetUnitActor(unit);
        unit.AddBar(Instantiate(_uiBar));
        unit.SetTypeTeam(typeTeam);
        unit.gameObject.SetActive(true);
        unitActorList.Add(unit);

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
                    RangeCell(_dragFieldBlock, _dragUnitActor.attackCells, _dragUnitActor.minRangeValue);
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
        _fieldManager.ClearMovements();
        _fieldManager.SetMovementBlocks(block, cells);
    }

    private void RangeCell(FieldBlock block, Vector2Int[] cells, int minRangeValue)
    {
        _fieldManager.ClearRanges();
        _fieldManager.SetRangeBlocks(block, cells, minRangeValue);
    }


    //공격명령
    public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        yield return new UnitManagerAction(this, ActionAttackUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, MovementUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, ActionAdditiveAttackUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, ActionCastleAttackUnits(fieldManager, typeTeam));
        yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
    }

    private class UnitManagerAction : CustomYieldInstruction
    {
        private bool isRunning = false;
        private MonoBehaviour mono;
        private IEnumerator enumerator;
        private Coroutine coroutine;

        public override bool keepWaiting {
            get{
                //Debug.Log("keepwaiting");
                return isRunning;
            }
        }

        public UnitManagerAction(MonoBehaviour mono, IEnumerator enumerator)
        {
            this.mono = mono;
            this.enumerator = enumerator;
            coroutine = mono.StartCoroutine(ActionCoroutine());
        }

        private IEnumerator ActionCoroutine()
        {
            isRunning = true;
            yield return mono.StartCoroutine(enumerator);
            isRunning = false;
        }
    }

    private IEnumerator ActionAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        List<UnitActor> units = new List<UnitActor>();

        for (int i = 0; i < unitActorList.Count; i++)
        {
            var unit = unitActorList[i];
            if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT.Castle)
            {
                unit.ActionAttack(fieldManager, gameTestManager);
                units.Add(unit);
            }
        }

        int index = 0;
        while (index < units.Count)
        {
            //Debug.Log("index" + index + " " + units[index].isRunning);
            if (!units[index].isRunning)
            {
                index++;
            }
            yield return null;
        }
        yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
//        yield return null; //모든 코루틴 사용자가 끝날때까지 대기
    }

    private IEnumerator ActionCastleAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        FieldBlock[] fieldBlocks = fieldManager.GetSideBlocks(typeTeam);
        List<UnitActor> units = new List<UnitActor>();

        int defenceCount = 1;
        int unitsCount = fieldBlocks.Length;

        while(defenceCount > 0 && unitsCount > 0)
        {
            var block = fieldBlocks[Random.Range(0, fieldBlocks.Length)];

            if (!units.Contains(block.castleActor))
            {

                var isAttack = block.castleActor.DirectAttack(fieldManager, gameTestManager);

                if (isAttack)
                {
                    units.Add(block.castleActor);
                    defenceCount--;
                }
            }
            unitsCount--;
        }
       

        int index = 0;
        while (index < units.Count)
        {
            if (!units[index].isRunning)
            {
                index++;
                //Debug.Log("index" + index);
            }
            yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
        }
        yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
    }

    private IEnumerator ActionAdditiveAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        List<UnitActor> units = new List<UnitActor>();

        for (int i = 0; i < unitActorList.Count; i++)
        {
            var unit = unitActorList[i];
            if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT.Castle/* && unit.typeUnitAttack == TYPE_UNIT_ATTACK.Normal*/)
            {
                unit.ActionAttack(fieldManager, gameTestManager);
                units.Add(unit);
            }
        }

        int index = 0;
        while (index < units.Count)
        {
            if (!units[index].isRunning)
            {
                index++;
                //Debug.Log("index" + index);
            }
            yield return null;
        }
        yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
//        yield return null; //모든 코루틴 사용자가 끝날때까지 대기
    }

    private IEnumerator MovementUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {

        List<UnitActor> units = new List<UnitActor>();

        for (int i = 0; i < unitActorList.Count; i++)
        {
            var unit = unitActorList[i];
            var nowBlock = fieldManager.FindActorBlock(unit);
            if (nowBlock != null)
            {
                if (unit.typeTeam == typeTeam)
                {
                    var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, typeTeam);

                    if (movementBlock != null)
                    {
                        unit.MovementAction(nowBlock, movementBlock);
                        units.Add(unit);
                    }
                }
            }
        }
        
        int index = 0;
        while (index < units.Count)
        {
            if (!units[index].isRunning)
            {
                index++;
                //Debug.Log("index" + index);
            }
            yield return null;
        }

        yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
    }

    private IEnumerator DeadUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        List<UnitActor> deadList = new List<UnitActor>();

        var blocks = fieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                if (blocks[i].unitActor.IsDead())
                {
                    var deadUnit = blocks[i].unitActor;
                    deadList.Add(deadUnit);
                    blocks[i].ResetUnitActor();
                }
            }
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
            unitActorList.Remove(arr[i]);
            DestroyImmediate(arr[i].gameObject);
        }

        yield return new WaitForSeconds(Setting.FREAM_TIME * 5f);
    }
}
