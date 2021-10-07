using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitSelectorMenu : MonoBehaviour
{

    [SerializeField]
    UnitManager _unitManager;

    private UnitActor _uActor;
    
    public void Show(UnitActor uActor, Vector2 sp)
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().anchoredPosition = sp;
        _uActor = uActor;
    }

    public void ShowUnitInformation()
    {
        showInformationEvent?.Invoke(_uActor);
        Hide();
    }

    public void Cancel()
    {
        _unitManager.CancelChangeUnitActor();
        Hide();
    }

    public void ChangePosition()
    {
        _unitManager.DragUnitActor(_uActor);
        Hide();
    }

    public void ReturnUnit()
    {
        _unitManager.ReturnUnitActor(_uActor);
        onReturnUnitEvent?.Invoke(_uActor);
        Hide();
    }

    public void Hide()
    {
        _uActor = null;
        gameObject.SetActive(false);
    }

    public event System.Action<UnitActor> onReturnUnitEvent;

    public event System.Action<UnitActor> showInformationEvent;
}
