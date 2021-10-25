using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSelectorMenu : MonoBehaviour
{
    [SerializeField]
    private Button _informationBtn;
    [SerializeField]
    private Button _modifiedBtn;
    [SerializeField]
    private Button _returnBtn;

    private UnitActor _uActor;
    
    public void Initialize()
    {
        _informationBtn.onClick.AddListener(ShowUnitInformationEvent);
        _modifiedBtn.onClick.AddListener(ChangePositionEvent);
        _returnBtn.onClick.AddListener(ReturnUnitEvent);
    }

    public void CleanUp()
    {
        _informationBtn.onClick.RemoveListener(ShowUnitInformationEvent);
        _modifiedBtn.onClick.RemoveListener(ChangePositionEvent);
        _returnBtn.onClick.RemoveListener(ReturnUnitEvent);
    }

    public void Show(UnitActor uActor, Vector2 screenPosition)
    {
        gameObject.SetActive(true);

        AudioManager.ActivateAudio("BTN_OK", AudioManager.TYPE_AUDIO.SFX, false);

        transform.position = screenPosition;
        _uActor = uActor;

        _modifiedBtn.gameObject.SetActive(uActor.typeTeam == TYPE_TEAM.Left);
        _returnBtn.gameObject.SetActive(uActor.typeTeam == TYPE_TEAM.Left);
    }

    public void Hide()
    {
        _uActor = null;
        gameObject.SetActive(false);
    }



    private void ShowUnitInformationEvent()
    {
        _showInformationEvent?.Invoke(_uActor, _informationBtn.transform.position);
        Hide();
    }

    private void ChangePositionEvent()
    {
        _dragEvent.Invoke(_uActor);
        Hide();
    }

    private void ReturnUnitEvent()
    {
        _returnEvent?.Invoke(_uActor);
        Hide();
    }

    public void Cancel()
    {
        _cancelEvent?.Invoke();
        Hide();
    }

   
    #region ##### Listener #####

    public System.Action<UnitActor, Vector2> _showInformationEvent;

    public System.Action _cancelEvent;
    public System.Action<UnitActor> _dragEvent;
    public System.Action<UnitActor> _returnEvent;

    public void SetOnInformationListener(System.Action<UnitActor, Vector2> act) => _showInformationEvent = act;
    public void SetOnCancelListener(System.Action act) => _cancelEvent = act;
    public void SetOnDragListener(System.Action<UnitActor> act) => _dragEvent = act;
    public void SetOnReturnListener(System.Action<UnitActor> act) => _returnEvent = act;

    #endregion
}
