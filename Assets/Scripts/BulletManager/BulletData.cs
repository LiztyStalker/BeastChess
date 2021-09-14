using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    private enum TYPE_BULLET_ACTION { Direct, Curve, Drop}

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField] 
    private TYPE_BULLET_ACTION _typeBulletAction;

    [SerializeField] 
    private EffectData _arriveEffectData;

    [SerializeField] 
    private float _movementTime = 1f;

    public GameObject prefab => _bulletPrefab;
    

    public Vector2 SetPosition(Transform startTr, Transform arriveTr, ref float nowTime)
    {
        nowTime += Time.deltaTime / _movementTime;
        switch (_typeBulletAction)
        {
            case TYPE_BULLET_ACTION.Direct:
                return Vector2.Lerp(startTr.position, arriveTr.position, nowTime);
            case TYPE_BULLET_ACTION.Curve:
                var center = (startTr.position + arriveTr.position) * 8f;
                center -= Vector3.up;

                var startRelPos = startTr.position - center;
                var arriveRelPos = arriveTr.position - center;
                var nowPos = Vector3.Slerp(startRelPos, arriveRelPos, nowTime);
                nowPos += center;
                return nowPos;
            case TYPE_BULLET_ACTION.Drop:
                var pos = new Vector2(arriveTr.position.x, arriveTr.position.y + 10f);
                return Vector2.Lerp(pos, arriveTr.position, nowTime);
        }
        return Vector2.zero;
    }

    public void ActivateEffect(Vector3 position)
    {
        EffectManager.Current.ActivateEffect(_arriveEffectData, position);
    }
}
