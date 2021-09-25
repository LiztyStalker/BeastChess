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
    string _name;

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

    public SkeletonDataAsset SkeletonDataAsset => _skeletonDataAsset;

    public new string name => _name;

    public int HealthValue => _healthValue;

    public int DamageValue => _damageValue;

    public bool IsAttack => _isAttack;

    public int AttackCount => _attackCount;

    public int MovementValue => _movementValue;

    public int EmployCostValue => _employCostValue;

    public int MaintenanceCostValue => _maintenanceCostValue;

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


    public string SoldierName => _name;

    public AudioClip AttackClip => _attackClip;

    public AudioClip DeadClip => _deadClip;

    public AudioClip HitClip => _hitClip;

    public BulletData BulletData => _bulletData;

    public SkillData[] Skills => _skills;

    public int DefensiveValue => _defensiveValue;

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

    //private Vector2Int[] GetAttackCells(int range)
    //{
    //    if (_attackCells != null) return _attackCells;

    //    List<Vector2Int> cells = new List<Vector2Int>();

    //    switch (_typeUnitAttackRange)
    //    {
    //        case TYPE_UNIT_ATTACK_RANGE.Normal:
    //            for(int x = 1; x <= range; x++)
    //            {
    //                cells.Add(new Vector2Int(x, 0));
    //            }
    //            break;
    //        case TYPE_UNIT_ATTACK_RANGE.Vertical:
    //            for (int y = 0; y < range; y++)
    //            {
    //                if (y == 0)
    //                {
    //                    cells.Add(new Vector2Int(1, y));
    //                }
    //                else
    //                {
    //                    cells.Add(new Vector2Int(1, y));
    //                    cells.Add(new Vector2Int(1, -y));
    //                }
    //            }

    //            break;
    //        case TYPE_UNIT_ATTACK_RANGE.Triangle:
    //            for (int x = 1; x <= range; x++)
    //            {
    //                cells.Add(new Vector2Int(x, 0));

    //                for (int y = 1; y < x; y++)
    //                {
    //                    cells.Add(new Vector2Int(x, y));
    //                    cells.Add(new Vector2Int(x, -y));
    //                }
    //            }
    //            break;
    //        case TYPE_UNIT_ATTACK_RANGE.Square:
    //            for (int x = -range; x <= range; x++)
    //            {
    //                cells.Add(new Vector2Int(x, 0));

    //                for (int y = 1; y <= range; y++)
    //                {
    //                    cells.Add(new Vector2Int(x, y));
    //                    cells.Add(new Vector2Int(x, -y));
    //                }
    //            }
    //            break;
    //        //case TYPE_UNIT_ATTACK_RANGE.Rhombus:
    //        //    for (int x = -_attackRangeValue; x <= _attackRangeValue; x++)
    //        //    {
    //        //        cells.Add(new Vector2Int(x, 0));

    //        //        for (int y = 0; y < x; y++)
    //        //        {
    //        //            cells.Add(new Vector2Int(x, y));
    //        //            cells.Add(new Vector2Int(x, -y));
    //        //        }
    //        //    }
    //        //    break;
    //        case TYPE_UNIT_ATTACK_RANGE.Cross:
    //            for (int x = -range; x <= range + 1; x++)
    //            {
    //                cells.Add(new Vector2Int(x, 0));

    //                if (x == 0)
    //                {
    //                    for (int y = 1; y <= range; y++)
    //                    {
    //                        cells.Add(new Vector2Int(x, y));
    //                        cells.Add(new Vector2Int(x, -y));
    //                    }
    //                }
    //            }
    //            break;
    //    }

    //    _attackCells = cells.ToArray();

    //    return _attackCells;
    //}
}
