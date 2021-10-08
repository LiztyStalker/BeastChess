#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class PlayTest
{
    protected Camera camera;
    protected BattleFieldManager battieFieldManager;
    protected FieldGenerator fieldGenerator;
    protected UnitManager unitManager;

    [UnitySetUp]
    public virtual IEnumerator UnitySetUp()
    {
        var cameraObject = new GameObject();
        camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 7f;
        camera.transform.position = Vector3.back * 10f;


        //¸ÊÇÊµå Á¦ÀÛ
        var gameObject = new GameObject();
        fieldGenerator = gameObject.AddComponent<FieldGenerator>();
        unitManager = gameObject.AddComponent<UnitManager>();
        battieFieldManager = gameObject.AddComponent<BattleFieldManager>();

        yield return null;
        
        battieFieldManager.ClearAllUnits();

        Assert.NotNull(battieFieldManager);
    }

    [UnityTearDown]
    public virtual IEnumerator UnityTearDown()
    {
        //¸ÊÇÊµå ¹× À¯´Ö Á¦°Å
        UnitManager.CleanUp();
        FieldManager.CleanUp();
        yield return null;
        Object.DestroyImmediate(battieFieldManager.gameObject);
        Object.DestroyImmediate(camera.gameObject);
    }

    [UnityTest]
    public IEnumerator BattieField_Created()
    {
        yield return null;
        Assert.NotNull(battieFieldManager);
    }



}
#endif