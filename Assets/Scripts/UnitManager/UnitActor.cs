using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_TEAM {None = -1, Left, Right}

public class UnitActor : MonoBehaviour, ICaster
{
    [SerializeField]
    private UIBar _uiBar;

    [SerializeField]
    private SkeletonAnimation _sAnimation;

    private StatusActor _skillActor = new StatusActor();

    private CommanderActor _commanderActor { get; set; }

    private Spine.Skeleton _skeleton { get; set; }

    public int uKey { get; private set; }

    public void SetKey(int key) => uKey = key;

    private UnitCard _uCard { get; set; }

    //    public int healthValue => _uCard.healthValue;

    public UnitCard unitCard => _uCard;

    public float HealthRate() => _uCard.HealthRate(uKey); //_uCard.totalNowHealthValue / _uCard.totalMaxHealthValue; //(float)_nowHealthValue / (float)healthValue;

    public bool IsDead() => _uCard.IsDead(uKey); // _nowHealthValue == 0;

    public int damageValue => _skillActor.GetValue<StatusValueAttack>(_uCard.damageValue);

    public int attackCount => _uCard.attackCount;

    public TYPE_TEAM typeTeam { get; private set; }

    //private int _nowHealthValue;

    public int minRangeValue => _uCard.minRangeValue;

    private int _employCostValue => _uCard.employCostValue;

    public int priorityValue => _uCard.priorityValue;// + _commanderActor.GetBonusCommanderMaster(typeUnitGroup);

    public TYPE_UNIT_FORMATION typeUnit => _uCard.typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _uCard.typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _uCard.typeUnitClass;

    public TYPE_UNIT_ATTACK typeUnitAttack => _uCard.typeUnitAttack;

    private TYPE_BATTLE_TURN typeBattleTurn { get; set; }

    public TYPE_MOVEMENT typeMovement => _uCard.typeMovement;

    public Vector2Int[] attackCells => _uCard.attackCells;
    public Vector2Int[] movementCells => _uCard.movementCells;
    public Vector2Int[] chargeCells => _uCard.chargeCells;

