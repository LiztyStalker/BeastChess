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

    public void ShowSelectorMenu(TYPE_TEAM typeTeam, Vector2 screenPosition)
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        var hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);
        for (int i = 0; i < hits.Length; i++)
        {
            var uActor = hits[i].collider.GetComponent<UnitActor>();
            if(uActor != null && uActor.typeUnit != TYPE_UNIT_FORMATION.Castle) {
                _uiSelectorMenu.Show(uActor, screenPosition);
                break;
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
