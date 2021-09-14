using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTester : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform startTr;

    [SerializeField]
    Transform arriveTr;

    [SerializeField]
    BulletData _directBulletData;

    [SerializeField]
    BulletData _curveBulletData;

    [SerializeField]
    BulletData _dropBulletData;

    private void OnGUI()
    {
        if (GUILayout.Button("Test DirectBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_directBulletData, startTr, arriveTr, null);
        }

        if (GUILayout.Button("Test CurveBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_curveBulletData, startTr, arriveTr, null);
        }

        if (GUILayout.Button("Test DropBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_dropBulletData, startTr, arriveTr, null);
        }

    }

}
