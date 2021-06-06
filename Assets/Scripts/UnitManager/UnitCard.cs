using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitCard
{
    public Sprite icon => _unitData.icon;
    public SkeletonDataAsset skeletonDataAsset => _unitData.skeletonDataAsset;

    public int healthValue => _unitData.healthValue;

    public int costValue => _unitData.costValue;
    public string name => _unitData.name;

    public int damageValue => _unitData.damageValue;

    public int attackCount => _unitData.attackCount;

    public int minRangeValue => _unitData.minRangeValue;

    public int priorityValue => _unitData.priorityValue;

    public TYPE_UNIT typeUnit => _unitData.typeUnit;

    public TYPE_UNIT_ATTACK typeUnitAttack => _unitData.typeUnitAttack;

    public Vector2Int[] attackCells => _unitData.attackCells;
    public Vector2Int[] movementCells => _unitData.movementCells;

    public AudioClip deadClip => _unitData.deadClip;
    public AudioClip attackClip => _unitData.attackClip;

    public Sprite bullet => _unitData.bullet;

    public Vector2Int[] formationCells { get; private set; }

    private UnitData _unitData;

    public UnitCard(UnitData unitData)
    {
        _unitData = unitData;
        formationCells = new Vector2Int[] { new Vector2Int(0, 0) };
    }

    public void SetFormation(Vector2Int[] formations)
    {
        formationCells = formations;
    }
   
}
