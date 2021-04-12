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

        //float flip = 1;

        //Vector2 dir = _targetBlock.transform.position - transform.position;

        //float rad = Mathf.Atan2(dir.y, dir.x);

        //float velX = Mathf.Cos(rad) * _moveSpeed * flip;
        //float velY = Mathf.Sin(rad) * _moveSpeed;

        _rigidbody.gravityScale = 0f;
        //_rigidbody.velocity = new Vector2(velX, velY);

        //float flip = 1;

        //float gravity = -Physics2D.gravity.y;

        //Vector2 distance = _targetBlock.transform.position - transform.position;

        //Debug.Log($"{gravity} {distance} {_moveSpeed}");

        //float angle = 0.5f * Mathf.Asin((gravity * distance.x) / (_moveSpeed * _moveSpeed));
        //Debug.Log($"{angle}");

        ////전위차 보정
        //angle += Mathf.Atan2(distance.y, distance.x);
        //Debug.Log($"{angle}");

        //float vX = Mathf.Cos(angle * flip) * _moveSpeed;// *flip;
        //float vY = Mathf.Sin(angle) * _moveSpeed;

        //Debug.Log($"{vX}{vY}");

        //_rigidbody.velocity = new Vector2(vX, vY);
        //_rigidbody.gravityScale = 1f;
    }

    public void SetData(UnitActor unitActor, FieldBlock targetBlock, float movement)
    {
        _unitActor = unitActor;
        _targetBlock = targetBlock;
        _moveSpeed = movement;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targetBlock.transform.position) > .1f) {
            transform.position = Vector2.MoveTowards(transform.position, _targetBlock.transform.position, 0.2f);
        }
        else
        {
            if (_targetBlock.unitActor != null)
            {
                if (!_targetBlock.unitActor.IsDead())
                {
                    if (_targetBlock.castleActor != null)
                        GameTestManager.IncreaseHealth(_unitActor.damageValue, _targetBlock.unitActor.typeTeam);
                    else
                        _targetBlock.unitActor.IncreaseHealth(_unitActor.damageValue);
                }
            }
            DestroyImmediate(gameObject);
        }
    }

}
