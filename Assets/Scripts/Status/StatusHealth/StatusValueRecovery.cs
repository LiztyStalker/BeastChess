using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusValueRecovery : StatusHealth, IStatusHealth
{
    public StatusValueRecovery(TYPE_VALUE typeValue, float value, TYPE_STATE_HEALTH typeStateHealth, int turnCount) : base(typeValue, value, typeStateHealth, turnCount)
    {
    }
}
