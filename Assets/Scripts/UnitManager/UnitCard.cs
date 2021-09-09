using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Linq;

public enum UnitLiveType {Live, Dead}

public class UnitHealth
{
    public int key;
    public UnitLiveType unitLiveType;
    public int nowHealthValue;
    public int maxHealthValue;

    public UnitHealth(int key)
    {
        this.key = key;
        nowHealthValue = 0;
        maxHealthValue = 0;
    }

    public void SetNowHealth(int value)
    {
        nowHealthValue = value;
    }

    public void SetMaxHealth(UnitData uData)
    {
        maxHealthValue = uData.healthValue;
    }

    public void SetMaxHealth(UnitHealth unitHealth)
    {
        maxHealthValue = unitHealth.maxHealthValue;
    }

    public float HealthRate()
    {
        return (float)nowHealthValue / maxHealthValue;
    }
    public bool IsDead() => nowHealthValue == 0;
    public void DecreaseHealth(int value)
    {
        if (nowHealthValue - value < 0)
            nowHealthValue = 0;
        else
            nowHealthValue -= value;
    }

    public void IncreaseHealth(int value)
    {
        if (nowHealthValue + value >= maxHealthValue)
            nowHealthValue = maxHealthValue;
        else
            nowHealthValue += value;
    }


    public void SetUnitLiveType(float weight)
    {
        if (Random.Range(0f, 1f) > weight)
            unitLiveType = UnitLiveType.Dead;
    }
}




public class UnitCard : IUnitKey
{
    private const int LEVEL_MAX = 10;

    public SkeletonDataAsset skeletonDataAsset => _uData.skeletonDataAsset;
    
    private UnitData _uData { get; set; }

    public UnitData unitData => _uData;

    //unitData 각각 독립적으로 제작 필요
    //각 유닛의 체력만 다름

    public Sprite icon => _uData.icon;

    private UnitHealth unitHealth { get; set; }

    public SkillData[] skills => _uData.skills;

    private Dictionary<int, UnitHealth> _unitDic = new Dictionary<int, UnitHealth>();

    public int[] unitKeys => _unitDic.Keys.ToArray();

    public int LiveSquadCount {
        get
        {
            int liveSquadCount = 0;
            foreach(var value in _unitDic.Values)
            {
                if (!value.IsDead()) liveSquadCount++;
            }
            return liveSquadCount;
        }
    }

    public bool IsAllDead()
    {
#if UNITY_EDITOR
        if (unitHealth != null) return unitHealth.IsDead();
#endif

        int count = 0;
        foreach(var value in _unitDic.Values)
        {
            if (value.IsDead()) count++;
        }
        return unitData.squadCount == count;
    }


