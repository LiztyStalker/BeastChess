using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateValueAttack : State, IStateValue
{
    public StateValueAttack(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}
