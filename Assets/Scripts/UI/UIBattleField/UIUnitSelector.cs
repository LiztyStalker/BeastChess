using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitSelector : MonoBehaviour
{
    private UIUnitSelectorMenu _uiSelectorMenu;

    public void Initialize()
    {
        _uiSelectorMenu = GetComponentInChildren<UIUnitSelectorMenu>(true);
        _uiSelectorMenu.Initialize();
        _uiSelectorMenu.Hide();
    }

    public void CleanUp()
    {
        _uiSelectorMenu.CleanUp();
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    //입력 장치 통합 필요
    //private void Update()
    //{

    //    if (Input.GetMouseButtonDown(0) && !_uiSelectorMenu.isActiveAndEnabled)
    //    {
    //        var wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        var hits = Physics2D.RaycastAll(wp, Vector2.zero);
    //        for (int i = 0; i < hits.Length; i++)
    //        {
    //            if(hits[i].collider.tag == "Unit")
    //            {
    //                _uiSelectorMenu.Show(hits[i].collider.GetComponent<UnitActor>(), Input.mousePosition);
    //                break;
    //            }
    //        }
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        _uiSelectorMenu.Cancel();
    //    }
    //}

    public void ShowSelectorMenu(TYPE_TEAM typeTeam, Vector2 screenPosition)
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        var hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            var uActor = hits[i].collider.GetComponent<UnitActor>();
            if(uActor != null) {
                if (uActor.typeTeam == typeTeam)
                {
                    _uiSelectorMenu.Show(uActor, screenPosition);
                    break;
                }
            }
        }
    }

    public void CloseMenu()
    {
        _uiSelectorMenu.Hide();
    }



    #region ##### Listener #####
    public void SetOnInformationListener(System.Action<UnitActor, Vector2> act) => _uiSelectorMenu.SetOnInformationListener(act);
    public void SetOnCancelListener(System.Action act) => _uiSelectorMenu.SetOnCancelListener(act);
    public void SetOnDragListener(System.Action<UnitActor> act) => _uiSelectorMenu.SetOnDragListener(act);
    public void SetOnReturnListener(System.Action<UnitActor> act) => _uiSelectorMenu.SetOnReturnListener(act);

    #endregion
}
