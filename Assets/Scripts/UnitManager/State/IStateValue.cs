using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateValue : IState
{
    State.StateType stateType { get; }
    float value { get; }
}
