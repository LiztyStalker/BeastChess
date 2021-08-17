using UnityEngine;


[System.Serializable]
public class State : IState
{
    public enum TYPE_VALUE { Value, Rate }

#if UNITY_EDITOR
    [SerializeField]
    protected string _name;
#endif

    [SerializeField]
    private TYPE_VALUE _typeValue;

    [SerializeField]
    private float _value;

    public TYPE_VALUE typeValue => _typeValue;
    public float value => _value;
}
