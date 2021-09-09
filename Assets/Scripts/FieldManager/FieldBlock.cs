using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlock : MonoBehaviour, IFieldBlock
{
    [SerializeField]
    private SpriteRenderer _renderer;

    private IUnitActor _castleActor;

    private List<IUnitActor> _unitActors = new List<IUnitActor>();

    public Vector2Int coordinate { get; private set; }
    public IUnitActor unitActor
    {
        get
        {
            if (_castleActor != null)
            {
                return _castleActor;
            }
            else
            {
                if (_unitActors.Count > 0)
                    return _unitActors[0];
            }
            return null;
        }
    }
    public bool isMovement { get; private set; }
    public bool isRange { get; private set; }
    public bool isFormation { get; private set; }

    public Vector2 position => transform.position;

    /// <summary>
    /// FieldBlock�� UnitActor�� �����մϴ�
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="isPosition"></param>
    public void SetUnitActor(IUnitActor unitActor, bool isPosition = true)
    {
        if (unitActor.typeUnit == TYPE_UNIT_FORMATION.Castle)
            _castleActor = unitActor;
        else
        {
            _unitActors.Add(unitActor);
        }

        if(isPosition)
            unitActor.SetPosition(transform.position);
    }

    /// <summary>
    /// FieldBlock�� ��ǥ�� �����մϴ�
    /// </summary>
    /// <param name="coor"></param>
    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }

    /// <summary>
    /// UnitActor�� FieldBlock�� �����ϴ�
    /// </summary>
    /// <param name="uActor"></param>
    public void LeaveUnitActor(IUnitActor uActor)
    {
        if (_unitActors.Contains(uActor))
            _unitActors.Remove(uActor);
    }


    /// <summary>
    /// ��Ÿ� ��� ������ �����մϴ�
    /// </summary>
    public void SetRangeColor(bool isActive)
    {
        isRange = isActive;
        SetBlockColor();
    }

    /// <summary>
    /// �̵� ��� ������ �����մϴ�
    /// </summary>
    public void SetMovementColor(bool isActive)
    {
        isMovement = isActive;
        SetBlockColor();
    }

    /// <summary>
    /// ��ġ ��� ������ �����մϴ�
    /// </summary>
    public void SetFormationColor(bool isActive)
    {
        isFormation = isActive;
        SetBlockColor();
    }

    private void SetBlockColor()
    {
        if (isFormation && unitActor != null)
            _renderer.color = Color.magenta;
        else if (isMovement && isRange)
            _renderer.color = Color.green;
        else if (isMovement)
            _renderer.color = Color.yellow;
        else if(isRange)
            _renderer.color = Color.red;
        else
            _renderer.color = Color.white;
    }

    /// <summary>
    /// FieldBlock�� ��� ����ϴ�
    /// </summary>
    public void CleanUp()
    {
        _castleActor = null;
        _unitActors.Clear();
    }
}

