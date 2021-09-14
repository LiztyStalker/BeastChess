using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _current = null;
    public static EffectManager Current => _current;

//    private List<EffectActor> _actorList = new List<EffectActor>();

    private Dictionary<EffectData, List<EffectActor>> _effectDic = new Dictionary<EffectData, List<EffectActor>>();

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
            foreach (var value in _effectDic.Values)
            {
                for (int i = 0; i < value.Count; i++)
                    value[i].Inactivate();
            }
            _effectDic.Clear();
        }
    }

    public EffectActor ActivateEffect(EffectData effectData, Vector3 position)
    {
        var actor = GetEffectActor(effectData);
        actor.SetData(effectData);
        actor.Activate(position);
        return actor;
    }

    public void InactiveEffect(EffectData effectData)
    {
        if (_effectDic.ContainsKey(effectData))
        {
            var list = _effectDic[effectData];
            for (int i = 0; i < list.Count; i++)
            {
                var actor = list[i];
                if (actor.isActiveAndEnabled && actor.IsEffectData(effectData))
                {
                    actor.Inactivate();
                    break;
                }
            }
        }

    }

    private EffectActor GetEffectActor(EffectData effectData)
    {
        if (!_effectDic.ContainsKey(effectData))
        {
            _effectDic.Add(effectData, new List<EffectActor>());
        }

        var list = _effectDic[effectData];
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].isActiveAndEnabled && list[i].IsEffectData(effectData)) return list[i];
        }

        var gameObejct = new GameObject();        
        var actor = gameObejct.AddComponent<EffectActor>();        
        actor.transform.SetParent(transform);
        list.Add(actor);
        return actor;
    }



}
