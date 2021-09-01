using UnityEngine;
using System.Reflection;

public abstract class Status
{
    public enum TYPE_VALUE { Value, Rate }

    public enum TYPE_STATE_HEALTH { Direct, Turn }

    [SerializeField]
    private TYPE_VALUE _typeValue;

    [SerializeField]
    private float _value;

    public TYPE_VALUE typeValue => _typeValue;
    public float value => _value;   

    public Status(TYPE_VALUE typeValue, float value)
    {
        _typeValue = typeValue;
        _value = value;
    }
}


[System.Serializable]
public class StatusSerializable
{
    //common
    [SerializeField]
    private Status.TYPE_VALUE _typeValue;

    [SerializeField]
    private float _value;

    [SerializeField]
    private bool _isExtend = false;

    //extend
    [SerializeField]
    private Status.TYPE_STATE_HEALTH _typeStateHealth;

    [SerializeField]
    private int _turnCount;


    //class
    [SerializeField]
    private string _typeStateClass;

    public static System.Type GetStateType(string className)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        return assembly.GetType(className);
    }


    public IStatus ConvertState()
    {
        var type = GetStateType(_typeStateClass);
        if (type is IStatusHealth)
            return (IStatus)System.Activator.CreateInstance(type, _typeValue, _value, _typeStateClass, _turnCount);
        else
            return (IStatus)System.Activator.CreateInstance(type, _typeValue, _value);
    }

#if UNITY_EDITOR
    public StatusSerializable(System.Type type, bool isExtend = false)
    {
        _typeStateClass = type.Name;
        _isExtend = isExtend;
    }
#endif
}

