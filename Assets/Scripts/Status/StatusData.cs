using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatusData", menuName = "ScriptableObjects/StatusData")]
public class StatusData : ScriptableObject
{
    public enum TYPE_STATUS_LIFE_SPAN { Always, Turn, Caster }

    [SerializeField]
    private Sprite _icon;


    [Tooltip("[Always] - ��� �����˴ϴ�\n[Turn] - TurnCount��ŭ ����˴ϴ�. ���� ��ų�� ������ TurnCount�� �ʱ�ȭ �˴ϴ�\n[Caster] - �����ڰ� ����ϸ� �������� �ʽ��ϴ�")]
    [SerializeField]
    private TYPE_STATUS_LIFE_SPAN _typeStatusLifeSpan;

    [Tooltip("[n = 0] - ����\n[n > 0] - n ��ŭ �� ī��Ʈ ����")]
    [SerializeField, Range(0, 100)]
    private int _turnCount;


    
    [SerializeField]
    [Tooltip("��ø ���θ� �����մϴ�")]
    private bool _isOverlapped = false;

    [SerializeField, Range(0, 100)]
    [Tooltip("[n = 0] - �������� ��ø�� �����մϴ�\n[n > 0] - �ִ� n��ŭ ��ø�˴ϴ�")]
    private int _overlapCount = 0;
    
   

    [SerializeField]
    private List<StatusSerializable> _editorStatusList = new List<StatusSerializable>();

    [System.NonSerialized]
    private List<IStatus> _stateList = null;



    #region ##### Getter Setter #####

    public Sprite Icon => _icon;
    public TYPE_STATUS_LIFE_SPAN typeStatusLifeSpan => _typeStatusLifeSpan;
    public bool IsOverlapped => _isOverlapped;
    public int OverlapCount => _overlapCount;
    public int TurnCount
    {
        get
        {
            if (_typeStatusLifeSpan == TYPE_STATUS_LIFE_SPAN.Turn)
                return _turnCount;
            return -1;
        }
    }


    #endregion


    private void Initialize()
    {
        for (int i = 0; i < _editorStatusList.Count; i++)
        {
            _stateList.Add(_editorStatusList[i].ConvertState());
        }
    }

    public IStatus[] GetStateArray()
    {
        if (_stateList == null && _editorStatusList.Count > 0)
        {
            _stateList = new List<IStatus>();
            Initialize();
        }
        return _stateList.ToArray();
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

#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

    public StatusData()
    {
        _typeStatusLifeSpan = TYPE_STATUS_LIFE_SPAN.Always;
        _turnCount = 1;
        _isOverlapped = false;
        _overlapCount = 1;
    }

    public void SetTypeStatusLifeSpan(TYPE_STATUS_LIFE_SPAN typeStateLifeSpan)
    {
        _typeStatusLifeSpan = typeStateLifeSpan;
    }

    public void SetTurnCount(int turnCount)
    {
        _turnCount = turnCount;
    }

    public void SetIsOverlap(bool isOverlapped)
    {
        _isOverlapped = isOverlapped;
    }

    public void SetOverlapCount(int overlapCount)
    {
        _overlapCount = overlapCount;
    }

#endif

}
