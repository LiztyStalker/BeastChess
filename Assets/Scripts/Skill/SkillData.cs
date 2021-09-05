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


    //[Header("범위")]
    //[SerializeField]
    //[Tooltip("[All] - 맵 전체 범위 \n[MyselfRange] - 자신\n[UnitGroupRange] 특정 유닛 병종 우선- \n[UnitClassRange] - 특정 유닛 병과 우선\n[AscendingPriorityRange] - 높은 위험도 우선\n[DecendingPriorityRange] - 낮은 위험도 우선\n")]
    //private TYPE_UNIT_ATTACK_RANGE _typeSkillRange;

    //[SerializeField]
    //[Tooltip("자신을 포함할지 결정합니다")]
    //private bool _isMyself = false;

    //[SerializeField]
    //[Tooltip("스킬 범위를 지정합니다\n[n = 0] - 특정 유닛만\n[n > 0] - n 범위")]
    //private int _skillRangeValue = 1;



    //[Header("목표타입")]
    //[Tooltip("[All] - 범위의 모든 병사가 목표가 됩니다\n[Random] - 범위의 n만큼 랜덤의 병사가 목표가 됩니다")]
    //[SerializeField]
    //private TYPE_TARGET_SKILL_RANGE _typeTargetSkillRange;

    //[Tooltip("[Random] - 범위의 n만큼 랜덤의 병사가 목표가 됩니다")]
    //[SerializeField]
    //private int _targetSkillRangeCount;

    //[SerializeField]
    //private TYPE_UNIT_GROUP _typeUnitGroup;

    //[SerializeField]
    //private TYPE_UNIT_CLASS _typeUnitClass;


        
    //[Header("목표")]
    //[Tooltip("[All] - 모든 병사\n[Alies] - 아군\n[Enemy] - 적군")]
    //[SerializeField]
    //private TYPE_TARGET_TEAM _typeTargetTeam;

//    [Header("체력증감")]
    [Tooltip("True이면 스킬 발동시 병사의 체력이 증가하거나 감소합니다.")]
    [SerializeField]
    private bool _isIncreaseNowHealthValue = false;

    [SerializeField]
    [Tooltip("'+' 이면 체력이 증가하고, '-' 이면 체력이 감소합니다")]
    private int _increaseNowHealthValue = 0;

//    [Header("상태이상")]
    [Tooltip("상태이상 데이터")]
    [SerializeField]
    private StatusData _statusData = null;

//    [Header("이펙트")]
    [Tooltip("이펙트 데이터. 없으면 곧바로 적용되고 있으면 이펙트에 따라 적용됩니다")]
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
