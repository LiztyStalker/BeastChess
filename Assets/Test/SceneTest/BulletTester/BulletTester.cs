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

    [SerializeField]
    BulletData _directBulletData_R;

    [SerializeField]
    BulletData _curveBulletData_R;

    [SerializeField]
    BulletData _dropBulletData_R;
    private void OnGUI()
    {
        if (GUILayout.Button("Test DirectBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_directBulletData, startTr.position, arriveTr.position, null);
        }

        if (GUILayout.Button("Test CurveBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_curveBulletData, startTr.position, arriveTr.position, null);
        }

        if (GUILayout.Button("Test DropBullet Activate"))
        {
            BulletManager.Current.ActivateBullet(_dropBulletData, startTr.position, arriveTr.position, null);
        }

        if (GUILayout.Button("Test DirectBullet_Rotate Activate"))
        {
            BulletManager.Current.ActivateBullet(_directBulletData_R, startTr.position, arriveTr.position, null);
        }

        if (GUILayout.Button("Test CurveBullet_Rotate Activate"))
        {
            BulletManager.Current.ActivateBullet(_curveBulletData_R, startTr.position, arriveTr.position, null);
        }

        if (GUILayout.Button("Test DropBullet_Rotate Activate"))
        {
            BulletManager.Current.ActivateBullet(_dropBulletData_R, startTr.position, arriveTr.position, null);
        }



    }

}
