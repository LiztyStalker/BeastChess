using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateValueAttack : State, IStateValue
{
    public StateType stateType { get; private set; }

    public float value { get; private set; }


}
