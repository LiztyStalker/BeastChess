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
public enum TYPE_UNIT_ATTACK_RANGE { Normal, Triangle, Square, Vertical, Cross, Rhombus}

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
    [Header("Common")]
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

    [Header("Health")]
    [SerializeField]
    int _healthValue = 100;

    [Header("Attack")]
    [SerializeField]
    TYPE_UNIT_ATTACK _typeUnitAttack;

    [SerializeField]
    int _damageValue = 35;

    [SerializeField]
    int _attackCount = 1;

    [SerializeField]
    TYPE_UNIT_ATTACK_RANGE _typeUnitAttackRange;

    [SerializeField]
    int _minRangeValue = 0;

    [SerializeField]
    int _attackRangeValue = 1;

    [Header("Proficiency")]
    [SerializeField]
    int _proficiencyValue = 30;

    [Header("Movement")]
    [SerializeField]
    int _movementValue = 1;

    [SerializeField]
    TYPE_MOVEMENT _typeMovement = TYPE_MOVEMENT.Normal;

    [Header("Bullet")]
    [SerializeField]
    Sprite _bullet;


    [Header("Skill")]
    [SerializeField]
    SkillData[] _skills;

    //[Header("Movement")]
    //[SerializeField]
    //int _movementValue = 1;
    [Header("Priority")]
    [SerializeField]
    int _priorityValue = 0;

    [Header("Cost")]
    [SerializeField]
    int _employCostValue = 10;

    [SerializeField]
    int _maintenanceCostValue = 1;

    [Header("Sound")]
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

    public Sprite icon => _icon;

    public SkeletonDataAsset skeletonDataAsset => _skeletonDataAsset;

    public new string name => _name;

    public int healthValue => _healthValue;

    public int damageValue => _damageValue;

    public int attackCount => _attackCount;

    public int movementValue => _movementValue;

    public int employCostValue => _employCostValue;

    public int maintenanceCostValue => _maintenanceCostValue;

    public int priorityValue => _priorityValue;

    public int proficiencyValue => _proficiencyValue;

    public int squadCount => _squadCount;

    //public int rangeValue => _rangeValue;

    public int minRangeValue => _minRangeValue;

    public int attackRangeValue => _attackRangeValue;

    public int attackMinRangeValue => _minRangeValue;

    public TYPE_MOVEMENT typeMovement => _typeMovement;

    [System.NonSerialized]
    private Vector2Int[] _attackCells = null;
    [System.NonSerialized]
    private Vector2Int[] _movementCells = null;
    [System.NonSerialized]
    private Vector2Int[] _chargeCells = null;


    public Vector2Int[] attackCells => GetAttackCells((int)_attackRangeValue);

    public Vector2Int[] movementCells => GetMovementCells((int)_movementValue);

    public Vector2Int[] chargeCells => GetChargeCells((int)_movementValue);

    public TYPE_UNIT_FORMATION typeUnit => _typeUnit;

    public TYPE_UNIT_GROUP typeUnitGroup => _typeUnitGroup;

    public TYPE_UNIT_CLASS typeUnitClass => _typeUnitClass;

    public TYPE_UNIT_ATTACK_RANGE typeUnitAttackRange => _typeUnitAttackRange;

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

    //public static bool IsDefenceUnitClassOpposition(TYPE_UNIT_CLASS typeAttackUnitClass)
    //{
    //    switch (typeHitClass)
    //    {
    //        case TYPE_UNIT_CLASS.LightSoldier:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
    //        case TYPE_UNIT_CLASS.MiddleSoldier:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
    //        case TYPE_UNIT_CLASS.Shooter:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
    //        case TYPE_UNIT_CLASS.Skirmisher:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
    //        case TYPE_UNIT_CLASS.HeavySoldier:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.Shooter || typeAttackUnitClass == TYPE_UNIT_CLASS.Skirmisher || typeAttackUnitClass == TYPE_UNIT_CLASS.Wizard);
    //        case TYPE_UNIT_CLASS.Charger:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.HeavySoldier);
    //        case TYPE_UNIT_CLASS.Wizard:
    //            return (typeAttackUnitClass == TYPE_UNIT_CLASS.MiddleSoldier || typeAttackUnitClass == TYPE_UNIT_CLASS.LightSoldier);
    //        case TYPE_UNIT_CLASS.Supporter:
    //            break;
    //    }
    //    return false;
    //}

    public TYPE_UNIT_ATTACK typeUnitAttack => _typeUnitAttack;

    public string soldierName => _name;

    public AudioClip attackClip => _attackClip;

    public AudioClip deadClip => _deadClip;

    public AudioClip hitClip => _hitClip;

    public Sprite bullet => _bullet;

    public SkillData[] skills => _skills;

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

    private Vector2Int[] GetAttackCells(int range)
    {
        if (_attackCells != null) return _attackCells;

        List<Vector2Int> cells = new List<Vector2Int>();

        switch (_typeUnitAttackRange)
        {
            case TYPE_UNIT_ATTACK_RANGE.Normal:
                for(int x = 1; x <= range; x++)
                {
                    cells.Add(new Vector2Int(x, 0));
                }
                break;
            case TYPE_UNIT_ATTACK_RANGE.Vertical:
                for (int y = 0; y < range; y++)
                {
                    if (y == 0)
                    {
                        cells.Add(new Vector2Int(1, y));
                    }
                    else
                    {
                        cells.Add(new Vector2Int(1, y));
                        cells.Add(new Vector2Int(1, -y));
                    }
                }

                break;
            case TYPE_UNIT_ATTACK_RANGE.Triangle:
                for (int x = 1; x <= range; x++)
                {
                    cells.Add(new Vector2Int(x, 0));

                    for (int y = 1; y < x; y++)
                    {
                        cells.Add(new Vector2Int(x, y));
                        cells.Add(new Vector2Int(x, -y));
                    }
                }
                break;
            case TYPE_UNIT_ATTACK_RANGE.Square:
                for (int x = -range; x <= range; x++)
                {
                    cells.Add(new Vector2Int(x, 0));

                    for (int y = 1; y <= range; y++)
                    {
                        cells.Add(new Vector2Int(x, y));
                        cells.Add(new Vector2Int(x, -y));
                    }
                }
                break;
            //case TYPE_UNIT_ATTACK_RANGE.Rhombus:
            //    for (int x = -_attackRangeValue; x <= _attackRangeValue; x++)
            //    {
            //        cells.Add(new Vector2Int(x, 0));

            //        for (int y = 0; y < x; y++)
            //        {
            //            cells.Add(new Vector2Int(x, y));
            //            cells.Add(new Vector2Int(x, -y));
            //        }
            //    }
            //    break;
            case TYPE_UNIT_ATTACK_RANGE.Cross:
                for (int x = -range; x <= range + 1; x++)
                {
                    cells.Add(new Vector2Int(x, 0));

                    if (x == 0)
                    {
                        for (int y = 1; y <= range; y++)
                        {
                            cells.Add(new Vector2Int(x, y));
                            cells.Add(new Vector2Int(x, -y));
                        }
                    }
                }
                break;
        }

        _attackCells = cells.ToArray();

        return _attackCells;
    }
}
