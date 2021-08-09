using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

    [Header("Attack")]
    [SerializeField]
    TYPE_UNIT_ATTACK _typeUnitAttack;

    [SerializeField]
    TYPE_UNIT_ATTACK_RANGE _typeUnitAttackRange;


    [SerializeField]
    private int activateTurn;

    [SerializeField]
    private List<State> stateList = new List<State>();

    public List<State> GetStateList() => stateList;


    public new string name => _name;
        
    public Sprite icon => _icon;

    public string description => _description;



    public void AddState(State state)
    {
        stateList.Add(state);
    }

    public void RemoveState(State state)
    {
        stateList.Remove(state);
    }




}
