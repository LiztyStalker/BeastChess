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


    #region ##### DragAndDrop Actor #####

    private class DragBlock
    {
        public UnitActor unitActor;
        public FieldBlock fieldBlock;
        public Vector2Int formation;
    }
    
    private class DragActor
    {
        public List<DragBlock> dragBlocks = new List<DragBlock>();
        public TYPE_TEAM typeTeam;
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

    #endregion



    DragActor _dragActor = new DragActor();

    List<UnitActor> unitActorList = new List<UnitActor>();

    private bool isRunningL = false;
    private bool isRunningR = false;

    public bool isRunning => isRunningL || isRunningR;

    public bool IsDrag()
    {
        return !_dragActor.IsEmpty();
    }




    public UnitCard[] GetUnitCards(params string[] names)
    {
        if (unitStorage == null)
            unitStorage = new UnitStorage();
        return unitStorage.GetUnitCards(names);
    }
    
    public UnitCard[] GetRandomUnitCards(int count)
    {
        if (unitStorage == null)
            unitStorage = new UnitStorage();
        return unitStorage.GetRandomUnitCards(count);
    }

    /// <summary>
    /// 성 유닛 배치
    /// </summary>
    /// <param name="fieldManager"></param>
    /// <param name="typeTeam"></param>
    public void CreateCastleUnit(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var sideBlocks = fieldManager.GetSideBlocks(typeTeam);
        var castleData = unitStorage.GetCastleUnit();
        var uCard = new UnitCard(castleData);
        for(int i = 0; i < sideBlocks.Length; i++)
        {
            CreateUnit(uCard, sideBlocks[i], typeTeam);
        }
    }       

    /// <summary>
    /// 유닛 배치
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="fieldBlock"></param>
    /// <param name="typeTeam"></param>
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


    /// <summary>
    /// 드래그 중인 유닛 배치
    /// </summary>
    /// <param name="typeTeam"></param>
    public void CreateDragUnits()
    {
        while (!_dragActor.IsEmpty())
        {
            var dragBlock = _dragActor.PopBlock();

            if (dragBlock == null) return;

            var unit = dragBlock.unitActor;

            dragBlock.fieldBlock.SetUnitActor(unit);
            unit.AddBar(Instantiate(_uiBar));
            unit.SetTypeTeam(_dragActor.typeTeam);
            unit.gameObject.SetActive(true);

            unitActorList.Add(unit);
            unit.SetLayer();
        }
    }

    /// <summary>
    /// 블록 내 해당 유닛 배치
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="blocks"></param>
    /// <param name="typeTeam"></param>
    public void CreateUnits(UnitCard uCard, FieldBlock[] blocks, TYPE_TEAM typeTeam)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            CreateUnit(uCard, blocks[i], typeTeam);
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


    public void DragUnitActor(UnitCard uCard, TYPE_TEAM dropTeam)
    {
        _dragActor.typeTeam = dropTeam;
        for (int i = 0; i < uCard.formationCells.Length; i++)
        {
            var unitActor = Instantiate(_unitActor);
            unitActor.gameObject.SetActive(true);
            unitActor.SetTypeTeam(dropTeam);
            unitActor.SetData(uCard);

            _dragActor.Add(new DragBlock {
                unitActor = unitActor,
                formation = uCard.formationCells[i],                
            });
        }
    }

    public bool DropUnitActor(UnitCard uCard)
    {
        ClearCellColor();
        if (!_dragActor.IsEmpty() && _dragActor.IsAllFormation())
        {
            CreateDragUnits();
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

                if (block != null && _fieldManager.IsTeamUnitBlock(block, _dragActor.typeTeam))
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

       
    /// <summary>
    /// 블록 유닛 임시 배치
    /// Drag
    /// </summary>
    /// <param name="originFieldBlock"></param>
    private void SetBlocks(FieldBlock originFieldBlock)
    {
        ClearCellColor();
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
                        MovementCellColor(offsetFieldBlock, block.unitActor.movementCells);
                        RangeCellColor(offsetFieldBlock, block.unitActor.attackCells, block.unitActor.minRangeValue);
                    }
                    else
                    {
                        block.fieldBlock = null;
                        block.unitActor.transform.position = offsetFieldBlock.transform.position;
                        NotEmptyCellColor(offsetFieldBlock);
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
                ClearCellColor();
            }
        }
    }

    /// <summary>
    /// 비지 않은 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    private void NotEmptyCellColor(FieldBlock block) => _fieldManager.SetFormation(block);

    /// <summary>
    /// 이동가능 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    private void MovementCellColor(FieldBlock block, Vector2Int[] cells) => _fieldManager.SetMovementBlocks(block, cells);

    /// <summary>
    /// 비거리 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    /// <param name="minRangeValue"></param>
    private void RangeCellColor(FieldBlock block, Vector2Int[] cells, int minRangeValue) => _fieldManager.SetRangeBlocks(block, cells, minRangeValue);



    private void ClearCellColor()
    {
        _fieldManager.ClearMovements();
        _fieldManager.ClearRanges();
        _fieldManager.ClearFormations();
    }



    public int IsLivedUnits(TYPE_TEAM typeTeam)
    {
        int cnt = 0;
        for (int i = 0; i < unitActorList.Count; i++)
        {
            if (unitActorList[i].typeTeam == typeTeam && unitActorList[i].typeUnit != TYPE_UNIT_FORMATION.Castle)
                cnt++;
        }
        return cnt;
    }

    public bool IsLiveUnitsEmpty()
    {
        int lCnt = IsLivedUnits(TYPE_TEAM.Left);
        int rCnt = IsLivedUnits(TYPE_TEAM.Right);
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

    bool isChargeL = false;
    bool isChargeR = false;

    public IEnumerator ActionUnits(FieldManager fieldManager, TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {           
            isRunningL = true;
        }

        if (typeTeam == TYPE_TEAM.Right)
        {
            isRunningR = true;
        }

        for (int i = 0; i < unitActorList.Count; i++)
        {
            if (unitActorList[i].typeTeam == typeTeam)
            {
                unitActorList[i].SetBattleTurn(typeBattleTurn);
            }
        }

        //Debug.Log(typeTeam + " " + typeBattleTurn);

        switch (typeBattleTurn)
        {
            case TYPE_BATTLE_TURN.Forward:
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, ForwardUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam, TYPE_UNIT_GROUP.FootSoldier | TYPE_UNIT_GROUP.Charger | TYPE_UNIT_GROUP.Supporter));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                break;
            case TYPE_BATTLE_TURN.Backward:
                yield return new UnitManagerAction(this, BackwardUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                break;
            case TYPE_BATTLE_TURN.Charge:
                if (typeTeam == TYPE_TEAM.Left)
                {
                    isChargeL = true;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    isChargeR = true;
                }
                yield return new UnitManagerAction(this, ChargeReadyUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, ChargeUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, ChargeAttackUnits(fieldManager, typeTeam));
                if (typeTeam == TYPE_TEAM.Left)
                {
                    isChargeL = false;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    isChargeR = false;
                }
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                
                break;
            case TYPE_BATTLE_TURN.Guard:
                yield return new UnitManagerAction(this, GuardUnits(fieldManager, typeTeam));
                if (typeTeam == TYPE_TEAM.Left)
                {
                    while (isChargeR)
                    {
                        yield return null;
                    }
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    while (isChargeL)
                    {
                        yield return null;
                    }
                }
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                break;
            case TYPE_BATTLE_TURN.Shoot:
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, ForwardUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam, TYPE_UNIT_GROUP.Shooter));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                break;
        }

        yield return null;

        //Debug.Log(isRunningL + " " + isRunningR);
        if (typeTeam == TYPE_TEAM.Left)
            isRunningL = false;

        if (typeTeam == TYPE_TEAM.Right)
            isRunningR = false;

        for (int i = 0; i < unitActorList.Count; i++)
        {
            if (unitActorList[i].typeTeam == typeTeam)
            {
                unitActorList[i].SetBattleTurn(TYPE_BATTLE_TURN.None);
            }
        }

        yield return null;
    }





    #region ##### UnitManagerAction #####

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

    #endregion





    private IEnumerator AttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {

        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle && (unit.typeUnitGroup & typeClass) == unit.typeUnitGroup)
                {
                    unit.ActionAttack(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
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

    private IEnumerator ChargeReadyUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle)
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
    
    private IEnumerator ChargeUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                var nowBlock = fieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.chargeCells, typeTeam, unit.typeMovement);

                        if (movementBlock != null)
                        {
                            unit.ChargeAction(nowBlock, movementBlock);
                            units.Add(unit);
                        }
                    }
                }
            }
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

    private IEnumerator ChargeAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {

        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle && (unit.typeUnitGroup & typeClass) == unit.typeUnitGroup)
                {
                    unit.ActionChargeAttack(fieldManager, gameTestManager);
                    units.Add(unit);
                }
            }
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

    private IEnumerator GuardUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle)
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

    private IEnumerator CastleAttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        FieldBlock[] fieldBlocks = fieldManager.GetSideBlocks(typeTeam);
        List<UnitActor> units = new List<UnitActor>();

        int defenceCount = 7;
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

    private IEnumerator ForwardUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                var nowBlock = fieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, typeTeam, unit.typeMovement);

                        if (movementBlock != null)
                        {
                            unit.ForwardAction(nowBlock, movementBlock);
                            units.Add(unit);
                            //yield return null;
                        }
                    }
                }
            }
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

    private IEnumerator BackwardUnits(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var fieldBlocks = fieldManager.GetAllBlocks(typeTeam, true);

        List<UnitActor> units = new List<UnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                var nowBlock = fieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = fieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, (unit.typeTeam == TYPE_TEAM.Left) ? TYPE_TEAM.Right : TYPE_TEAM.Left, unit.typeMovement);

                        if (movementBlock != null)
                        {
                            unit.BackwardAction(nowBlock, movementBlock);
                            units.Add(unit);
                            //yield return null;
                        }
                    }
                }
            }
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

            yield return new WaitForSeconds(Settings.FRAME_END_TIME);
        }
        yield return null;
    }


    private void RemoveUnitActor(UnitActor unitActor)
    {
        unitActorList.Remove(unitActor);
        DestroyImmediate(unitActor.gameObject);
    }

    public void ClearAllUnits()
    {
        var blocks = _fieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                RemoveUnitActor(blocks[i].unitActor);
            }
        }
    }

    public void ClearDeadUnits()
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
                RemoveUnitActor(arr[i]);
            }
        }
    }
}
