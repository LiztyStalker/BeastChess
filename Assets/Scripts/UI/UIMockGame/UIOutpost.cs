using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutpost : MonoBehaviour
{

    [SerializeField]
    private UICommanderOutpost _commanderOutpost;

    [SerializeField]
    private UIUnitOutpost _unitOutpost;

    public void Initialize()
    {
        _commanderOutpost.Initialize();
        _unitOutpost.Initialize();
    }

    public void SetChallengeCommander(bool isChallenge)
    {
        _commanderOutpost.SetChallenge(isChallenge);
    }

    public void SetChallengeUnit(bool isChallenge)
    {
        _unitOutpost.SetChallenge(isChallenge);
    }

    public void SetUnitCardAction(bool isAction)
    {
        _unitOutpost.SetUnitCardAction(isAction);
    }

    public void CleanUp()
    {
        _commanderOutpost.CleanUp();
        _unitOutpost.CleanUp();
    }


    #region ##### Listener #####

    public void RefreshCommanderCard(RegionMockGameActor region) => _commanderOutpost.RefreshCommanderCard(region);
    public void RefreshUnits(UnitCard[] unitCards, bool isAction) => _unitOutpost.SetUnitCards(unitCards, isAction);

    public void SetOnUnitListener(System.Action act) => _unitOutpost.SetOnUnitListener(act);
    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _unitOutpost.SetOnUnitInformationListener(act);
    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _commanderOutpost.SetOnSkillInformationListener(act);
    public void SetOnUnitChangeListener(System.Action<TYPE_BATTLE_TEAM, UnitCard> act) => _unitOutpost.SetOnUnitChangeListener(act);
    public void SetOnCommanderDataListener(System.Action<CommanderCard, TYPE_BATTLE_TEAM> act) => _commanderOutpost.SetOnCommanderDataListener(act);

    public void SetOnEnoughListener(System.Func<TYPE_BATTLE_TEAM, UnitCard, bool> act) => _unitOutpost.SetOnEnoughListener(act);

    public void AddOnRefreshListener(System.Action<TYPE_BATTLE_TEAM> act)
    {
        _commanderOutpost.AddOnRefreshListener(act);
        _unitOutpost.AddOnRefreshListener(act);
    }

    public void RemoveOnRefreshListener(System.Action<TYPE_BATTLE_TEAM> act)
    {
        _commanderOutpost.RemoveOnRefreshListener(act);
        _unitOutpost.RemoveOnRefreshListener(act);
    }
    #endregion
}
