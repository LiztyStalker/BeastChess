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
    private TribeData _tribeData;

    [SerializeField]
    private TYPE_COMMANDER_MASTER _typeCommanderMaster;

    [SerializeField]
    private TYPE_INFLUENCE _typeInfluence;

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

    public string key => name;
    public string title => _name;
    public Sprite icon => _icon;
    public TribeData tribeData => _tribeData;
    public TYPE_COMMANDER_MASTER typeCommanderMaster => _typeCommanderMaster;
    public TYPE_INFLUENCE typeInfluence => _typeInfluence;
    public int leadershipValue => _leadershipValue;
    public int leadershipIncreaseValue => _leadershipIncreaseValue;
    public SkillData[] skills => _skills.ToArray();
    public int costValue => _costValue;
    public int maintanenceValue => _maintanenceValue;
}
