using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletActor : MonoBehaviour
{
    private GameObject _prefab;
    private BulletData _data;

    private System.Action<BulletActor> _arrivedCallback;

    private Transform _startTransform;
    private Transform _arriveTransform;

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

    public void SetTransform(Transform startTr, Transform arriveTr)
    {
        _startTransform = startTr;
        _arriveTransform = arriveTr;
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
        transform.position = _startTransform.position;
        _particles = _prefab.GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {

        transform.position = _data.SetPosition(_startTransform, _arriveTransform, ref _nowTime);

        if (_nowTime > 1f)
        {
            if (!_isEffectActivate)
            {
                _data.ActivateEffect(transform.position);
                _isEffectActivate = true;
            }
        }

        if (_isEffectActivate)
        {
            ActivateEffect();
        }

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
        _startTransform = null;
        _arriveTransform = null;
        _nowTime = 0f;
    }

    public void CleanUp()
    {
        DestroyImmediate(_prefab);
        _data = null;
        _arrivedCallback = null;
        _startTransform = null;
        _arriveTransform = null;
        _prefab = null;
    }

}