    public bool IsDead(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            return health.IsDead();
        }
        return true;
    }

    public void DecreaseHealth(int uKey, int value)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.DecreaseHealth(value);
        }
    }

    public void IncreaseHealth(int uKey, int value)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.IncreaseHealth(value);
        }
    }

    public void SetUnitLiveType(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.SetUnitLiveType(CommanderActor.DEAD_RATE);
        }
    }

    public float TotalHealthRate()
    {
        return (float)totalNowHealthValue / totalMaxHealthValue;
    }

    public float HealthRate(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            return health.HealthRate();
        }
        return 0f;
    }

    //public int healthValue => _unitData.healthValue;
    public int totalNowHealthValue { 
        get
        {
#if UNITY_EDITOR
            if (unitHealth != null) return unitHealth.nowHealthValue;
#endif
            int value = 0;
            foreach(var data in _unitDic.Values)
            {
                value += data.nowHealthValue;
            }
            return value;
        }
    }

    public int totalMaxHealthValue
    {
        get
        {
#if UNITY_EDITOR
            if (unitHealth != null) return unitHealth.maxHealthValue;
#endif

            int value = 0;
            foreach (var data in _unitDic.Values)
            {
                value += data.maxHealthValue;
            }
            return value;
        }
    }

    public string name => _uData.name;

    public int levelValue { get; private set; } = 1;

    public int nowExpValue { get; private set; }

    public int maxExpValue => levelValue * 100;

    public void IncreaseExpValue(int value)
    {
        if (levelValue < LEVEL_MAX) {

            nowExpValue += value;
            while (nowExpValue > maxExpValue)
            {
                nowExpValue -= maxExpValue;
                if (levelValue + 1 < LEVEL_MAX)
                    levelValue++;

                if(levelValue == LEVEL_MAX)
                {
                    nowExpValue = 0;
                    break;
                }
            }
        }
    }

    public int employCostValue => _uData.employCostValue;

    public int maintenenceCostValue => _uData.maintenanceCostValue;

    public int damageValue => _uData.damageValue;

    public int movementValue => _uData.movementValue;

    public int attackCount => _uData.attackCount;

    //public int attackRangeValue => _uData.attackRangeValue;

    //public int attackMinRangeValue => _uData.attackMinRangeValue;

    //public int minRangeValue => _uData.minRangeValue;

    public TargetData TargetData => _uData.TargetData;

    public int priorityValue => _uData.priorityValue;

    public int proficiencyValue => _uData.proficiencyValue;

    public int squadCount => _uData.squadCount;

    public TYPE_UNIT_FORMATION typeUnit => _uData.typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _uData.typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _uData.typeUnitClass;

//    public TYPE_UNIT_ATTACK typeUnitAttack => _uData.typeUnitAttack;

    public TYPE_MOVEMENT typeMovement => _uData.typeMovement;

//    public TYPE_UNIT_ATTACK_RANGE typeUnitAttackRange => _uData.typeUnitAttackRange;

//    public Vector2Int[] attackCells => _uData.attackCells;
    public Vector2Int[] movementCells => _uData.movementCells;
    public Vector2Int[] chargeCells => _uData.chargeCells;

    public AudioClip deadClip => _uData.deadClip;
    public AudioClip attackClip => _uData.attackClip;

    public Sprite bullet => _uData.bullet;

    public Vector2Int[] formationCells { get; private set; }


    public UnitCard(UnitData unitData, bool isTest = false)
    {
        _uData = unitData;
        for(int i = 0; i < unitData.squadCount; i++)
        {
            var uKey = UnitKey.InsertKey(this);
            if(uKey >= 0)
            {
                var health = GetUnitHealth(uKey);
                if(health != null)
                {
                    health.SetMaxHealth(unitData);
                    health.SetNowHealth(unitData.healthValue);
#if UNITY_EDITOR
                    if (isTest)
                    {
                        if(Random.Range(0, 100) > 50)
                            health.SetNowHealth(0);
                        else
                            health.SetNowHealth(Random.Range(0, unitData.healthValue + 1));
                    }
#endif
                }
            }
        }
    }

    public void RecoveryUnit(float rate)
    {
        var keys = unitKeys;
        for(int i = 0; i < keys.Length; i++)
        {
            var unit = _unitDic[keys[i]];
            if (unit.unitLiveType == UnitLiveType.Live)
            {
                _unitDic[keys[i]].IncreaseHealth((int)(_uData.healthValue * rate));
            }
        }
    }
    

    public UnitHealth GetUnitHealth(int key)
    {
        if (_unitDic.ContainsKey(key))
        {
            return _unitDic[key];
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"UnitDic is Not ContainsKey '{key}'. but Created Temporary Data In UnitEditor");
            if (unitHealth == null)
            {
                unitHealth = new UnitHealth(key);
                unitHealth.SetMaxHealth(unitData);
                unitHealth.SetNowHealth(unitData.healthValue);
                return unitHealth;
            }
        }
#endif
        return null;
    }

    public void SetFormation(Vector2Int[] formations)
    {
        formationCells = formations;
    }

    public bool SetKey(int key)
    {
        if (!_unitDic.ContainsKey(key))
        {
            _unitDic.Add(key, new UnitHealth(key));
            return true;
        }
        return false;
    }
}
