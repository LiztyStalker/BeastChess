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

    public void SetChallenge(bool isChallenge)
    {
        _commanderOutpost.SetChallenge(isChallenge);
        _unitOutpost.SetChallenge(isChallenge);
    }

    public void CleanUp()
    {
        _commanderOutpost.CleanUp();
        _unitOutpost.CleanUp();
    }


    #region ##### Listener #####

    public void RefreshCommanderCard(RegionMockGameActor region) => _commanderOutpost.RefreshCommanderCard(region);
    public void RefreshUnits(UnitCard[] unitCards) => _unitOutpost.SetUnitCards(unitCards);

    public void SetOnUnitListener(System.Action act) => _unitOutpost.SetOnUnitListener(act);
    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _unitOutpost.SetOnUnitInformationListener(act);
    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _commanderOutpost.SetOnSkillInformationListener(act);
    public void SetOnUnitChangeListener(System.Action<TYPE_TEAM, UnitCard> act) => _unitOutpost.SetOnUnitChangeListener(act);
    public void SetOnCommanderDataListener(System.Action<CommanderCard, TYPE_TEAM> act) => _commanderOutpost.SetOnCommanderDataListener(act);

    public void AddOnRefreshListener(System.Action<TYPE_TEAM> act)
    {
        _commanderOutpost.AddOnRefreshListener(act);
        _unitOutpost.AddOnRefreshListener(act);
    }

    public void RemoveOnRefreshListener(System.Action<TYPE_TEAM> act)
    {
        _commanderOutpost.RemoveOnRefreshListener(act);
        _unitOutpost.RemoveOnRefreshListener(act);
    }
    #endregion
}
