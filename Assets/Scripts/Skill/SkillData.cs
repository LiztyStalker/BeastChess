using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TYPE_SKILL_RANGE { All, MineRange, TargetRange}

public enum TYPE_MINE { Mine, NotMine}

public enum TYPE_TARGET_TEAM { All, Alies, Enemy}

public enum TYPE_SKILL_TARGET { Random, Priority}


[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

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
    private int _turnCount;

    [SerializeField]
    private List<State> _stateList = new List<State>();

    //Runtime에서 State 하위 클래스를 가져오지 못함
    //삽입은 하위 클래스가 가능


    public IState[] GetStateArray() {
        for(int i = 0; i < _stateList.Count; i++)
        {
            Debug.Log(_stateList[i].GetType().Name);
        }
        return _stateList.ToArray();
    }


    public new string name => _name;
        
    public Sprite icon => _icon;

    public string description => _description;



    public void AddState(State state)
    {
        Debug.Log(state.GetType().Name);
        _stateList.Add(state);
    }

    public void RemoveState(State state)
    {
        _stateList.Remove(state);
    }

    public void RemoveAt(int index)
    {
        _stateList.RemoveAt(index);
    }




}
