using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IFieldBlock
{
    Vector2Int coordinate { get; }
    IUnitActor unitActor { get; }
    IUnitActor castleActor { get; }
    bool isMovement { get; }
    bool isRange { get; }
    bool isFormation { get; }

    void SetUnitActor(IUnitActor unitActor, bool isPosition = true);
    void SetCoordinate(Vector2Int coor);

    void ResetUnitActor();
    void ResetRange();
    void ResetMovement();
    void ResetFormation();

    void SetRange();
    void SetMovement();
    void SetFormation();

    Vector2 position { get; }
}
