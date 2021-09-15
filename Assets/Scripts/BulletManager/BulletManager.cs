using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager _current = null;
    public static BulletManager Current => _current;

//    private List<EffectActor> _actorList = new List<EffectActor>();

    private Dictionary<BulletData, List<BulletActor>> _actorDic = new Dictionary<BulletData, List<BulletActor>>();

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
            foreach (var value in _actorDic.Values)
            {
                for (int i = 0; i < value.Count; i++)
                    value[i].Inactivate();
            }
            _actorDic.Clear();
        }
    }

    public BulletActor ActivateBullet(BulletData data, Vector2 startPos, Vector2 arrivePos, System.Action<BulletActor> arrivedCallback)
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

    public void InactiveBullet(BulletData data)
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

    private BulletActor GetActor(BulletData data)
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
        actor.transform.SetParent(transform);
        list.Add(actor);
        return actor;
    }



}
