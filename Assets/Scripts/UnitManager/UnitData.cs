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

/// <summary>
/// 공격범위타입
/// </summary>
public enum TYPE_UNIT_ATTACK_RANGE { Normal, Triangle, Square, Vertical, Cross, Rhombus, Round}

/// <summary>
/// 방어
/// </summary>
public enum TYPE_UNIT_ARMOR {None, Light, Middle, Heavy}

public enum TYPE_UNIT_CLASS {
                                Building = -1,
                                LightSoldier,
                                MiddleSoldier,
                                Skirmisher,
                                Shooter,
                                Charger,
                                HeavySoldier,
                                Supporter,
                                Wizard
                            }

public enum TYPE_MOVEMENT { Normal, Rush, Penetration }


[System.Serializable]
public class UnitData : ScriptableObject
{
    [SerializeField]
    string _key;

    [SerializeField]
    TYPE_UNIT_FORMATION _typeUnit;

    [SerializeField]
    TYPE_UNIT_GROUP _typeUnitGroup;

    [SerializeField]
    TYPE_UNIT_CLASS _typeUnitClass;

    [SerializeField]
    Sprite _icon;

    [SerializeField]
    SkeletonDataAsset _skeletonDataAsset;

    [SerializeField, SpineSkin(dataField: "_skeletonDataAsset")]
    string _skin;

    [SerializeField]
    int _tier;

    [SerializeField]
    UnitData[] _promotionUnits;

    [SerializeField]
    int _squadCount = 4;

    [SerializeField]
    int _healthValue = 100;

    [SerializeField]
    private bool _isAttack = true;

    [SerializeField]
    int _damageValue = 35;

    [SerializeField]
    int _attackCount = 1;

    [SerializeField]
    private TargetData _targetData = new TargetData(true);

    [SerializeField]
    private int _defensiveValue = 0;

    [SerializeField]
    int _proficiencyValue = 30;

    [SerializeField]
    int _movementValue = 1;

    [SerializeField]
    TYPE_MOVEMENT _typeMovement = TYPE_MOVEMENT.Normal;

    [SerializeField]
    BulletData _bulletData;

    [SerializeField]
    SkillData[] _skills;

    [SerializeField]
    int _priorityValue = 0;

    [SerializeField]
    int _appearCostValue = 10;

    [SerializeField]
    int _employCostValue = 10;

    [SerializeField]
    int _maintenanceCostValue = 1;

    [SerializeField]
    int _promotionCostValue = 10;

    [SerializeField]
    AudioClip _attackClip;

    [SerializeField]
    AudioClip _deadClip;

    [SerializeField]
    AudioClip _hitClip;

    //[Header("Attack Range")]
    //[SerializeField]
    //Vector2Int[] _attackCells = new Vector2Int[] { new Vector2Int(1, 0) };

    //[Header("Movement Range")]
    //[SerializeField]
    //Vector2Int[] _movementCells = new Vector2Int[] { new Vector2Int(1, 0) };


    [System.NonSerialized]
    private Vector2Int[] _attackCells = null;
    [System.NonSerialized]
    private Vector2Int[] _movementCells = null;
    [System.NonSerialized]
    private Vector2Int[] _chargeCells = null;


    #region ##### Getter Setter #####

    public Sprite Icon => _icon;

    public string Key => _key;

    //public new string name => //번역

    public SkeletonDataAsset SkeletonDataAsset => _skeletonDataAsset;

    public string Skin => _skin;
    public int Tier => _tier;
    public UnitData[] PromotionUnits => _promotionUnits;

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

    public TYPE_MOVEMENT TypeMovement => _typeMovement;

    public Vector2Int[] MovementCells => GetMovementCells((int)_movementValue);

    public Vector2Int[] ChargeCells => GetChargeCells((int)_movementValue);

    public TYPE_UNIT_FORMATION TypeUnit => _typeUnit;

    public TYPE_UNIT_GROUP TypeUnitGroup => _typeUnitGroup;

    public TYPE_UNIT_CLASS TypeUnitClass => _typeUnitClass;

    public AudioClip AttackClip => _attackClip;

    public AudioClip DeadClip => _deadClip;

    public AudioClip HitClip => _hitClip;

    public BulletData BulletData => _bulletData;

    public SkillData[] Skills => _skills;


    #endregion

    public static bool IsAttackUnitClassOpposition(TYPE_UNIT_CLASS typeHitClass, TYPE_UNIT_CLASS typeAttackUnitClass)
    {
        switch (typeHitClass)
        {
            case TYPE_UNIT_CLASS.LightSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.MiddleSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.Shooter:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.Skirmisher:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.HeavySoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.Wizard);
            case TYPE_UNIT_CLASS.Charger:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
            case TYPE_UNIT_CLASS.Wizard:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier);
            case TYPE_UNIT_CLASS.Supporter:
                break;
        }
        return false;
    }

    public static bool IsDefenceUnitClassOpposition(TYPE_UNIT_CLASS typeHitClass, TYPE_UNIT_CLASS typeAttackUnitClass)
    {
        switch (typeHitClass)
        {
            case TYPE_UNIT_CLASS.LightSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher);
            case TYPE_UNIT_CLASS.MiddleSoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier);
            case TYPE_UNIT_CLASS.Shooter:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier);
            case TYPE_UNIT_CLASS.Skirmisher:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter);  
            case TYPE_UNIT_CLASS.HeavySoldier:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Charger || typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier);
            case TYPE_UNIT_CLASS.Charger:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher);
            case TYPE_UNIT_CLASS.Wizard:
                return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter);
            case TYPE_UNIT_CLASS.Supporter:
                break;
        }
        return false;
    }

   

