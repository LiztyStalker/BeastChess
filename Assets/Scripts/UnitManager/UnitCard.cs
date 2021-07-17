using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Linq;

public class UnitHealth
{
    public int key;
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
}




public class UnitCard : IUnitKey
{
    public Sprite icon => _uData.icon;
    public SkeletonDataAsset skeletonDataAsset => _uData.skeletonDataAsset;
    
    private UnitData _uData { get; set; }

    public UnitData unitData => _uData;

    //unitData 각각 독립적으로 제작 필요
    //각 유닛의 체력만 다름


    private UnitHealth unitHealth;

    private Dictionary<int, UnitHealth> _unitDic = new Dictionary<int, UnitHealth>();

    public int[] unitArray => _unitDic.Keys.ToArray();

    //    private Dictionary<UnitActor, UnitHealth> unitHealthValues = new Dictionary<UnitActor, UnitHealth>();

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


    public int costValue => _uData.costValue;
    public string name => _uData.name;

    public int damageValue => _uData.damageValue;

    public int attackCount => _uData.attackCount;

    public int minRangeValue => _uData.minRangeValue;

    public int priorityValue => _uData.priorityValue;

    public int proficiencyValue => _uData.proficiencyValue;

    public int squadCount => _uData.squadCount;

    public TYPE_UNIT_FORMATION typeUnit => _uData.typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _uData.typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _uData.typeUnitClass;

    public TYPE_UNIT_ATTACK typeUnitAttack => _uData.typeUnitAttack;

    public TYPE_MOVEMENT typeMovement => _uData.typeMovement;

    public Vector2Int[] attackCells => _uData.attackCells;
    public Vector2Int[] movementCells => _uData.movementCells;
    public Vector2Int[] chargeCells => _uData.chargeCells;

    public AudioClip deadClip => _uData.deadClip;
    public AudioClip attackClip => _uData.attackClip;

    public Sprite bullet => _uData.bullet;

    public Vector2Int[] formationCells { get; private set; }


    public UnitCard(UnitData unitData)
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
                }
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
