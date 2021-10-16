using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameObjectCreater<T> where T : MonoBehaviour
{
    private static Dictionary<string, T> _dic = new Dictionary<string, T>();

    public static T Create(string gameObjectName, Transform tr)
    {
        if (!_dic.ContainsKey(typeof(T).Name))
        {
            var obj = DataStorage.Instance.GetDataOrNull<GameObject>(gameObjectName, null, null);
            var behaviour = obj.GetComponent<T>();
            _dic.Add(typeof(T).Name, behaviour);
        }

        var gameObj = _dic[typeof(T).Name];
        var block = Object.Instantiate(gameObj);
        block.transform.SetParent(tr);
        return block;
    }
}