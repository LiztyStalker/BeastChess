using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private string name;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private string description;

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

    public void AddState(State state)
    {
        stateList.Add(state);
    }

    public void RemoveState(State state)
    {
        stateList.Remove(state);
    }




}
