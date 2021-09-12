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
//    [Header("정보")]
    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

//    [Header("발동조건")]
    [Tooltip("[Passive] - 항상 스킬이 발동됩니다\n[PreActive] - 전투가 시작될때 발동합니다\n[Active] - 전투 진행 도중에 공격에 의해서 발동됩니다")]
    [SerializeField]
    private TYPE_SKILL_ACTIVATE _typeSkillActivate;

    [SerializeField, Range(0f, 1f)]
    [Tooltip("공격시 스킬 시전 확률입니다")]
    private float _skillActivateRate = 0.2f;

    [Header("목표 데이터")]
    [SerializeField]
    private TargetData _targetData = new TargetData();

    [Tooltip("True이면 스킬 발동시 병사의 체력이 증가하거나 감소합니다.")]
    [SerializeField]
    private bool _isIncreaseNowHealthValue = false;

    [SerializeField]
    [Tooltip("'+' 이면 체력이 증가하고, '-' 이면 체력이 감소합니다")]
    private int _increaseNowHealthValue = 0;

    [Tooltip("상태이상 데이터")]
    [SerializeField]
    private StatusData _statusData = null;

    [Tooltip("이펙트 데이터. 없으면 곧바로 적용되고 있으면 이펙트에 따라 적용됩니다")]
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
