using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;



#region ##### UnitHealthData #####

//리팩토링 - GetUnitHealth 리팩토링 진행하면 UnitCard 안으로 넣을 준비 필요
public enum TYPE_UNIT_LIFE {Live, Dead}

public class UnitHealthData
{
    public int key;
    public TYPE_UNIT_LIFE unitLiveType;
    public int nowHealthValue;
    public int maxHealthValue;

    public UnitHealthData(int key)
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

    public void SetMaxHealth(UnitHealthData unitHealth)
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


    public void SetTypeUnitLife(float weight)
    {
        if (Random.Range(0f, 1f) > weight)
            unitLiveType = TYPE_UNIT_LIFE.Dead;
    }

    public void SetUnitLiveType(TYPE_UNIT_LIFE typeUnitLive)
    {
        unitLiveType = typeUnitLive;
    }
}

#endregion



public class UnitCard : IUnitKey
{
    private const int UNIT_LEVEL_MAX = 9;

    private UnitData _uData { get; set; }
    public UnitData UnitData => _uData;
    public SkeletonDataAsset SkeletonDataAsset => _uData.SkeletonDataAsset;

    public string UnitName => TranslatorStorage.Instance.GetTranslator<UnitData>(_uData.Key, "Name");

    public string Description => TranslatorStorage.Instance.GetTranslator<UnitData>(_uData.Key, "Description");

    public string Skin => _uData.Skin;

    public int Tier => _uData.Tier;

    public UnitData[] PromotionUnits => _uData.PromotionUnits;

    public Sprite Icon => _uData.Icon;

    private UnitHealthData _unitHealth { get; set; }

    public SkillData[] Skills => _uData.Skills;

    public int AppearCostValue => _uData.AppearCostValue;

    public int EmployCostValue => _uData.EmployCostValue;

    public int MaintenenceCostValue => _uData.MaintenanceCostValue;

    public int PromotionCostValue => _uData.PromotionCostValue;

    public int DamageValue => _uData.DamageValue;

    public int DefensiveValue => _uData.DefensiveValue;

    public int MovementValue => _uData.MovementValue;

    public bool IsAttack => _uData.IsAttack;

    public int AttackCount => _uData.AttackCount;

    public TargetData AttackTargetData => _uData.AttackTargetData;

    public int PriorityValue => _uData.PriorityValue;

    public int ProficiencyValue => _uData.ProficiencyValue;

    public int SquadCount => _uData.SquadCount;

    public TYPE_UNIT_FORMATION TypeUnit => _uData.TypeUnit;

    public TYPE_UNIT_GROUP TypeUnitGroup => _uData.TypeUnitGroup;

    public TYPE_UNIT_CLASS TypeUnitClass => _uData.TypeUnitClass;

    public TYPE_UNIT_MOVEMENT TypeMovement => _uData.TypeMovement;



    public AudioClip DeadClip => _uData.DeadClip;
    public AudioClip AttackClip => _uData.AttackClip;
    public BulletData BulletData => _uData.BulletData;



    #region ##### Initialize && CleanUp #####

    public static UnitCard[] Create(UnitData[] unitDatas)
    {
        List<UnitCard> list = new List<UnitCard>(unitDatas.Length);
        for (int i = 0; i < unitDatas.Length; i++)
        {
            var data = Create(unitDatas[i]);
            list.Add(data);
        }
        return list.ToArray();
    }

    public static UnitCard Create(UnitData unitData)
    {
        return new UnitCard(unitData);
    }


    private UnitCard(UnitData unitData, bool IsTest = false)
    {
        _uData = unitData;
        if (!IsTest)
        {
            for (int i = 0; i < unitData.SquadCount; i++)
            {
                var uKey = UnitKeyGenerator.InsertKey(this);
                if (uKey >= 0)
                {
                    var health = GetUnitHealth(uKey);
                    if (health != null)
                    {
                        health.SetMaxHealth(unitData);
                        health.SetNowHealth(unitData.HealthValue);
                    }
                }
            }

            var formations = GetRandomFormation(unitData.SquadCount);
            SetFormation(formations);
        }
        else
        {
            var uKey = UnitKeyGenerator.InsertKey(this);
            if (uKey >= 0)
            {
                var health = GetUnitHealth(uKey);
                if (health != null)
                {
                    health.SetMaxHealth(unitData);
                    health.SetNowHealth(unitData.HealthValue);
                }
                SetFormation(new Vector2Int[] { Vector2Int.zero });
            }
        }
    }

#if UNITY_EDITOR
    public static UnitCard CreateTest(UnitData unitData)
    {
        return new UnitCard(unitData, true);
    }

#endif


    public bool SetKey(int key)
    {
        if (!_unitDic.ContainsKey(key))
        {
            _unitDic.Add(key, new UnitHealthData(key));
            return true;
        }
        return false;
    }



    #endregion



    #region ##### Exp #####
    public int LevelValue { get; private set; } = 1;
    private int _nowExpValue { get; set; }
    private int _maxExpValue => LevelValue * 100;

