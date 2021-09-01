using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusValueAttack : Status, IStatusValue
{
    public StatusValueAttack(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}
