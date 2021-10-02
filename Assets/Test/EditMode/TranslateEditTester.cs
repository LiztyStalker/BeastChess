#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class TranslateEditTester
{

    [Test]
    public static void Translate_UnitData_Conscript_Name()
    {
        var translate = TranslatorStorage.Instance.GetTranslator<UnitData>("Conscript", "Name");
        Debug.Log(translate);
        Assert.That(translate, Is.EqualTo("Â¡Áýº´"));
    }

    [Test]
    public static void Translate_UnitData_Conscript_Description()
    {
        var translate = TranslatorStorage.Instance.GetTranslator<UnitData>("Conscript", "Description");
        Debug.Log(translate);
        Assert.That(translate, Is.EqualTo("Â¡Áýº´"));

    }

}
#endif