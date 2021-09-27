using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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

    public string key => name;

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

    [System.Obsolete("JsonData로 변경필요")]
    public static UnitData Create(Object jData)
    {
        UnitData asset = CreateInstance<UnitData>();
        asset.SetData(jData);
        AssetDatabase.CreateAsset(asset, string.Format($"Assets/Resources/Units/UnitData_{jData.name}.asset"));
        AssetDatabase.SaveAssets();
        return asset;
    }

    private void SetData(Object jData)
    {
        //_icon = (jData.Contain("Key")) ? jData["Key"].ToString()  : "";
        //_typeUnitGroup = (jData.Contain("Group")) ? jData["Group"].ToString()  : "";
        //_typeUnitClass = (jData.Contain("Class")) ? jData["Class"].ToString()  : "";
        //_skeletonDataAsset = (jData.Contain("Character")) ? jData["Character"].ToString()  : "";
        //_skin = (jData.Contain("Skin")) ? jData["Skin"].ToString()  : "";
        //_tier = (jData.Contain("Tier")) ? jData["Tier"].ToString()  : "";
        //_promotionUnits = (jData.Contain("PromitionUnits")) ? jData["PromitionUnits"].ToString()  : "";
        //_squadCount = (jData.Contain("SquadCount")) ? jData["SquadCount"].ToString()  : "";
        //_healthValue = (jData.Contain("HealthValue")) ? jData["HealthValue"].ToString()  : "";
        //_isAttack = (jData.Contain("IsAttack")) ? jData["IsAttack"].ToString()  : "";
        //_damageValue = (jData.Contain("AttackValue")) ? jData["AttackValue"].ToString()  : "";
        //_attackCount = (jData.Contain("AttackCount")) ? jData["AttackCount"].ToString()  : "";
        //_targetData = new TargetData(jData);
        //_defensiveValue = (jData.Contain("DefensiveValue")) ? jData["DefensiveValue"].ToString()  : "";
        //_proficiencyValue = (jData.Contain("ProficiencyValue")) ? jData["ProficiencyValue"].ToString()  : "";
        //_movementValue = (jData.Contain("MovementValue")) ? jData["MovementValue"].ToString()  : "";
        //_typeMovement = (jData.Contain("TypeMovement")) ? jData["TypeMovement"].ToString()  : "";
        //_bulletData = (jData.Contain("BulletDataKey")) ? jData["BulletDataKey"].ToString()  : "";
        //_skills = (jData.Contain("SkillKeys")) ? jData["SkillKeys"].ToString()  : "";
        //_priorityValue = (jData.Contain("PriorityValue")) ? jData["PriorityValue"].ToString()  : "";
        //_employCostValue = (jData.Contain("EmployCostValue")) ? jData["EmployCostValue"].ToString()  : "";
        //_maintenanceCostValue = (jData.Contain("MaintenanceCostValue")) ? jData["MaintenanceCostValue"].ToString()  : "";
        //_promotionCostValue = (jData.Contain("PromotionCostValue")) ? jData["PromotionCostValue"].ToString()  : "";
        //_attackClip = (jData.Contain("AttackClipKey")) ? jData["AttackClipKey"].ToString()  : "";
        //_deadClip = (jData.Contain("DeadClipKey")) ? jData["DeadClipKey"].ToString()  : "";
        //_hitClip = (jData.Contain("HitClipKey")) ? jData["HitClipKey"].ToString()  : "";
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
