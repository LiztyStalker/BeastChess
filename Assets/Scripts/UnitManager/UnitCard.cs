using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;




public class UnitCard : IUnitKey
{

    #region ##### UnitHealthData #####

    public enum TYPE_UNIT_LIFE { Live, Dead }

    private class UnitHealthData
    {
        internal int key;
        internal TYPE_UNIT_LIFE unitLiveType;
        internal int nowHealthValue;
        internal int maxHealthValue;

        internal UnitHealthData(int key)
        {
            this.key = key;
            nowHealthValue = 0;
            maxHealthValue = 0;
        }

        internal void SetNowHealth(int value)
        {
            nowHealthValue = value;
        }

        internal void SetMaxHealth(UnitData uData)
        {
            maxHealthValue = uData.HealthValue;
        }

        internal void SetMaxHealth(UnitHealthData unitHealth)
        {
            maxHealthValue = unitHealth.maxHealthValue;
        }

        internal float HealthRate()
        {
            return (float)nowHealthValue / maxHealthValue;
        }
        internal bool IsDead() => nowHealthValue == 0;
        internal void DecreaseHealth(int value)
        {
            if (nowHealthValue - value < 0)
                nowHealthValue = 0;
            else
                nowHealthValue -= value;
        }

        internal void IncreaseHealth(int value)
        {
            if (nowHealthValue + value >= maxHealthValue)
                nowHealthValue = maxHealthValue;
            else
                nowHealthValue += value;
        }


        internal void SetTypeUnitLife(float weight)
        {
            if (Random.Range(0f, 1f) > weight)
                unitLiveType = TYPE_UNIT_LIFE.Dead;
        }

        internal void SetTypeUnitLife(TYPE_UNIT_LIFE typeUnitLive)
        {
            unitLiveType = typeUnitLive;
        }
    }

    #endregion


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

    /// <summary>
    /// 다중 UnitCard 생성
    /// </summary>
    /// <param name="unitDatas"></param>
    /// <returns></returns>
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

    /// <summary>
    /// UnitCard 생성
    /// </summary>
    /// <param name="unitData"></param>
    /// <returns></returns>
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

    /// <summary>
    /// UnitCard 테스트 생성
    /// </summary>
    /// <param name="unitData"></param>
    /// <returns></returns>
    public static UnitCard CreateTest(UnitData unitData)
    {
        return new UnitCard(unitData, true);
    }

#endif

    /// <summary>
    /// Key 적용
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
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
    private int _levelValue = 1;
    private int _nowExpValue = 0;
    private int _maxExpValue => _levelValue * 100;

    /// <summary>
    /// 경험치 증가
    /// </summary>
    /// <param name="value"></param>
    public void IncreaseExpValue(int value)
    {
        if (_levelValue < UNIT_LEVEL_MAX)
        {

            _nowExpValue += value;
            while (_nowExpValue > _maxExpValue)
            {
                _nowExpValue -= _maxExpValue;
                if (_levelValue + 1 < UNIT_LEVEL_MAX)
                    _levelValue++;

                if (_levelValue == UNIT_LEVEL_MAX)
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

    /// <summary>
    /// 살아있는 분대 수
    /// </summary>
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

    /// <summary>
    /// 모두 사망했는지 여부
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 해당 unitKey에 대해 사망했는지 여부
    /// </summary>
    /// <param name="uKey"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 체력 감소
    /// </summary>
    /// <param name="uKey"></param>
    /// <param name="value"></param>
    public void DecreaseHealth(int uKey, int value)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.DecreaseHealth(value);
        }
    }

    /// <summary>
    /// 체력 증가
    /// </summary>
    /// <param name="uKey"></param>
    /// <param name="value"></param>
    public void IncreaseHealth(int uKey, int value)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.IncreaseHealth(value);
        }
    }

    /// <summary>
    /// 병사 생명 상태 적용
    /// </summary>
    /// <param name="uKey"></param>
    public void SetTypeUnitLife(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            health.SetTypeUnitLife(CommanderActor.DEAD_RATE);
        }
    }

    /// <summary>
    /// 분대 체력 백분율
    /// </summary>
    /// <returns></returns>
    public float TotalHealthRate()
    {
        return (float)TotalNowHealthValue / TotalMaxHealthValue;
    }

    /// <summary>
    /// 병사 체력 백분율
    /// </summary>
    /// <param name="uKey"></param>
    /// <returns></returns>
    public float HealthRate(int uKey)
    {
        var health = GetUnitHealth(uKey);
        if (health != null)
        {
            return health.HealthRate();
        }
        return 0f;
    }

    /// <summary>
    /// 현재 분대 체력 
    /// </summary>
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

    /// <summary>
    /// 최대 분대 체력
    /// </summary>
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

    /// <summary>
    /// 분대 회복
    /// </summary>
    public void AllRecoveryUnits()
    {
        var keys = UnitKeys;
        for (int i = 0; i < keys.Length; i++)
        {
            var unit = _unitDic[keys[i]];
            _unitDic[keys[i]].SetNowHealth(_unitDic[keys[i]].maxHealthValue);
            _unitDic[keys[i]].SetTypeUnitLife(TYPE_UNIT_LIFE.Live);
        }
    }

    /// <summary>
    /// 분대 회복
    /// </summary>
    /// <param name="rate"></param>
    public void RecoveryUnits(float rate)
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
    

    /// <summary>
    /// 병사 현재 체력
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetUnitNowHealth(int key)
    {
        return GetUnitHealth(key).nowHealthValue;
    }

    /// <summary>
    /// 병사 최대 체력
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetUnitMaxHealth(int key)
    {
        return GetUnitHealth(key).maxHealthValue;
    }

    /// <summary>
    /// 병사체력 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private UnitHealthData GetUnitHealth(int key)
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
