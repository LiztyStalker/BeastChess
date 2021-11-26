using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using LitJson;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TYPE_UNIT_FORMATION { Castle = -1, Ground, Air, }


public enum TYPE_INFLUENCE { Herbivore, Carnivore, Omnivore }




[System.Flags]
public enum TYPE_UNIT_GROUP {
    None = 0,
    FootSoldier = 1,
    Shooter = 2,
    Charger = 4,
    Supporter = 8,
    All = 15
}

/// <summary>
/// 공격타입
/// </summary>
public enum TYPE_UNIT_ATTACK { Normal, Priority, RandomRange, Range}


public enum TYPE_UNIT_CLASS {
                                Building = -1,
                                LightSoldier,
                                MiddleSoldier,
                                Skirmisher,
                                Shooter,
                                Charger,
                                HeavySoldier,
                                Supporter,
                                Wizard,
                                Obstacle
                            }

public enum TYPE_UNIT_MOVEMENT { Normal, Rush, Penetration }


[System.Serializable]
public class UnitData : ScriptableObject
{
    [SerializeField]
    private string _key;

    [SerializeField]
    private TYPE_UNIT_FORMATION _typeUnit;

    [SerializeField]
    private TYPE_UNIT_GROUP _typeUnitGroup;

    [SerializeField]
    private TYPE_UNIT_CLASS _typeUnitClass;

    [SerializeField]
    private bool _isAppearBarracks;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private SkeletonDataAsset _skeletonDataAsset;

    [SerializeField]
    private string _characterKey;

    [SerializeField, SpineSkin(dataField: "_skeletonDataAsset")]
    private string _skin;

    [SerializeField]
    private int _tier;

    //직접 키를 등록해야 함
    [SerializeField]
    private string[] _promotionUnitKeys = new string[0];

    [SerializeField]
    private int _squadCount = 4;

    [SerializeField]
    private int _healthValue = 100;

    [SerializeField]
    private bool _isAttack = true;

    [SerializeField]
    private int _damageValue = 35;

    [SerializeField]
    private int _attackCount = 1;

    [SerializeField]
    private TargetData _targetData = new TargetData(true);

    [SerializeField]
    private int _defensiveValue = 0;

    [SerializeField]
    private int _proficiencyValue = 30;

    [SerializeField]
    private int _movementValue = 1;

    [SerializeField]
    private TYPE_UNIT_MOVEMENT _typeMovement = TYPE_UNIT_MOVEMENT.Normal;

    [SerializeField]
    private BulletData _bulletData;

    [SerializeField]
    private string _bulletDataKey;

    [SerializeField]
    private string[] _skillKeys = new string[0];

    [SerializeField]
    private int _priorityValue = 0;

    [SerializeField]
    private int _appearCostValue = 10;

    [SerializeField]
    private int _employCostValue = 10;

    [SerializeField]
    private int _maintenanceCostValue = 1;

    [SerializeField]
    private int _promotionCostValue = 10;

    [SerializeField]
    private AudioClip _attackClip;

    [SerializeField]
    private string _attackClipKey;

    [SerializeField]
    private AudioClip _deadClip;

    [SerializeField]
    private string _deadClipKey;

    [SerializeField]
    private AudioClip _hitClip;

    [SerializeField]
    private string _hitClipKey;


    #region ##### Getter Setter #####
    public string Key => _key;

    public Sprite Icon
    {
        get
        {
            if(_icon == null)
            {
                _icon = DataStorage.Instance.GetDataOrNull<Sprite>(_key, "Icon", null);
                Debug.Assert(_icon != null, $"UnitData : {_key}에 대한 아이콘을 찾을 수 없습니다");
            }
            return _icon;
        }
    }

    public SkeletonDataAsset SkeletonDataAsset
    {
        get
        {
            if(_skeletonDataAsset == null)
            {
                _skeletonDataAsset = DataStorage.Instance.GetDataOrNull<SkeletonDataAsset>(_characterKey, null, "SkeletonData");
                Debug.Assert(_skeletonDataAsset != null, $"UnitData : {_key}에 대한 SkeletonDataAsset을 찾을 수 없습니다");
            }
            return _skeletonDataAsset;
        }
    }

    public string Skin => _skin;
    public int Tier => _tier;

    public bool IsAppearBarracks => _isAppearBarracks;

