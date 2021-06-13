using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameTestManager;

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

    //UnitActor _dragUnitActor;
    //FieldBlock _dragFieldBlock;

    private class DragBlock
    {
        public UnitActor unitActor;
        public FieldBlock fieldBlock;
        public Vector2Int formation;
    }
    
    private class DragActor
    {
        public List<DragBlock> dragBlocks = new List<DragBlock>();
        public bool IsEmpty() => dragBlocks.Count == 0;
        public bool IsAllFormation()
        {
            for(int i = 0; i < dragBlocks.Count; i++)
            {
                if (dragBlocks[i].fieldBlock == null) return false;
            }
            return true;
        }
        public void Clear() => dragBlocks.Clear();
        public DragBlock PopBlock()
        {
            if (!IsEmpty())
            {
                var block = dragBlocks[dragBlocks.Count - 1];
                dragBlocks.RemoveAt(dragBlocks.Count - 1);
                return block;
            }
            return null;
        }
        public void Add(DragBlock block) => dragBlocks.Add(block);
    }

    DragActor _dragActor = new DragActor();

    List<UnitActor> unitActorList = new List<UnitActor>();

    public bool IsDrag()
    {
        return !_dragActor.IsEmpty();
//        return _dragUnitActor != null;
    }

    //public UnitData[] GetRandomUnit(int count)
    //{
    //    if (unitStorage == null)
    //        unitStorage = new UnitStorage();
    //    return unitStorage.GetRandomUnits(count);
    //}
    
    public UnitCard[] GetRandomUnitCards(int count)
    {
        if (unitStorage == null)
            unitStorage = new UnitStorage();
        return unitStorage.GetRandomUnitCards(count);

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

        unit.SetTypeTeam(typeTeam);
        unit.SetData(new UnitCard(unitData));
        unit.AddBar(Instantiate(_uiBar));

        unitActorList.Add(unit);
        fieldBlock.SetUnitActor(unit);
        unit.SetLayer();

        //_dragUnitActor = null;
        //_dragActor.Clear();
        
        _dragActor.Clear();
    }

    public void CreateUnit(UnitCard uCard, FieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var unit = Instantiate(_unitActor);
        unit.gameObject.SetActive(true);

        unit.SetTypeTeam(typeTeam);
        unit.SetData(uCard);
        unit.AddBar(Instantiate(_uiBar));

        unitActorList.Add(unit);
        fieldBlock.SetUnitActor(unit);
        unit.SetLayer();

        _dragActor.Clear();
    }

    public void CreateUnits(TYPE_TEAM typeTeam)
    {
        while (!_dragActor.IsEmpty())
        {
            var dragBlock = _dragActor.PopBlock();

            if (dragBlock == null) return;

            var unit = dragBlock.unitActor;

            dragBlock.fieldBlock.SetUnitActor(unit);
            unit.AddBar(Instantiate(_uiBar));
            unit.SetTypeTeam(typeTeam);
            unit.gameObject.SetActive(true);

            unitActorList.Add(unit);
            unit.SetLayer();
        }
    }

    public void CreateUnits(UnitCard uCard, FieldBlock[] blocks, TYPE_TEAM typeTeam)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            var unit = Instantiate(_unitActor);
            unit.gameObject.SetActive(true);

            unit.SetTypeTeam(typeTeam);
            unit.SetData(uCard);
            unit.AddBar(Instantiate(_uiBar));

            unitActorList.Add(unit);
            blocks[i].SetUnitActor(unit);
            unit.SetLayer();
        }
    }

    private void DestroyAllDragUnit()
    {
        while (!_dragActor.IsEmpty())
        {
            var dragBlock = _dragActor.PopBlock();
            DestroyImmediate(dragBlock.unitActor.gameObject);
        }
    }


    public void DragUnit(UnitCard uCard)
    {
        for (int i = 0; i < uCard.formationCells.Length; i++)
        {
            var unitActor = Instantiate(_unitActor);
            unitActor.gameObject.SetActive(true);
            unitActor.SetData(uCard);

            _dragActor.Add(new DragBlock { unitActor = unitActor, formation = uCard.formationCells[i] });
        }
    }

    public bool DropUnit(UnitCard uCard, TYPE_TEAM typeTeam)
    {
        ClearCell();
        Debug.Log(!_dragActor.IsEmpty() +" "+ _dragActor.IsAllFormation());
        if (!_dragActor.IsEmpty() && _dragActor.IsAllFormation())
        {
            CreateUnits(typeTeam);
            return true;
        }
        else
        {
            DestroyAllDragUnit();
        }       
        return false;
    }


    private void Update()
    {
        if (!_dragActor.IsEmpty())
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 100f);

            bool isCheck = false;

            for(int i = 0; i < hits.Length; i++)
            {
                var block = hits[i].collider.GetComponent<FieldBlock>();

                if (block != null && _fieldManager.IsTeamUnitBlock(block, TYPE_TEAM.Left))
                 {
                    SetBlocks(block);
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                SetBlocks(null);
            }

        }
    }

    private void SetBlocks(FieldBlock originFieldBlock)
    {
        ClearCell();
        if (originFieldBlock != null)
        {
            for (int i = 0; i < _dragActor.dragBlocks.Count; i++)
            {
                var block = _dragActor.dragBlocks[i];
                var offsetFieldBlock = _fieldManager.GetBlock(originFieldBlock, block.formation);


                if (offsetFieldBlock != null)
                {
                    if (offsetFieldBlock.unitActor == null)
                    {
                        //formation
                        block.fieldBlock = offsetFieldBlock;
                        block.unitActor.transform.position = offsetFieldBlock.transform.position;
                        MovementCell(offsetFieldBlock, block.unitActor.movementCells);
                        RangeCell(offsetFieldBlock, block.unitActor.attackCells, block.unitActor.minRangeValue);
                    }
                    else
                    {
                        block.fieldBlock = null;
                        block.unitActor.transform.position = offsetFieldBlock.transform.position;
                        NotEmptyCells(offsetFieldBlock);
                    }
                }
                else
                {
                    block.fieldBlock = null;
                    //formation
                    block.unitActor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector2)block.formation;
                }
            }
        }
        else
        {
            for (int i = 0; i < _dragActor.dragBlocks.Count; i++)
            {
                var block = _dragActor.dragBlocks[i];
                block.fieldBlock = null;
                //formation
                block.unitActor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector2)block.formation;
                ClearCell();
            }
        }
    }

    private void ClearCell()
    {
        _fieldManager.ClearMovements();
        _fieldManager.ClearRanges();
        _fieldManager.ClearFormations();
    }

    private void NotEmptyCells(FieldBlock block)
    {
        _fieldManager.SetFormation(block);
    }

    private void MovementCell(FieldBlock block, Vector2Int[] cells)
    {
        //_fieldManager.ClearMovements();
        _fieldManager.SetMovementBlocks(block, cells);
    }

    private void RangeCell(FieldBlock block, Vector2Int[] cells, int minRangeValue)
    {
        //_fieldManager.ClearRanges();
        _fieldManager.SetRangeBlocks(block, cells, minRangeValue);
    }

    public bool IsLiveUnitsEmpty()
    {
        int lCnt = 0;
        int rCnt = 0;

        for (int i = 0; i < unitActorList.Count; i++)
        {
            if (unitActorList[i].typeTeam == TYPE_TEAM.Left && unitActorList[i].typeUnit != TYPE_UNIT.Castle)
                lCnt++;
            else if (unitActorList[i].typeTeam == TYPE_TEAM.Right && unitActorList[i].typeUnit != TYPE_UNIT.Castle)
                rCnt++;
        }

        if (lCnt == 0 || rCnt == 0) return true;
        return false;
    }
   
    //공격명령
    //public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    //{
    //    yield return new UnitManagerAction(this, ActionAttackUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, MovementUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, ActionAdditiveAttackUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, ActionCastleAttackUnits(fieldManager, typeTeam));
    //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
    //}

    public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_BATTLE_TURN typeBattleTurn)
    {
        switch (typeBattleTurn)
        {
            case TYPE_BATTLE_TURN.Forward:
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, ForwardUnits(fieldManager));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Left));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Right));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                break;
            case TYPE_BATTLE_TURN.Backward:
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, BackwardUnits(fieldManager));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Left));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Right));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                break;
            case TYPE_BATTLE_TURN.Charge:
                yield return new UnitManagerAction(this, ChargeReadyUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, ChargeUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Left));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Right));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                break;
            case TYPE_BATTLE_TURN.Guard:
                yield return new UnitManagerAction(this, GuardUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Left));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Right));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                break;
            case TYPE_BATTLE_TURN.Shoot:
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, ForwardUnits(fieldManager));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Left));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, TYPE_TEAM.Right));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager));
                break;
        }
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


    private IEnumerator AttackUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit= fieldBlocksL[index].unitActor;
            }
            else {
                unit= fieldBlocksR[index].unitActor;
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                if (unit.typeUnit != TYPE_UNIT.Castle)
                {
                    unit.ActionAttack(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
            }
        }
        yield return null;
    }

    private IEnumerator ChargeReadyUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit = fieldBlocksL[index].unitActor;
            }
            else
            {
                unit = fieldBlocksR[index].unitActor;
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                if (unit.typeUnit != TYPE_UNIT.Castle)
                {
                    unit.ActionChargeReady(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
            }
        }
        yield return null;
    }

    private IEnumerator ChargeUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit = fieldBlocksL[index].unitActor;
            }
            else
            {
                unit = fieldBlocksR[index].unitActor;
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                if (unit.typeUnit != TYPE_UNIT.Castle)
                {
                    //Debug.Log("ActionAttack");
                    unit.ActionCharge(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
                Debug.Log(System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }
        yield return null;
    }

    private IEnumerator GuardUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit = fieldBlocksL[index].unitActor;
            }
            else
            {
                unit = fieldBlocksR[index].unitActor;
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                if (unit.typeUnit != TYPE_UNIT.Castle)
                {
                    unit.ActionGuard(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
            }
        }
        yield return null;
    }

    //private IEnumerator ActionAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    //{

    //    var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

    //    List<UnitActor> units = new List<UnitActor>();
    //    for (int i = 0; i < fieldBlocks.Length; i++)
    //    {
    //        var unit = fieldBlocks[i].unitActor;
    //        if (unit != null)
    //        {
    //            if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT.Castle)
    //            {
    //                unit.ActionAttack(fieldManager, gameTestManager);
    //                units.Add(unit);
    //            }
    //        }
    //    }


    //    if (units.Count > 0)
    //    {
    //        int index = 0;
    //        while (index < units.Count)
    //        {
    //            if (!units[index].isRunning)
    //            {
    //                index++;
    //            }
    //            yield return null;
    //        }
    //    }
    //    yield return null;
    //}

    private IEnumerator CastleAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        FieldBlock[] fieldBlocks = fieldManager.GetSideBlocks(typeTeam);
        List<UnitActor> units = new List<UnitActor>();

        int defenceCount = 3;
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


        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;                
            }
        }
        yield return null;
    }

    //private IEnumerator AdditiveAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    //{

    //    var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

    //    List<UnitActor> units = new List<UnitActor>();
    //    for (int i = 0; i < fieldBlocks.Length; i++)
    //    {
    //        var unit = fieldBlocks[i].unitActor;
    //        if (unit != null)
    //        {
    //            if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT.Castle/* && unit.typeUnitAttack == TYPE_UNIT_ATTACK.Normal*/)
    //            {
    //                unit.ActionAttack(fieldManager, gameTestManager);
    //                units.Add(unit);
    //            }
    //        }
    //    }
    //    if (units.Count > 0)
    //    {
    //        int index = 0;
    //        while (index < units.Count)
    //        {
    //            if (!units[index].isRunning)
    //            {
    //                index++;
    //            }
    //            yield return null;
    //        }
    //    }
    //    yield return null;
    //}



    private IEnumerator ForwardUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            FieldBlock nowBlock = null;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit = fieldBlocksL[index].unitActor;
                nowBlock = fieldBlocksL[index];
            }
            else
            {
                unit = fieldBlocksR[index].unitActor;
                nowBlock = fieldBlocksR[index];
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, unit.typeTeam);
                if (movementBlock != null)
                {
                    unit.ForwardAction(nowBlock, movementBlock);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
            }
        }
        yield return null;
    }

    //private IEnumerator ForwardUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    //{
    //    var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

    //    List<UnitActor> units = new List<UnitActor>();
    //    for (int i = 0; i < fieldBlocks.Length; i++)
    //    {
    //        var unit = fieldBlocks[i].unitActor;
    //        if (unit != null)
    //        {
    //            var nowBlock = fieldManager.FindActorBlock(unit);
    //            if (nowBlock != null)
    //            {
    //                if (unit.typeTeam == typeTeam)
    //                {
    //                    var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, typeTeam);

    //                    if (movementBlock != null)
    //                    {
    //                        unit.ForwardAction(nowBlock, movementBlock);
    //                        units.Add(unit);
    //                        //yield return null;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    if (units.Count > 0)
    //    {
    //        int index = 0;
    //        while (index < units.Count)
    //        {
    //            if (!units[index].isRunning)
    //            {
    //                index++;
    //            }
    //            yield return null;
    //        }
    //        yield return new WaitForSeconds(Setting.FRAME_END_TIME);
    //    }
    //    yield return null;
    //}

    private IEnumerator BackwardUnits(FieldManager fieldManager)
    {

        var fieldBlocksL = fieldManager.GetAllBlocks(TYPE_TEAM.Left);
        var fieldBlocksR = fieldManager.GetAllBlocks(TYPE_TEAM.Right);
        var blockCount = fieldBlocksL.Length + fieldBlocksR.Length;

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < blockCount; i++)
        {
            int index = i / 2;
            FieldBlock nowBlock = null;
            UnitActor unit = null;
            if (i % 2 == 0)
            {
                unit = fieldBlocksL[index].unitActor;
                nowBlock = fieldBlocksL[index];
            }
            else
            {
                unit = fieldBlocksR[index].unitActor;
                nowBlock = fieldBlocksR[index];
            }

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, (unit.typeTeam == TYPE_TEAM.Left) ? TYPE_TEAM.Right : TYPE_TEAM.Left);
                if (movementBlock != null)
                {
                    unit.BackwardAction(nowBlock, movementBlock);
                    units.Add(unit);
                }
            }
        }
        yield return null;

        if (units.Count > 0)
        {
            int index = 0;
            while (index < units.Count)
            {
                if (!units[index].isRunning)
                {
                    index++;
                }
                yield return null;
            }
        }
        yield return null;
    }


    private IEnumerator DeadUnits(FieldManager fieldManager)
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


        if (deadList.Count > 0)
        {
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
            Debug.Log("DeadUnits");
        }
        yield return null;
    }

    //private IEnumerator DeadUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    //{
    //    List<UnitActor> deadList = new List<UnitActor>();

    //    var blocks = fieldManager.GetAllBlocks();

    //    for (int i = 0; i < blocks.Length; i++)
    //    {
    //        if (blocks[i].unitActor != null)
    //        {
    //            if (blocks[i].unitActor.IsDead())
    //            {
    //                var deadUnit = blocks[i].unitActor;
    //                deadList.Add(deadUnit);
    //                blocks[i].ResetUnitActor();
    //            }
    //        }
    //    }


    //    if (deadList.Count > 0)
    //    {
    //        var arr = deadList.ToArray();

    //        for (int i = 0; i < arr.Length; i++)
    //        {
    //            switch (arr[i].typeTeam)
    //            {
    //                case TYPE_TEAM.Left:
    //                    deadL++;
    //                    break;
    //                case TYPE_TEAM.Right:
    //                    deadR++;
    //                    break;
    //            }
    //            unitActorList.Remove(arr[i]);
    //            DestroyImmediate(arr[i].gameObject);
    //        }

    //        yield return new WaitForSeconds(Setting.FRAME_END_TIME);
    //    }
    //    yield return null;
    //}

    public IEnumerator ClearUnits()
    {
        List<UnitActor> deadList = new List<UnitActor>();

        var blocks = _fieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                var deadUnit = blocks[i].unitActor;
                deadList.Add(deadUnit);
                blocks[i].ResetUnitActor();
            }
        }

        if (deadList.Count > 0)
        {
            var arr = deadList.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                switch (arr[i].typeTeam)
                {
                    case TYPE_TEAM.Left:
                        //deadL++;
                        break;
                    case TYPE_TEAM.Right:
                        //deadR++;
                        break;
                }
                unitActorList.Remove(arr[i]);
                DestroyImmediate(arr[i].gameObject);
            }
        }
        yield return null;

    }
}
