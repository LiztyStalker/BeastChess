using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    public enum TYPE_BULLET_ACTION { Move, Curve, Drop, Direct }

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private string _bulletPrefabKey;

    [SerializeField]
    private TYPE_BULLET_ACTION _typeBulletAction;

    [SerializeField]
    private EffectData _arriveEffectData;

    [SerializeField]
    private string _arriveEffectDataKey;

    [SerializeField]
    private float _movementSpeed = 1f;

    [SerializeField]
    private bool _isRotate = false;


    #region ##### Getter Setter #####
    public GameObject prefab {
        get
        {
            if (_bulletPrefab == null)
                _bulletPrefab = DataStorage.Instance.GetDataOrNull<GameObject>(_bulletPrefabKey, "Bullet", null);
            return _bulletPrefab;
        }
    }
    public EffectData ArriveEffectData
    {
        get
        {
            if (_arriveEffectData == null)
                _arriveEffectData = DataStorage.Instance.GetDataOrNull<EffectData>(_arriveEffectDataKey);
            return _arriveEffectData;
        }
    }
    public bool IsRotate => _isRotate;
    public float MovementSpeed => _movementSpeed;
    public TYPE_BULLET_ACTION TypeBulletAction => _typeBulletAction;

    #endregion

}
