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
        _particles = _prefab.GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {

        _nowTime = CalculateTime(_nowTime);
        transform.position = GetPosition(_startPos, _arrivePos, _nowTime);
    
        if(_data.IsRotate) transform.eulerAngles = GetEuler(_startPos, _arrivePos, _nowTime);

        if (_nowTime > 1f)
        {
            if (!_isEffectActivate)
            {
                EffectManager.Current.ActivateEffect(_data.ArriveEffectData, transform.position);
                _isEffectActivate = true;
            }
        }

        if (_isEffectActivate)
        {
            ActivateEffect();
        }

    }

    private float CalculateTime(float nowTime)
    {
        return nowTime += Time.deltaTime / _data.MovementTime;
    }

    private Vector3 GetEuler(Vector3 startPos, Vector3 arrivePos, float nowTime)
    {
        switch (_data.TypeBulletAction)
        {
            case BulletData.TYPE_BULLET_ACTION.Drop:
                startPos = new Vector2(arrivePos.x, arrivePos.y + 10f);
                break;              
            case BulletData.TYPE_BULLET_ACTION.Curve:
                var futurePos = GetSlerp(startPos, arrivePos, CalculateTime(nowTime));
                startPos = transform.position;
                arrivePos = futurePos;
                break;
        }

        var direction = arrivePos - startPos;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Vector3.forward * angle;
    }

    private Vector2 GetPosition(Vector3 startPos, Vector3 arrivePos, float nowTime)
    {
        switch (_data.TypeBulletAction)
        {
            case BulletData.TYPE_BULLET_ACTION.Direct:
                return Vector2.Lerp(startPos, arrivePos, nowTime);
            case BulletData.TYPE_BULLET_ACTION.Curve:
                return GetSlerp(startPos, arrivePos, nowTime);
            case BulletData.TYPE_BULLET_ACTION.Drop:
                var pos = new Vector2(arrivePos.x, arrivePos.y + 10f);
                return Vector2.Lerp(pos, arrivePos, nowTime);
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


    private void ActivateEffect()
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

    public void Inactivate()
    {
        gameObject.SetActive(false);
        _arrivedCallback?.Invoke(this);
        _arrivedCallback = null;
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
