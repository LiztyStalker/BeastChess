using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IFieldBlock
{
    IUnitActor[] unitActors { get; }

    IUnitActor GetUnitActor();

    Vector2Int coordinate { get; }
    Vector2 position { get; }

    int UnitActorCount(TYPE_TEAM typeTeam, TYPE_UNIT_FORMATION typeUnitFormation);


    bool isMovement { get; }
    bool isRange { get; }
    bool isFormation { get; }

    bool IsHasUnitActor();
    bool IsHasCastleUnitActor();
    bool IsHasGroundUnitActor();
    bool IsEqualUnitActor(IUnitActor uActor);

    void SetUnitActor(IUnitActor unitActor, bool isPosition = true);
    void SetCoordinate(Vector2Int coor);
    void LeaveUnitActor(IUnitActor uActor);


    void SetRangeColor(bool isActive);
    void SetMovementColor(bool isActive);
    void SetFormationColor(bool isActive);

    void CleanUp();

    void Turn();
}
