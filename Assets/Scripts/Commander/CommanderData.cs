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
    private string _key;

    [SerializeField]
    private Sprite _icon;
     
    [SerializeField]
    private TribeData _tribeData;

    [SerializeField]
    private string _tribeDataKey;

    [SerializeField]
    private TYPE_COMMANDER_MASTER _typeCommanderMaster;

    [SerializeField]
    private TYPE_INFLUENCE _typeInfluence;

    [SerializeField]
    private int _leadershipValue;

    [SerializeField]
    private int _leadershipIncreaseValue;

    [SerializeField]
    private SkillData[] _skills;

    [SerializeField]
    private string[] _skillKeys;

    [SerializeField]
    private bool _isAppearBarracks = true;


    [Header("Cost")]
    [SerializeField]
    private int _costValue;

    [Header("MaintanenceValue")]
    [SerializeField]
    private int _maintanenceValue;




    #region ##### Getter Setter #####
    public string Key => _key;
    public string CommanderName => TranslatorStorage.Instance.GetTranslator<CommanderData>(Key, "Name");
    public Sprite icon => _icon;
    public TribeData tribeData {
        get
        {
            if (_tribeData == null)
            {
                _tribeData = DataStorage.Instance.GetDataOrNull<TribeData>(_tribeDataKey);
            }
            return _tribeData;

        }
    }
    public TYPE_COMMANDER_MASTER typeCommanderMaster => _typeCommanderMaster;
    public TYPE_INFLUENCE typeInfluence => _typeInfluence;
    public int leadershipValue => _leadershipValue;
    public int leadershipIncreaseValue => _leadershipIncreaseValue;
    public SkillData[] skills
    {
        get
        {
            if (_skills == null)
            {
                if (_skillKeys != null)
                {
                    _skills = new SkillData[_skillKeys.Length];
                    for (int i = 0; i < _skillKeys.Length; i++)
                    {
                        _skills[i] = DataStorage.Instance.GetDataOrNull<SkillData>(_skillKeys[i]);
                    }
                }
            }
            return _skills;
        }
    }
    public int costValue => _costValue;
    public int maintanenceValue => _maintanenceValue;

    public bool IsAppearBarracks => _isAppearBarracks;

    #endregion
}
