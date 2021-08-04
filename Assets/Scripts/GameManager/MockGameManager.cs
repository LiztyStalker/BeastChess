using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockGameCommander
{
    public CommanderActor commanderActor = CommanderActor.Create();
    public int costValue = 500;
    public int ironValue = 10;
    public void SetCommanderCard(CommanderCard commanderCard) => commanderActor.SetCommanderCard(commanderCard);
    public void AddCard(UnitCard uCard)
    {
        costValue -= uCard.employCostValue;
        commanderActor.AddCard(uCard);
    }
    public void RemoveCard(UnitCard uCard)
    {
        costValue += uCard.employCostValue;
        commanderActor.RemoveCard(uCard);
    }
}

public class MockGameOutpost
{
    private static MockGameOutpost _instance;
    public static MockGameOutpost instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MockGameOutpost();
            }
            return _instance;
        }
    }

    public MockGameCommander commander_L = new MockGameCommander();
    public MockGameCommander commander_R = new MockGameCommander();

    public BattleFieldData battleFieldData;

    public void SetCommanderCard(CommanderCard commanderCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.SetCommanderCard(commanderCard);
        else
            commander_R.SetCommanderCard(commanderCard);
    }

    public void AddCard(UnitCard uCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.AddCard(uCard);
        else
            commander_R.AddCard(uCard);
    }

    public void RemoveCard(UnitCard uCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.RemoveCard(uCard);
        else
            commander_R.RemoveCard(uCard);
    }

    public int GetCostValue(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? commander_L.costValue : commander_R.costValue;
    }

    public int GetIronValue(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? commander_L.ironValue : commander_R.ironValue;
    }

}

public class MockGameData
{

    private static MockGameData _instance;
    public static MockGameData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MockGameData();
            }
            return _instance;
        }
    }

    public List<UnitCard> totalUnits_L = new List<UnitCard>();
    public List<UnitCard> totalUnits_R = new List<UnitCard>();

    public void InitializeUnits()
    {
        totalUnits_L.AddRange(DataStorage.Instance.GetRandomUnitCards(100));
        totalUnits_R.AddRange(DataStorage.Instance.GetRandomUnitCards(100));
    }

}



public class MockGameManager : MonoBehaviour
{
    [SerializeField]
    Transform _dragPanel;

    [SerializeField]
    UIBattleField uiBattleField;

    [SerializeField]
    UIOutpost _lOutpost;

    [SerializeField]
    UIUnitOutpostBarrack uiBarrack;

    //[SerializeField]
    //UIOutpost _rOutpost;

    public Transform dragPanel => _dragPanel;

    void Start()
    {
        uiBattleField.Initialize();
        _lOutpost.Initialize();
        _lOutpost.SetOnUnitListener(() => 
            {
            if (uiBarrack.isActiveAndEnabled)
                uiBarrack.Hide();
            else
                uiBarrack.Show(TYPE_TEAM.Left);
            }
        );

        //_rOutpost.Initialize();

        MockGameData.instance.InitializeUnits();

        uiBarrack.Hide();
    }



    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_MockGame");
    }

}
