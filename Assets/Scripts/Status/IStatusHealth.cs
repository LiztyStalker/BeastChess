using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusHealth : IStatus
{
    Status.TYPE_STATE_HEALTH typeStateHealth { get; }

}
