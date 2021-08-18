using UnityEngine;
using System.Reflection;

public abstract class State
{
    public enum TYPE_VALUE { Value, Rate }

    [SerializeField]
    private TYPE_VALUE _typeValue;

    [SerializeField]
    private float _value;

    public TYPE_VALUE typeValue => _typeValue;
    public float value => _value;   

    public State(TYPE_VALUE typeValue, float value)
    {
        _typeValue = typeValue;
        _value = value;
    }
}


[System.Serializable]
public class StateSerializable
{
    [SerializeField]
    private State.TYPE_VALUE _typeValue;

    [SerializeField]
    private float _value;

    [SerializeField]
    private string _typeStateClass;
    
    public IState ConvertState()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var type = assembly.GetType(_typeStateClass);
        return (IState)System.Activator.CreateInstance(type, _typeValue, _value);
    }

#if UNITY_EDITOR
    public StateSerializable(System.Type type)
    {
        _typeStateClass = type.Name;
    }
#endif
}

