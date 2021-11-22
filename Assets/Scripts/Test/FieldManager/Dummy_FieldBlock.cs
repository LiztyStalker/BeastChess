#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_FieldBlock : IFieldBlock
{
    private List<IUnitActor> _unitActors = new List<IUnitActor>();

    private Vector2 _position;

    public Vector2Int coordinate { get; private set; }

    public IUnitActor[] unitActors => _unitActors.ToArray();

    public IUnitActor GetUnitActor()
    {
        if (unitActors.Length > 0)
            return unitActors[0];
        return null;
    }


    public bool isMovement => false;

    public bool isRange => false;

    public bool isFormation => false;

    public Vector2 position => _position;

    /// <summary>
    /// UnitActor를 설정합니다
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="isPosition"></param>
    public void SetUnitActor(IUnitActor unitActor, bool isPosition = true)
    {
        _unitActors.Add(unitActor);
    }
    /// <summary>
    /// 좌표를 정합ㅎ니다
    /// </summary>
    /// <param name="coor"></param>
    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }
    /// <summary>
    /// UnitActor를 모두 비웁니다
    /// </summary>
    public void LeaveUnitActor()
    {
        _unitActors.Clear();
    }

    /// <summary>
    /// UnitActor가 비어있는지 확인합니다
    /// </summary>
    /// <returns></returns>
    //public bool IsEmptyInUnitActors() => unitActors.Count == 0;

    /// <summary>
    /// 등록된 모든 UnitActor를 가져옵니다
    /// 없으면 리스트가 0으로 비어있습니다
    /// </summary>
    /// <returns></returns>
    public IUnitActor[] GetUnitActors() => _unitActors.ToArray();

    /// <summary>
    /// 해당 위치의 유닛을 가져옵니다
    /// 없으면 null을 반환합니다
    /// </summary>
    /// <param name="typeUnitFormation"></param>
    /// <returns></returns>
    public IUnitActor GetUnitActor(TYPE_UNIT_FORMATION typeUnitFormation = TYPE_UNIT_FORMATION.Ground)
    {
        for (int i = 0; i < _unitActors.Count; i++)
        {
            if (_unitActors[i].typeUnit == typeUnitFormation)
                return _unitActors[i];
        }
        return null;
    }


    public void SetRangeColor(bool isActive)
    {
    }

    public void SetMovementColor(bool isActive)
    {
    }

    public void SetFormationColor(bool isActive)
    {
    }

    public void LeaveUnitActor(IUnitActor uActor)
    {
        if (_unitActors.Contains(uActor))
            _unitActors.Remove(uActor);
    }

    public void CleanUp()
    {
        for(int i = 0; i < _unitActors.Count; i++)
        {
            _unitActors[i].CleanUp();
        }
    }

    public bool IsHasUnitActor()
    {
        throw new System.NotImplementedException();
    }

    public void Turn()
    {
        throw new System.NotImplementedException();
    }

    public bool IsEqualUnitActor(IUnitActor uActor)
    {
        throw new System.NotImplementedException();
    }

    public int UnitActorCount(TYPE_TEAM typeTeam, TYPE_UNIT_FORMATION typeUnitFormation)
    {
        throw new System.NotImplementedException();
    }

    public bool IsHasCastleUnitActor()
    {
        throw new System.NotImplementedException();
    }

    public bool IsHasGroundUnitActor()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }
}

#endif