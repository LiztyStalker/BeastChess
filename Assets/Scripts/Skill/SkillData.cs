using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TYPE_SKILL_ACTIVATE { Passive, PreActive, Active}

//public enum TYPE_SKILL_RANGE { All, MyselfRange, UnitGroupRange, UnitClassRange, AscendingPriorityRange, DecendingPriorityRange}

//public enum TYPE_TARGET_TEAM { All, Alies, Enemy}

//public enum TYPE_TARGET_SKILL_RANGE { All, Random }

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillData : ScriptableObject
{
//    [Header("����")]
    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;



//    [Header("�ߵ�����")]
    [Tooltip("[Passive] - �׻� ��ų�� �ߵ��˴ϴ�\n[PreActive] - ������ ���۵ɶ� �ߵ��մϴ�\n[Active] - ���� ���� ���߿� ���ݿ� ���ؼ� �ߵ��˴ϴ�")]
    [SerializeField]
    private TYPE_SKILL_ACTIVATE _typeSkillActivate;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("���ݽ� ��ų ���� Ȯ���Դϴ�")]
    private float _skillActivateRate = 0.2f;

    [Header("��ǥ ������")]
    [SerializeField]
    private TargetData _targetData = new TargetData();


    //[Header("����")]
    //[SerializeField]
    //[Tooltip("[All] - �� ��ü ���� \n[MyselfRange] - �ڽ�\n[UnitGroupRange] Ư�� ���� ���� �켱- \n[UnitClassRange] - Ư�� ���� ���� �켱\n[AscendingPriorityRange] - ���� ���赵 �켱\n[DecendingPriorityRange] - ���� ���赵 �켱\n")]
    //private TYPE_UNIT_ATTACK_RANGE _typeSkillRange;

    //[SerializeField]
    //[Tooltip("�ڽ��� �������� �����մϴ�")]
    //private bool _isMyself = false;

    //[SerializeField]
    //[Tooltip("��ų ������ �����մϴ�\n[n = 0] - Ư�� ���ָ�\n[n > 0] - n ����")]
    //private int _skillRangeValue = 1;



    //[Header("��ǥŸ��")]
    //[Tooltip("[All] - ������ ��� ���簡 ��ǥ�� �˴ϴ�\n[Random] - ������ n��ŭ ������ ���簡 ��ǥ�� �˴ϴ�")]
    //[SerializeField]
    //private TYPE_TARGET_SKILL_RANGE _typeTargetSkillRange;

    //[Tooltip("[Random] - ������ n��ŭ ������ ���簡 ��ǥ�� �˴ϴ�")]
    //[SerializeField]
    //private int _targetSkillRangeCount;

    //[SerializeField]
    //private TYPE_UNIT_GROUP _typeUnitGroup;

    //[SerializeField]
    //private TYPE_UNIT_CLASS _typeUnitClass;


        
    //[Header("��ǥ")]
    //[Tooltip("[All] - ��� ����\n[Alies] - �Ʊ�\n[Enemy] - ����")]
    //[SerializeField]
    //private TYPE_TARGET_TEAM _typeTargetTeam;

//    [Header("ü������")]
    [Tooltip("True�̸� ��ų �ߵ��� ������ ü���� �����ϰų� �����մϴ�.")]
    [SerializeField]
    private bool _isIncreaseNowHealthValue = false;

    [SerializeField]
    [Tooltip("'+' �̸� ü���� �����ϰ�, '-' �̸� ü���� �����մϴ�")]
    private int _increaseNowHealthValue = 0;

//    [Header("�����̻�")]
    [Tooltip("�����̻� ������")]
    [SerializeField]
    private StatusData _statusData = null;

//    [Header("����Ʈ")]
    [Tooltip("����Ʈ ������. ������ ��ٷ� ����ǰ� ������ ����Ʈ�� ���� ����˴ϴ�")]
    [SerializeField]
    private EffectData _effectData = null;



    #region ##### Getter Setter #####


    
    public string key => name;
        
    public Sprite icon => _icon;



    public TYPE_SKILL_ACTIVATE typeSkillActivate => _typeSkillActivate;

    public float skillActivateRate => _skillActivateRate;



    public TargetData TargetData => _targetData;


    //    public TYPE_SKILL_RANGE typeSkillRange => _typeSkillRange;
    //public TYPE_UNIT_ATTACK_RANGE typeSkillRange => _typeSkillRange;

    //public bool isMyself => _isMyself;

    //public int skillRangeValue => _skillRangeValue;



    //public TYPE_TARGET_SKILL_RANGE typeTargetSkillRange  => _typeTargetSkillRange;

    //public int targetSkillRangeCount => _targetSkillRangeCount;
    
    //public TYPE_UNIT_GROUP typeUnitGroup => _typeUnitGroup;

    //public TYPE_UNIT_CLASS typeUnitClass => _typeUnitClass;



    //public TYPE_TARGET_TEAM typeTargetTeam => _typeTargetTeam;    



    public bool isIncreaseNowHealthValue => _isIncreaseNowHealthValue;

    public int increaseNowHealthValue => _increaseNowHealthValue;



    public StatusData statusData => _statusData;



    public EffectData effectData => _effectData;

    #endregion

    public void Calculate<T>(ref float rate, ref int value, int overlapCount) where T : IStatus
    {
        _statusData.Calculate<T>(ref rate, ref value, overlapCount);
    }
}
