using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_TEAM {None = -1, Left, Right}

public class UnitActor : MonoBehaviour, IUnitActor
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

//    public int minRangeValue => _uCard.minRangeValue;

    private int _employCostValue => _uCard.employCostValue;

    public int priorityValue => _uCard.priorityValue;// + _commanderActor.GetBonusCommanderMaster(typeUnitGroup);

    public TYPE_UNIT_FORMATION typeUnit => _uCard.typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _uCard.typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _uCard.typeUnitClass;

//    public TYPE_UNIT_ATTACK typeUnitAttack => _uCard.typeUnitAttack;

    public TYPE_BATTLE_TURN typeBattleTurn { get; private set; }

    public TYPE_MOVEMENT typeMovement => _uCard.typeMovement;

//    public Vector2Int[] attackCells => _uCard.attackCells;
    public Vector2Int[] movementCells => _uCard.movementCells;
    public Vector2Int[] chargeCells => _uCard.chargeCells;

    public TargetData TargetData => _uCard.TargetData;

    public Vector2 position => transform.position;

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

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
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

    public IFieldBlock[] GatheringStatePreActive(ICaster caster, SkillData skillData, TYPE_TEAM typeTeam)
    {
        //switch (skillData.typeSkillRange)
        //{
        //    //완료
        //    case TYPE_SKILL_RANGE.All:
        //        return fieldManager.GetBlocksOnUnitActor(skillData.typeTargetTeam, typeTeam);
        //    case TYPE_SKILL_RANGE.MyselfRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                return fieldManager.GetBlocksOnUnitActor(nowBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitClassRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitClass, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitGroupRange:
        //        {
        //            //자신을 찾기
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {

        //                var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitGroup, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.AscendingPriorityRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, true);
        //                if (fieldBlock != null)
        //                {
        //                    return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.DecendingPriorityRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, false);
        //                if (fieldBlock != null)
        //                {
        //                    return fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                }
        //            }
        //        }
        //        break;
        //}
        return null;
    }


    public void SetStatePreActive(FieldManager fieldManager, ICaster caster, SkillData skillData)
    {
        //switch (skillData.typeSkillRange)
        //{
        //    //완료
        //    case TYPE_SKILL_RANGE.All:
        //        {
        //            var blocks = fieldManager.GetBlocksOnUnitActor(skillData.typeTargetTeam, typeTeam);
        //            for (int i = 0; i < blocks.Length; i++)
        //            {
        //                blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.MyselfRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var blocks = fieldManager.GetBlocksOnUnitActor(nowBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                for (int i = 0; i < blocks.Length; i++)
        //                {
        //                    blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitClassRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitClass, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    for (int i = 0; i < blocks.Length; i++)
        //                    {
        //                        blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.UnitGroupRange:
        //        {
        //            //자신을 찾기
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {

        //                var fieldBlock = fieldManager.FindActorBlock(nowBlock, skillData.typeUnitGroup, skillData.typeTargetTeam, typeTeam);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    for (int i = 0; i < blocks.Length; i++)
        //                    {
        //                        //Debug.Log("SetState " + blocks[i].coordinate + " | " + blocks[i].unitActor.name);
        //                        blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.AscendingPriorityRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, true);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    for (int i = 0; i < blocks.Length; i++)
        //                    {
        //                        blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    case TYPE_SKILL_RANGE.DecendingPriorityRange:
        //        {
        //            var nowBlock = fieldManager.FindActorBlock(this);
        //            if (nowBlock != null)
        //            {
        //                var fieldBlock = fieldManager.FindActorBlock(skillData.typeTargetTeam, typeTeam, false);
        //                if (fieldBlock != null)
        //                {
        //                    var blocks = fieldManager.GetBlocksOnUnitActor(fieldBlock, skillData.skillRangeValue, skillData.isMyself, skillData.typeTargetTeam, typeTeam);
        //                    for (int i = 0; i < blocks.Length; i++)
        //                    {
        //                        blocks[i].unitActor.SetSkill(caster, skillData, TYPE_SKILL_ACTIVATE.PreActive);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //}
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

    private int counterAttackRate = 1;

    public void IncreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1)
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

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void Destroy()
    {
//        Debug.Log("Destroy " + typeUnit);
        DestroyImmediate(gameObject);
    }


    public bool DirectAttack(GameManager gameTestManager)
    {
        _attackFieldBlocks = FieldManager.GetTargetBlocks(this, TargetData, typeTeam);

        if (_attackFieldBlocks.Length > 0)
        {
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







    private int _nowAttackCount;
    private IFieldBlock[] _attackFieldBlocks;

    UnitAction _unitAction = new UnitAction();

    public bool isRunning => _unitAction.isRunning && !IsDead();



    private IEnumerator ActionAttackCoroutine(GameManager gameTestManager)
    {
        if (IsHasAnimation("Attack"))
        {
            var nowBlock = FieldManager.FindActorBlock(this);
            //공격방위
            _attackFieldBlocks = FieldManager.GetTargetBlocks(this, TargetData, typeTeam);// FieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (_attackFieldBlocks.Length > 0)
            {
                for (int i = 0; i < _attackFieldBlocks.Length; i++)
                {
                    if (_attackFieldBlocks[i].unitActor != null && _attackFieldBlocks[i].unitActor.typeTeam != typeTeam && !_attackFieldBlocks[i].unitActor.IsDead())
                    {
                        SetAnimation("Attack", false);
                        _nowAttackCount = attackCount;
                        yield break;
                    }
                    //else if (_attackFieldBlocks[i].castleActor != null && _attackFieldBlocks[i].castleActor.typeTeam != typeTeam)
                    //{
                    //    SetAnimation("Attack", false);
                    //    _nowAttackCount = attackCount;
                    //    yield break;
                    //}
                }
            }
        }
        _unitAction.isRunning = false;
        yield break;
    }



    private void AttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        //attackBlocks = SetAttackBlocks();
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
        for (int index = 0; index < _attackFieldBlocks.Length; index++)
        {
            var attackBlock = _attackFieldBlocks[index];
            //공격 애니메이션

            //횟수만큼 공격
            if (attackBlock != null)
            {
                if (attackBlock.unitActor != null)
                {
                    //탄환이 없으면
                    if (_uCard.bullet == null)
                    {
                        if (attackBlock.unitActor.typeUnit == TYPE_UNIT_FORMATION.Castle)
                        {
                            GameManager.IncreaseHealth(damageValue, attackBlock.unitActor.typeTeam);
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

    private void AttackBullet(IFieldBlock attackBlock)
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



    private IEnumerator ActionGuardCoroutine(GameManager gameTestManager)
    {
        if(IsHasAnimation("Guard"))
            SetAnimation("Guard", false);
        else
            DefaultAnimation(false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeAttackCoroutine(GameManager gameTestManager)
    {
        if (IsHasAnimation("Charge_Attack") || IsHasAnimation("Attack"))
        {
            var nowBlock = FieldManager.FindActorBlock(this);
            //공격방위
            _attackFieldBlocks = FieldManager.GetTargetBlocks(this, TargetData, typeTeam);// FieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (_attackFieldBlocks.Length > 0)
            {
                for (int i = 0; i < _attackFieldBlocks.Length; i++)
                {
                    if (_attackFieldBlocks[i].unitActor != null && _attackFieldBlocks[i].unitActor.typeTeam != typeTeam && !_attackFieldBlocks[i].unitActor.IsDead())
                    {
                        if (IsHasAnimation("Charge_Attack"))
                            SetAnimation("Charge_Attack", false);
                        else if (IsHasAnimation("Attack"))
                            SetAnimation("Attack", false);

                        _nowAttackCount = attackCount;
                        yield break;
                    }
                    //else if (_attackFieldBlocks[i].castleActor != null && _attackFieldBlocks[i].castleActor.typeTeam != typeTeam)
                    //{
                    //    if (IsHasAnimation("Charge_Attack"))
                    //        SetAnimation("Charge_Attack", false);
                    //    else if (IsHasAnimation("Attack"))
                    //        SetAnimation("Attack", false);

                    //    _nowAttackCount = attackCount;
                    //    yield break;
                    //}
                }

                DefaultAnimation(false);

            }
        }
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeCoroutine(GameManager gameTestManager)
    {
        if (IsHasAnimation("Charge"))
            SetAnimation("Charge", false);
        else if (IsHasAnimation("Forward"))
            SetAnimation("Forward", false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }
    private IEnumerator ActionChargeReadyCoroutine(GameManager gameTestManager)
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





    public void ActionAttack(GameManager gameTestManager)
    {
        if(typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionAttackCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionChargeReady(GameManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeReadyCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionChargeAttack(GameManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeAttackCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionGuard(GameManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionGuardCoroutine(gameTestManager), WaitUntilAction());
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

    public void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, ForwardActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator ForwardActionCoroutine(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        if(IsHasAnimation("Forward"))
            SetAnimation("Forward", true);
        else if (IsHasAnimation("Move"))
            SetAnimation("Move", true);

        nowBlock.LeaveUnitActor(this);
        movementBlock.SetUnitActor(this, false);

        while (Vector2.Distance(transform.position, movementBlock.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        DefaultAnimation(true);
        yield return null;
    }

    public void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, BackwardActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator BackwardActionCoroutine(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        if(IsHasAnimation("Backward"))
            SetAnimation("Backward", true);
        else if(IsHasAnimation("Move"))
            SetAnimation("Move", true);

        nowBlock.LeaveUnitActor(this);
        movementBlock.SetUnitActor(this, false);

        while (Vector2.Distance(transform.position, movementBlock.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        yield return null;
    }

    private void DefaultAnimation(bool isLoop) => SetAnimation("Idle", isLoop);


    private int chargeRange = 1;

    public void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, ChargeActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator ChargeActionCoroutine(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        chargeRange = (typeTeam == TYPE_TEAM.Left) ? movementBlock.coordinate.x - nowBlock.coordinate.x : nowBlock.coordinate.x - movementBlock.coordinate.x;



        if(IsHasAnimation("Charge"))
            SetAnimation("Charge", true);
        else if(IsHasAnimation("Forward"))
            SetAnimation("Forward", true);

        nowBlock.LeaveUnitActor(this);
        movementBlock.SetUnitActor(this, false);



        while (Vector2.Distance(transform.position, movementBlock.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(Settings.MIN_UNIT_MOVEMENT, Settings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        DefaultAnimation(true);
        yield return null;
    }



    private System.Action<ICaster> _deadEvent;
    public void SetOnDeadListener(System.Action<ICaster> act) => _deadEvent = act;

}

