using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IFieldBlock
{
    IUnitActor unitActor { get; }
    Vector2Int coordinate { get; }
    Vector2 position { get; }


    bool isMovement { get; }
    bool isRange { get; }
    bool isFormation { get; }

    void SetUnitActor(IUnitActor unitActor, bool isPosition = true);
    void SetCoordinate(Vector2Int coor);
    void LeaveUnitActor(IUnitActor uActor);


    void SetRangeColor(bool isActive);
    void SetMovementColor(bool isActive);
    void SetFormationColor(bool isActive);

    void CleanUp();
}
