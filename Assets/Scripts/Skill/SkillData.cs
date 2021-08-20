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



    [Header("�ߵ�����")]
    [Tooltip("[Passive] - �׻� ��ų�� �ߵ��˴ϴ�\n[PreActive] - ������ ���۵ɶ� �ߵ��մϴ�\n[Active] - ���� ���� ���߿� ���ݿ� ���ؼ� �ߵ��˴ϴ�")]
    [SerializeField]
    private TYPE_SKILL_ACTIVATE _typeSkillActivate;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("��ų ���� Ȯ���Դϴ�")]
    private float _skillActivateRate = 0.2f;


    [Header("�����ֱ�")]
    [Tooltip("[Always] - ��� �����˴ϴ�\n[Turn] - TurnCount��ŭ ����˴ϴ�. ���� ��ų�� ������ TurnCount�� �ʱ�ȭ �˴ϴ�\n[Caster] - �����ڰ� ����ϸ� �������� �ʽ��ϴ�")]
    [SerializeField]
    private TYPE_SKILL_LIFE_SPAN _typeSkillLifeSpan;

    [Tooltip("[n = 0] - ����\n[n > 0] - n ��ŭ �� ī��Ʈ ����")]
    [SerializeField, Range(0, 100)]
    private int _turnCount;
    

    [Header("��ø����")]
    [SerializeField]
    [Tooltip("��ø ���θ� �����մϴ�")]
    private bool _isOverlapped = false;

    [SerializeField, Range(0, 100)]
    [Tooltip("[n = 0] - �������� ��ø�� �����մϴ�\n[n > 0] - �ִ� n��ŭ ��ø�˴ϴ�")]
    private int _overlapCount = 0;


    [Header("����")]
    [SerializeField]
    [Tooltip("[All] - �� ��ü ���� \n[MyselfRange] - �ڽ�\n[UnitGroupRange] Ư�� ���� ���� �켱- \n[UnitClassRange] - Ư�� ���� ���� �켱\n[AscendingPriorityRange] - ���� ���赵 �켱\n[DecendingPriorityRange] - ���� ���赵 �켱\n")]
    private TYPE_SKILL_RANGE _typeSkillRange;

    [SerializeField]
    [Tooltip("�ڽ��� �������� �����մϴ�")]
    private bool _isMyself = false;

    [SerializeField]
    [Tooltip("��ų ������ �����մϴ�\n[n = 0] - Ư�� ���ָ�\n[n > 0] - n ����")]
    private int _skillRangeValue = 1;
    
    [SerializeField]
    private TYPE_UNIT_GROUP _typeUnitGroup;

    [SerializeField]
    private TYPE_UNIT_CLASS _typeUnitClass;
        
    [Header("��ǥ")]
    [SerializeField]
    private TYPE_TARGET_TEAM _typeTargetTeam;
    



    [Header("�����̻�")]
    [SerializeField]
    private List<StateSerializable> _editorStateList = new List<StateSerializable>();

    [System.NonSerialized]
    private List<IState> _stateList = null;

    //Runtime���� State ���� Ŭ������ �������� ����
    //Unity�� abstract�� �������� ���� (���)
    //������ ���� Ŭ������ ����


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
