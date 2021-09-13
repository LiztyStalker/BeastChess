using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class UnitManager : MonoBehaviour
{

    [SerializeField]
    BattleFieldManager gameTestManager;

    [SerializeField]
    UnitActor _unitActor;

    [SerializeField]
    UIBar _uiBar;

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


    List<UnitCard> _usedCardList = new List<UnitCard>();

    DragActor _dragActors = new DragActor();

    //List<UnitActor> unitActorList = new List<UnitActor>();

    Dictionary<int, IUnitActor> unitActorDic = new Dictionary<int, IUnitActor>();

    private bool isRunningL = false;
    private bool isRunningR = false;

    public bool isRunning => isRunningL || isRunningR;

    public bool IsDrag()
    {
        return !_dragActors.IsEmpty();
    }
         
    public UnitCard[] GetUnitCards(params string[] names)
    {
        return DataStorage.Instance.GetUnitCards(names);
    }
    
    public UnitCard[] GetRandomUnitCards(int count, bool isTest = false)
    {
        return DataStorage.Instance.GetRandomUnitCards(count, isTest);
    }

    /// <summary>
    /// 성 유닛 배치
    /// </summary>
    /// <param name="fieldManager"></param>
    /// <param name="typeTeam"></param>
    public void CreateCastleUnit(TYPE_TEAM typeTeam)
    {
        var sideBlocks = FieldManager.GetSideBlocks(typeTeam);
        var castleData = DataStorage.Instance.GetCastleUnit();
    
        for (int i = 0; i < sideBlocks.Length; i++)
        {
            var uCard = new UnitCard(castleData);
            var uKey = uCard.unitKeys[0];
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
    public void CreateUnit(UnitCard uCard, int uKey, IFieldBlock fieldBlock, TYPE_TEAM typeTeam)
    {
        var uActor = Instantiate(_unitActor);
        uActor.gameObject.SetActive(true);

        uActor.SetTypeTeam(typeTeam);
        uActor.SetData(uCard);
        uActor.SetKey(uKey);
        uActor.SetOnDeadListener(DeadUnitEvent);

        uActor.AddBar(Instantiate(_uiBar));

        unitActorDic.Add(uActor.uKey, uActor);
//        unitActorList.Add(uActor);

        fieldBlock.SetUnitActor(uActor);
        uActor.SetLayer();
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
            uActor.AddBar(Instantiate(_uiBar));
            uActor.SetTypeTeam(_dragActors.typeTeam);
            uActor.SetActive(true);

            if (caster != null)
            {
                //아군 지휘관만 패시브 가능
                SetState(uActor, caster, TYPE_SKILL_ACTIVATE.Passive);
            }

            unitActorDic.Add(uActor.uKey, uActor);
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
            if(blocks[i].unitActor != null)
            {
                blocks[i].unitActor.RemoveStatusData(caster);
            }
        }
    }
    
    private void SetState(IUnitActor uActor, ICaster caster, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        uActor.SetSkill(caster, caster.skills, typeSkillActivate);
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

    Dictionary<int, IFieldBlock> cancelDic = new Dictionary<int, IFieldBlock>();

    public void DragUnitActor(IUnitActor unitActor)
    {
        cancelDic.Clear();

        _dragActors.isChanged = true;
        _dragActors.typeTeam = unitActor.typeTeam;
        _dragActors.uCard = unitActor.unitCard;

        var uCard = unitActor.unitCard;
        _usedCardList.Remove(uCard);

        for (int i = 0; i < uCard.unitKeys.Length; i++)
        {
            var uKey = uCard.unitKeys[i];
            if (unitActorDic.ContainsKey(uKey))
            {
                var uActor = unitActorDic[uKey];
                _dragActors.Add(new DragBlock
                {
                    unitActor = uActor,
                    formation = uCard.formationCells[i],
                });
                unitActorDic.Remove(uKey);

                var fieldBlock = FieldManager.FindActorBlock(uActor);
                fieldBlock.LeaveUnitActor(uActor);

                cancelDic.Add(uKey, fieldBlock);
            }
        }
    }

    public void DragUnitActor(UnitCard uCard, TYPE_TEAM dropTeam)
    {
        _dragActors.isChanged = false;
        _dragActors.typeTeam = dropTeam;
        _dragActors.uCard = uCard;

        for (int i = 0; i < uCard.unitKeys.Length; i++)
        {
            var uKey = uCard.unitKeys[i];

            if (i < uCard.formationCells.Length)
            {
                if (!uCard.IsDead(uKey))
                {
                    var uActor = Instantiate(_unitActor);
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
        for (int i = 0; i < uCard.unitKeys.Length; i++)
        {
            var uKey = uCard.unitKeys[i];
            if (unitActorDic.ContainsKey(uKey))
            {
                var uActor = unitActorDic[uKey];
                unitActorDic.Remove(uKey);

                var fieldBlock = FieldManager.FindActorBlock(uActor);
                fieldBlock.LeaveUnitActor(uActor);

                uActor.Destroy();
            }
        }
        _usedCardList.Remove(uCard);
    }

    public void CancelChangeUnitActor()
    {
        ClearCellColor();
        while (!_dragActors.IsEmpty())
        {
            var dragBlock = _dragActors.PopBlock();

            if (dragBlock == null) return;

            var uActor = dragBlock.unitActor;

            if (cancelDic.ContainsKey(uActor.uKey))
            {
                cancelDic[uActor.uKey].SetUnitActor(uActor);
                cancelDic.Remove(uActor.uKey);

                unitActorDic.Add(uActor.uKey, uActor);
            }
        }

        _usedCardList.Add(_dragActors.uCard);

        _dragActors.isChanged = false;
        _dragActors.Clear();


        cancelDic.Clear();
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
            DestroyAllDragUnit();
        }
        return false;
    }

    private void Update()
    {
        if(_dragActors.isChanged && Input.GetMouseButtonDown(0))
        {
            DropUnitActor(_dragActors.uCard);
        }

        if (!_dragActors.IsEmpty())
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 100f);

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
                    if (offsetFieldBlock.unitActor == null)
                    {
                        //formation
                        block.fieldBlock = offsetFieldBlock;
                        block.unitActor.SetPosition(offsetFieldBlock.position);
                        MovementCellColor(offsetFieldBlock, block.unitActor.movementCells);
                        //RangeCellColor(offsetFieldBlock, block.unitActor.attackCells, block.unitActor.minRangeValue);
                        RangeCellColor(offsetFieldBlock, _dragActors.uCard.TargetData, _dragActors.typeTeam);
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



    public int IsLivedUnits(TYPE_TEAM typeTeam)
    {
        int cnt = 0;

        foreach(var key in unitActorDic.Keys)
        {
            var uActor = unitActorDic[key];
            if (uActor.typeTeam == typeTeam && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                cnt++;
        }

        //for (int i = 0; i < unitActorList.Count; i++)
        //{
        //    if (unitActorList[i].typeTeam == typeTeam && unitActorList[i].typeUnit != TYPE_UNIT_FORMATION.Castle)
        //        cnt++;
        //}
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


    public IEnumerator ActionUnits(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {           
            isRunningL = true;
        }

        if (typeTeam == TYPE_TEAM.Right)
        {
            isRunningR = true;
        }


        foreach(var value in unitActorDic.Values)
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
                yield return new UnitManagerAction(this, AttackUnits(typeTeam));//, TYPE_UNIT_GROUP.FootSoldier | TYPE_UNIT_GROUP.Charger | TYPE_UNIT_GROUP.Supporter));
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
                    isChargeL = true;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    isChargeR = true;
                }
                yield return new UnitManagerAction(this, ChargeReadyUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, ChargeUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, ChargeAttackUnits(typeTeam));
                if (typeTeam == TYPE_TEAM.Left)
                {
                    isChargeL = false;
                }

                if (typeTeam == TYPE_TEAM.Right)
                {
                    isChargeR = false;
                }
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                
                break;
            case TYPE_BATTLE_TURN.Guard:
                yield return new UnitManagerAction(this, GuardUnits(typeTeam));
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
                yield return new UnitManagerAction(this, AttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                yield return new UnitManagerAction(this, CastleAttackUnits(typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(typeTeam));
                break;
            //case TYPE_BATTLE_TURN.Shoot:
            //    yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
            //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
            //    yield return new UnitManagerAction(this, ForwardUnits(fieldManager, typeTeam));
            //    yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam, TYPE_UNIT_GROUP.Shooter));
            //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
            //    yield return new UnitManagerAction(this, CastleAttackUnits(fieldManager, typeTeam));
            //    yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
            //    break;
        }

        yield return null;


        //Debug.Log(isRunningL + " " + isRunningR);
        if (typeTeam == TYPE_TEAM.Left)
            isRunningL = false;

        if (typeTeam == TYPE_TEAM.Right)
            isRunningR = false;


        foreach(var value in unitActorDic.Values)
        {
            if (value.typeTeam == typeTeam)
            {
                value.SetBattleTurn(TYPE_BATTLE_TURN.None);
            }
        }
        //for (int i = 0; i < unitActorList.Count; i++)
        //{
        //    if (unitActorList[i].typeTeam == typeTeam)
        //    {
        //        unitActorList[i].SetBattleTurn(TYPE_BATTLE_TURN.None);
        //    }
        //}

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

        var dic = new Dictionary<ICaster, Dictionary<SkillData, List<IFieldBlock>>>();
        var blocks = FieldManager.GetAllBlocks();

        //지휘관 스킬 
        //스킬에 적용되는 병사 수집
        for (int i = 0; i < blocks.Length; i++)
        {
            var uActor = blocks[i].unitActor;
            if (uActor != null)
            {
                dic = GetheringPreActiveBlocks(uActor, lcActor, dic);
                dic = GetheringPreActiveBlocks(uActor, rcActor, dic);               
            }
        }

        //지휘관 스킬 적용
        SetStatePreActive(dic);

        //병사 스킬 곧바로 적용
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                dic.Clear();
                dic = GetheringPreActiveBlocks(blocks[i].unitActor, blocks[i].unitActor, dic);
                SetStatePreActive(dic);
            }
        }        

        yield return null;
    }

    public IEnumerator ReleasePreActiveActionUnits()
    {
        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            var uActor = blocks[i].unitActor;
            if(uActor != null)
            {
                uActor.RemoveStatusData();
            }
        }
        yield return null;
    }

    private void SetStatePreActive(Dictionary<ICaster, Dictionary<SkillData, List<IFieldBlock>>> dic)
    {
        foreach(var key in dic.Keys)
        {
            foreach(var skill in dic[key].Keys)
            {
                //Debug.Log("SetStatePreActive Count" + dic[key][skill].Count);
                for(int i = 0; i < dic[key][skill].Count; i++)
                {
                    dic[key][skill][i].unitActor.SetSkill(key, skill, TYPE_SKILL_ACTIVATE.PreActive);
                }
            }
        }
    }

    private IFieldBlock[] GatheringPreActive(IUnitActor uActor, ICaster caster, SkillData skillData)
    {
        return uActor.GatheringStatePreActive(caster, skillData, caster.typeTeam);
    }

    private Dictionary<ICaster, Dictionary<SkillData, List<IFieldBlock>>> GetheringPreActiveBlocks(IUnitActor targetUnitActor, ICaster cActor, Dictionary<ICaster, Dictionary<SkillData, List<IFieldBlock>>> dic)
    {
        if(!dic.ContainsKey(cActor))
            dic.Add(cActor, new Dictionary<SkillData, List<IFieldBlock>>());

        for (int i = 0; i < cActor.skills.Length; i++)
        {
            var skillData = cActor.skills[i];
            var blocks = GatheringPreActive(targetUnitActor, cActor, skillData);

            if (blocks != null)
            {
                if (!dic[cActor].ContainsKey(skillData))
                    dic[cActor].Add(skillData, new List<IFieldBlock>());

                for (int j = 0; j < blocks.Length; j++)
                {
                    if (!dic[cActor][skillData].Contains(blocks[j]))
                    {
                        dic[cActor][skillData].Add(blocks[j]);
                    }
                }
            }

            //uActor.SetStatePreActive(_fieldManager, cActor, skillData);
        }
        return dic;
    }

    public static void CastSkills(ICaster caster, TYPE_TEAM typeTeam)
    {
        for (int i = 0; i < caster.skills.Length; i++)
            CastSkill(caster, caster.skills[i], typeTeam);
    }

    public static void CastSkill(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    {
        var blocks = FieldManager.GetTargetBlocks(caster, skillData.TargetData, typeTeam);
        for(int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i].unitActor != null) blocks[i].unitActor.SetSkill(caster, skillData, skillData.typeSkillActivate);
        }
    }

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
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle && (unit.typeUnitGroup & typeClass) == unit.typeUnitGroup)
                {
                    unit.ActionAttack(gameTestManager);
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

    private IEnumerator ChargeReadyUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ChargeReadyUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    unit.ActionChargeReady(gameTestManager);
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
    
    private IEnumerator ChargeUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ChargeUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                var nowBlock = FieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, unit.chargeCells, typeTeam, unit.typeMovement);

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

    private IEnumerator ChargeAttackUnits(TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {
        SetStep("ChargeAttackUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle && (unit.typeUnitGroup & typeClass) == unit.typeUnitGroup)
                {
                    unit.ActionChargeAttack(gameTestManager);
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

    private IEnumerator GuardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("GuardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (unit != null)
            {
                if (unit.typeTeam == typeTeam && unit.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    unit.ActionGuard(gameTestManager);
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

            if (!units.Contains(block.unitActor))
            {
                var isAttack = block.unitActor.DirectAttack(gameTestManager);

                if (isAttack)
                {
                    units.Add(block.unitActor);
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

    private IEnumerator ForwardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("ForwardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;
            if (unit != null)
            {
                var nowBlock = FieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, typeTeam, unit.typeMovement);

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

    private IEnumerator BackwardUnits(TYPE_TEAM typeTeam)
    {
        SetStep("BackwardUnits");

        var fieldBlocks = FieldManager.GetAllBlocks(typeTeam, true);

        List<IUnitActor> units = new List<IUnitActor>();
        for (int i = 0; i < fieldBlocks.Length; i++)
        {
            var unit = fieldBlocks[i].unitActor;

            if (units.Contains(unit)) unit = null;

            if (unit != null)
            {
                var nowBlock = FieldManager.FindActorBlock(unit);
                if (nowBlock != null)
                {
                    if (unit.typeTeam == typeTeam)
                    {
                        var movementBlock = FieldManager.GetMovementBlock(nowBlock.coordinate, unit.movementCells, (unit.typeTeam == TYPE_TEAM.Left) ? TYPE_TEAM.Right : TYPE_TEAM.Left, unit.typeMovement);

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



    private IEnumerator DeadUnits(TYPE_TEAM typeTeam)
    {
        SetStep("DeadUnits");

        var deadList = new List<IUnitActor>();

        var blocks = FieldManager.GetAllBlocks();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                if (blocks[i].unitActor.IsDead())
                {
                    var deadUnitActor = blocks[i].unitActor;
                    deadList.Add(deadUnitActor);
                    blocks[i].LeaveUnitActor(deadUnitActor);
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
                unitActorDic.Remove(arr[i].uKey);
                arr[i].Destroy();
            }

            yield return new WaitForSeconds(Settings.FRAME_END_TIME);
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
        unitActorDic.Remove(uActor.uKey);
        uActor.Destroy();
    }

    public void ClearAllUnits()
    {
        var blocks = FieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].unitActor != null)
            {
                if(blocks[i].unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                    RemoveUnitActor(blocks[i].unitActor);
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
            if (blocks[i].unitActor != null)
            {
                if (blocks[i].unitActor.typeUnit != TYPE_UNIT_FORMATION.Castle)
                {
                    var deadUnitActor = blocks[i].unitActor;
                    deadList.Add(deadUnitActor);
                    blocks[i].LeaveUnitActor(deadUnitActor);
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
