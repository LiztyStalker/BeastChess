using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{

    private static Dictionary<BulletData, List<BulletActor>> _actorDic = new Dictionary<BulletData, List<BulletActor>>();

    private static GameObject _gameObject;
    private static GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject();
                _gameObject.transform.position = Vector3.zero;
                _gameObject.name = "BulletManager";
                Object.DontDestroyOnLoad(_gameObject);
            }
            return _gameObject;
        }
    }
    
    public static BulletActor ActivateBullet(BulletData data, Vector2 startPos, Vector2 arrivePos, System.Action<BulletActor> arrivedCallback = null)
    {
        if(data == null)
        {
            Debug.LogError("BulletData를 지정하세요");
            return null;
        }

        var actor = GetActor(data);
        actor.SetData(data);
        actor.SetPosition(startPos, arrivePos);
        actor.SetArrivedCallback(arrivedCallback);
        actor.Activate();
        return actor;
    }

    public static void InactiveBullet(BulletData data)
    {
        if (_actorDic.ContainsKey(data))
        {
            var list = _actorDic[data];
            for (int i = 0; i < list.Count; i++)
            {
                var actor = list[i];
                if (actor.isActiveAndEnabled && actor.IsData(data))
                {
                    actor.Inactivate();
                    break;
                }
            }
        }

    }

    private static BulletActor GetActor(BulletData data)
    {
        if (!_actorDic.ContainsKey(data))
        {
            _actorDic.Add(data, new List<BulletActor>());
        }

        var list = _actorDic[data];
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].isActiveAndEnabled && list[i].IsData(data)) return list[i];
        }

        var gameObejct = new GameObject();        
        var actor = gameObejct.AddComponent<BulletActor>();        
        actor.transform.SetParent(gameObject.transform);
        list.Add(actor);
        return actor;
    }



}
