using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitActor : MonoBehaviour, IUnitActor
{

    private const float COUNTER_RATE = 2f;
    private const float REVERSE_COUNTER_RATE = 0.5f;

    [SerializeField]
    private UIBar _uiBar;

    [SerializeField]
    private SkeletonAnimation _sAnimation;

    private StatusActor _statusActor = new StatusActor();
    public StatusActor StatusActor => _statusActor;

    private Spine.Skeleton _skeleton { get; set; }

    public int uKey { get; private set; }

    public void SetKey(int key) => uKey = key;


    private UnitCard _unitCard;


    public float counterValue => _statusActor.GetValue<StatusValueCounter>(COUNTER_RATE);
    public float reverseCounterValue => _statusActor.GetValue<StatusValueRevCounter>(REVERSE_COUNTER_RATE);

    public UnitCard unitCard => _unitCard;

    public int nowHealthValue => _statusActor.GetValue<StatusValueMaxHealth>(_unitCard.GetUnitNowHealth(uKey));

    public int maxHealthValue => _statusActor.GetValue<StatusValueMaxHealth>(_unitCard.GetUnitMaxHealth(uKey));


    public float HealthRate() => _unitCard.HealthRate(uKey);

    public bool IsDead() => _unitCard.IsDead(uKey);

    public int damageValue => _statusActor.GetValue<StatusValueAttack>(_unitCard.damageValue);

    public int defensiveValue => _statusActor.GetValue<StatusValueDefensive>(_unitCard.defensiveValue);

    public bool IsAttack => _unitCard.IsAttack;

    public int attackCount => _statusActor.GetValue<StatusValueAttackCount>(_unitCard.attackCount);

    public TYPE_TEAM typeTeam { get; private set; }

    private int _employCostValue => _unitCard.employCostValue;

    public int priorityValue => _statusActor.GetValue<StatusValuePriority>(_unitCard.priorityValue);

    public int proficiencyValue => _statusActor.GetValue<StatusValueProficiency>(_unitCard.proficiencyValue);

    public TYPE_UNIT_FORMATION typeUnit => _unitCard.typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _unitCard.typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _unitCard.typeUnitClass;

    public TYPE_MOVEMENT typeMovement => _unitCard.typeMovement;

    public TYPE_BATTLE_TURN TypeBattleTurn { get; private set; }

    public int movementValue => _statusActor.GetValue<StatusValueMovement>(_unitCard.movementValue);
    public int chargeMovementValue => _statusActor.GetValue<StatusValueChargeMovement>(movementValue * 2);


    public TargetData AttackTargetData => _unitCard.AttackTargetData;

    public Vector3 position => transform.position;

    public SkillData[] skills => _unitCard.Skills;

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
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn) => TypeBattleTurn = typeBattleTurn;

    public void SetData(UnitCard uCard)
    {
        _unitCard = uCard;

        if (_unitCard.SkeletonDataAsset != null)
        {
            _sAnimation.skeletonDataAsset = _unitCard.SkeletonDataAsset;
            _sAnimation.Initialize(false);
            _skeleton = _sAnimation.skeleton;
            _skeleton.SetSlotsToSetupPose();
            _skeleton.SetSkin(_unitCard.Skin);
            //_animationState = _sAnimation.state;
            _sAnimation.AnimationState.Event += AttackEvent;
            _sAnimation.AnimationState.SetEmptyAnimation(0, 0f);
            DefaultAnimation(true);

            SetColor((typeTeam == TYPE_TEAM.Left) ? Color.blue : Color.red);
        }
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
        _uiBar.gameObject.SetActive(_unitCard.typeUnit != TYPE_UNIT_FORMATION.Castle);
        _uiBar.SetBar(HealthRate());
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

    public void ReceiveStatusData(ICaster caster, StatusData statusData)
    {
        _statusActor.AddStatusData(caster, statusData);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }

    public void ReleaseStatusData(ICaster caster)
    {
        _statusActor.RemoveStatusData(caster);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }

    public void ReleaseStatusData(StatusData statusData)
    {
        _statusActor.RemoveStatusData(statusData);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }

    public void SetStatusData(ICaster caster, StatusData status)
    {
        _statusActor.AddStatusData(caster, status);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }

    private void Update()
    {
        if (attackCount == 0)
        {
            _nowAttackCount = 0;
            DefaultAnimation(true);
            _unitAction.isRunning = false;
        }
    }

    public void RemoveStatusData()
    {
        //if (!IsDead())
        //{
        //    _statusActor.RemovePreActiveSkill(_uiBar);
        //}
    }

    public void RemoveStatusData(ICaster caster)
    {
        _statusActor.RemoveStatusData(caster);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }

    public bool IsHasStatusData(StatusData.TYPE_STATUS_LIFE_SPAN typeStatusLifeSpan)
    {
        return _statusActor.IsHasStatusData(typeStatusLifeSpan);
    }

    public bool IsHasStatusData(StatusData statusData)
    {
        return _statusActor.IsHasStatusData(statusData);
    }

    public bool IsHasStatus<T>() where T : IStatus => _statusActor.IsHasStatus<T>();




    private int counterAttackRate = 1;

    public int DescreaseHealth(IUnitActor attackActor, int value, int additiveRate = 1)
    {
        //흘리기 판정
        if (StatusActor.IsHasStatus<StatusEffectParrying>())
        {
            return 0;
        }

        if (BattleFieldSettings.Invincible) return 0;

        var attackValue = value;
        if (UnitData.IsAttackUnitClassOpposition(typeUnitClass, attackActor.typeUnitClass))
            attackValue = (int)((float)value * counterValue);

        if (UnitData.IsDefenceUnitClassOpposition(typeUnitClass, attackActor.typeUnitClass))
            attackValue = (int)((float)value * reverseCounterValue);

        switch (TypeBattleTurn)
        {
            case TYPE_BATTLE_TURN.Guard:
                if (attackActor.TypeBattleTurn == TYPE_BATTLE_TURN.Forward)
                {
                    attackValue = (int)((float)attackValue * counterValue);
                }
                else
                {
                    attackValue = (int)((float)attackValue * reverseCounterValue);
                    counterAttackRate = additiveRate;
                }
                break;
            case TYPE_BATTLE_TURN.Backward:
                if (attackActor.TypeBattleTurn == TYPE_BATTLE_TURN.Forward)
                {
                    attackValue = (int)((float)attackValue * reverseCounterValue);
                }
                else if (attackActor.TypeBattleTurn == TYPE_BATTLE_TURN.Charge)
                {
                    attackValue *= additiveRate;
                }
                else if (attackActor.typeUnitGroup == TYPE_UNIT_GROUP.Shooter)
                {
                    attackValue = (int)((float)attackValue * counterValue);
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

        return DecreaseHealth(attackValue);
    }




    public int DecreaseHealth(int value)
    {
        if (value <= 0)
            value = 1;

        _unitCard.DecreaseHealth(uKey, value);

        if (_unitCard.IsDead(uKey))
        {
            SetAnimation("Dead", false);
            GameObject game = new GameObject();
            var audio = game.AddComponent<AudioSource>();
            audio.PlayOneShot(_unitCard.deadClip);

            _unitCard.SetUnitLiveType(uKey);

            _deadEvent?.Invoke(this);
        }


        _uiBar?.SetBar(HealthRate());
        return value;
    }

    public int IncreaseHealth(int value)
    {
        if(value < 0)
        {
            value = 0;
        }

        _unitCard.IncreaseHealth(uKey, value);
        _uiBar?.SetBar(HealthRate());
        return value;
    }


    private bool IsHasAnimation(string name) => (_sAnimation.skeleton.Data.FindAnimation(name) != null);

    private void SetAnimation(string name, bool loop)
    {
        if (IsHasAnimation(name))
        {
            if (_sAnimation.SkeletonDataAsset != null)
            {
                var track = _sAnimation.AnimationState.SetAnimation(0, name, loop);
                var proficiencyTime = Random.Range(0.5f + 0.5f * Mathf.Clamp01(((float)proficiencyValue / 100f)), 1.5f - 0.5f * Mathf.Clamp01(((float)proficiencyValue / 100f))) * (float)attackCount;
                track.TimeScale = proficiencyTime;
            }
        }
    }

    public void Dead()
    {
        SetAnimation("Dead", false);
    }

    public void Turn()
    {
        _statusActor.Turn(this, _uiBar);
    }


    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }


    public bool DirectAttack(BattleFieldManager gameTestManager)
    {
        _attackFieldBlocks = FieldManager.GetTargetBlocks(this, AttackTargetData, typeTeam);

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

    private IEnumerator ActionAttackCoroutine(BattleFieldManager gameTestManager)
    {
        var isCast = CastSkills(TYPE_SKILL_CAST.AttackCast);

        if (!isCast) {
            if (IsHasAnimation("Attack"))
            {
                var nowBlock = FieldManager.FindActorBlock(this);
                //공격방위
                _attackFieldBlocks = FieldManager.GetTargetBlocks(this, AttackTargetData, typeTeam);

                //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
                if (_attackFieldBlocks.Length > 0)
                {
                    for (int i = 0; i < _attackFieldBlocks.Length; i++)
                    {
                        if (_attackFieldBlocks[i].IsHasUnitActor())
                        {
                            var unitActors = _attackFieldBlocks[i].unitActors;
                            for (int j = 0; j < unitActors.Length; j++)
                            {
                                var uActor = unitActors[j];
                                if (uActor.typeTeam != typeTeam && !uActor.IsDead())
                                {
                                    SetAnimation("Attack", false);
                                    _nowAttackCount = attackCount;
                                    yield break;
                                }
                            }
                        }
                    }
                }
            }
        }

        _unitAction.isRunning = false;
    }



    private void AttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Attack();
        CastSkills(TYPE_SKILL_CAST.AttackedCast);
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
                if (attackBlock.IsHasUnitActor())
                {
                    //탄환이 없으면
                    if (_unitCard.BulletData == null)
                    {
                        DealAttack(attackBlock);
                    }
                    //탄환이 있으면
                    else
                    {
                        AttackBullet(attackBlock);
                    }
                    GameObject game = new GameObject();
                    var audio = game.AddComponent<AudioSource>();
                    audio.PlayOneShot(_unitCard.attackClip);
                }
            }
        }
    }

    private bool CastSkills(TYPE_SKILL_CAST typeSkillCast)
    {
        if (skills.Length > 0)
        {
            for (int i = 0; i < skills.Length; i++)
            {
//                Debug.Log("SkillRate " + StatusActor.GetValue<StatusValueSkillCastRate>(skills[i].skillCastRate));
                if (skills[i].IsTypeSkillCast(typeSkillCast))
                {
//                    Debug.Log("SkillRate " + StatusActor.GetValue<StatusValueSkillCastRate>(skills[i].skillCastRate));
                    if (skills[i].IsSkillCondition(this))
                    {
                        //Debug.Log("SkillRate " + StatusActor.GetValue<StatusValueSkillCastRate>(skills[i].skillCastRate));
                        if (StatusActor.GetValue<StatusValueSkillCastRate>(skills[i].skillCastRate) > Random.Range(0, 1f))
                        {
                            skills[i].CastSkillProcess(this, typeSkillCast);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    private void DealAttack(BulletActor bActor, IFieldBlock attackBlock)
    {
        DealAttack(attackBlock);
    }


    private void DealAttack(IFieldBlock attackBlock)
    {
        if (attackBlock.IsHasUnitActor())
        {
            for (int i = 0; i < attackBlock.unitActors.Length; i++)
            {
                var uActor = attackBlock.unitActors[i];
                DealAttack(uActor);
                break;
            }
        }
    }

    /// <summary>
    /// 공격을 가합니다
    /// </summary>
    /// <param name="uActor"></param>
    public void DealAttack(IUnitActor uActor)
    {
        //방어무시 확인
        var attackDamageValue = damageValue - (StatusActor.IsHasStatus<StatusEffectPenetrate>() ? 0 : uActor.defensiveValue);

        //성이면 성 직접 공격
        if (uActor.typeUnit == TYPE_UNIT_FORMATION.Castle)
        {
            BattleFieldManager.IncreaseHealth(attackDamageValue, uActor.typeTeam);
        }
        else
        {
            //가한 공격력
            int dealHealthValue = 0;
            if (TypeBattleTurn == TYPE_BATTLE_TURN.Charge)
            {
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue, chargeRange);
                chargeRange = 1;
            }
            else if (TypeBattleTurn == TYPE_BATTLE_TURN.Guard)
            {
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue, counterAttackRate);
                counterAttackRate = 1;
            }
            else
            {
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue);
            }


            //Debug.Log(IsHasStatus<StatusValueIncreaseNowHealth>());

            //체력 증가
            if (IsHasStatus<StatusValueIncreaseNowHealth>())
            {
                //공격력의 n만큼 체력 회복
                var increaseHealthValue = _statusActor.GetValue<StatusValueIncreaseNowHealth>(dealHealthValue);
                //Debug.Log("increaseHealthValue " + increaseHealthValue);
                IncreaseHealth(increaseHealthValue);
            }


            //Debug.Log(IsHasStatus<StatusValueDecreaseNowHealth>());

            //체력 감소
            if (IsHasStatus<StatusValueDecreaseNowHealth>())
            {
                //공격력의 n만큼 체력 감소
                var decreaseHealthValue = _statusActor.GetValue<StatusValueDecreaseNowHealth>(dealHealthValue);
                //Debug.Log("decreaseHealthValue " + decreaseHealthValue);
                DecreaseHealth(decreaseHealthValue);
            }
        }
        
    }


    private void AttackBullet(IFieldBlock attackBlock)
    {
        BulletManager.ActivateBullet(unitCard.BulletData, transform.position, attackBlock.position, actor => { DealAttack(actor, attackBlock);  });
    }



    private IEnumerator ActionGuardCoroutine(BattleFieldManager gameTestManager)
    {
        if(IsHasAnimation("Guard"))
            SetAnimation("Guard", false);
        else
            DefaultAnimation(false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeAttackCoroutine(BattleFieldManager gameTestManager)
    {
        if (IsHasAnimation("Charge_Attack") || IsHasAnimation("Attack"))
        {
            var nowBlock = FieldManager.FindActorBlock(this);
            //공격방위
            _attackFieldBlocks = FieldManager.GetTargetBlocks(this, AttackTargetData, typeTeam);// FieldManager.GetAttackBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

            //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
            if (_attackFieldBlocks.Length > 0)
            {
                for (int i = 0; i < _attackFieldBlocks.Length; i++)
                {
                    if (_attackFieldBlocks[i].IsHasUnitActor())
                    {
                        for (int j = 0; j < _attackFieldBlocks[i].unitActors.Length; j++)
                        {
                            var uActor = _attackFieldBlocks[i].unitActors[j];
                            if (uActor.typeTeam != typeTeam && !uActor.IsDead())
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
                    }
                }

                DefaultAnimation(false);

            }
        }
        _unitAction.isRunning = false;
        yield break;
    }

    private IEnumerator ActionChargeCoroutine(BattleFieldManager gameTestManager)
    {
        if (IsHasAnimation("Charge"))
            SetAnimation("Charge", false);
        else if (IsHasAnimation("Forward"))
            SetAnimation("Forward", false);

        _nowAttackCount = attackCount;
        _unitAction.isRunning = false;
        yield break;
    }
    private IEnumerator ActionChargeReadyCoroutine(BattleFieldManager gameTestManager)
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





    public void ActionAttack(BattleFieldManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionAttackCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionChargeReady(BattleFieldManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeReadyCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionChargeAttack(BattleFieldManager gameTestManager)
    {
        if (typeUnit != TYPE_UNIT_FORMATION.Castle)
            _unitAction.SetUnitAction(this, ActionChargeAttackCoroutine(gameTestManager), WaitUntilAction());
    }

    public void ActionGuard(BattleFieldManager gameTestManager)
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
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
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
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
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
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.position, Random.Range(BattleFieldSettings.MIN_UNIT_MOVEMENT, BattleFieldSettings.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        DefaultAnimation(true);
        yield return null;
    }



    private System.Action<ICaster> _deadEvent;
    public void SetOnDeadListener(System.Action<ICaster> act) => _deadEvent = act;

    public void CleanUp()
    {
        _statusActor.Clear();
    }

}

