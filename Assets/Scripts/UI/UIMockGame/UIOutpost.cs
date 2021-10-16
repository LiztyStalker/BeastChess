using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOutpost : MonoBehaviour
{

    [SerializeField]
    UICommanderOutpost _commanderOutpost;

    [SerializeField]
    UIUnitOutpost _unitOutpost;

    public void Initialize()
    {
        _commanderOutpost.Initialize();
        _unitOutpost.Initialize();
        _unitOutpost.SetOnRefreshCostValueListener(_commanderOutpost.RefreshCost);
    }

    public void CleanUp()
    {
        _commanderOutpost.CleanUp();
        _unitOutpost.CleanUp();
    }

    public void SetOnUnitListener(System.Action act) => _unitOutpost.SetOnUnitListener(act);
    public void SetOnUnitInformationListener(System.Action<UnitCard> act) => _unitOutpost.SetOnUnitInformationListener(act);
    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _commanderOutpost.SetOnSkillInformationListener(act);
}
