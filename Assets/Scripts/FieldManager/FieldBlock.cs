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
    /// FieldBlock에 UnitActor를 적용합니다
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
    /// FieldBlock의 좌표를 설정합니다
    /// </summary>
    /// <param name="coor"></param>
    public void SetCoordinate(Vector2Int coor)
    {
        coordinate = coor;
    }

    /// <summary>
    /// UnitActor가 FieldBlock을 떠납니다
    /// </summary>
    /// <param name="uActor"></param>
    public void LeaveUnitActor(IUnitActor uActor)
    {
        if (_unitActors.Contains(uActor))
            _unitActors.Remove(uActor);
    }


    /// <summary>
    /// 사거리 블록 색상을 지정합니다
    /// </summary>
    public void SetRangeColor(bool isActive)
    {
        isRange = isActive;
        SetBlockColor();
    }

    /// <summary>
    /// 이동 블록 색상을 지정합니다
    /// </summary>
    public void SetMovementColor(bool isActive)
    {
        isMovement = isActive;
        SetBlockColor();
    }

    /// <summary>
    /// 배치 블록 색상을 지정합니다
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
    /// FieldBlock을 모두 지웁니다
    /// </summary>
    public void CleanUp()
    {
        _castleActor = null;
        _unitActors.Clear();
    }
}

