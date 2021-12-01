using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActor : MonoBehaviour, IUnitActor
{
    private const float COUNTER_RATE = 2f;
    private const float REVERSE_COUNTER_RATE = 0.5f;



    private UIBar _uiBar;




    private SkeletonAnimation _sAnimation;

    private SkeletonAnimation skeletonAnimation
    {
        get
        {
            if (_sAnimation == null)
            {
                _sAnimation = GetComponentInChildren<SkeletonAnimation>(true);
                Debug.Assert(_sAnimation != null, "SkeletonAnimation을 찾을 수 없습니다");
            }
            return _sAnimation;
        }
    }

    private Spine.Skeleton _skeleton { get; set; }



    public int uKey { get; private set; }
    public void SetKey(int key) => uKey = key;



    private UnitCard _unitCard;
    public UnitCard unitCard => _unitCard;



    public TYPE_BATTLE_TEAM typeTeam { get; private set; }
    public TYPE_UNIT_FORMATION typeUnit => _unitCard.TypeUnit;
    public TYPE_UNIT_GROUP typeUnitGroup => _unitCard.TypeUnitGroup;
    public TYPE_UNIT_CLASS typeUnitClass => _unitCard.TypeUnitClass;
    public TYPE_UNIT_MOVEMENT typeMovement => _unitCard.TypeMovement;
    public TYPE_BATTLE_TURN TypeBattleTurn { get; private set; }
    public TargetData AttackTargetData => _unitCard.AttackTargetData;
    public Vector3 position => transform.position;
    public SkillData[] skills => _unitCard.Skills;




    private StatusActor _statusActor = new StatusActor();
    public StatusActor StatusActor => _statusActor;

    public float counterValue => _statusActor.GetValue<StatusValueCounter>(COUNTER_RATE);
    public float reverseCounterValue => _statusActor.GetValue<StatusValueRevCounter>(REVERSE_COUNTER_RATE);
    public int nowHealthValue => _statusActor.GetValue<StatusValueMaxHealth>(_unitCard.GetUnitNowHealth(uKey));
    public int maxHealthValue => _statusActor.GetValue<StatusValueMaxHealth>(_unitCard.GetUnitMaxHealth(uKey));
    public float HealthRate() => _unitCard.HealthRate(uKey);
    public bool IsDead() => _unitCard.IsDead(uKey);
    public int damageValue => _statusActor.GetValue<StatusValueAttack>(_unitCard.DamageValue);
    public int defensiveValue => _statusActor.GetValue<StatusValueDefensive>(_unitCard.DefensiveValue);
    public bool IsAttack => _unitCard.IsAttack;
    public int attackCount => _statusActor.GetValue<StatusValueAttackCount>(_unitCard.AttackCount);
    private int _employCostValue => _unitCard.EmployCostValue;
    public int priorityValue => _statusActor.GetValue<StatusValuePriority>(_unitCard.PriorityValue);
    public int proficiencyValue => _statusActor.GetValue<StatusValueProficiency>(_unitCard.ProficiencyValue);
    public int movementValue => _statusActor.GetValue<StatusValueMovement>(_unitCard.MovementValue);
    public int chargeMovementValue => _statusActor.GetValue<StatusValueChargeMovement>(movementValue * 2);



    #region ##### Initialize & CleanUp #####

    public void SetData(UnitCard uCard)
    {
        Debug.Assert(uCard != null, "UnitCard를 등록하지 않았습니다");

        _unitCard = uCard;

        if (_unitCard.SkeletonDataAsset != null)
        {
            skeletonAnimation.skeletonDataAsset = _unitCard.SkeletonDataAsset;
            skeletonAnimation.Initialize(false);
            _skeleton = skeletonAnimation.skeleton;
            _skeleton.SetSlotsToSetupPose();

            if (!string.IsNullOrEmpty(_unitCard.Skin))
                _skeleton.SetSkin(_unitCard.Skin);

            //_animationState = _sAnimation.state;
            skeletonAnimation.AnimationState.Event += AttackEvent;
            skeletonAnimation.AnimationState.SetEmptyAnimation(0, 0f);
            DefaultAnimation(true);

            SetColor((typeTeam == TYPE_BATTLE_TEAM.Left) ? Color.blue : Color.red);

            _isDeaded = false;
        }
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

    public void SetUIBar(UIBar uiBar)
    {
        _uiBar = uiBar;
        _uiBar.transform.SetParent(transform);
        _uiBar.transform.localPosition = Vector3.up * 1f;
        _uiBar.gameObject.SetActive(_unitCard.TypeUnit != TYPE_UNIT_FORMATION.Castle);
        _uiBar.SetBar(HealthRate());
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    public void SetBattleTurn(TYPE_BATTLE_TURN typeBattleTurn) => TypeBattleTurn = typeBattleTurn;
    public void SetTypeTeam(TYPE_BATTLE_TEAM typeTeam)
    {
        this.typeTeam = typeTeam;

        switch (typeTeam)
        {
            case TYPE_BATTLE_TEAM.Left:
                transform.localScale = Vector3.one;
                break;
            case TYPE_BATTLE_TEAM.Right:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;
        }
    }



    public void CleanUp()
    {
        _statusActor.Clear();
    }


    public void Destroy()
    {
        Debug.Assert(gameObject != null, "gameObject가 이미 파괴되었습니다");
        DestroyImmediate(gameObject);
    }


    #endregion



    private void Update()
    {
        if (attackCount == 0)
        {
            DefaultAnimation(true);
            _unitAction.isRunning = false;
        }
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void RefreshLayer()
    {
        skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = -(int)transform.position.y;
    }

    public void Turn()
    {
        _statusActor.Turn(this, _uiBar);
    }



    #region ##### Status #####


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


#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
    public void SetStatusData(ICaster caster, StatusData status)
    {
        _statusActor.AddStatusData(caster, status);
        _uiBar?.ShowStatusDataArray(_statusActor.GetStatusElementArray());
    }
#endif


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

    #endregion



    #region ##### Health #####

    /// <summary>
    /// 체력 회복
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IncreaseHealth(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        _unitCard.IncreaseHealth(uKey, value);
        _uiBar?.SetBar(HealthRate());
        return value;
    }



    private int _counterAttackRate = 1;

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
                    _counterAttackRate = additiveRate;
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

        return DecreaseHealth(attackValue);
    }




    private bool _isDeaded = false;

    public int DecreaseHealth(int value)
    {
        if (value <= 0)
            value = 1;

        _unitCard.DecreaseHealth(uKey, value);

        if (_unitCard.IsDead(uKey) && !_isDeaded)
        {
            SetAnimation("Dead", false);

            AudioManager.ActivateAudio(_unitCard.DeadClip, AudioManager.TYPE_AUDIO.SFX);

            _unitCard.SetTypeUnitLife(uKey);

            _deadEvent?.Invoke(this);

            _isDeaded = true;
        }

        _uiBar?.SetBar(HealthRate());
        return value;
    }

    #endregion



    #region ##### Animation #####

    internal bool IsHasAnimation(string name) => (skeletonAnimation.skeleton.Data.FindAnimation(name) != null);

    public void SetAnimation(string name, bool loop)
    {
        if (IsHasAnimation(name))
        {
            if (skeletonAnimation.SkeletonDataAsset != null)
            {
                var track = skeletonAnimation.AnimationState.SetAnimation(0, name, loop);
                var proficiencyTime = Random.Range(0.5f + 0.5f * Mathf.Clamp01(((float)proficiencyValue / 100f)), 1.5f - 0.5f * Mathf.Clamp01(((float)proficiencyValue / 100f))) * (float)attackCount;
                track.TimeScale = proficiencyTime;
            }
        }
    }
    public void DefaultAnimation(bool isLoop) => SetAnimation("Idle", isLoop);

    #endregion




    //[System.Obsolete("UnitActionController로 이전 예정")]
    //private IEnumerator WaitUntilAction()
    //{
    //    //특정 조건이 성공할 때까지 대기 true가 나오면 yield 종료
    //    yield return new WaitUntil(() => !_unitAction.isRunning);
    //}

    UnitActionController _unitAction = new UnitActionController();
    public bool isRunning => _unitAction.isRunning && !IsDead();

    public void ActionUnit<T>() where T : IUnitActionState
    {
        _unitAction.SetUnitAction<T>(this, this, _unitActionData, CastSkills);
    }

    public void ActionUnit<T>(IFieldBlock nowBlock, IFieldBlock movementBlock) where T : IUnitActionState
    {
        _unitActionData.nowBlock = nowBlock;
        _unitActionData.movementBlock = movementBlock;
        _unitAction.SetUnitAction<T>(this, this, _unitActionData, CastSkills);
    }



    #region ##### Attack #####



    private UnitActionData _unitActionData = new UnitActionData();

    public void ActionAttack()
    {
        ActionUnit<UnitActionAttack>();
        //if (typeUnit != TYPE_UNIT_FORMATION.Castle && !IsDead())
        //    _unitAction.SetUnitAction(this, ActionAttackCoroutine(), WaitUntilAction());
    }





    private void AttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Attack();
        CastSkills(TYPE_SKILL_CAST.AttackedCast);
        AttackCounting();
    }

    public void Attack()
    {
        var _attackFieldBlocks = _unitActionData.attackFieldBlocks;
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

                    AudioManager.ActivateAudio(_unitCard.AttackClip, AudioManager.TYPE_AUDIO.SFX);
                }
            }
        }
    }

    private void AttackBullet(IFieldBlock attackBlock)
    {
        BulletManager.ActivateBullet(unitCard.BulletData, transform.position, attackBlock.position, actor => { DealAttack(actor, attackBlock); });
    }

    private void AttackCounting()
    {
        _unitActionData.nowAttackCount--;

        if (_unitActionData.nowAttackCount > 0)
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



    private void DealAttack(BulletActor bActor, IFieldBlock attackBlock)
    {
        DealAttack(attackBlock);
    }


    private void DealAttack(IFieldBlock attackBlock)
    {
        if (attackBlock.IsHasUnitActor())
        {
            if (attackBlock.unitActors.Length > 0)
            {
                var uActor = attackBlock.unitActors[0];
                for (int i = 1; i < attackBlock.unitActors.Length; i++)
                {
                    var tmpUnitActor = attackBlock.unitActors[i];
                    if (tmpUnitActor.typeUnit == TYPE_UNIT_FORMATION.Castle)
                    {
                        uActor = tmpUnitActor;
                        break;
                    }
                }
                DealAttack(uActor);
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
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue, _unitActionData.chargeRange);
                _unitActionData.chargeRange = 1;
            }
            else if (TypeBattleTurn == TYPE_BATTLE_TURN.Guard)
            {
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue, _counterAttackRate);
                _counterAttackRate = 1;
            }
            else
            {
                dealHealthValue = uActor.DescreaseHealth(this, attackDamageValue);
            }

            //체력 증가
            if (IsHasStatus<StatusValueIncreaseNowHealth>())
            {
                //공격력의 n만큼 체력 회복
                var increaseHealthValue = _statusActor.GetValue<StatusValueIncreaseNowHealth>(dealHealthValue);
                IncreaseHealth(increaseHealthValue);
            }

            //체력 감소
            if (IsHasStatus<StatusValueDecreaseNowHealth>())
            {
                //공격력의 n만큼 체력 감소
                var decreaseHealthValue = _statusActor.GetValue<StatusValueDecreaseNowHealth>(dealHealthValue);
                DecreaseHealth(decreaseHealthValue);
            }
        }
    }




    /// <summary>
    /// 성이 공격
    /// </summary>
    /// <returns></returns>
    public bool DirectAttack()
    {
        _unitActionData.attackFieldBlocks = FieldManager.GetTargetBlocks(this, AttackTargetData, typeTeam);

        if (_unitActionData.attackFieldBlocks.Length > 0)
        {
            ActionUnit<UnitActionCastleAttack>();
            return true;
        }
        return false;
    }


    #endregion



    #region ##### Guard #####

    public void ActionGuard()
    {
        _unitAction.SetUnitAction<UnitActionGuard>(this, this, _unitActionData, CastSkills);
    }

    #endregion



    #region ##### Charge #####

    public void ActionChargeReady()
    {
        _unitAction.SetUnitAction<UnitActionChargeReady>(this, this, _unitActionData, CastSkills);
    }

    public void ChargeAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        ActionUnit<UnitActionCharge>(nowBlock, movementBlock);
    }

    public void ActionChargeAttack()
    {
        _unitAction.SetUnitAction<UnitActionChargeAttack>(this, this, _unitActionData, CastSkills);
    }


    #endregion



    #region ##### Forward #####

    public void ForwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        ActionUnit<UnitActionCharge>(nowBlock, movementBlock);
    }

    #endregion



    #region ##### Backward #####
    public void BackwardAction(IFieldBlock nowBlock, IFieldBlock movementBlock)
    {
        ActionUnit<UnitActionCharge>(nowBlock, movementBlock);
    }

    #endregion


    #region ##### Skill #####
    private bool CastSkills(TYPE_SKILL_CAST typeSkillCast)
    {
        if (skills.Length > 0)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].IsTypeSkillCast(typeSkillCast))
                {
                    if (skills[i].IsSkillCondition(this))
                    {
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


    #endregion



    #region ##### Listener #####

    private System.Action<ICaster> _deadEvent;
    public void SetOnDeadListener(System.Action<ICaster> act) => _deadEvent = act;

    #endregion

}

