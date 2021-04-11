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

    //public int movementValue => _movementValue;

    //public int rangeValue => _rangeValue;

    public TYPE_TEAM typeTeam => _typeTeam;

    private int _nowHealthValue;


    //int _movementValue => _unitData.movementvalue;

    int _costValue => _unitData.costValue;

    public int priorityValue => _unitData.priorityValue;

    //int _rangeValue => _unitData.rangeValue;

    public TYPE_UNIT typeUnit => _unitData.typeUnit;

    public TYPE_UNIT_ATTACK typeUnitAttack => _unitData.typeUnitAttack;

    public Vector2Int[] attackCells => _unitData.attackCells;
    public Vector2Int[] movementCells => _unitData.movementCells;

    public void SetTypeTeam(TYPE_TEAM typeTeam)
    {
        _typeTeam = typeTeam;
        //_renderer.color = GetTeamColor(typeTeam);
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
            _sAnimation.GetComponent<MeshRenderer>().sortingOrder = -(int)transform.position.y;
            _skeleton = _sAnimation.skeleton;
            _skeleton.SetSlotsToSetupPose();
            //_animationState = _sAnimation.state;
            _sAnimation.AnimationState.Event += AttackEvent;
            _sAnimation.AnimationState.SetEmptyAnimation(0, 0f);
            SetAnimation("Idle", true);
        }


        _nowHealthValue = healthValue;


    }

    public void AddBar(UIBar uiBar)
    {
        _uiBar = uiBar;
        _uiBar.transform.SetParent(transform);
        _uiBar.transform.localPosition = Vector3.up * 1.25f;
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
        if (_nowHealthValue - value < 0)
        {
            _nowHealthValue = 0;
            SetAnimation("Dead", false);
        }
        else
            _nowHealthValue -= value;

        //        _renderer.color = Color.Lerp(Color.white, GetTeamColor(_typeTeam), HealthRate());
        _uiBar.SetBar(HealthRate());
    }

    private void SetAnimation(string name, bool loop)
    {
        if (_sAnimation.SkeletonDataAsset != null)
        {
            //Debug.Log(_sAnimation.AnimationState + " " + _sAnimation.GetInstanceID());
            _sAnimation.AnimationState.SetAnimation(0, name, loop);
        }
    }

    public void Dead()
    {
        SetAnimation("Dead", false);
    }




    public class UnitAction : CustomYieldInstruction
    {
        public bool isRunning = false;

        public override bool keepWaiting => isRunning;
                
        IEnumerator enumerator1;
        IEnumerator enumerator2;
        Coroutine coroutine;
        MonoBehaviour mono;

        public UnitAction(MonoBehaviour mono, IEnumerator enumerator1, IEnumerator enumerator2)
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
        var attackDirection = attackCells;

        blocks = fieldManager.GetBlocks(nowBlock.coordinate, attackDirection, typeTeam);

        //공격 사거리 이내에 적이 1기라도 있으면 공격패턴
        if (blocks.Length > 0)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].unitActor != null && blocks[i].unitActor.typeTeam != typeTeam)
                {
                    SetAnimation("Attack", false);
                    _nowAttackCount = attackCount;
                    yield break;
                }
            }
        }
        else{
            _unitAction.isRunning = false;
        }

        yield return null;
    }

    
    public void AttackEvent(TrackEntry trackEntry, Spine.Event e)
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
                }

                break;
            case TYPE_UNIT_ATTACK.Range:
                attackBlocks = blocks;
                break;
            case TYPE_UNIT_ATTACK.Priority:
                List<FieldBlock> list = new List<FieldBlock>();
                list.AddRange(blocks);
                list.Sort((a1, a2) => {
                    if (a2.unitActor != null && a1.unitActor != null)
                        return a2.unitActor.priorityValue - a1.unitActor.priorityValue;
                    return 0;
                });

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].unitActor != null && !list[i].unitActor.IsDead())
                    {
                        attackBlocks[0] = list[i];
                        break;
                    }
                }
                break;
        }


        //공격횟수가 남았고 적이 있으면
        //반복

        for (int index = 0; index < attackBlocks.Length; index++)
        {
            var attackBlock = attackBlocks[index];
            //공격 애니메이션

            //횟수만큼 공격
            if (attackBlock != null)
            {
                if (attackBlock.unitActor != null)
                {
                    if (attackBlock.unitActor.typeUnit == TYPE_UNIT.Castle)
                        gameTestManager.IncreaseHealth(damageValue, typeTeam);
                    else
                    {
                        attackBlock.unitActor.IncreaseHealth(damageValue);
                    }
                }
            }
        }

        if (_nowAttackCount > 0)
        {
            _unitAction.isRunning = true;
            SetAnimation("Attack", false);
            _nowAttackCount--;
        }
        else
        {
            _unitAction.isRunning = false;
            SetAnimation("Idle", true);
        }
    }


   

    UnitAction _unitAction;

    private IEnumerator AttackEvent()
    {
        yield return new WaitUntil(() => !_unitAction.isRunning);
    }
    public IEnumerator ActionAttack(FieldManager fieldManager, GameTestManager gameTestManager)
    {
        this.gameTestManager = gameTestManager;
        _unitAction = new UnitAction(this, ActionAttackCoroutine(fieldManager, gameTestManager), AttackEvent());
        yield return _unitAction;
    }


}
