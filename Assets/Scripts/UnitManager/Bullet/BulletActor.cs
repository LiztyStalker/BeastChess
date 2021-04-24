using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletActor : MonoBehaviour
{

    UnitActor _unitActor;

    FieldBlock _targetBlock;

    float _moveSpeed = 1f;

    Rigidbody2D _rigidbody;

    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
    }

    public void SetData(UnitActor unitActor, FieldBlock targetBlock, float movement)
    {
        _unitActor = unitActor;
        _targetBlock = targetBlock;
        _moveSpeed = movement;

        var direction = targetBlock.transform.position - _unitActor.transform.position;
        var radian = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.Rotate(new Vector3(0f, 0f, radian));

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targetBlock.transform.position) > .1f) {
            transform.position = Vector2.MoveTowards(transform.position, _targetBlock.transform.position, Setting.BULLET_MOVEMENT);
        }
        else
        {
            if (_targetBlock.unitActor != null)
            {
                if (!_targetBlock.unitActor.IsDead())
                {
                    _targetBlock.unitActor.IncreaseHealth(_unitActor.damageValue);
                }
            }
            else if (_targetBlock.castleActor != null)
                GameManager.IncreaseHealth(_unitActor.damageValue, _targetBlock.castleActor.typeTeam);

            DestroyImmediate(gameObject);
        }
    }

}