    private UnitData[] _promotionUnits = null;
    public UnitData[] PromotionUnits
    {
        get
        {
            if(_promotionUnits == null)
            {
                Debug.Assert(_promotionUnitKeys != null, $"UnitData : PromotionUnitKeys가 null 입니다.");
                _promotionUnits = new UnitData[_promotionUnitKeys.Length];
                for (int i = 0; i < _promotionUnitKeys.Length; i++)
                {
                    _promotionUnits[i] = DataStorage.Instance.GetDataOrNull<UnitData>(_promotionUnitKeys[i]);
                    Debug.Assert(_promotionUnits[i] != null, $"UnitData : {_promotionUnitKeys[i]}에 대한 UnitData를 찾을 수 없습니다");
                }
            }
            return _promotionUnits;
        }
    }

    public int HealthValue => _healthValue;

    public int DamageValue => _damageValue;

    public bool IsAttack => _isAttack;

    public int AttackCount => _attackCount;

    public int DefensiveValue => _defensiveValue;

    public int MovementValue => _movementValue;

    public int AppearCostValue => _appearCostValue;

    public int EmployCostValue => _employCostValue;

    public int MaintenanceCostValue => _maintenanceCostValue;

    public int PromotionCostValue => _promotionCostValue;

    public int PriorityValue => _priorityValue;

    public int ProficiencyValue => _proficiencyValue;

    public int SquadCount => _squadCount;

    public TargetData AttackTargetData => _targetData;

    public TYPE_UNIT_MOVEMENT TypeMovement => _typeMovement;

    public TYPE_UNIT_FORMATION TypeUnit => _typeUnit;

    public TYPE_UNIT_GROUP TypeUnitGroup => _typeUnitGroup;

    public TYPE_UNIT_CLASS TypeUnitClass => _typeUnitClass;

    public AudioClip AttackClip
    {
        get
        {
            if (_attackClip == null && !string.IsNullOrEmpty(_attackClipKey))
            {
                _attackClip = DataStorage.Instance.GetDataOrNull<AudioClip>(_attackClipKey, null, null);
                Debug.Assert(_attackClip != null, $"UnitData : {_attackClipKey}에 대한 AudioClip을 찾을 수 없습니다");
            }
            return _attackClip;
        }
    }

    public AudioClip DeadClip {
        get
        {
            if (_deadClip == null && !string.IsNullOrEmpty(_deadClipKey))
            {
                _deadClip = DataStorage.Instance.GetDataOrNull<AudioClip>(_deadClipKey, null, null);
                Debug.Assert(_deadClip != null, $"UnitData : {_deadClipKey}에 대한 AudioClip을 찾을 수 없습니다");
            }
            return _deadClip;
        }
    }

    public AudioClip HitClip {
        get
        {
            if (_hitClip == null && !string.IsNullOrEmpty(_hitClipKey))
            {
                _hitClip = DataStorage.Instance.GetDataOrNull<AudioClip>(_hitClipKey, null, null);
                Debug.Assert(_hitClip != null, $"UnitData : {_hitClipKey}에 대한 AudioClip을 찾을 수 없습니다");
            }
            return _hitClip;
        }
    }

    public BulletData BulletData {
        get
        {
            if (_bulletData == null && !string.IsNullOrEmpty(_bulletDataKey))
            {
                _bulletData = DataStorage.Instance.GetDataOrNull<BulletData>(_bulletDataKey);
                Debug.Assert(_bulletData != null, $"UnitData : {_bulletDataKey}에 대한 BulletData를 찾을 수 없습니다");
            }
            return _bulletData;
        }
    }


    /// <summary>
    /// Promotion과 같은 문제
    /// </summary>
    private SkillData[] _skills = null;

    public SkillData[] Skills {
        get
        {
            if (_skills == null)
            {
                Debug.Assert(_skillKeys != null, $"UnitData : SkillKeys가 null 입니다.");
                _skills = new SkillData[_skillKeys.Length];
                for (int i = 0; i < _skillKeys.Length; i++)
                {
                    _skills[i] = DataStorage.Instance.GetDataOrNull<SkillData>(_skillKeys[i]);
                    Debug.Assert(_skills[i] != null, $"UnitData : {_skillKeys[i]}에 대한 SkillData를 찾을 수 없습니다");
                }
            }
            return _skills;
        }
    }


    #endregion

