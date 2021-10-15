using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleFieldRoundLayout : MonoBehaviour
{
    [SerializeField]
    private RectTransform _arrowTr;

    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound)
    {
        _arrowTr.eulerAngles = Vector3.forward * ((float)typeBattleRound * 90f);
    }
}
