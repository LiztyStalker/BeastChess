using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UICommonPlayTest
{
    [UnityTest]
    public IEnumerator UICommon_Initialize()
    {
        UICommon.Current.IsCanvasActivated();
        Debug.Log(UICommon.Current);
        Assert.IsNotNull(UICommon.Current);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UICommon_UIUnitInformation()
    {
        var data = DataStorage.Instance.GetDataOrNull<UnitData>("Conscript");
        var ui = UICommon.Current.GetUICommon<UIUnitInformation>();
        ui.Show(UnitCard.Create(data), Vector2.zero);
        Debug.Log(UICommon.Current.IsCanvasActivated<UIUnitInformation>());
        Assert.IsTrue(UICommon.Current.IsCanvasActivated<UIUnitInformation>());
        yield return null;
    }

    [UnityTest]
    public IEnumerator UICommon_UISkillInformation()
    {
        var data = DataStorage.Instance.GetDataOrNull<SkillData>("Rooted");
        var ui = UICommon.Current.GetUICommon<UISkillInformation>();
        ui.Show(data, Vector2.zero);
        Debug.Log(UICommon.Current.IsCanvasActivated<UISkillInformation>());
        Assert.IsTrue(UICommon.Current.IsCanvasActivated<UISkillInformation>());
        yield return null;
    }

    [UnityTest]
    public IEnumerator UICommon_UIPopup()
    {
        var ui = UICommon.Current.GetUICommon<UIPopup>();
        ui.ShowApplyPopup("Test");
        Debug.Log(UICommon.Current.IsCanvasActivated<UIPopup>());
        Assert.IsTrue(UICommon.Current.IsCanvasActivated<UIPopup>());
        yield return null;
    }
}
