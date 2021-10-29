
public class RegionMockGameActor
{
    public CommanderActor commanderActor = CommanderActor.Create();

    private int _costValue = 2500;
    private int _increaseCostValue = 200;
    
    public int maxCostValue => _costValue + _increaseCostValue * commanderActor.nowLevel;
    public int nowCostValue => maxCostValue - commanderActor.GetNowCostValue();


    public int ironValue = 10;
    public void SetCommanderCard(CommanderCard commanderCard) => commanderActor.SetCommanderCard(commanderCard);
    public void AddCard(UnitCard uCard)
    {
        //nowCostValue -= uCard.employCostValue;
        commanderActor.AddCard(uCard);
    }
    public void AddCards(UnitCard[] uCards)
    {
        for (int i = 0; i < uCards.Length; i++)
        {
            commanderActor.AddCard(uCards[i]);
        }
    }
    public void RemoveCard(UnitCard uCard)
    {
        //nowCostValue += uCard.employCostValue;
        commanderActor.RemoveCard(uCard);
    }

    public void ClearCards()
    {
        commanderActor.ClearCards();
    }
    public void AllRecovery()
    {
        commanderActor.AllRecovery();
    }
    public bool IsEnoughLeadership(UnitCard uCard)
    {
        return commanderActor.IsEnoughLeadership(uCard);
    }

    public bool IsEnoughEmployCost(UnitCard uCard)
    {
        return (nowCostValue - uCard.employCostValue >= 0);
    }

    public UnitCard[] GetUnitCards() => commanderActor.unitDataArray;

    public bool IsEmptyUnitDataArray()
    {
        return commanderActor.IsEmptyUnitDataArray();
    }

    public int nowLeadershipValue => commanderActor.nowLeadershipValue;
    public int maxLeadershipValue => commanderActor.maxLeadershipValue;
}
