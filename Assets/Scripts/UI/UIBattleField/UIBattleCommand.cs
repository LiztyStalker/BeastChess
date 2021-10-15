using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleCommand : MonoBehaviour
{
    [SerializeField]
    private UIBattleButton[] uiBattleButtons;

    [SerializeField]
    private UIBattleButton[] uIBattleShowButtons;

    private List<TYPE_BATTLE_TURN> typeBattleTurnList = new List<TYPE_BATTLE_TURN>();

    public TYPE_BATTLE_TURN[] GetTypeBattleTurnArray() => typeBattleTurnList.ToArray();
    public void Initialize()
    {
        for (int i = 0; i < uiBattleButtons.Length; i++)
        {
            uiBattleButtons[i].SetOnClickedListener(OnBattleTurnAddClickedEvent);
        }

        for (int i = 0; i < uIBattleShowButtons.Length; i++)
        {
            uIBattleShowButtons[i].SetOnClickedListener(OnBattleTurnRemoveClickedEvent);
        }
    }

    public void CleanUp()
    {

    }

    public void Show()
    {
        typeBattleTurnList.Clear();
        ShowBattleTurn();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBattleTurnAddClickedEvent(TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (typeBattleTurnList.Count < uIBattleShowButtons.Length)
        {
            typeBattleTurnList.Add(typeBattleTurn);
            ShowBattleTurn();
        }
    }

    private void OnBattleTurnRemoveClickedEvent(TYPE_BATTLE_TURN typeBattleTurn)
    {
        if (0 < typeBattleTurnList.Count)
        {
            typeBattleTurnList.Remove(typeBattleTurn);
            ShowBattleTurn();
        }
    }

    private void ShowBattleTurn()
    {
        for (int i = 0; i < uIBattleShowButtons.Length; i++)
        {
            if (i < typeBattleTurnList.Count)
                uIBattleShowButtons[i].SetTurn(typeBattleTurnList[i]);
            else
                uIBattleShowButtons[i].SetTurn(TYPE_BATTLE_TURN.None);
        }
    }
}
