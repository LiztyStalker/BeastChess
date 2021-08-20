using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TYPE_SKILL_LIFE_SPAN { Always, Turn, Caster}

public enum TYPE_SKILL_ACTIVATE { Passive, PreActive, Active}

public enum TYPE_SKILL_RANGE { All, MyselfRange, UnitGroupRange, UnitClassRange, AscendingPriorityRange, DecendingPriorityRange}

public enum TYPE_TARGET_TEAM { All, Alies, Enemy}

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;



    [Header("발동조건")]
    [Tooltip("[Passive] - 항상 스킬이 발동됩니다\n[PreActive] - 전투가 시작될때 발동합니다\n[Active] - 전투 진행 도중에 공격에 의해서 발동됩니다")]
    [SerializeField]
    private TYPE_SKILL_ACTIVATE _typeSkillActivate;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("스킬 시전 확률입니다")]
    private float _skillActivateRate = 0.2f;


    [Header("생명주기")]
    [Tooltip("[Always] - 계속 유지됩니다\n[Turn] - TurnCount만큼 진행됩니다. 같은 스킬을 받으면 TurnCount가 초기화 됩니다\n[Caster] - 시전자가 사망하면 유지되지 않습니다")]
    [SerializeField]
    private TYPE_SKILL_LIFE_SPAN _typeSkillLifeSpan;

    [Tooltip("[n = 0] - 무한\n[n > 0] - n 만큼 턴 카운트 진행")]
    [SerializeField, Range(0, 100)]
    private int _turnCount;
    

    [Header("중첩여부")]
    [SerializeField]
    [Tooltip("중첩 여부를 결정합니다")]
    private bool _isOverlapped = false;

    [SerializeField, Range(0, 100)]
    [Tooltip("[n = 0] - 무한으로 중첩이 가능합니다\n[n > 0] - 최대 n만큼 중첩됩니다")]
    private int _overlapCount = 0;


    [Header("범위")]
    [SerializeField]
    [Tooltip("[All] - 맵 전체 범위 \n[MyselfRange] - 자신\n[UnitGroupRange] 특정 유닛 병종 우선- \n[UnitClassRange] - 특정 유닛 병과 우선\n[AscendingPriorityRange] - 높은 위험도 우선\n[DecendingPriorityRange] - 낮은 위험도 우선\n")]
    private TYPE_SKILL_RANGE _typeSkillRange;

    [SerializeField]
    [Tooltip("자신을 포함할지 결정합니다")]
    private bool _isMyself = false;

    [SerializeField]
    [Tooltip("스킬 범위를 지정합니다\n[n = 0] - 특정 유닛만\n[n > 0] - n 범위")]
    private int _skillRangeValue = 1;
    
    [SerializeField]
    private TYPE_UNIT_GROUP _typeUnitGroup;

    [SerializeField]
    private TYPE_UNIT_CLASS _typeUnitClass;
        
    [Header("목표")]
    [SerializeField]
    private TYPE_TARGET_TEAM _typeTargetTeam;
    



    [Header("상태이상")]
    [SerializeField]
    private List<StateSerializable> _editorStateList = new List<StateSerializable>();

    [System.NonSerialized]
    private List<IState> _stateList = null;

    //Runtime에서 State 하위 클래스를 가져오지 못함
    //Unity는 abstract를 가져오지 못함 (상속)
    //삽입은 하위 클래스가 가능


    public IState[] GetStateArray() {
        if(_stateList == null && _editorStateList.Count > 0)
        {
            _stateList = new List<IState>();
            Initialize();
        }
        return _stateList.ToArray();
    }

    private void Initialize()
    {
        for(int i = 0; i < _editorStateList.Count; i++)
        {
            _stateList.Add(_editorStateList[i].ConvertState());
        }
    }


    public string key => name;
        
    public Sprite icon => _icon;

    public TYPE_SKILL_ACTIVATE typeSkillActivate => _typeSkillActivate;

    public float skillActivateRate => _skillActivateRate;

    public TYPE_SKILL_LIFE_SPAN typeSkillLifeSpan => _typeSkillLifeSpan;

    public bool isMyself => _isMyself;

    public int skillRangeValue => _skillRangeValue;

    public TYPE_UNIT_GROUP typeUnitGroup => _typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _typeUnitClass;

    public TYPE_TARGET_TEAM typeTargetTeam => _typeTargetTeam;

    public int turnCount
    {
        get
        {
            if (_typeSkillLifeSpan == TYPE_SKILL_LIFE_SPAN.Turn)
                return turnCount;
            return -1;
        }
    }


    public bool isOverlapped => _isOverlapped;

    public int overlapCount => _overlapCount;

    public TYPE_SKILL_RANGE typeSkillRange => _typeSkillRange;

    public void Calculate<T>(ref float rate, ref int value, int overlapCount) where T : IState
    {


        if (_stateList == null && _editorStateList.Count > 0)
        {
            _stateList = new List<IState>();
            Initialize();
        }

        if (_stateList != null)
        {
            for (int i = 0; i < _stateList.Count; i++)
            {
                var state = _stateList[i];
                //Debug.Log(state.GetType().Name + " " + typeof(T).Name);
                if (state is T)
                {
                    switch (state.typeValue)
                    {
                        case State.TYPE_VALUE.Value:
                            value += (int)state.value * overlapCount;
                            break;
                        case State.TYPE_VALUE.Rate:
                            rate += state.value * overlapCount;
                            break;
                    }
                }
            }
        }

    }


#if UNITY_EDITOR

    public void AddState(StateSerializable state)
    {
        Debug.Log(state.GetType().Name);
        _editorStateList.Add(state);
    }

    public void RemoveState(StateSerializable state)
    {
        _editorStateList.Remove(state);
    }

    public void RemoveAt(int index)
    {
        _editorStateList.RemoveAt(index);
    }

#endif



}