    public SkillData[] skills => _uCard.skills;

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        this.typeTeam = typeTeam;

        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                transform.localScale = Vector3.one;
                break;
            case TYPE_TEAM.Right:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;
        }
    }

    public void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn)
    {
        this.typeBattleTurn = typeBattleTurn;
    }
    
    public void SetData(UnitCard uCard)
    {
        _uCard = uCard;

        if (_uCard.skeletonDataAsset != null)
        {
            _sAnimation.skeletonDataAsset = _uCard.skeletonDataAsset;
            _sAnimation.Initialize(false);
            _skeleton = _sAnimation.skeleton;
            _skeleton.SetSlotsToSetupPose();
            //_animationState = _sAnimation.state;
            _sAnimation.AnimationState.Event += AttackEvent;
            _sAnimation.AnimationState.SetEmptyAnimation(0, 0f);
            DefaultAnimation(true);

            SetColor((typeTeam == TYPE_TEAM.Left) ? Color.blue : Color.red);
        }

        //_nowHealthValue = healthValue;
    }

    public void SetLayer()
    {
        if (_sAnimation != null)
        {
            _sAnimation.GetComponent<MeshRenderer>().sortingOrder = -(int)transform.position.y;
            //Debug.Log("Layer " + _sAnimation.GetComponent<MeshRenderer>().sortingOrder);
        }
    }

    public void AddBar(UIBar uiBar)
    {
        _uiBar = uiBar;
        _uiBar.transform.SetParent(transform);
        _uiBar.transform.localPosition = Vector3.up * 1f;
        _uiBar.gameObject.SetActive(_uCard.typeUnit != TYPE_UNIT_FORMATION.Castle);
        _uiBar.SetBar(HealthRate());
    }

    private Color GetTeamColor(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return Color.blue;
            case TYPE_TEAM.Right:
                return Color.red;
        }
        return Color.black;
    }

    private void SetColor(Color color)
    {
        try
        {
            _skeleton.FindSlot("LBand").SetColor(color);
            _skeleton.FindSlot("RBand").SetColor(color);
        }
        catch
        {

        }
    }

    public FieldBlock[] GatheringStatePreActive(FieldManager fieldManager, ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    {
        switch (skillData.typeSkillRange)
        {
            //완료
            case TYPE_SKILL_RANGE.All:
                return fieldManager.GetBlocksOnUnitActor(skillData.typeTargetTeam, typeTeam);
            case TYPE_SKILL_RANGE.MyselfRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        return fieldManager.GetBlocksOnUnitActor(nowBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                    }
                }
                break;
            case TYPE_SKILL_RANGE.UnitClassRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitClass, skillData.typeTargetTeam, typeTeam);
                        if (fieldBlock != null)
                        {
                            return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.UnitGroupRange:
                {
                    //자신을 찾기
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {

                        var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitGroup, skillData.typeTargetTeam, typeTeam);
                        if (fieldBlock != null)
                        {
                            return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.AscendingPriorityRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, true);
                        if (fieldBlock != null)
                        {
                            return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.DecendingPriorityRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, false);
                        if (fieldBlock != null)
                        {
                            return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                        }
                    }
                }
                break;
        }
        return null;
    }


    public void SetStatePreActive(FieldManager fieldManager, ICaster caster, SkillData skillData)
    {
        switch (skillData.typeSkillRange)
        {
            //완료
            case TYPE_SKILL_RANGE.All:
                {
                    var blocks = fieldManager.GetBlocksOnUnitActor(skillData.typeTargetTeam, typeTeam);
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                    }
                }
                break;
            case TYPE_SKILL_RANGE.MyselfRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var blocks = fieldManager.GetBlocksOnUnitActor(nowBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                        for (int i = 0; i < blocks.Length; i++)
                        {
                            blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.UnitClassRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitClass, skillData.typeTargetTeam, typeTeam);
                        if (fieldBlock != null)
                        {
                            var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                            for (int i = 0; i < blocks.Length; i++)
                            {
                                blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                            }
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.UnitGroupRange:
                {
                    //자신을 찾기
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {

                        var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitGroup, skillData.typeTargetTeam, typeTeam);
                        if (fieldBlock != null)
                        {
                            var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                            for (int i = 0; i < blocks.Length; i++)
                            {
                                //Debug.Log("SetState " + blocks[i].coordinate + " | " + blocks[i].unitActor.name);
                                blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                            }
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.AscendingPriorityRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, true);
                        if (fieldBlock != null)
                        {
                            var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                            for (int i = 0; i < blocks.Length; i++)
                            {
                                blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                            }
                        }
                    }
                }
                break;
            case TYPE_SKILL_RANGE.DecendingPriorityRange:
                {
                    var nowBlock = fieldManager.FindActorBlock(this);
                    if (nowBlock != null)
                    {
                        var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, false);
                        if (fieldBlock != null)
                        {
                            var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
                            for (int i = 0; i < blocks.Length; i++)
                            {
                                blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
                            }
                        }
                    }
                }
                break;
        }
    }

    public void SetStatePreActive(FieldManager fieldManager)
    {
        for(int i = 0; i < skills.Length; i++)
        {
            if(skills[i].typeSkillActivate == TYPE_SKILL_ACTIVATE.PreActive)
            {
                var skillData = skills[i];
                SetStatePreActive(fieldManager, this, skillData);
            }
        }
    }
    
    public void SetSkill(ICaster caster, SkillData skillData, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        if (skillData.typeSkillActivate == typeSkillActivate)
            _skillActor.Add(caster, skillData);

        _skillActor.ShowSkill(_uiBar);
    }

    public void SetSkill(ICaster caster, SkillData[] skills, TYPE_SKILL_ACTIVATE typeSkillActivate)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].typeSkillActivate == typeSkillActivate)
            {
                _skillActor.Add(caster, skills[i]);
            }
        }
        _skillActor.ShowSkill(_uiBar);
    }

    public void RemovePreActiveSkill()
    {
        if (!IsDead())
        {
            _skillActor.RemovePreActiveSkill(_uiBar);
        }
    }

    public void RemoveSkill(ICaster caster)
    {
        if (!IsDead())
        {
            _skillActor.Remove(caster, _uiBar);
        }
    }

    int counterAttackRate = 1;

    public void IncreaseHealth(UnitActor attackActor, int value, int additiveRate = 1)
    {

        if (Settings.Invincible) return;


        var attackValue = value;
        if (UnitData.IsAttackUnitClassOpposition(typeUnitClass, attackActor.typeUnitClass))
            attackValue = value * 2;

        if (UnitData.IsDefenceUnitClassOpposition(typeUnitClass, attackActor.typeUnitClass))
            attackValue = value / 2;

        switch (typeBattleTurn)
        {
            case TYPE_BATTLE_TURN.Guard:
                if (attackActor.typeBattleTurn == TYPE_BATTLE_TURN.Forward)
                {
                    attackValue *= 2;
                }
                else
                {
                    attackValue /= 2;
                    counterAttackRate = additiveRate;
                }
                break;
            case TYPE_BATTLE_TURN.Backward:
                if (attackActor.typeBattleTurn == TYPE_BATTLE_TURN.Forward)
                {
                    attackValue /= 2;
                }
                else if (attackActor.typeBattleTurn == TYPE_BATTLE_TURN.Charge)
                {
                    attackValue *= additiveRate;
                }
                else if (attackActor.typeUnitGroup == TYPE_UNIT_GROUP.Shooter)
                {
                    attackValue *= 2;
                }
                else
                {
                    attackValue *= additiveRate;
                }
                break;
            default:
                attackValue *= additiveRate;
                break;
        }

        //Debug.Log(attackActor.unitCard.name + " " + attackValue);

        _uCard.DecreaseHealth(uKey, attackValue);
        if (_uCard.IsDead(uKey))
        {
            SetAnimation("Dead", false);
            GameObject game = new GameObject();
            var audio = game.AddComponent<AudioSource>();
            audio.PlayOneShot(_uCard.deadClip);

            _uCard.SetUnitLiveType(uKey);

            _deadEvent?.Invoke(this);
            //
        }


        _uiBar.SetBar(HealthRate());
    }

    private bool IsHasAnimation(string name)
    {
        return (_sAnimation.skeleton.Data.FindAnimation(name) != null);

    }

    private void SetAnimation(string name, bool loop)
    {
        if (_sAnimation.SkeletonDataAsset != null)
        {
            var track = _sAnimation.AnimationState.SetAnimation(0, name, loop);
            track.TimeScale = Random.Range(0.5f + 0.5f * Mathf.Clamp01(((float)_uCard.proficiencyValue / 100f)), 1.5f - 0.5f * Mathf.Clamp01(((float)_uCard.proficiencyValue / 100f))) * (float)attackCount;
        }
    }

    public void Dead()
    {
        SetAnimation("Dead", false);
    }

    public void Turn()
    {
        _skillActor.Turn(_uiBar);
    }


    private FieldBlock[] SetAttackBlocks()
    {
        attackBlocks = new FieldBlock[1];

        switch (typeUnitAttack)
        {
            case TYPE_UNIT_ATTACK.Normal:
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].unitActor != null && !blocks[i].unitActor.IsDead())
                    {
                        attackBlocks[0] = blocks[i];
                        break;
                    }
                    else if (blocks[i].castleActor != null)
                    {
                        attackBlocks[0] = blocks[i];
                        break;
                    }
                }
                break;
            case TYPE_UNIT_ATTACK.RandomRange:

                List<int> shupple = new List<int>();
                for (int i = 0; i < blocks.Length; i++)
                {
                    shupple.Add(i);
                }

                for (int i = 0; i < 10; i++)
                {
                    var index = shupple[Random.Range(0, shupple.Count)];
                    var value = shupple[index];
                    shupple.RemoveAt(index);
                    shupple.Add(value);
                }

                for (int i = 0; i < shupple.Count; i++)
                {
                    if (blocks[shupple[i]].unitActor != null && !blocks[shupple[i]].unitActor.IsDead())
                    {
                        attackBlocks[0] = blocks[shupple[i]];
                        break;
                    }
                    else if (blocks[shupple[i]].castleActor != null)
                    {
                        attackBlocks[0] = blocks[shupple[i]];
                        break;
                    }
                }

                break;
            case TYPE_UNIT_ATTACK.Range:
                attackBlocks = blocks;
                break;
            case TYPE_UNIT_ATTACK.Priority:
                List<FieldBlock> list = new List<FieldBlock>();
                if (blocks != null)
                {
                    list.AddRange(blocks);
                    list.Sort((a1, a2) =>
                    {
                        if (a2.unitActor != null && a1.unitActor != null)
                            return a2.unitActor.priorityValue - a1.unitActor.priorityValue;
                        return 0;
                    });

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].castleActor != null)
                        {
                            attackBlocks[0] = list[i];
                            break;
                        }
                        else if (list[i].unitActor != null && !list[i].unitActor.IsDead())
                        {
                            attackBlocks[0] = list[i];
                            break;
                        }
                    }
                }
                break;
        }
        return attackBlocks;

    }

    public bool DirectAttack(FieldManager fieldManager, GameManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;

        var nowBlock = fieldManager.FindActorBlock(this);

        blocks = fieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

        if (blocks.Length > 0)
        {
            attackBlocks = SetAttackBlocks();
            _unitAction.SetUnitAction(this, CastleAttack(), null);
            return true;
        }
        return false;
    }




    public class UnitAction : CustomYieldInstruction
    {
        bool _isRunning = false;

        public bool isRunning { get { return _isRunning; } set { _isRunning = value; /*Debug.Log("Set IsRunning" + _isRunning);*/ } }

        public override bool keepWaiting
        {
            get
            {
                Debug.Log("IsRunning " + isRunning);
                return isRunning;
            }
        }

        IEnumerator enumerator1;
        IEnumerator enumerator2;
        Coroutine coroutine;
        MonoBehaviour mono;

        public void SetUnitAction(MonoBehaviour mono, IEnumerator enumerator1, IEnumerator enumerator2 = null)
        {
            this.enumerator1 = enumerator1;
            this.enumerator2 = enumerator2;
            this.mono = mono;
            coroutine = mono.StartCoroutine(ActionCoroutine());
        }

        private IEnumerator ActionCoroutine()
        {
            isRunning = true;
            yield return mono.StartCoroutine(enumerator1);
            if (enumerator2 != null)
                yield return mono.StartCoroutine(enumerator2);
            isRunning = false;
        }
    }







    int _nowAttackCount;
    FieldBlock[] attackBlocks;
    FieldBlock[] blocks;
    GameManager gameTestManager;


    public bool isRunning => _unitAction.isRunning && !IsDead();

    UnitAction _unitAction = new UnitAction();


    private IEnumerator ActionAttackCoroutine(FieldManager fieldManager, GameManager gameTestManager)
    {
        if (IsHasAnimation("Attack"))
        {
            var nowBlock = fieldManager.FindActorBlock(this);
            //공격방위
            blocks = fieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (blocks.Length > 0)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].unitActor != null && blocks[i].unitActor.typeTeam != typeTeam && !blocks[i].unitActor.IsDead())
                    {

                        //if (IsHasAnimation("Attack"))
                            SetAnimation("Attack", false);

                        _nowAttackCount = attackCount;
                        yield break;
                    }
                    else if (blocks[i].castleActor != null && blocks[i].castleActor.typeTeam != typeTeam)
                    {
                        //if (IsHasAnimation("Attack"))
                            SetAnimation("Attack", false);

                        _nowAttackCount = attackCount;
                        yield break;
                    }
                }
            }
        }
        _unitAction.isRunning = false;
        yield break;
    }



    public void AttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        attackBlocks = SetAttackBlocks();
        Attack();
        AttackCounting();
    }

    private void AttackCounting()
    {
        _nowAttackCount--;

        if (_nowAttackCount > 0)
        {
            _unitAction.isRunning = true;
            if (IsHasAnimation("Attack"))
                SetAnimation("Attack", false);
        }
        else
        {
            _unitAction.isRunning = false;
            DefaultAnimation(false);
        }
    }

    private void Attack()
    {
        for (int index = 0; index < attackBlocks.Length; index++)
        {
            var attackBlock = attackBlocks[index];
            //공격 애니메이션

            //횟수만큼 공격
            if (attackBlock != null)
            {
                if (attackBlock.unitActor != null || attackBlock.castleActor != null)
                {
                    //탄환이 없으면
                    if (_uCard.bullet == null)
                    {
                        if (attackBlock.castleActor != null)
                        {
                            GameManager.IncreaseHealth(damageValue, attackBlock.castleActor.typeTeam);
                        }
                        else
                        {
                            if (typeBattleTurn == TYPE_BATTLE_TURN.Charge)
                            {
                                attackBlock.unitActor.IncreaseHealth(this, damageValue, chargeRange);
                                chargeRange = 1;
                            }
                            else if(typeBattleTurn == TYPE_BATTLE_TURN.Guard)
                            {
                                //Debug.Log("Guard " + counterAttackRate + typeBattleTurn);
                                attackBlock.unitActor.IncreaseHealth(this, damageValue, counterAttackRate);
                                counterAttackRate = 1;
                            }
                            else
                            {
                                attackBlock.unitActor.IncreaseHealth(this, damageValue);
                            }
                        }
                    }
                    //탄환이 있으면
                    else
                    {
                        AttackBullet(attackBlock);
                    }
                    GameObject game = new GameObject();
                    var audio = game.AddComponent<AudioSource>();
                    audio.PlayOneShot(_uCard.attackClip);
                }
            }
        }
    }

    private void AttackBullet(FieldBlock attackBlock)
    {
        var bullet = new GameObject();
        var actor = bullet.AddComponent<BulletActor>();
        var renderer = bullet.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "Unit";
        renderer.sortingOrder = (int)-transform.position.y - 5;
        bullet.AddComponent<Rigidbody2D>();

        renderer.sprite = _uCard.bullet;
        actor.SetData(this, attackBlock, 1f);
        actor.transform.position = transform.position;
        actor.gameObject.SetActive(true);
        //탄환 알고리즘에 의해 날아가서 데미지를 가하도록 하기
    }



    private IEnumerator ActionGuardCoroutine(FieldManager fieldManager, GameManager gameTestManager)
    {
        if(IsHasAnimation("Guard"))
            SetAnimation("Guard", false);
        else
            DefaultAnimation(false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeAttackCoroutine(FieldManager fieldManager, GameManager gameTestManager)
    {
        if (IsHasAnimation("Charge_Attack") || IsHasAnimation("Attack"))
        {
            var nowBlock = fieldManager.FindActorBlock(this);
            //공격방위
            blocks = fieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (blocks.Length > 0)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].unitActor != null && blocks[i].unitActor.typeTeam != typeTeam && !blocks[i].unitActor.IsDead())
                    {
                        if (IsHasAnimation("Charge_Attack"))
                            SetAnimation("Charge_Attack", false);
                        else if (IsHasAnimation("Attack"))
                            SetAnimation("Attack", false);

                        _nowAttackCount = attackCount;
                        yield break;
                    }
                    else if (blocks[i].castleActor != null && blocks[i].castleActor.typeTeam != typeTeam)
                    {
                        if (IsHasAnimation("Charge_Attack"))
                            SetAnimation("Charge_Attack", false);
                        else if (IsHasAnimation("Attack"))
                            SetAnimation("Attack", false);

                        _nowAttackCount = attackCount;
                        yield break;
                    }
                }

                DefaultAnimation(false);

            }
        }
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeCoroutine(FieldManager fieldManager, GameManager gameTestManager)
    {
        if (IsHasAnimation("Charge"))
            SetAnimation("Charge", false);
        else if (IsHasAnimation("Forward"))
            SetAnimation("Forward", false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }
    private IEnumerator ActionChargeReadyCoroutine(FieldManager fieldManager, GameManager gameTestManager)
    {

        if (IsHasAnimation("Charge_Ready"))
            SetAnimation("Charge_Ready", false);
        else
            DefaultAnimation(false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }



    private IEnumerator WaitUntilAction()
    {
        //특정 조건이 성공할 때까지 대기 true가 나오면 yield 종료
        yield return new WaitUntil(() => !_unitAction.isRunning);
    }





    public void ActionAttack(FieldManager fieldManager, GameManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        if(typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionAttackCoroutine(fieldManager, gameTestManager), WaitUntilAction());
    }

    public void ActionChargeReady(FieldManager fieldManager, GameManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeReadyCoroutine(fieldManager, gameTestManager), WaitUntilAction());
    }

    public void ActionChargeAttack(FieldManager fieldManager, GameManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeAttackCoroutine(fieldManager, gameTestManager), WaitUntilAction());
    }

    public void ActionGuard(FieldManager fieldManager, GameManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionGuardCoroutine(fieldManager, gameTestManager), WaitUntilAction());
    }







    

    private IEnumerator CastleAttack()
    {
        _nowAttackCount = attackCount;
        while (_nowAttackCount > 0)
        {
            Attack();
            _nowAttackCount--;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ForwardAction(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, ForwardActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator ForwardActionCoroutine(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        if(IsHasAnimation("Forward"))
            SetAnimation("Forward", true);
        else if (IsHasAnimation("Move"))
            SetAnimation("Move", true);

        nowBlock.ResetUnitActor();
        movementBlock.SetUnitActor(this, false);

        while (Vector2.Distance(transform.position, movementBlock.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.transform.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        DefaultAnimation(true);
        yield return null;
    }

    public void BackwardAction(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, BackwardActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator BackwardActionCoroutine(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        if(IsHasAnimation("Backward"))
            SetAnimation("Backward", true);
        else if(IsHasAnimation("Move"))
            SetAnimation("Move", true);

        nowBlock.ResetUnitActor();
        movementBlock.SetUnitActor(this, false);

        while (Vector2.Distance(transform.position, movementBlock.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.transform.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        yield return null;
    }

    private void DefaultAnimation(bool isLoop) => SetAnimation("Idle", isLoop);


    private int chargeRange = 1;

    public void ChargeAction(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, ChargeActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator ChargeActionCoroutine(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        chargeRange = (typeTeam == TYPE_TEAM.Left) ? movementBlock.coordinate.x - nowBlock.coordinate.x : nowBlock.coordinate.x - movementBlock.coordinate.x;



        if(IsHasAnimation("Charge"))
            SetAnimation("Charge", true);
        else if(IsHasAnimation("Forward"))
            SetAnimation("Forward", true);

        nowBlock.ResetUnitActor();
        movementBlock.SetUnitActor(this, false);



        while (Vector2.Distance(transform.position, movementBlock.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.transform.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        DefaultAnimation(true);
        yield return null;
    }



    private System.Action<ICaster> _deadEvent;
    public void SetOnDeadListener(System.Action<ICaster> act) => _deadEvent = act;


}

