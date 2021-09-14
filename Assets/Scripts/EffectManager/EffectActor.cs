using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_EFFECT_LIFESPAN { }

public class EffectActor : MonoBehaviour
{
    private GameObject _effectPrefab;
    private EffectData _effectData;

    private ParticleSystem[] _particles;

    private System.Action<EffectActor> _inactiveCallback;

    private void SetName()
    {
        gameObject.name = $"EffectActor_{_effectData.name}";
    }

    public void SetData(EffectData effectData)
    {
        _effectData = effectData;
        SetName();
    }

    public void SetInactiveCallback(System.Action<EffectActor> callback) => _inactiveCallback = callback;

    public bool IsEffectData(EffectData effectData) => _effectData == effectData;

    public void Activate()
    {
        gameObject.SetActive(true);

        _effectPrefab = Instantiate(_effectData.effectPrefab);
        _effectPrefab.transform.SetParent(transform);
        _effectPrefab.transform.localPosition = Vector3.zero;
        _effectPrefab.transform.localScale = Vector3.one * 0.1f;

        _particles = _effectPrefab.GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        int cnt = 0;
        for(int i = 0; i < _particles.Length; i++)
        {
            if (!_particles[i].isPlaying)
            {
                cnt++;
            }
        }

        if(cnt == _particles.Length)
        {
            Inactivate();
        }
    }

    public void Inactivate()
    {
        gameObject.SetActive(false);
        _inactiveCallback?.Invoke(this);
        _inactiveCallback = null;
    }

    public void CleanUp()
    {
        DestroyImmediate(_effectPrefab);
        _effectData = null;
        _particles = null;
        _inactiveCallback = null;
    }

}
