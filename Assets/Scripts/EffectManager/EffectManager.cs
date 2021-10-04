using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{

    private static Dictionary<EffectData, List<EffectActor>> _effectDic = new Dictionary<EffectData, List<EffectActor>>();

    private static GameObject _gameObject;

    private static GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject();
                _gameObject.transform.position = Vector3.zero;
                _gameObject.name = "EffectManager";
                Object.DontDestroyOnLoad(_gameObject);
            }
            return _gameObject;
        }
    }


    public static EffectActor ActivateEffect(EffectData effectData, Vector3 position, System.Action<EffectActor> callback = null)
    {
        if (effectData != null)
        {
            var actor = GetEffectActor(effectData);
            actor.SetData(effectData);
            actor.SetInactiveCallback(callback);
            actor.Activate(position);
            return actor;
        }
        return null;
    }

    public static void InactiveEffect(EffectData effectData)
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

    private static EffectActor GetEffectActor(EffectData effectData)
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
        actor.transform.SetParent(gameObject.transform);
        list.Add(actor);
        return actor;
    }



}
