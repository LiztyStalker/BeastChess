using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitKey
{
    bool SetKey(int key);
}


public class UnitKeyGenerator
{
    static List<int> uKeyList = new List<int>();
    static int nowKey = 0;

    public static bool Contains(int key)
    {
        return uKeyList.Contains(key);
    }

    /// <summary>
    /// Ű�� �����մϴ�
    /// ���Կ� �����ϸ� ���
    /// �����ϸ� -1
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static int InsertKey(IUnitKey unit)
    {
        var key = nowKey++;
        if (!Contains(key))
        {
            if (unit.SetKey(key))
            {
                SetKey(key);
                return key;
            }
        }
        Debug.Log("InsertKey is Count Over");
        return -1;
    }

    public static void RemoveKey(int key)
    {
        uKeyList.Remove(key);
    }

    public static void SetKey(int key)
    {
        uKeyList.Add(key);
    }
}