    public static bool IsAttackUnitClassOpposition(TYPE_UNIT_CLASS typeHitClass, TYPE_UNIT_CLASS typeAttackUnitClass)
    {
        switch (typeHitClass)
        {
            case TYPE_UNIT_CLASS.LightSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.Charger);
            case TYPE_UNIT_CLASS.MiddleSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Charger);
            case TYPE_UNIT_CLASS.Shooter:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.Skirmisher:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.HeavySoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier);
            case TYPE_UNIT_CLASS.Charger:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher);
            //case TYPE_UNIT_CLASS.Wizard:
            //    return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier);
            //case TYPE_UNIT_CLASS.Supporter:
            //    break;
        }
        return false;
    }

    public static bool IsDefenceUnitClassOpposition(TYPE_UNIT_CLASS typeHitClass, TYPE_UNIT_CLASS typeAttackUnitClass)
    {
        switch (typeHitClass)
        {
            case TYPE_UNIT_CLASS.LightSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.MiddleSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.Shooter:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.Charger);
            case TYPE_UNIT_CLASS.Skirmisher:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Charger);  
            case TYPE_UNIT_CLASS.HeavySoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher);
            case TYPE_UNIT_CLASS.Charger:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier);
            //case TYPE_UNIT_CLASS.Wizard:
            //    return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter);
            //case TYPE_UNIT_CLASS.Supporter:
            //    break;
        }
        return false;
    }


    #region ##### Editor #####

