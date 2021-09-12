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

    [Tooltip("True�̸� ��ų �ߵ��� ������ ü���� �����ϰų� �����մϴ�.")]
    [SerializeField]
    private bool _isIncreaseNowHealthValue = false;

    [SerializeField]
    [Tooltip("'+' �̸� ü���� �����ϰ�, '-' �̸� ü���� �����մϴ�")]
    private int _increaseNowHealthValue = 0;

    [Tooltip("�����̻� ������")]
    [SerializeField]
    private StatusData _statusData = null;

    [Tooltip("����Ʈ ������. ������ ��ٷ� ����ǰ� ������ ����Ʈ�� ���� ����˴ϴ�")]
    [SerializeField]
    private EffectData _effectData = null;



    #region ##### Getter Setter #####
    
    public string key => name;        
    public Sprite icon => _icon;

    public TYPE_SKILL_ACTIVATE typeSkillActivate => _typeSkillActivate;
    public float skillActivateRate => _skillActivateRate;

    public TargetData TargetData => _targetData;

    public bool isIncreaseNowHealthValue => _isIncreaseNowHealthValue;
    public int increaseNowHealthValue => _increaseNowHealthValue;

    public StatusData statusData => _statusData;

    public EffectData effectData => _effectData;

    #endregion



    public void Calculate<T>(ref float rate, ref int value, int overlapCount) where T : IStatus
    {
        _statusData.Calculate<T>(ref rate, ref value, overlapCount);
    }



#if UNITY_EDITOR && UNITY_INCLUDE_TESTS



    public void SetData(TYPE_SKILL_ACTIVATE typeSkillActivate, float skillActivateRate = 0.5f)
    {
        _typeSkillActivate = typeSkillActivate;
        _skillActivateRate = skillActivateRate;
    }

    public void SetData(bool isIncreaseNowHealthValue, int increaseNowHealthValue = 0)
    {
        _isIncreaseNowHealthValue = isIncreaseNowHealthValue;
        _increaseNowHealthValue = increaseNowHealthValue;
    }

    public void SetData(TargetData targetData)
    {
        _targetData = targetData;
    }

    public void SetData(StatusData statusData)
    {
        _statusData = statusData;
    }

    public void SetData(EffectData effectData)
    {
        _effectData = effectData;
    }


#endif

}
