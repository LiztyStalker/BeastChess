using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlock : MonoBehaviour
{
    public Vector2Int coordinate { get; private set; }
    public UnitActor unitActor { get; private set; }

    public void SetUnitActor(UnitActor unitActor)
    {
        this.unitActor = unitActor;
        unitActor.transform.position = transform.position;
    }

    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }

    public void ResetUnitActor()
    {
        unitActor = null;
    }
}
