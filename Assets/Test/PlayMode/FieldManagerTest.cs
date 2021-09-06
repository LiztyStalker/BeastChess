using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FieldManagerTest
{
    [Test]
    public void FieldManagerTestSimplePasses()
    {
        var cells = FieldManager.GetCells(TYPE_TARGET_RANGE.Normal, 0, 5);
        foreach(var cell in cells)
        {
            Debug.Log(cell);
        }
        Assert.Pass();
    }
}
