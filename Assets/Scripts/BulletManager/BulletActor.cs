using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletActor : MonoBehaviour
{
    private GameObject _prefab;
    private BulletData _data;

    private System.Action<BulletActor> _arrivedCallback;

    private Vector2 _startPos;
    private Vector2 _arrivePos;

    private float _nowTime = 0f;

    private SpriteRenderer _spriteRenderer{get;set;}
    private ParticleSystem[] _particles { get; set; }

    private bool _isEffectActivate = false;

    private void SetName()
    {
        gameObject.name = $"BulletActor_{_data.name}";
    }

    public void SetData(BulletData data)
    {
        _data = data;
        SetName();
    }

    public void SetPosition(Vector2 startPos, Vector2 arrivePos)
    {
        _startPos = startPos;
        _arrivePos = arrivePos;
    }

    public void SetArrivedCallback(System.Action<BulletActor> callback) => _arrivedCallback = callback;

    public bool IsData(BulletData data) => _data == data;

    public void Activate()
    {
        _nowTime = 0f;
        gameObject.SetActive(true);
        _isEffectActivate = false;

        if (_prefab == null)
        {
            _prefab = Instantiate(_data.prefab);
            _prefab.transform.SetParent(transform);
            _prefab.transform.localPosition = Vector3.zero;
        }

        transform.position = _startPos;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sortingLayerName = "FrontEffect";
            _spriteRenderer.sortingOrder = (int)-transform.position.y - 5;
        }

        _particles = _prefab.GetComponentsInChildren<ParticleSystem>();
        if (_particles != null)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                var psRenderer = _particles[i].GetComponent<ParticleSystemRenderer>();
                if(psRenderer != null)
                {
                    psRenderer.sortingLayerName = "FrontEffect";
                    psRenderer.sortingOrder = (int)-transform.position.y + 10;
                }
            }
        }
    }

    private void Update()
    {

        _nowTime = CalculateTime(_nowTime);
        transform.position = GetPosition(_startPos, _arrivePos, _nowTime);
    
        if(_data.IsRotate) transform.eulerAngles = GetEuler(_startPos, _arrivePos, _nowTime);

        if (Vector2.Distance(transform.position, _arrivePos) < 0.1)
        {
            if (!_isEffectActivate)
            {
                OnArriveEvent();
                EffectManager.Current.ActivateEffect(_data.ArriveEffectData, transform.position);
                _isEffectActivate = true;
            }
        }

        if (_isEffectActivate)
        {
            ReadyInactivate();
        }

    }

    private float CalculateTime(float nowTime)
    {
        return nowTime += Time.deltaTime * _data.MovementSpeed;
    }

    private Vector3 GetEuler(Vector3 startPos, Vector3 arrivePos, float nowTime)
    {
        switch (_data.TypeBulletAction)
        {
            case BulletData.TYPE_BULLET_ACTION.Drop:
                startPos = new Vector2(arrivePos.x, arrivePos.y + 10f);
                break;              
            //case BulletData.TYPE_BULLET_ACTION.Curve:
            //    //현재 위치에서 미래 위치를 예측해서 각도 찾기
            //    var futurePos = GetSlerp(startPos, arrivePos, CalculateTime(nowTime));
            //    startPos = transform.position;
            //    arrivePos = futurePos;
            //    break;
        }

        var direction = arrivePos - startPos;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Vector3.forward * angle;
    }

    private Vector2 GetPosition(Vector3 startPos, Vector3 arrivePos, float nowTime)
    {
        switch (_data.TypeBulletAction)
        {
            case BulletData.TYPE_BULLET_ACTION.Curve:
            case BulletData.TYPE_BULLET_ACTION.Direct:
                return Vector2.MoveTowards(startPos, arrivePos, nowTime);
            case BulletData.TYPE_BULLET_ACTION.Drop:
                var pos = new Vector2(arrivePos.x, arrivePos.y + 10f);
                return Vector2.MoveTowards(pos, arrivePos, nowTime);
            //case BulletData.TYPE_BULLET_ACTION.Curve:
                //return GetSlerp(startPos, arrivePos, nowTime);

        }
        return Vector2.zero;
    }

    private Vector2 GetSlerp(Vector3 startPos, Vector3 arrivePos, float nowTime)
    {
        var center = (startPos + arrivePos) * 8f;
        center -= Vector3.up;

        var startRelPos = startPos - center;
        var arriveRelPos = arrivePos - center;
        var nowPos = Vector3.Slerp(startRelPos, arriveRelPos, nowTime);
        nowPos += center;
        return nowPos;
    }


    private void ReadyInactivate()
    {
        if (_particles == null)
        {
            Inactivate();
        }
        else
        {
            int cnt = 0;
            for (int i = 0; i < _particles.Length; i++)
            {
                if (!_particles[i].isPlaying)
                {
                    cnt++;
                }
            }
            if (cnt == _particles.Length)
            {
                Inactivate();
            }
        }
    }

    private void OnArriveEvent()
    {
        _arrivedCallback?.Invoke(this);
        _arrivedCallback = null;
    }

    public void Inactivate()
    {
        gameObject.SetActive(false);
        _startPos = Vector2.zero;
        _arrivePos = Vector2.zero;
        _nowTime = 0f;
    }

    public void CleanUp()
    {
        DestroyImmediate(_prefab);
        _data = null;
        _arrivedCallback = null;
        _startPos = Vector2.zero;
        _arrivePos = Vector2.zero;
        _prefab = null;
    }

}
