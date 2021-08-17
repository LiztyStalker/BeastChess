using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateValue : IState
{
    float value { get; }
}
