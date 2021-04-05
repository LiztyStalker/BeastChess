using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public enum TYPE_UNIT { Castle = -1, Ground, Air, }

public class UnitData : ScriptableObject
{
    [SerializeField]
    TYPE_UNIT _typeUnit;

    [SerializeField]
    Sprite _icon;

    [SerializeField]
    SkeletonDataAsset _skeletonDataAsset;

    [Range(1, 1000)]
    [SerializeField]
    int _healthValue = 100;

    [SerializeField]
    int _damageValue = 35;

    [SerializeField]
    int _movementValue = 1;

    [SerializeField]
    int _costValue = 1;

    [SerializeField]
    int _rangeValue = 1;




    public Sprite icon => _icon;

    public SkeletonDataAsset skeletonDataAsset => _skeletonDataAsset;

    public int healthValue => _healthValue;

    public int damageValue => _damageValue;

    public int movementvalue => _movementValue;

    public int costValue => _costValue;

    public int rangeValue => _rangeValue;

    public TYPE_UNIT typeUnit => _typeUnit;


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
