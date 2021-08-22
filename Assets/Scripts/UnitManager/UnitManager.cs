using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public interface IUnitKey
{
    bool SetKey(int key);
}


public class UnitKey
{
    static List<int> uKeyList = new List<int>();
    static System.Random rand;
    static int nowKey = 0;

    public static bool Contains(int key)
    {
        return uKeyList.Contains(key);
    }

    /// <summary>
    /// 키를 삽입합니다
    /// 삽입에 성공하면 양수
    /// 실패하면 -1
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static int InsertKey(IUnitKey unit)
    {
        var key = nowKey++;
        if (!Contains(key))
        {
            if (unit.SetKey(key))
            {
                SetKey(key);
                return key;
            }
        }
        Debug.Log("InsertKey is Count Over");
        return -1;
    }

    public static void RemoveKey(int key)
    {
        uKeyList.Remove(key);
    }

    public static void SetKey(int key)
    {
        //Debug.Log("Key " + key);
        uKeyList.Add(key);
    }
}

public class UnitManager : MonoBehaviour
{

    [SerializeField]
    GameManager gameTestManager;

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


#if UNITY_EDITOR
    public string nowStep { get; private set; }

#endif


    #region ##### DragAndDrop Actor #####

    private class DragBlock
    {
        public UnitActor unitActor;
        public FieldBlock fieldBlock;
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

    Dictionary<int, UnitActor> unitActorDic = new Dictionary<int, UnitActor>();

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
    public void CreateCastleUnit(FieldManager fieldManager, TYPE_TEAM typeTeam)
    {
        var sideBlocks = fieldManager.GetSideBlocks(typeTeam);
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
    public void CreateUnit(UnitCard uCard, int uKey, FieldBlock fieldBlock, TYPE_TEAM typeTeam)
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
            uActor.gameObject.SetActive(true);

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
        var blocks = _fieldManager.GetAllBlocks();
        for(int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i].unitActor != null)
            {
                blocks[i].unitActor.RemoveSkill(caster);
            }
        }
    }
    
    private void SetState(UnitActor uActor, ICaster caster, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        uActor.SetSkill(caster, caster.skills, typeSkillActivate);
    }

    public bool IsUsedCard(UnitCard uCard)
    {
        return _usedCardList.Contains(uCard);
    }

    public void CreateUnits(UnitCard uCard, int[] uKeys, FieldBlock[] blocks, TYPE_TEAM typeTeam)
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
            DestroyImmediate(dragBlock.unitActor.gameObject);
        }
    }

    Dictionary<int, FieldBlock> cancelDic = new Dictionary<int, FieldBlock>();

    public void DragUnitActor(UnitActor uActor)
    {
        cancelDic.Clear();

        _dragActors.isChanged = true;
        _dragActors.typeTeam = uActor.typeTeam;
        _dragActors.uCard = uActor.unitCard;

        var uCard = uActor.unitCard;
        _usedCardList.Remove(uCard);

        for (int i = 0; i < uCard.unitKeys.Length; i++)
        {
            var uKey = uCard.unitKeys[i];
            if (unitActorDic.ContainsKey(uKey))
            {
                var actor = unitActorDic[uKey];
                _dragActors.Add(new DragBlock
                {
                    unitActor = actor,
                    formation = uCard.formationCells[i],
                });
                unitActorDic.Remove(uKey);

                var fieldBlock = _fieldManager.FindActorBlock(actor);
                fieldBlock.ResetUnitActor();

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

    public void ReturnUnitActor(UnitActor uActor)
    {
        var uCard = uActor.unitCard;
        for (int i = 0; i < uCard.unitKeys.Length; i++)
        {
            var uKey = uCard.unitKeys[i];
            if (unitActorDic.ContainsKey(uKey))
            {
                var actor = unitActorDic[uKey];
                unitActorDic.Remove(uKey);

                var fieldBlock = _fieldManager.FindActorBlock(actor);
                fieldBlock.ResetUnitActor();

                DestroyImmediate(actor.gameObject);
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
                var block = hits[i].collider.GetComponent<FieldBlock>();

                if (block != null && _fieldManager.IsTeamUnitBlock(block, _dragActors.typeTeam))
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
            for (int i = 0; i < _dragActors.dragBlocks.Count; i++)
            {
                var block = _dragActors.dragBlocks[i];
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
            for (int i = 0; i < _dragActors.dragBlocks.Count; i++)
            {
                var block = _dragActors.dragBlocks[i];
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
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, DeadUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, ForwardUnits(fieldManager, typeTeam));
                yield return new UnitManagerAction(this, AttackUnits(fieldManager, typeTeam));//, TYPE_UNIT_GROUP.FootSoldier | TYPE_UNIT_GROUP.Charger | TYPE_UNIT_GROUP.Supporter));
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



    public IEnumerator PreActiveActionUnits(FieldManager fieldManager, CommanderActor lcActor, CommanderActor rcActor)
    {

        var dic = new Dictionary<ICaster, Dictionary<SkillData, List<FieldBlock>>>();
        var blocks = fieldManager.GetAllBlocks();

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

    private void SetStatePreActive(Dictionary<ICaster, Dictionary<SkillData, List<FieldBlock>>> dic)
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

    private FieldBlock[] GatheringPreActive(UnitActor uActor, ICaster caster, SkillData skillData)
    {
        return uActor.GatheringStatePreActive(_fieldManager, caster, skillData, caster.typeTeam);
    }

    private Dictionary<ICaster, Dictionary<SkillData, List<FieldBlock>>> GetheringPreActiveBlocks(UnitActor targetUnitActor, ICaster cActor, Dictionary<ICaster, Dictionary<SkillData, List<FieldBlock>>> dic)
    {
        if(!dic.ContainsKey(cActor))
            dic.Add(cActor, new Dictionary<SkillData, List<FieldBlock>>());

        for (int i = 0; i < cActor.skills.Length; i++)
        {
            var skillData = cActor.skills[i];
            var blocks = GatheringPreActive(targetUnitActor, cActor, skillData);

            if (blocks != null)
            {
                if (!dic[cActor].ContainsKey(skillData))
                    dic[cActor].Add(skillData, new List<FieldBlock>());

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






    #region ##### Action #####

    private IEnumerator AttackUnits(FieldManager fieldManager, TYPE_TEAM typeTeam, TYPE_UNIT_GROUP typeClass = TYPE_UNIT_GROUP.All)
    {
        SetStep("AttackUnits");

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
        SetStep("ChargeReadyUnits");

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
        SetStep("ChargeUnits");

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
        SetStep("ChargeAttackUnits");

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
        SetStep("GuardUnits");

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
        SetStep("CastleAttackUnits");

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
        SetStep("ForwardUnits");

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
        SetStep("BackwardUnits");

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
        SetStep("DeadUnits");

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
                unitActorDic.Remove(arr[i].uKey);
//                unitActorList.Remove(arr[i]);
                DestroyImmediate(arr[i].gameObject);
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



    private void RemoveUnitActor(UnitActor uActor)
    {
        unitActorDic.Remove(uActor.uKey);
        //unitActorList.Remove(unitActor);
        DestroyImmediate(uActor.gameObject);
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
        ClearUnitCards();
    }

    public void ClearUnitCards()
    {
        _usedCardList.Clear();
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



    //public void RetreatUnits(CommanderActor cActor)
    //{
        //for(int i = 0; i < unitActorList.Count; i++)
        //{
        //    var unitActor = unitActorList[i];

        //}
    //}
}
