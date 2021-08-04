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

    public void SetOnUnitListener(System.Action act) => _unitOutpost.SetOnUnitListener(act);
}