#if UNITY_EDITOR
    [UnityEditor.MenuItem("ScriptableObjects/Resources/Units")]
    public static UnitData Create()
    {
        UnitData asset = CreateInstance<UnitData>();
        AssetDatabase.CreateAsset(asset, string.Format("Assets/Resources/Units/UnitData.asset"));
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static UnitData Create(JsonData jData)
    {
        UnitData asset = CreateInstance<UnitData>();
        asset.SetData(jData);        
        AssetDatabase.CreateAsset(asset, string.Format($"Assets/Data/Units/UnitData_{asset.Key}.asset"));
        AssetDatabase.SaveAssets();
        return asset;
    }

    public void SetData(JsonData jData)
    {
        _key = (jData.ContainsKey("Key")) ?(jData["Key"].ToString()) : null;
        _icon = (jData.ContainsKey("Key")) ? DataStorage.Instance.GetDataOrNull<Sprite>(jData["Key"].ToString(), "Icon", null) : null;
        _typeUnitGroup = (jData.ContainsKey("Group")) ? (TYPE_UNIT_GROUP)System.Enum.Parse(typeof(TYPE_UNIT_GROUP), jData["Group"].ToString()) : TYPE_UNIT_GROUP.FootSoldier;
        _typeUnitClass = (jData.ContainsKey("Class")) ? (TYPE_UNIT_CLASS)System.Enum.Parse(typeof(TYPE_UNIT_CLASS), jData["Class"].ToString())  : TYPE_UNIT_CLASS.LightSoldier;
        _skeletonDataAsset = (jData.ContainsKey("Character")) ? DataStorage.Instance.GetDataOrNull<SkeletonDataAsset>(jData["Character"].ToString(), null, "SkeletonData") : null;
        _skin = (jData.ContainsKey("Skin")) ? jData["Skin"].ToString() : "";
        _tier = (jData.ContainsKey("Tier")) ? int.Parse(jData["Tier"].ToString()) : 1;
        _promotionUnits = (jData.ContainsKey("PromitionUnits")) ? DataStorage.Instance.GetDataArrayOrZero<UnitData>(jData["PromitionUnits"].ToString().Split('/')) : null;
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
        _typeMovement = (jData.ContainsKey("TypeMovement")) ? (TYPE_MOVEMENT)System.Enum.Parse(typeof(TYPE_MOVEMENT), jData["TypeMovement"].ToString()) : TYPE_MOVEMENT.Normal;
        _bulletData = (jData.ContainsKey("BulletDataKey")) ? DataStorage.Instance.GetDataOrNull<BulletData>(jData["BulletDataKey"].ToString())  : null;
        _skills = (jData.ContainsKey("SkillKeys")) ? DataStorage.Instance.GetDataArrayOrZero<SkillData>(jData["SkillKeys"].ToString().Split('/')) : null;
        _priorityValue = (jData.ContainsKey("PriorityValue")) ? int.Parse(jData["PriorityValue"].ToString()) : 0;
        _appearCostValue = (jData.ContainsKey("AppearCostValue")) ? int.Parse(jData["AppearCostValue"].ToString()) : 10;
        _employCostValue = (jData.ContainsKey("EmployCostValue")) ? int.Parse(jData["EmployCostValue"].ToString()) : 100;
        _maintenanceCostValue = (jData.ContainsKey("MaintenanceCostValue")) ? int.Parse(jData["MaintenanceCostValue"].ToString()) : 5;
        _promotionCostValue = (jData.ContainsKey("PromotionCostValue")) ? int.Parse(jData["PromotionCostValue"].ToString()) : 100;
        _attackClip = (jData.ContainsKey("AttackClipKey")) ? ((jData["AttackClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["AttackClipKey"].ToString(), null, null) : null) : null;
        _deadClip = (jData.ContainsKey("DeadClipKey")) ? ((jData["DeadClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["DeadClipKey"].ToString(), null, null) : null) : null;
        _hitClip = (jData.ContainsKey("HitClipKey")) ? ((jData["HitClipKey"] != null) ? DataStorage.Instance.GetDataOrNull<AudioClip>(jData["HitClipKey"].ToString(), null, null) : null) : null;
    }

#endif

    private Vector2Int[] GetMovementCells(int range)
    {
        if (_movementCells != null) return _movementCells;

        List<Vector2Int> cells = new List<Vector2Int>();
        for(int x = 1; x <= range; x++)
        {
            cells.Add(new Vector2Int(range - x + 1, 0));
        }
        _movementCells = cells.ToArray();
        return _movementCells;
    }

    private Vector2Int[] GetChargeCells(int range)
    {
        if (_chargeCells != null) return _chargeCells;

        List<Vector2Int> cells = new List<Vector2Int>();
        for (int x = 1; x <= range * 2; x++)
        {
            cells.Add(new Vector2Int(range * 2 - x + 1, 0));
        }
        _chargeCells = cells.ToArray();
        return _chargeCells;
    }

}
