using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderCard
{

    private const int COMMANDER_EXP_VALUE = 1000;
    private const int COMMANDER_MAX_LEVEL = 9;

    private CommanderData _commanderData;

    public Sprite Icon => _commanderData.icon;
    public string name => _commanderData.name;
    public string CommanderName => _commanderData.CommanderName;
    public string TribeName => _commanderData.tribeData.TribeName;

    public TYPE_INFLUENCE TypeInfluence => _commanderData.typeInfluence;
    public TYPE_COMMANDER_MASTER TypeCommanderMaster => _commanderData.typeCommanderMaster;

    public int nowLeadershipValue => _commanderData.leadershipValue;
    public int maxLeadershipValue => _commanderData.leadershipValue + levelValue * _commanderData.leadershipIncreaseValue;

    public SkillData[] skills => _commanderData.skills;
    public int costValue => _commanderData.costValue;
    public int maintanenceValue => _commanderData.maintanenceValue;

       
    private int _levelValue = 1;
    private int _nowExpValue = 0;

    public int levelValue => _levelValue;

    public int nowExpValue => _nowExpValue;

    public int maxExpValue => _levelValue * COMMANDER_EXP_VALUE;

    public float expValueRate => nowExpValue / (float)maxExpValue;


    public static CommanderCard Create(CommanderData commanderData, int levelValue = 1)
    {
        return new CommanderCard(commanderData, levelValue);
    }

    private CommanderCard(CommanderData commanderData, int levelValue)
    {
        _commanderData = commanderData;
        _levelValue = levelValue;
        _nowExpValue = 0;
    }

    public void IncreaseExpValue(int value)
    {
        _nowExpValue += value;
        if(_nowExpValue / maxExpValue >= 1)
        {
            while(_nowExpValue > maxExpValue)
            {
                _nowExpValue -= maxExpValue;
                AddLevel();
            }
        }
    }

    public void AddLevel()
    {
        _levelValue++;
    }

    
    
}
