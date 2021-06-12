using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleButton : MonoBehaviour
{
    [SerializeField]
    private TYPE_BATTLE_TURN _typeBattleTurn;

    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(delegate { _clickedEvent?.Invoke(_typeBattleTurn); });
        ShowData();
    }

    public void SetTurn(TYPE_BATTLE_TURN typeBattleTurn)
    {
        _typeBattleTurn = typeBattleTurn;
        ShowData();
    }

    private void ShowData()
    {
        _button.GetComponentInChildren<Text>().text = _typeBattleTurn.ToString();
    }

    private System.Action<TYPE_BATTLE_TURN> _clickedEvent;
    public void SetOnClickedListener(System.Action<TYPE_BATTLE_TURN> act) => _clickedEvent = act;
   
}
