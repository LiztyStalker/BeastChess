using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public enum TYPE_UNIT { Castle = -1, Ground, Air, }

public enum TYPE_UNIT_ATTACK { Normal, Priority, RandomRange, Range}

[System.Serializable]
public struct CellField
{
    [SerializeField]
    public bool[] cells;
}

[System.Serializable]
public struct CellGrid
{
    [SerializeField]
    public CellField[] cellFields;
}

[System.Serializable]
public class UnitData : ScriptableObject
{
    [Header("Common")]
    [SerializeField]
    string _name;

    [SerializeField]
    TYPE_UNIT _typeUnit;

    [SerializeField]
    Sprite _icon;

    [SerializeField]
    SkeletonDataAsset _skeletonDataAsset;

    [Header("Health")]
    [Range(1, 1000)]
    [SerializeField]
    int _healthValue = 100;

    [Header("Attack")]
    [SerializeField]
    TYPE_UNIT_ATTACK _typeUnitAttack;

    [SerializeField]
    int _damageValue = 35;

    [SerializeField]
    int _attackCount = 1;

    [Header("Bullet")]
    [SerializeField]
    GameObject _bullet;


    //[Header("Movement")]
    //[SerializeField]
    //int _movementValue = 1;
    [Header("Priority")]
    [SerializeField]
    int _priorityValue = 0;

    [Header("Cost")]
    [SerializeField]
    int _costValue = 1;

    [Header("Sound")]
    [SerializeField]
    AudioClip _attackClip;

    [SerializeField]
    AudioClip _deadClip;

    [SerializeField]
    AudioClip _hitClip;


    //[SerializeField]
    //int _rangeValue = 1;

    //[SerializeField, Cell]
    //CellGrid _attackCells;

    //[SerializeField, Cell]
    //CellGrid _movementCells;
    [Header("Attack Range")]
    [SerializeField]
    Vector2Int[] _attackCells = new Vector2Int[] { new Vector2Int(1, 0) };

    [Header("Movement Range")]
    [SerializeField]
    Vector2Int[] _movementCells = new Vector2Int[] { new Vector2Int(1, 0) };

    public Sprite icon => _icon;

    public SkeletonDataAsset skeletonDataAsset => _skeletonDataAsset;

    public int healthValue => _healthValue;

    public int damageValue => _damageValue;

    public int attackCount => _attackCount;

    //public int movementvalue => _movementValue;

    public int costValue => _costValue;

    public int priorityValue => _priorityValue;

    //public int rangeValue => _rangeValue;

       

    public Vector2Int[] attackCells => _attackCells;

    public Vector2Int[] movementCells => _movementCells;

    public TYPE_UNIT typeUnit => _typeUnit;

    public TYPE_UNIT_ATTACK typeUnitAttack => _typeUnitAttack;



    public AudioClip attackClip => _attackClip;

    public AudioClip deadClip => _deadClip;

    public AudioClip hitClip => _hitClip;

    public GameObject bullet => _bullet;

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
}
