using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;





public class UnitManager : MonoBehaviour
{

    private static UnitManager _current;

    public static UnitManager Current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<UnitManager>();
            }
            return _current;
        }
    }

    

    [SerializeField]
    private BattleFieldManager gameTestManager;

    private UnitActor _unitActor;

    private UnitActor unitActor
    {
        get
        {
            if(_unitActor == null)
            {
                var obj = DataStorage.Instance.GetDataOrNull<GameObject>("UnitActor", null, null);
                _unitActor = obj.GetComponent<UnitActor>();
            }
            return _unitActor;
        }
    }

    private UIBar _uiBar;

    private UIBar uiBar
    {
        get
        {
            if (_uiBar == null)
            {
                var obj = DataStorage.Instance.GetDataOrNull<GameObject>("UIBar", null, null);
                _uiBar = obj.GetComponent<UIBar>();
            }
            return _uiBar;
        }
    }


    [HideInInspector]
    public int deadL;

    [HideInInspector]
    public int deadR;


#if UNITY_EDITOR
    public string nowStep { get; private set; }

#endif


    #region ##### DragAndDrop Actor #####

    private class DragBlock
    {
        public IUnitActor unitActor;
        public IFieldBlock fieldBlock;
        public Vector2Int formation;
    }
    
    private class DragActor
    {
        public bool isChanged = false;
        public UnitCard uCard;
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
        public void Clear()
        {
            dragBlocks.Clear();
            uCard = null;
        }
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


    private List<UnitCard> _usedCardList = new List<UnitCard>();

    private DragActor _dragActors = new DragActor();

    private Dictionary<int, IUnitActor> _unitActorDic = new Dictionary<int, IUnitActor>();

    private bool _isRunningL = false;
    private bool _isRunningR = false;

    public bool isRunning => _isRunningL || _isRunningR;


  
    public bool IsDrag()
    {
        return !_dragActors.IsEmpty();
    }
        
   
    /// <summary>
    /// 성 유닛 배치
    /// </summary>
    /// <param name="fieldManager"></param>
    /// <param name="typeTeam"></param>
    public void CreateCastleUnit(TYPE_TEAM typeTeam)
    {
        var sideBlocks = FieldManager.GetSideBlocks(typeTeam);
        var castleData = DataStorage.Instance.GetDataOrNull<UnitData>("Castle");// GetCastleUnit();
    
        for (int i = 0; i < sideBlocks.Length; i++)
        {
            var uCard = UnitCard.Create(castleData);
            var uKey = uCard.UnitKeys[0];
            CreateUnit(uCard, uKey, sideBlocks[i], typeTeam);
            _usedCardList.Add(uCard);
        }
    }       

    /// <summary>
    /// 유닛 배치
    /// </summary>
    /// <param name="uCard"></param>
    /// <param name="fieldBlock"></param>
    /// <param name="typeTeam"></param>
    public IUnitActor CreateUnit(UnitCard uCard, int uKey, IFieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var uActor = Instantiate(unitActor);
        uActor.gameObject.SetActive(true);
        uActor.name = $"UnitActor_{uKey}_{uCard.name}";
        uActor.SetTypeTeam(typeTeam);
        uActor.SetData(uCard);
        uActor.SetKey(uKey);
        uActor.SetOnDeadListener(DeadUnitEvent);

        uActor.AddBar(Instantiate(uiBar));

        _unitActorDic.Add(uActor.uKey, uActor);

        fieldBlock.SetUnitActor(uActor);
        uActor.SetLayer();

        CastSkills(uActor, TYPE_SKILL_CAST.DeployCast);

        return uActor;
    }


    /// <summary>
    /// 드래그 중인 유닛 배치
    /// </summary>
    /// <param name="typeTeam"></param>
    public void CreateDragUnits(ICaster caster)
    {
        while (!_dragActors.IsEmpty())
        {
            var dragBlock = _dragActors.PopBlock();

            if (dragBlock == null) return;

            var uActor = dragBlock.unitActor;

            dragBlock.fieldBlock.SetUnitActor(uActor);
            uActor.AddBar(Instantiate(uiBar));
            uActor.SetTypeTeam(_dragActors.typeTeam);
            uActor.SetActive(true);

            CastSkills(uActor, TYPE_SKILL_CAST.DeployCast);

            _unitActorDic.Add(uActor.uKey, uActor);
            uActor.SetLayer();
        }

        _dragActors.isChanged = false;
        _usedCardList.Add(_dragActors.uCard);
        _dragActors.Clear();
    }

    private void DeadUnitEvent(ICaster caster)
    {
        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i].IsHasUnitActor())
            {
                for (int j = 0; j < blocks[i].unitActors.Length; j++)
                {
                    blocks[i].unitActors[j].RemoveStatusData(caster);
                }
            }
        }
    }
    

    public bool IsUsedCard(UnitCard uCard)
    {
        return _usedCardList.Contains(uCard);
    }

    public void CreateUnits(UnitCard uCard, int[] uKeys, IFieldBlock[] blocks, TYPE_TEAM typeTeam)
    {
        for (int i = 0; i < uKeys.Length; i++)
        {
            CreateUnit(uCard, uKeys[i], blocks[i], typeTeam);
        }
        _usedCardList.Add(uCard);
    }
         
    private void DestroyAllDragUnit()
    {
        while (!_dragActors.IsEmpty())
        {
            var dragBlock = _dragActors.PopBlock();
            dragBlock.unitActor.Destroy();
        }
    }

    private Dictionary<int, IFieldBlock> _cancelDic = new Dictionary<int, IFieldBlock>();

    public void DragUnitActor(IUnitActor unitActor)
    {
        _cancelDic.Clear();

        _dragActors.isChanged = true;
        _dragActors.typeTeam = unitActor.typeTeam;
        _dragActors.uCard = unitActor.unitCard;

        var uCard = unitActor.unitCard;
        _usedCardList.Remove(uCard);

        for (int i = 0; i < uCard.UnitKeys.Length; i++)
        {
            var uKey = uCard.UnitKeys[i];
            if (_unitActorDic.ContainsKey(uKey))
            {
                var uActor = _unitActorDic[uKey];
                _dragActors.Add(new DragBlock
                {
                    unitActor = uActor,
                    formation = uCard.formationCells[i],
                });
                _unitActorDic.Remove(uKey);

                var fieldBlock = FieldManager.FindActorBlock(uActor);
                fieldBlock.LeaveUnitActor(uActor);

                _cancelDic.Add(uKey, fieldBlock);
            }
        }
    }

    public void DragUnitActor(UnitCard uCard, TYPE_TEAM dropTeam)
    {
        _dragActors.isChanged = false;
        _dragActors.typeTeam = dropTeam;
        _dragActors.uCard = uCard;

        for (int i = 0; i < uCard.UnitKeys.Length; i++)
        {
            var uKey = uCard.UnitKeys[i];

            if (i < uCard.formationCells.Length)
            {
                if (!uCard.IsDead(uKey))
                {
                    var uActor = Instantiate(unitActor);
                    uActor.name = $"UnitActor_{uKey}";
                    uActor.gameObject.SetActive(true);
                    uActor.SetTypeTeam(dropTeam);
                    uActor.SetData(uCard);
                    uActor.SetKey(uKey);
                    uActor.SetOnDeadListener(DeadUnitEvent);

                    _dragActors.Add(new DragBlock
                    {
                        unitActor = uActor,
                        formation = uCard.formationCells[i],
                    });
                }
            }
        }
    }

    public void ReturnUnitActor(IUnitActor unitActor)
    {
        var uCard = unitActor.unitCard;
        for (int i = 0; i < uCard.UnitKeys.Length; i++)
        {
            var uKey = uCard.UnitKeys[i];
            if (_unitActorDic.ContainsKey(uKey))
            {
                var uActor = _unitActorDic[uKey];
                _unitActorDic.Remove(uKey);

                var fieldBlock = FieldManager.FindActorBlock(uActor);
                fieldBlock.LeaveUnitActor(uActor);

                uActor.Destroy();
            }
        }
        _usedCardList.Remove(uCard);
    }

    public void ReturnUnitCards(ICommanderActor cActor)
    {
        for(int i = 0; i < _usedCardList.Count; i++)
        {
            cActor.ReturnSupply(_usedCardList[i]);
        }
    }

    public void CancelChangeUnitActor()
    {
        ClearCellColor();
        while (!_dragActors.IsEmpty())
        {
            var dragBlock = _dragActors.PopBlock();

            if (dragBlock == null) return;

            var uActor = dragBlock.unitActor;

            if (_cancelDic.ContainsKey(uActor.uKey))
            {
                _cancelDic[uActor.uKey].SetUnitActor(uActor);
                _cancelDic.Remove(uActor.uKey);

                _unitActorDic.Add(uActor.uKey, uActor);
            }
        }

        _usedCardList.Add(_dragActors.uCard);

        _dragActors.isChanged = false;
        _dragActors.Clear();


        _cancelDic.Clear();
    }

    public void CancelUnitActor()
    {
        ClearCellColor();
        DestroyAllDragUnit();
        _dragActors.Clear();
    }

    public bool DropUnitActor(ICaster caster, UnitCard uCard)
    {
        ClearCellColor();
        if (!_dragActors.IsEmpty() && _dragActors.IsAllFormation())
        {
            CreateDragUnits(caster);
            return true;
        }
        else
        {
            DestroyAllDragUnit();
        }       
        return false;
    }

    public bool DropUnitActor(UnitCard uCard)
    {
        ClearCellColor();
        if (!_dragActors.IsEmpty() && _dragActors.IsAllFormation())
        {
            CreateDragUnits(null);
            return true;
        }
        else
        {
            //DestroyAllDragUnit();
        }
        return false;
    }


    public void ClickedAction(Vector2 screenPosition)
    {
        if (_dragActors.isChanged)
        {
            DropUnitActor(_dragActors.uCard);
        }
    }

    public void ClickAction(Vector2 screenPosition)
    {
        if (!_dragActors.IsEmpty())
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(screenPosition), Vector3.forward, 100f);

            bool isCheck = false;

            for(int i = 0; i < hits.Length; i++)
            {
                var block = hits[i].collider.GetComponent<IFieldBlock>();

                if (block != null && FieldManager.IsTeamUnitBlock(block, _dragActors.typeTeam))
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
    private void SetBlocks(IFieldBlock originFieldBlock)
    {
        ClearCellColor();
        if (originFieldBlock != null)
        {
            for (int i = 0; i < _dragActors.dragBlocks.Count; i++)
            {
                var block = _dragActors.dragBlocks[i];
                var offsetFieldBlock = FieldManager.GetBlock(originFieldBlock, block.formation);


                if (offsetFieldBlock != null)
                {
                    if (!offsetFieldBlock.IsHasUnitActor() || (offsetFieldBlock.IsHasCastleUnitActor() && !offsetFieldBlock.IsHasGroundUnitActor()))
                    {
                        //formation
                        block.fieldBlock = offsetFieldBlock;
                        block.unitActor.SetPosition(offsetFieldBlock.position);
                        var movementCells = FieldManager.GetMovementCells(block.unitActor.movementValue);
                        MovementCellColor(offsetFieldBlock, movementCells);
                        //RangeCellColor(offsetFieldBlock, block.unitActor.attackCells, block.unitActor.minRangeValue);
                        RangeCellColor(offsetFieldBlock, _dragActors.uCard.AttackTargetData, _dragActors.typeTeam);
                    }
                    else
                    {
                        block.fieldBlock = null;
                        block.unitActor.SetPosition(offsetFieldBlock.position);
                        NotEmptyCellColor(offsetFieldBlock);
                    }
                }
                else
                {
                    block.fieldBlock = null;
                    //formation
                    block.unitActor.SetPosition((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + block.formation);
                }
            }
        }
        else
        {
            for (int i = 0; i < _dragActors.dragBlocks.Count; i++)
            {
                var block = _dragActors.dragBlocks[i];
                block.fieldBlock = null;
                //formation
                block.unitActor.SetPosition((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + block.formation);
                ClearCellColor();
            }
        }
    }

    /// <summary>
    /// 비지 않은 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    private void NotEmptyCellColor(IFieldBlock block) => FieldManager.SetFormationColor(block);

    /// <summary>
    /// 이동가능 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    private void MovementCellColor(IFieldBlock block, Vector2Int[] cells) => FieldManager.SetMovementBlocksColor(block, cells);

    /// <summary>
    /// 비거리 블록 칠하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    /// <param name="minRangeValue"></param>
    //private void RangeCellColor(IFieldBlock block, Vector2Int[] cells, int minRangeValue) => FieldManager.SetRangeBlocksColor(block, cells, minRangeValue);
    private void RangeCellColor(IFieldBlock block, TargetData targetData, TYPE_TEAM typeTeam) => FieldManager.SetRangeBlocksColor(block, targetData, typeTeam);



    private void ClearCellColor()
    {
        FieldManager.ClearMovements();
        FieldManager.ClearRanges();
        FieldManager.ClearFormations();
    }

    public string BattleResultToString()
    {
        var countL = _unitActorDic.Values.Where(uActor => uActor.typeUnit != TYPE_UNIT_FORMATION.Castle).
            GroupBy(actor => actor.unitCard.name).
            Select(actor => new { name = actor.Key, count = actor.Count() }).ToArray();

        var str = "";
        for(int i = 0; i < countL.Length; i++)
        {
            str += countL[i].name + " " + countL[i].count.ToString() + "\n";
        }

        return str;
    }

    public int IsLivedUnitCount(TYPE_TEAM typeTeam)
    {
        int cnt = 0;

        foreach(var key in _unitActorDic.Keys)
        {
            var uActor = _unitActorDic[key];
            if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                cnt++;
        }
        return cnt;
    }

    public bool IsLiveUnitsEmpty()
    {
        int lCnt = IsLivedUnitCount(TYPE_TEAM.Left);
        int rCnt = IsLivedUnitCount(TYPE_TEAM.Right);
        if (lCnt == 0 || rCnt == 0) return true;
        return false;
    }

    
    private bool _isChargeL = false;
    private bool _isChargeR = false;


    public IEnumerator ActionUnits(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {           
            _isRunningL = true;
        }

        if (typeTeam == TYPE_TEAM.Right)
        {
            _isRunningR = true;
        }


        foreach(var value in _unitActorDic.Values)
        {
            if (value.typeTeam == typeTeam)
            {
                value.SetBattleTurn(typeBattleTurn);
            }
        }
        
        switch (typeBattleTurn)
        {
            case TYPE_BATTLE_TURN.Forward:
                yield return new UnitManagerAction(this, AttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, ForwardUnits(typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(typeTeam, TYPE_UNIT_GROUP.FootSoldier | TYPE_UNIT_GROUP.Charger | TYPE_UNIT_GROUP.Supporter));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                break;
            case TYPE_BATTLE_TURN.Backward:
                yield return new UnitManagerAction(this, BackwardUnits(typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                break;
            case TYPE_BATTLE_TURN.Charge:
                if (typeTeam == TYPE_TEAM.Left)
                {
                    _isChargeL = true;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    _isChargeR = true;
                }
                yield return new UnitManagerAction(this, ChargeReadyUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, ChargeUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, ChargeAttackUnits(typeTeam));
                if (typeTeam == TYPE_TEAM.Left)
                {
                    _isChargeL = false;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    _isChargeR = false;
                }
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                
                break;
            case TYPE_BATTLE_TURN.Guard:
                yield return new UnitManagerAction(this, GuardUnits(typeTeam));
                if (typeTeam == TYPE_TEAM.Left)
                {
                    while (_isChargeR)
                    {
                        yield return null;
                    }
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    while (_isChargeL)
                    {
                        yield return null;
                    }
                }
                yield return new UnitManagerAction(this, AttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                break;            
        }

        yield return null;


        //Debug.Log(isRunningL + " " + isRunningR);
        if (typeTeam == TYPE_TEAM.Left)
            _isRunningL = false;

        if (typeTeam == TYPE_TEAM.Right)
            _isRunningR = false;


        foreach(var value in _unitActorDic.Values)
        {
            if (value.typeTeam == typeTeam)
            {
                value.SetBattleTurn(TYPE_BATTLE_TURN.None);
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



    //사전작동
    //각각의 지휘관 스킬의 사전 작동 스킬을 찾는다
    //각 스킬마다 스킬에 적합한 유닛을 기억한다
    //모든 유닛을 찾았으면 스킬을 적용한다
    public IEnumerator SetPreActiveActionUnits(ICommanderActor lcActor, ICommanderActor rcActor)
    {

        CastSkills(lcActor, TYPE_SKILL_CAST.PreCast);
        CastSkills(rcActor, TYPE_SKILL_CAST.PreCast);

        var blocks = FieldManager.GetAllBlocks();
        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block.IsHasUnitActor())
            {
                for (int j = 0; j < block.unitActors.Length; j++)
                {
                    CastSkills(block.unitActors[j], TYPE_SKILL_CAST.PreCast);
                }
            }
        }
        yield return null;
    }

    public IEnumerator ReleasePreActiveActionUnits()
    {
        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            var unitActors = blocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                unitActors[j].RemoveStatusData();
            }
        }
        yield return null;
    }


    public static void CastSkills(ICaster caster, TYPE_SKILL_CAST typeSkillActivate)
    {          
        var skills = caster.skills;
        if (skills != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                //Debug.Log(caster);
                skills[i].CastSkillProcess(caster, typeSkillActivate);
            }
        }
    }

    //public static void ReceiveSkills(ICaster caster, TYPE_TEAM typeTeam)
    //{
    //    for (int i = 0; i < caster.skills.Length; i++)
    //        ReceiveSkill(caster, caster.skills[i], typeTeam);
    //}

    //public static void ReceiveSkill(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    //{
    //    var blocks = FieldManager.GetTargetBlocks(caster, skillData.TargetData, typeTeam);
    //    for (int i = 0; i < blocks.Length; i++)
    //    {
    //        if (blocks[i].unitActor != null) blocks[i].unitActor.ReceiveSkill(caster, skillData, skillData.typeSkillActivate);
    //    }
    //}


    public static void CleanUp()
    {
        var blocks = FieldManager.GetAllBlocks();
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].CleanUp();
        }
    }

    #region ##### Action #####

    private IEnumerator AttackUnits(TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {
        SetStep("AttackUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;

            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle && (uActor.typeUnitGroup & typeClass) == uActor.typeUnitGroup)
                {
                    uActor.ActionAttack(gameTestManager);
                    units.Add(uActor);
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

    private IEnumerator ChargeReadyUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ChargeReadyUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    uActor.ActionChargeReady(gameTestManager);
                    units.Add(uActor);
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
    
    private IEnumerator ChargeUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ChargeUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;

            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                var nowBlock = FieldManager.FindActorBlock(uActor);
                if (nowBlock != null)
                {
                    if (uActor.typeTeam == typeTeam)
                    {
                        var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, uActor.typeMovement, uActor.chargeMovementValue, typeTeam);

                        if (movementBlock != null)
                        {
                            uActor.ChargeAction(nowBlock, movementBlock);
                            units.Add(uActor);
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

    private IEnumerator ChargeAttackUnits(TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {
        SetStep("ChargeAttackUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle && (uActor.typeUnitGroup & typeClass) == uActor.typeUnitGroup)
                {
                    uActor.ActionChargeAttack(gameTestManager);
                    units.Add(uActor);
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

    private IEnumerator GuardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("GuardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    uActor.ActionGuard(gameTestManager);
                    units.Add(uActor);
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

    private IEnumerator CastleAttackUnits(TYPE_TEAM typeTeam)
    {
        SetStep("CastleAttackUnits");

        var fieldBlocks = FieldManager.GetSideBlocks(typeTeam);
        var units = new List<IUnitActor>();

        int defenceCount = 7;
        int unitsCount = fieldBlocks.Length;

        while(defenceCount > 0 && unitsCount > 0)
        {
            var block = fieldBlocks[Random.Range(0, fieldBlocks.Length)];

            for(int j = 0; j < block.unitActors.Length; j++)
            {
                var uActor = block.unitActors[j];
                if (!units.Contains(uActor))
                {
                    var isAttack = uActor.DirectAttack(gameTestManager);

                    if (isAttack)
                    {
                        units.Add(uActor);
                        defenceCount--;
                    }
                }
                unitsCount--;
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

    private IEnumerator ForwardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ForwardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {

            var unitActors = fieldBlocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];                
                var nowBlock = FieldManager.FindActorBlock(uActor);
                if (nowBlock != null)
                {
                    if (uActor.typeTeam == typeTeam)
                    {
                        var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, uActor.typeMovement, uActor.movementValue, typeTeam);

                        if (movementBlock != null)
                        {
                            uActor.ForwardAction(nowBlock, movementBlock);
                            units.Add(uActor);
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

    private IEnumerator BackwardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("BackwardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam, true);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unitActors = fieldBlocks[i].unitActors;
            for (int j = 0; j < unitActors.Length; j++)
            {
                var uActor = unitActors[j];
                if (units.Contains(uActor)) uActor = null;

                if (uActor != null)
                {
                    var nowBlock = FieldManager.FindActorBlock(uActor);
                    if (nowBlock != null)
                    {
                        if (uActor.typeTeam == typeTeam)
                        {
                            var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, uActor.typeMovement, uActor.movementValue, (uActor.typeTeam == TYPE_TEAM.Left) ? TYPE_TEAM.Right : TYPE_TEAM.Left);

                            if (movementBlock != null)
                            {
                                uActor.BackwardAction(nowBlock, movementBlock);
                                units.Add(uActor);
                                //yield return null;
                            }
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



    private IEnumerator DeadUnits(TYPE_TEAM typeTeam)
    {
        SetStep("DeadUnits");

        var deadList = new List<IUnitActor>();

        var blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].IsHasUnitActor())
            {
                var unitActors = blocks[i].unitActors;
                for (int j = 0; j < unitActors.Length; j++)
                {
                    var uActor = unitActors[j];
                    if (uActor.IsDead())
                    {
                        deadList.Add(uActor);
                        blocks[i].LeaveUnitActor(uActor);
                    }
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
                _unitActorDic.Remove(arr[i].uKey);
                arr[i].Destroy();
            }

            yield return new WaitForSeconds(BattleFieldSettings.FRAME_END_TIME);
        }
        yield return null;
    }


    private void SetStep(string step)
    {
#if UNITY_EDITOR
        nowStep = step;
#endif
    }

    #endregion



    private void RemoveUnitActor(IUnitActor uActor)
    {
        _unitActorDic.Remove(uActor.uKey);
        uActor.Destroy();
    }

    public void ClearAllUnits(bool isIncludeCastle = false)
    {
        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].IsHasUnitActor())
            {
                var unitActors = blocks[i].unitActors;
                for (int j = 0; j < unitActors.Length; j++)
                {
                    var uActor = unitActors[j];
                    if (isIncludeCastle)
                    {
                        RemoveUnitActor(uActor);
                        blocks[i].LeaveUnitActor(uActor);
                    }
                    else
                    {
                        if (uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                        {
                            RemoveUnitActor(uActor);
                            blocks[i].LeaveUnitActor(uActor);
                        }
                    }
                }
            }
        }
        ClearUnitCards();
    }

    public void ClearUnitCards()
    {
        _usedCardList.Clear();
    }

    public void ClearDeadUnits()
    {
        var deadList = new List<IUnitActor>();

        var blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].IsHasUnitActor())
            {


                var unitActors = blocks[i].unitActors;
                for (int j = 0; j < unitActors.Length; j++)
                {
                    var uActor = unitActors[j];
                    if (uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                    {
                        deadList.Add(uActor);
                        blocks[i].LeaveUnitActor(uActor);
                    }
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



    //public void RetreatUnits(CommanderActor cActor)
    //{
        //for(int i = 0; i < unitActorList.Count; i++)
        //{
        //    var unitActor = unitActorList[i];

        //}
    //}
}
