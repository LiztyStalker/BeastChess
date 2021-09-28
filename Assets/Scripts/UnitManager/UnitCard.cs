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
        maxHealthValue = uData.HealthValue;
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
    private const int LEVEL_MAX = 9;
    private UnitData _uData { get; set; }
    public UnitData unitData => _uData;

    public SkeletonDataAsset skeletonDataAsset => _uData.SkeletonDataAsset;

    public string Skin => _uData.Skin;

    public int Tier => _uData.Tier;

    public UnitData[] PromotionUnits => _uData.PromotionUnits;

    //unitData 각각 독립적으로 제작 필요
    //각 유닛의 체력만 다름

    public Sprite icon => _uData.Icon;

    private UnitHealth unitHealth { get; set; }

    public SkillData[] skills => _uData.Skills;

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
        return unitData.SquadCount == count;
    }


    public bool IsDead(int uKey)
    {
        if (typeUnit == TYPE_UNIT_FORMATION.Castle) return false;

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

    public int employCostValue => _uData.EmployCostValue;

    public int maintenenceCostValue => _uData.MaintenanceCostValue;

    public int PromotionCostValue => _uData.PromotionCostValue;

    public int damageValue => _uData.DamageValue;

    public int defensiveValue => _uData.DefensiveValue;

    public int movementValue => _uData.MovementValue;

    public bool IsAttack => _uData.IsAttack;

    public int attackCount => _uData.AttackCount;


    public TargetData AttackTargetData => _uData.AttackTargetData;

    public int priorityValue => _uData.PriorityValue;

    public int proficiencyValue => _uData.ProficiencyValue;

    public int squadCount => _uData.SquadCount;

    public TYPE_UNIT_FORMATION typeUnit => _uData.TypeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _uData.TypeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _uData.TypeUnitClass;

//    public TYPE_UNIT_ATTACK typeUnitAttack => _uData.typeUnitAttack;

    public TYPE_MOVEMENT typeMovement => _uData.TypeMovement;

//    public TYPE_UNIT_ATTACK_RANGE typeUnitAttackRange => _uData.typeUnitAttackRange;

//    public Vector2Int[] attackCells => _uData.attackCells;
    public Vector2Int[] movementCells => _uData.MovementCells;
    public Vector2Int[] chargeCells => _uData.ChargeCells;

    public AudioClip deadClip => _uData.DeadClip;
    public AudioClip attackClip => _uData.AttackClip;

    public BulletData BulletData => _uData.BulletData;

    public Vector2Int[] formationCells { get; private set; }


    public static UnitCard[] Create(UnitData[] unitDatas)
    {
        List<UnitCard> list = new List<UnitCard>();
        for(int i = 0; i < unitDatas.Length; i++)
        {
            list.Add(Create(unitDatas[i]));
        }
        return list.ToArray();
    }

    public static UnitCard Create(UnitData unitData)
    {
        return new UnitCard(unitData);
    }

    private UnitCard(UnitData unitData, bool isTest = false)
    {
        _uData = unitData;
        for(int i = 0; i < unitData.SquadCount; i++)
        {
            var uKey = UnitKeyGenerator.InsertKey(this);
            if(uKey >= 0)
            {
                var health = GetUnitHealth(uKey);
                if(health != null)
                {
                    health.SetMaxHealth(unitData);
                    health.SetNowHealth(unitData.HealthValue);
#if UNITY_EDITOR
                    if (isTest)
                    {
                        if(Random.Range(0, 100) > 50)
                            health.SetNowHealth(0);
                        else
                            health.SetNowHealth(Random.Range(0, unitData.HealthValue + 1));
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
                _unitDic[keys[i]].IncreaseHealth((int)(_uData.HealthValue * rate));
            }
        }
    }
    

    public int GetUnitNowHealth(int key)
    {
        return GetUnitHealth(key).nowHealthValue;
    }

    public int GetUnitMaxHealth(int key)
    {
        return GetUnitHealth(key).maxHealthValue;
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
                unitHealth.SetNowHealth(unitData.HealthValue);
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
