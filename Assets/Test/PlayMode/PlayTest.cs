#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class PlayTest
{
    protected Camera camera;
    protected BattleFieldManager battleFieldManager;
    protected FieldGenerator fieldGenerator;
    protected UnitManager unitManager;

    [UnitySetUp]
    public virtual IEnumerator UnitySetUp()
    {
        Debug.Log("UnitySetUp");

        var cameraObject = new GameObject();
        cameraObject.AddComponent<AudioListener>();
        camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 7f;
        camera.transform.position = Vector3.back * 10f;


        //¸ÊÇÊµå Á¦ÀÛ
        var gameObject = new GameObject();
        fieldGenerator = gameObject.AddComponent<FieldGenerator>();
        unitManager = gameObject.AddComponent<UnitManager>();
        battleFieldManager = gameObject.AddComponent<BattleFieldManager>();

        yield return null;
        
        battleFieldManager.ClearAllUnits();

        Assert.NotNull(battleFieldManager);
    }

    [UnityTearDown]
    public virtual IEnumerator UnityTearDown()
    {
        Debug.Log("TearDown");
        //¸ÊÇÊµå ¹× À¯´Ö Á¦°Å
        battleFieldManager.ClearAllUnits(true);

        UnitManager.CleanUp();
        FieldManager.CleanUp();

        yield return null;
        Object.DestroyImmediate(battleFieldManager.gameObject);
        Object.DestroyImmediate(camera.gameObject);
        yield return null;
        BattleFieldOutpost.Dispose();
    }

    [UnityTest]
    public IEnumerator BattieField_Created()
    {
        yield return null;
        Assert.NotNull(battleFieldManager);
    }



}
#endif