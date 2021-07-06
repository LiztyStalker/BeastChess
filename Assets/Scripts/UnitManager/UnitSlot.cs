using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlot
{
    UnitCard _unitCard;

    public void SetUnitCard(UnitCard unitCard) => _unitCard = unitCard;
    public UnitCard GetUnitCard() => _unitCard;
}
