using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    State.TYPE_VALUE typeValue { get; }
    float value { get; }

}
