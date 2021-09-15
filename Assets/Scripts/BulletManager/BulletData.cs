using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    public enum TYPE_BULLET_ACTION { Direct, Curve, Drop}

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField] 
    private TYPE_BULLET_ACTION _typeBulletAction;

    [SerializeField] 
    private EffectData _arriveEffectData;

    [SerializeField] 
    private float _movementSpeed = 1f;

    [SerializeField]
    private bool _isRotate = false;


    #region ##### Getter Setter #####
    public GameObject prefab => _bulletPrefab;
    public EffectData ArriveEffectData => _arriveEffectData;
    public bool IsRotate => _isRotate;
    public float MovementSpeed => _movementSpeed;
    public TYPE_BULLET_ACTION TypeBulletAction => _typeBulletAction;

    #endregion
   
}
