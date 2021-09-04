using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatusData", menuName = "ScriptableObjects/StatusData")]
public class StatusData : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;

//    [Header("상태이상")]
    [SerializeField]
    private List<StatusSerializable> _editorStatusList = new List<StatusSerializable>();

    [System.NonSerialized]
    private List<IStatus> _stateList = null;

    public IStatus[] GetStateArray()
    {
        if (_stateList == null && _editorStatusList.Count > 0)
        {
            _stateList = new List<IStatus>();
            Initialize();
        }
        return _stateList.ToArray();
    }

    private void Initialize()
    {
        for (int i = 0; i < _editorStatusList.Count; i++)
        {
            _stateList.Add(_editorStatusList[i].ConvertState());
        }
    }

    public void Calculate<T>(ref float rate, ref int value, int overlapCount) where T : IStatus
    {


        if (_stateList == null && _editorStatusList.Count > 0)
        {
            _stateList = new List<IStatus>();
            Initialize();
        }

        if (_stateList != null)
        {
            for (int i = 0; i < _stateList.Count; i++)
            {
                var state = _stateList[i];
                //Debug.Log(state.GetType().Name + " " + typeof(T).Name);
                if (state is T)
                {
                    switch (state.typeValue)
                    {
                        case Status.TYPE_VALUE.Value:
                            value += (int)state.value * overlapCount;
                            break;
                        case Status.TYPE_VALUE.Rate:
                            rate += state.value * overlapCount;
                            break;
                    }
                }
            }
        }

    }


#if UNITY_EDITOR

    public void AddState(StatusSerializable state)
    {
        Debug.Log(state.GetType().Name);
        _editorStatusList.Add(state);
    }

    public void RemoveState(StatusSerializable state)
    {
        _editorStatusList.Remove(state);
    }

    public void RemoveAt(int index)
    {
        _editorStatusList.RemoveAt(index);
    }

#endif


}
