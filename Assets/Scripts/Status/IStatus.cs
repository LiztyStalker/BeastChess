using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    Status.TYPE_VALUE typeValue { get; }
    float value { get; }

}