using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldBlock : MonoBehaviour, IFieldBlock
{

    private readonly Color COLOR_ORANGE = new Color(1f, 0.5f, 0f);

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] _sprites;

    private List<IUnitActor> _unitActors = new List<IUnitActor>();

    public Vector2Int coordinate { get; private set; }
    public IUnitActor[] unitActors => _unitActors.ToArray();

    public IUnitActor GetUnitActor()
    {
        if (_unitActors.Count > 0)
            return _unitActors[0];
        return null;
    }
    public bool isMovement { get; private set; }
    public bool isRange { get; private set; }
    public bool isFormation { get; private set; }

    public Vector2 position => transform.position;

    public bool IsHasUnitActor() => _unitActors.Count > 0;

    public bool IsHasCastleUnitActor()
    {
        for(int i = 0; i < _unitActors.Count; i++)
        {
            if (_unitActors[i].typeUnit == TYPE_UNIT_FORMATION.Castle) return true;
        }
        return false;
    }

    public bool IsHasGroundUnitActor()
    {
        for (int i = 0; i < _unitActors.Count; i++)
        {
            if (_unitActors[i].typeUnit == TYPE_UNIT_FORMATION.Ground) return true;
        }
        return false;
    }



    public bool IsEqualUnitActor(IUnitActor uActor) 
    {
        for (int i = 0; i < _unitActors.Count; i++)
        {
            if (_unitActors[i] == uActor) return true;
        }
        return false;
    }



    //    public bool IsEmptyInUnitActors() => (_castleActor == null && _unitActors.Count == 0);

    /// <summary>
    /// FieldBlock에 UnitActor를 적용합니다
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="isPosition"></param>
    public void SetUnitActor(IUnitActor unitActor, bool isPosition = true)
    {
        _unitActors.Add(unitActor);
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
        if (isFormation && IsHasUnitActor())
            _renderer.color = Color.blue;
        else if (isMovement && isRange)
            _renderer.color = COLOR_ORANGE;
        else if (isMovement)
            _renderer.color = Color.yellow;
        else if(isRange)
            _renderer.color = Color.red;
        else
            _renderer.color = Color.white;
    }


    public void Initialize()
    {
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    /// <summary>
    /// FieldBlock을 모두 지웁니다
    /// </summary>
    public void CleanUp()
    {
        _unitActors.Clear();
    }

    public void Turn()
    {
        for (int i = 0; i < unitActors.Length; i++)
            unitActors[i].Turn();
    }

    public int UnitActorCount(TYPE_BATTLE_TEAM typeTeam, TYPE_UNIT_FORMATION typeUnitFormation)
    {
        return _unitActors.Where(uActor => uActor.typeTeam == typeTeam && uActor.typeUnit == typeUnitFormation).Count();
    }

}

