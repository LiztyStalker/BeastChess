using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public enum TYPE_TEAM { Left, Right}

public class UnitActor : MonoBehaviour
{

    TYPE_TEAM _typeTeam;

    UnitData _unitData;

    [SerializeField]
    UIBar _uiBar;

    [SerializeField]
    private SkeletonAnimation _sAnimation;
    //Spine.AnimationState _animationState;
    Spine.Skeleton _skeleton;

    public void SetState() { }

    public int healthValue => _unitData.healthValue;

    public float HealthRate() => (float)_nowHealthValue / (float)healthValue;

    public bool IsDead() => _nowHealthValue == 0;

    public int damageValue => _unitData.damageValue;

    public int attackCount => _unitData.attackCount;

    public TYPE_TEAM typeTeam => _typeTeam;

    private int _nowHealthValue;

    public int minRangeValue => _unitData.minRangeValue;

    int _costValue => _unitData.costValue;

    public int priorityValue => _unitData.priorityValue;

    public TYPE_UNIT typeUnit => _unitData.typeUnit;

    public TYPE_UNIT_ATTACK typeUnitAttack => _unitData.typeUnitAttack;

    public Vector2Int[] attackCells => _unitData.attackCells;
    public Vector2Int[] movementCells => _unitData.movementCells;

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        _typeTeam = typeTeam;

        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                transform.localScale = Vector3.one;
                break;
            case TYPE_TEAM.Right:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;
        }

    }

    public void SetData(UnitData uData)
    {
        _unitData = uData;

        if (_unitData.skeletonDataAsset != null)
        {
            _sAnimation.skeletonDataAsset = _unitData.skeletonDataAsset;
            _sAnimation.Initialize(false);
            _skeleton = _sAnimation.skeleton;
            _skeleton.SetSlotsToSetupPose();
            //_animationState = _sAnimation.state;
            _sAnimation.AnimationState.Event += AttackEvent;
            _sAnimation.AnimationState.SetEmptyAnimation(0, 0f);
            SetAnimation("Idle", true);

            SetColor((_typeTeam == TYPE_TEAM.Left) ? Color.blue : Color.red);
        }

        _nowHealthValue = healthValue;
    }

    public void SetLayer()
    {
        if (_sAnimation != null)
        {
            _sAnimation.GetComponent<MeshRenderer>().sortingOrder = -(int)transform.position.y;
            Debug.Log("Layer " + _sAnimation.GetComponent<MeshRenderer>().sortingOrder);
        }
    }

    public void AddBar(UIBar uiBar)
    {
        _uiBar = uiBar;
        _uiBar.transform.SetParent(transform);
        _uiBar.transform.localPosition = Vector3.up * 1f;
        _uiBar.gameObject.SetActive(_unitData.typeUnit != TYPE_UNIT.Castle);
        _uiBar.SetBar(HealthRate());
    }


    private Color GetTeamColor(TYPE_TEAM typeTeam)
    {
        switch (_typeTeam)
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
        _skeleton.FindSlot("LBand").SetColor(color);
        _skeleton.FindSlot("RBand").SetColor(color);
    }

    public void IncreaseHealth(int value)
    {
        if (_nowHealthValue - value <= 0)
        {
            _nowHealthValue = 0;
            SetAnimation("Dead", false);

            GameObject game = new GameObject();
            var audio = game.AddComponent<AudioSource>();
            audio.PlayOneShot(_unitData.deadClip);

        }
        else
            _nowHealthValue -= value;

        _uiBar.SetBar(HealthRate());
    }

    private void SetAnimation(string name, bool loop)
    {
        if (_sAnimation.SkeletonDataAsset != null)
        {
            var track = _sAnimation.AnimationState.SetAnimation(0, name, loop);
            track.TimeScale = Random.Range(0.8f, 1.2f);
        }
    }

    public void Dead()
    {
        SetAnimation("Dead", false);
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
    GameTestManager gameTestManager;

    private IEnumerator ActionAttackCoroutine(FieldManager fieldManager, GameTestManager gameTestManager)
    {
        var nowBlock = fieldManager.FindActorBlock(this);
        //공격방위
        blocks = fieldManager.GetBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

        //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
        if (blocks.Length > 0)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null && blocks[i].unitActor.typeTeam != typeTeam && !blocks[i].unitActor.IsDead())
                {
                    SetAnimation("Attack", false);
                    _nowAttackCount = attackCount;
                    yield break;
                }
                else if(blocks[i].castleActor != null && blocks[i].castleActor.typeTeam != typeTeam)
                {
                    SetAnimation("Attack", false);
                    _nowAttackCount = attackCount;
                    yield break;
                }
            }
        }

        _unitAction.isRunning = false;
        yield break;
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
                    else if(blocks[shupple[i]].castleActor != null)
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
                        if(list[i].castleActor != null)
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
            SetAnimation("Attack", false);
        }
        else
        {
            _unitAction.isRunning = false;
            SetAnimation("Idle", true);
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
                    if (_unitData.bullet == null)
                    {
                        if (attackBlock.castleActor != null)
                            GameTestManager.IncreaseHealth(damageValue, attackBlock.castleActor.typeTeam);
                        else
                        {
                            attackBlock.unitActor.IncreaseHealth(damageValue);
                        }
                    }
                    //탄환이 있으면
                    else
                    {
                        AttackBullet(attackBlock);
                    }
                    GameObject game = new GameObject();
                    var audio = game.AddComponent<AudioSource>();
                    audio.PlayOneShot(_unitData.attackClip);
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

        renderer.sprite = _unitData.bullet;
        actor.SetData(this, attackBlock, 1f);
        actor.transform.position = transform.position;
        actor.gameObject.SetActive(true);
        //탄환 알고리즘에 의해 날아가서 데미지를 가하도록 하기
    }

    public bool isRunning => _unitAction.isRunning;

    UnitAction _unitAction = new UnitAction();

    private IEnumerator AttackEvent()
    {
        yield return new WaitUntil(() => !_unitAction.isRunning);
    }
    public void ActionAttack(FieldManager fieldManager, GameTestManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        if(typeUnit != TYPE_UNIT.Castle)
            _unitAction.SetUnitAction(this, ActionAttackCoroutine(fieldManager, gameTestManager), AttackEvent());
    }

    public bool DirectAttack(FieldManager fieldManager, GameTestManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;

        var nowBlock = fieldManager.FindActorBlock(this);

        blocks = fieldManager.GetBlocks(nowBlock.coordinate, attackCells, minRangeValue, typeTeam);

        if (blocks.Length > 0)
        {
            attackBlocks = SetAttackBlocks();
            _unitAction.SetUnitAction(this, CastleAttack(), null);
            return true;
        }
        return false;
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

    public void MovementAction(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        //1회 이동
        _unitAction.SetUnitAction(this, MovementActionCoroutine(nowBlock, movementBlock), null);
    }

    private IEnumerator MovementActionCoroutine(FieldBlock nowBlock, FieldBlock movementBlock)
    {
        SetAnimation("Move", true);
        nowBlock.ResetUnitActor();
        movementBlock.SetUnitActor(this, false);

        while (Vector2.Distance(transform.position, movementBlock.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementBlock.transform.position, Random.Range(Setting.MIN_UNIT_MOVEMENT, Setting.MAX_UNIT_MOVEMENT));
            yield return null;
        }

        SetAnimation("Idle", true);
        yield return null;
    }
}
