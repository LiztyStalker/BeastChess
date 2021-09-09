#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_FieldBlock : IFieldBlock
{
    private List<IUnitActor> unitActors = new List<IUnitActor>();

    private Vector2 _position;

    public Vector2Int coordinate { get; private set; }

    public IUnitActor unitActor => GetUnitActor(TYPE_UNIT_FORMATION.Ground);

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
        unitActors.Add(unitActor);
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
        unitActors.Clear();
    }

    /// <summary>
    /// UnitActor�� ����ִ��� Ȯ���մϴ�
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty() => unitActors.Count == 0;

    /// <summary>
    /// ��ϵ� ��� UnitActor�� �����ɴϴ�
    /// ������ ����Ʈ�� 0���� ����ֽ��ϴ�
    /// </summary>
    /// <returns></returns>
    public IUnitActor[] GetUnitActors() => unitActors.ToArray();

    /// <summary>
    /// �ش� ��ġ�� ������ �����ɴϴ�
    /// ������ null�� ��ȯ�մϴ�
    /// </summary>
    /// <param name="typeUnitFormation"></param>
    /// <returns></returns>
    public IUnitActor GetUnitActor(TYPE_UNIT_FORMATION typeUnitFormation = TYPE_UNIT_FORMATION.Ground)
    {
        for (int i = 0; i < unitActors.Count; i++)
        {
            if (unitActors[i].typeUnit == typeUnitFormation)
                return unitActors[i];
        }
        return null;
    }

    public void ResetRange()
    {
    }

    public void ResetMovement()
    {
    }

    public void ResetFormation()
    {
    }

    public void SetRange()
    {
    }

    public void SetMovement()
    {
    }

    public void SetFormation()
    {
    }

    public void LeaveUnitActor(IUnitActor uActor)
    {
    }
}

#endif