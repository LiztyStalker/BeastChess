using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIHelpBtn : MonoBehaviour
{

    private Button _button;

    private Text _text;

    private int _index;


    public void Initialize()
    {
        _button = GetComponent<Button>();
        _text = _button.GetComponentInChildren<Text>(true);
        _button.onClick.AddListener(OnClickedEvent);
    }

    public void CleanUp()
    {
        _button.onClick.RemoveListener(OnClickedEvent);
    }

    public void SetData(string text)
    {
        _text.text = text;
    }

    public void SetData(int index)
    {
        _index = index;
    }

    #region ##### Listener #####

    private void OnClickedEvent()
    {
        _clickedEvent?.Invoke(_index);
    }

    private System.Action<int> _clickedEvent;
    public void SetOnClickedListener(System.Action<int> act) => _clickedEvent = act;

    #endregion


}