    public void IncreaseExpValue(int value)
    {
        if (LevelValue < UNIT_LEVEL_MAX)
        {

            _nowExpValue += value;
            while (_nowExpValue > _maxExpValue)
            {
                _nowExpValue -= _maxExpValue;
                if (LevelValue + 1 < UNIT_LEVEL_MAX)
                    LevelValue++;

                if (LevelValue == UNIT_LEVEL_MAX)
                {
                    _nowExpValue = 0;
                    break;
                }
            }
        }
    }
    #endregion



    #region ##### Health #####

    private Dictionary<int, UnitHealthData> _unitDic = new Dictionary<int, UnitHealthData>();
    public int[] UnitKeys => _unitDic.Keys.ToArray();

    public int LiveSquadCount
    {
        get
        {
            int liveSquadCount = 0;
            foreach (var value in _unitDic.Values)
            {
                if (!value.IsDead()) liveSquadCount++;
            }
            return liveSquadCount;
        }
    }

    public bool IsAllDead()
    {
#if UNITY_EDITOR
        if (_unitHealth != null) return _unitHealth.IsDead();
#endif

        int count = 0;
        foreach (var value in _unitDic.Values)
        {
            if (value.IsDead()) count++;
        }
        return UnitData.SquadCount == count;
    }


    public bool IsDead(int uKey)
    {
        if (TypeUnit == TYPE_UNIT_FORMATION.Castle) return false;

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

    public void SetTypeUnitLife(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.SetTypeUnitLife(CommanderActor.DEAD_RATE);
        }
    }

    public float TotalHealthRate()
    {
        return (float)TotalNowHealthValue / TotalMaxHealthValue;
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

    public int TotalNowHealthValue
    {
        get
        {
#if UNITY_EDITOR
            if (_unitHealth != null) return _unitHealth.nowHealthValue;
#endif
            int value = 0;
            foreach (var data in _unitDic.Values)
            {
                value += data.nowHealthValue;
            }
            return value;
        }
    }

    public int TotalMaxHealthValue
    {
        get
        {
#if UNITY_EDITOR
            if (_unitHealth != null) return _unitHealth.maxHealthValue;
#endif

            int value = 0;
            foreach (var data in _unitDic.Values)
            {
                value += data.maxHealthValue;
            }
            return value;
        }
    }


    public void AllRecoveryUnit()
    {
        var keys = UnitKeys;
        for (int i = 0; i < keys.Length; i++)
        {
            var unit = _unitDic[keys[i]];
            _unitDic[keys[i]].SetNowHealth(_unitDic[keys[i]].maxHealthValue);
            _unitDic[keys[i]].SetUnitLiveType(TYPE_UNIT_LIFE.Live);
        }
    }

    public void RecoveryUnit(float rate)
    {
        var keys = UnitKeys;
        for(int i = 0; i < keys.Length; i++)
        {
            var unit = _unitDic[keys[i]];
            if (unit.unitLiveType == TYPE_UNIT_LIFE.Live)
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

    /// 리팩토링 필요 - 범용적이라 특정 행동을 하도록만 제작 필요
    public UnitHealthData GetUnitHealth(int key)
    {
        if (_unitDic.ContainsKey(key))
        {
            return _unitDic[key];
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"UnitDic is Not ContainsKey '{key}'. but Created Temporary Data In UnitEditor");
            if (_unitHealth == null)
            {
                _unitHealth = new UnitHealthData(key);
                _unitHealth.SetMaxHealth(UnitData);
                _unitHealth.SetNowHealth(UnitData.HealthValue);
            }
        }
#endif
        return _unitHealth;
    }



    #endregion



    #region ##### Formation #####
    
    public Vector2Int[] FormationCells { get; private set; }


    ///Test는 테스트용도로만 제작
    public void SetFormation(Vector2Int[] formations)
    {
        FormationCells = formations;
    }

    private Vector2Int[] GetRandomFormation(int count)
    {
        var formation = CreateFormationArray();
        for (int i = 0; i < count; i++)
        {
            formation = GetRandomFormation(formation);
        }

        return ConvertArrayToListFormation(formation);
    }

    private bool[][] CreateFormationArray()
    {
        bool[][] formation = new bool[3][];
        for (int i = 0; i < formation.Length; i++)
        {
            formation[i] = new bool[3];
        }
        return formation;
    }

    private bool[][] GetRandomFormation(bool[][] formation, int stackCnt = 100)
    {
        if (stackCnt >= 0)
        {
            stackCnt--;
            var dirX = Random.Range(0, formation.Length);
            var dirY = Random.Range(0, formation.Length);

            if (!formation[dirY][dirX])
                formation[dirY][dirX] = true;
            else
                return GetRandomFormation(formation, stackCnt);
        }
        return formation;
    }

    private Vector2Int[] ConvertArrayToListFormation(bool[][] formation)
    {
        List<Vector2Int> formationList = new List<Vector2Int>();
        for (int y = 0; y < formation.Length; y++)
        {
            for (int x = 0; x < formation[y].Length; x++)
            {
                if (formation[y][x])
                    formationList.Add(new Vector2Int(x - 1, y - 1));
            }
        }
        return formationList.ToArray();
    }

    #endregion
}
