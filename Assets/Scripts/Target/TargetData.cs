using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_TARGET_TEAM { All, Alies, Enemy}

public enum TYPE_TARGET_RANGE { Normal = 0, Triangle, Square, Vertical, Cross, Rhombus, Circle, Custom = 100 }

public enum TYPE_TARGET_PRIORITY { None, High, Low, Random}

[System.Serializable]
public class TargetData
{

    #region ##### Member #####

//    [Header("��ǥ")]
    [Tooltip("[All] - ��� ����\n[Alies] - �Ʊ�\n[Enemy] - ����")]
    [SerializeField]
    private TYPE_TARGET_TEAM _typeTargetTeam;

#if UNITY_EDITOR
    [SerializeField, HideInInspector]
    private bool _isAlwaysTargetEnemy = false;
#endif

    [SerializeField]
    private bool _isMyself = false;

    [Tooltip("��ü ��ǥ ����")]
    [SerializeField]
    private bool _isAllTargetRange = false;

//    [Header("��ǥ����")]
    [Tooltip("Ư�� ��ǥ ����")]
    [SerializeField]
    private TYPE_TARGET_RANGE _typeTargetRange;

//    [Header("��ǥ��������")]
    [Tooltip("��ǥ�� ������ ����")]
    [SerializeField]
    private int _targetStartRange = 0;

//    [Header("��ǥ������")]
    [Tooltip("��ǥ �������������� ����")]
    [SerializeField]
    private int _targetRange = 1;

//    [Header("��ǥ�켱����")]
    [Tooltip("[None] - �������\n[High] - ���� �켱����\n[Low] - ���� �켱����\n[Random] - ����")]
    [SerializeField]
    private TYPE_TARGET_PRIORITY _typeTargetPriority;

//    [Header("��ǥ �� ����")]
    [Tooltip("True - ��ǥ ���� ���� ��� ��ǥ\nFalse - ��ǥ ���� ���� �켱������ ���� ��ǥ ����")]
    [SerializeField]
    private bool _isTargetCount = true;

//    [Tooltip("��ǥ ��")]
    [SerializeField]
    private int _targetCount = 1;

    [System.NonSerialized]
    private Vector2Int[] _targetCells;

    #endregion


    #region ##### Getter Setter #####

    public TYPE_TARGET_TEAM TypeTargetTeam => _typeTargetTeam;
    public bool IsMyself => _isMyself;
    public bool IsAllTargetRange => _isAllTargetRange;
    public TYPE_TARGET_RANGE TypeTargetRange => _typeTargetRange; 
    public int TargetStartRange => _targetStartRange;
    public int TargetRange => _targetRange; 
    public TYPE_TARGET_PRIORITY TypeTargetPriority => _typeTargetPriority;
    public bool IsTargetCount => _isTargetCount;
    public int TargetCount => _targetCount;

    //public Vector2Int[] TargetCells => _targetCells;

    #endregion

    public TargetData(bool isTargetAlwaysEnemy = false)
    {
        if (isTargetAlwaysEnemy)
        {
            _typeTargetTeam = TYPE_TARGET_TEAM.Enemy;
            _isAlwaysTargetEnemy = isTargetAlwaysEnemy;
        }
    }

#if UNITY_EDITOR && UNITY_INCLUDE_TESTS

    public TargetData()
    {
        _typeTargetTeam = TYPE_TARGET_TEAM.All;
        _isMyself = true;
        _isAllTargetRange = false;
        _typeTargetRange = TYPE_TARGET_RANGE.Normal;
        _targetStartRange = 0;
        _targetRange = 5;
        _typeTargetPriority = TYPE_TARGET_PRIORITY.None;
        _isTargetCount = false;
        _targetCount = 0;
    }

    public TargetData(
            TYPE_TARGET_TEAM typeTargetTeam, 
            bool isMyself,
            TYPE_TARGET_RANGE typeTargetRange,
            int targetStartRange,
            int targetRange,
            TYPE_TARGET_PRIORITY typeTargetPriority,
            bool isTargetCount,
            int targetCount)
    {
        _typeTargetTeam = typeTargetTeam;
        _isMyself = isMyself;
        _isAllTargetRange = false;
        _typeTargetRange = typeTargetRange;
        _targetStartRange = targetStartRange;
        _targetRange = targetRange;
        _typeTargetPriority = typeTargetPriority;
        _isTargetCount = isTargetCount;
        _targetCount = targetCount;
    }

    public void SetTypeTeam(TYPE_TARGET_TEAM typeTargetTeam)
    {
        _typeTargetTeam = typeTargetTeam;
    }

    public void SetIsAllTargetRange(bool isAllTargetRange)
    {
        _isAllTargetRange = isAllTargetRange;
    }

    public void SetTypeTargetRange(
            TYPE_TARGET_RANGE typeTargetRange,
            bool isMyself,
            int targetStartRange,
            int targetRange)
    {
        _isMyself = isMyself;
        _typeTargetRange = typeTargetRange;
        _targetStartRange = targetStartRange;
        _targetRange = targetRange;
    }

    public void SetTypeTargetPriority(
            TYPE_TARGET_PRIORITY typeTargetPriority,
            bool isTargetCount,
            int targetCount)
    {
        _typeTargetPriority = typeTargetPriority;
        _isTargetCount = isTargetCount;
        _targetCount = targetCount;
    }

    
#endif
}