#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
    public UnitData()
    {
        _key = null;
        _typeUnit = TYPE_UNIT_FORMATION.Ground;
        _typeUnitGroup = TYPE_UNIT_GROUP.FootSoldier;
        _typeUnitClass = TYPE_UNIT_CLASS.LightSoldier;
        _icon = null;
        _skeletonDataAsset = null;
        _skin = null;
        _tier = 1;
        _promotionUnits = null;
        _squadCount = 1;
        _healthValue = 100;
        _isAttack = true;
        _damageValue = 10;
        _attackCount = 1;
        _targetData = new TargetData(true);
        _defensiveValue = 0;
        _proficiencyValue = 30;
        _movementValue = 1;
        _typeMovement = TYPE_UNIT_MOVEMENT.Normal;
        _bulletData = null;
        _skills = null;
        _priorityValue = 0;
        _appearCostValue = 10;
        _employCostValue = 10;
        _maintenanceCostValue = 1;
        _promotionCostValue = 10;
        _attackClip = null;
        _deadClip = null;
        _hitClip = null;
    }

    [UnityEditor.MenuItem("ScriptableObjects/Resources/Units")]
    public static UnitData Create()
    {
        UnitData asset = CreateInstance<UnitData>();
        AssetDatabase.CreateAsset(asset, string.Format("Assets/Resources/Units/UnitData.asset"));
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static UnitData Create(string key, JsonData jData)
    {
        UnitData asset = CreateInstance<UnitData>();
        asset.SetData(key, jData);
        var path = $"Assets/Data/Units/UnitData_{asset.Key}.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        return asset;
    }


    public void SetData(string key, JsonData jData)
    {
        _key = key;
        _isAppearBarracks = (jData.ContainsKey("IsAppearBarracks")) ? bool.Parse(jData["IsAppearBarracks"].ToString()) : false;
        //_icon = DataStorage.Instance.GetDataOrNull<Sprite>(_key, "Icon", null);
        _typeUnit = (jData.ContainsKey("Position")) ? (TYPE_UNIT_FORMATION)System.Enum.Parse(typeof(TYPE_UNIT_FORMATION), jData["Position"].ToString()) : TYPE_UNIT_FORMATION.Ground;
        _typeUnitGroup = (jData.ContainsKey("Group")) ? (TYPE_UNIT_GROUP)System.Enum.Parse(typeof(TYPE_UNIT_GROUP), jData["Group"].ToString()) : TYPE_UNIT_GROUP.FootSoldier;
        _typeUnitClass = (jData.ContainsKey("Class")) ? (TYPE_UNIT_CLASS)System.Enum.Parse(typeof(TYPE_UNIT_CLASS), jData["Class"].ToString())  : TYPE_UNIT_CLASS.LightSoldier;
        _characterKey = (jData.ContainsKey("Character")) ? jData["Character"].ToString() : null;
        //_skeletonDataAsset = (jData.ContainsKey("Character")) ? DataStorage.Instance.GetDataOrNull<SkeletonDataAsset>(jData["Character"].ToString(), null, "SkeletonData") : null;
        _skin = (jData.ContainsKey("Skin")) ? jData["Skin"].ToString() : "";
        _tier = (jData.ContainsKey("Tier")) ? int.Parse(jData["Tier"].ToString()) : 1;
        //_promotionUnits = (jData.ContainsKey("PromitionUnits")) ? DataStorage.Instance.GetDataArrayOrZero<UnitData>(jData["PromitionUnits"].ToString().Split('/')) : null;
        _promotionUnitKeys = (jData.ContainsKey("PromitionUnits")) ? jData["PromitionUnits"].ToString().Split('/') : null;
        _squadCount = (jData.ContainsKey("SquadCount")) ? int.Parse(jData["SquadCount"].ToString()) : 1;
        _healthValue = (jData.ContainsKey("HealthValue")) ? int.Parse(jData["HealthValue"].ToString()) : 100;
        _isAttack = (jData.ContainsKey("IsAttack")) ? ((jData["IsAttack"] != null) ? bool.Parse(jData["IsAttack"].ToString()) : true) : true;
        _damageValue = (jData.ContainsKey("AttackValue")) ? int.Parse(jData["AttackValue"].ToString()) : 30;
        _attackCount = (jData.ContainsKey("AttackCount")) ? int.Parse(jData["AttackCount"].ToString()) : 1;

        if (_targetData != null)
            _targetData.SetData(jData);
        else
            _targetData = TargetData.Create(jData);

        _defensiveValue = (jData.ContainsKey("DefensiveValue")) ? int.Parse(jData["DefensiveValue"].ToString()) : 0;
        _proficiencyValue = (jData.ContainsKey("ProficiencyValue")) ? int.Parse(jData["ProficiencyValue"].ToString()) : 10;
        _movementValue = (jData.ContainsKey("MovementValue")) ? int.Parse(jData["MovementValue"].ToString()) : 1;
        _typeMovement = (jData.ContainsKey("TypeMovement")) ? (TYPE_UNIT_MOVEMENT)System.Enum.Parse(typeof(TYPE_UNIT_MOVEMENT), jData["TypeMovement"].ToString()) : TYPE_UNIT_MOVEMENT.Normal;
        //_bulletData = (jData.ContainsKey("BulletDataKey")) ? DataStorage.Instance.GetDataOrNull<BulletData>(jData["BulletDataKey"].ToString())  : null;
        _bulletDataKey = (jData.ContainsKey("BulletDataKey")) ? jData["BulletDataKey"].ToString() : null;
        //_skills = (jData.ContainsKey("SkillKeys")) ? DataStorage.Instance.GetDataArrayOrZero<SkillData>(jData["SkillKeys"].ToString().Split('/')) : null;
        _skillKeys = (jData.ContainsKey("SkillKeys")) ? jData["SkillKeys"].ToString().Split('/') : null;
        _priorityValue = (jData.ContainsKey("PriorityValue")) ? int.Parse(jData["PriorityValue"].ToString()) : 0;
        _appearCostValue = (jData.ContainsKey("AppearCostValue")) ? int.Parse(jData["AppearCostValue"].ToString()) : 10;
        _employCostValue = (jData.ContainsKey("EmployCostValue")) ? int.Parse(jData["EmployCostValue"].ToString()) : 100;
        _maintenanceCostValue = (jData.ContainsKey("MaintenanceCostValue")) ? int.Parse(jData["MaintenanceCostValue"].ToString()) : 5;
        _promotionCostValue = (jData.ContainsKey("PromotionCostValue")) ? int.Parse(jData["PromotionCostValue"].ToString()) : 100;
        //_attackClip = (jData.ContainsKey("AttackClipKey")) ? ((jData["AttackClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["AttackClipKey"].ToString(), null, null) : null) : null;
        //_deadClip = (jData.ContainsKey("DeadClipKey")) ? ((jData["DeadClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["DeadClipKey"].ToString(), null, null) : null) : null;
        //_hitClip = (jData.ContainsKey("HitClipKey")) ? ((jData["HitClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["HitClipKey"].ToString(), null, null) : null) : null;
        _attackClipKey = (jData.ContainsKey("AttackClipKey")) ? jData["AttackClipKey"].ToString() : null;
        _deadClipKey = (jData.ContainsKey("DeadClipKey")) ? jData["DeadClipKey"].ToString() : null;
        _hitClipKey = (jData.ContainsKey("HitClipKey")) ? jData["HitClipKey"].ToString() : null;

        EditorUtility.SetDirty(this);
    }

#endif

    #endregion
}
