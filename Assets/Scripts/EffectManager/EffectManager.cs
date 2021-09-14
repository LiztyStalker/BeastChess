using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _current = null;
    public static EffectManager Current => _current;

    private List<EffectActor> _actorList = new List<EffectActor>();

    private void Awake()
    {
        if(_current == null)
        {
            _current = this;
            transform.position = Vector3.zero;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_current == this)
        {
            for (int i = 0; i < _actorList.Count; i++)
                _actorList[i].Inactivate();
        }
    }

    public EffectActor ActivateEffect(EffectData effectData)
    {
        var actor = GetEffectActor(effectData);
        actor.SetData(effectData);
        actor.Activate();
        return actor;
    }

    public void InactiveEffect(EffectData effectData)
    {
        for (int i = 0; i < _actorList.Count; i++)
        {
            if (_actorList[i].isActiveAndEnabled && _actorList[i].IsEffectData(effectData))
            {
                _actorList[i].Inactivate();
                break;
            }
        }
    }

    private EffectActor GetEffectActor(EffectData effectData)
    {
        for (int i = 0; i < _actorList.Count; i++)
        {
            if (!_actorList[i].isActiveAndEnabled && _actorList[i].IsEffectData(effectData)) return _actorList[i];
        }

        var gameObejct = new GameObject();        
        var actor = gameObejct.AddComponent<EffectActor>();        
        actor.transform.SetParent(transform);
        _actorList.Add(actor);
        return actor;
    }



}
