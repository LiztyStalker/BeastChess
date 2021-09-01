using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHealth : Status
{
    [SerializeField]
    protected TYPE_STATE_HEALTH _typeStateHealth;

    [SerializeField]
    protected int _turnCount;

    public TYPE_STATE_HEALTH typeStateHealth => _typeStateHealth;

    public int turnCount => _turnCount;

    public StatusHealth(TYPE_VALUE typeValue, float value, TYPE_STATE_HEALTH typeStateHealth, int turnCount) : base(typeValue, value)
    {
        _typeStateHealth = typeStateHealth;
        _turnCount = turnCount;
    }
}
