using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TYPE_SKILL_LIFE_SPAN { Always, Turn, Support}

public enum TYPE_SKILL_ACTIVATE { Passive, Active}

public enum TYPE_SKILL_RANGE { All, MineRange, TargetRange}

public enum TYPE_MINE { Mine, NotMine}

public enum TYPE_TARGET_TEAM { All, Alies, Enemy}

public enum TYPE_SKILL_TARGET { Random, Priority}


[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

    [Header("LifeSpan")]

    [SerializeField]
    private TYPE_SKILL_LIFE_SPAN _typeSkillLifeSpan;

    [SerializeField]
    private int _turnCount;

    [SerializeField]
    private TYPE_SKILL_ACTIVATE _typeSkillActivate;

    [SerializeField]
    private TYPE_SKILL_RANGE _typeSkillRange;

    [SerializeField]
    private int _skillRangeValue = 1;

    [SerializeField]
    private TYPE_SKILL_TARGET _typeSkillTarget;

    [SerializeField]
    private TYPE_MINE _typeMine;

    [SerializeField]
    private TYPE_TARGET_TEAM _typeTargetTeam;

    [SerializeField, Range(0f, 1f)]
    private float _skillActivateRate = 0.2f;

    [SerializeField]
    private bool _isOverlapped = false;

    [SerializeField]
    private bool _isMine = false;


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

    public int turnCount
    {
        get
        {
            if (_typeSkillLifeSpan == TYPE_SKILL_LIFE_SPAN.Turn)
                return turnCount;
            return -1;
        }
    }

    public bool isMine => _isMine;

    public bool isOverlapped => _isOverlapped;

    public void Calculate<T>(ref float rate, ref int value) where T : IState
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
                Debug.Log(state.GetType().Name + " " + typeof(T).Name);
                if (state is T)
                {
                    switch (state.typeValue)
                    {
                        case State.TYPE_VALUE.Value:
                            value += (int)state.value;
                            break;
                        case State.TYPE_VALUE.Rate:
                            rate += state.value;
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
