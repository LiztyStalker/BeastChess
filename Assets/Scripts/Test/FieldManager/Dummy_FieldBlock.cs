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
    /// UnitActor�� �����մϴ�
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="isPosition"></param>
    public void SetUnitActor(IUnitActor unitActor, bool isPosition = true)
    {
        _unitActors.Add(unitActor);
    }
    /// <summary>
    /// ��ǥ�� ���դ��ϴ�
    /// </summary>
    /// <param name="coor"></param>
    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }
    /// <summary>
    /// UnitActor�� ��� ���ϴ�
    /// </summary>
    public void LeaveUnitActor()
    {
        _unitActors.Clear();
    }

    /// <summary>
    /// UnitActor�� ����ִ��� Ȯ���մϴ�
    /// </summary>
    /// <returns></returns>
    //public bool IsEmptyInUnitActors() => unitActors.Count == 0;

    /// <summary>
    /// ��ϵ� ��� UnitActor�� �����ɴϴ�
    /// ������ ����Ʈ�� 0���� ����ֽ��ϴ�
    /// </summary>
    /// <returns></returns>
    public IUnitActor[] GetUnitActors() => _unitActors.ToArray();

    /// <summary>
    /// �ش� ��ġ�� ������ �����ɴϴ�
    /// ������ null�� ��ȯ�մϴ�
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