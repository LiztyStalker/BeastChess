using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusHealth : IStatus
{
    StatusValue.TYPE_STATE_HEALTH typeStateHealth { get; }

}
