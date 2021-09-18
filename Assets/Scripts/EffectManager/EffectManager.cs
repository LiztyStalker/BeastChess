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
            }
            return _gameObject;
        }
    }

    //private void Awake()
    //{
    //    if(_current == null)
    //    {
    //        _current = this;
    //        transform.position = Vector3.zero;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        DestroyImmediate(gameObject);
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (_current == this)
    //    {
    //        foreach (var value in _effectDic.Values)
    //        {
    //            for (int i = 0; i < value.Count; i++)
    //                value[i].Inactivate();
    //        }
    //        _effectDic.Clear();
    //    }
    //}

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
