using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusValue : IStatus
{
    StatusValue.TYPE_VALUE typeValue { get; }
    float value { get; }
}
