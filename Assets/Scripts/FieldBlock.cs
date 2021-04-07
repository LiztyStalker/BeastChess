using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBlock : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    public Vector2Int coordinate { get; private set; }
    public UnitActor unitActor { get; private set; }

    public bool isMovement { get; private set; }
    public bool isRange { get; private set; }

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

    public void ResetRange()
    {   
        isRange = false;
        SetBlockColor();
    }
    public void ResetMovement()
    {
        isMovement = false;
        SetBlockColor();
    }
    public void SetRange()
    {
        isRange = true;
        SetBlockColor();
    }
    public void SetMovement()
    {
        isMovement = true;
        SetBlockColor();
    }

    private void SetBlockColor()
    {
        if(isMovement && isRange)
            _renderer.color = Color.green;
        else if (isMovement)
            _renderer.color = Color.yellow;
        else if(isRange)
            _renderer.color = Color.red;
        else
            _renderer.color = Color.white;
    }
}
