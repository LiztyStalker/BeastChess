using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TYPE_COMMANDER_MASTER { Infantry, Shooter, Charger, Supporter}

[CreateAssetMenu(fileName = "CommanderData", menuName = "ScriptableObjects/CommanderData")]
public class CommanderData : ScriptableObject
{

    [Header("Common")]
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private TYPE_COMMANDER_MASTER _typeCommanderMaster;

    [SerializeField]
    private int _leadershipValue;

    [SerializeField]
    private int _leadershipIncreaseValue;

    [SerializeField]
    private List<SkillData> _skills;


    [Header("Cost")]
    [SerializeField]
    private int _costValue;

    [Header("MaintanenceValue")]
    [SerializeField]
    private int _maintanenceValue;

    public new string name => _name;
    public Sprite icon => _icon;
    public TYPE_COMMANDER_MASTER typeCommanderMaster => _typeCommanderMaster;
    public int leadershipValue => _leadershipValue;
    public int leadershipIncreaseValue => _leadershipIncreaseValue;
    public SkillData[] skills => _skills.ToArray();
    public int costValue => _costValue;
    public int maintanenceValue => _maintanenceValue;
}
