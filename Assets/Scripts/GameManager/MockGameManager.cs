using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MockGameOutpost
{

    #region ##### MockGameCommander #####
    public class MockGameCommander
    {
        public CommanderActor commanderActor = CommanderActor.Create();
        public int costValue = 1000;
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
        public bool IsEnoughLeadership(UnitCard uCard)
        {
            return commanderActor.IsEnoughLeadership(uCard);
        }

        public bool IsEnoughEmployCost(UnitCard uCard)
        {
            return (costValue - uCard.employCostValue >= 0);
        }

        public UnitCard[] GetUnitCards() => commanderActor.unitDataArray;

        public bool IsEmptyUnitDataArray()
        {
            return commanderActor.IsEmptyUnitDataArray();
        }

        public int nowLeadershipValue => commanderActor.nowLeadershipValue;
        public int maxLeadershipValue => commanderActor.maxLeadershipValue;
    }
    #endregion

    public static MockGameOutpost Current = null;

    public MockGameCommander commander_L = new MockGameCommander();
    public MockGameCommander commander_R = new MockGameCommander();
    private bool _isChallenge = false;
    private int _challengeLevel = 0;
    public BattleFieldData battleFieldData;


    public bool IsChallenge() => _isChallenge;
    public void SetChallenge(bool isChallenge) => _isChallenge = isChallenge;
    public int GetChallengeLevel() => _challengeLevel;
    public void AddChallengeLevel() => _challengeLevel++;
    public void ClearChallengeLevel() => _challengeLevel = 0;


    public static void InitializeMockGameOutpost()
    {
        if (Current == null)
            Current = new MockGameOutpost();
    }

    public static void Dispose()
    {
        Current = null;
    }

    public void SetCommanderCard(CommanderCard commanderCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.SetCommanderCard(commanderCard);
        else
            commander_R.SetCommanderCard(commanderCard);
    }
    public bool IsEnoughLeadership(UnitCard uCard, TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return commander_L.IsEnoughLeadership(uCard);
        else
            return commander_R.IsEnoughLeadership(uCard);
    }

    public bool IsEnoughEmployCost(UnitCard uCard, TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return commander_L.IsEnoughEmployCost(uCard);
        else
            return commander_R.IsEnoughEmployCost(uCard);
    }


    public void AddCard(UnitCard uCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.AddCard(uCard);
        else
            commander_R.AddCard(uCard);
        _refreshEvent?.Invoke();

    }

    public void RemoveCard(UnitCard uCard, TYPE_TEAM typeTeam)
    {

        if (typeTeam == TYPE_TEAM.Left)
            commander_L.RemoveCard(uCard);
        else
            commander_R.RemoveCard(uCard);
        _refreshEvent?.Invoke();
    }

    public int GetCostValue(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? commander_L.costValue : commander_R.costValue;
    }

    public int GetIronValue(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? commander_L.ironValue : commander_R.ironValue;
    }

    public string GetLeadershipText(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? $"{commander_L.nowLeadershipValue}/{commander_L.maxLeadershipValue}" : $"{commander_R.nowLeadershipValue}/{commander_R.maxLeadershipValue}";
    }

    public bool IsEmptyUnitDataArray(TYPE_TEAM typeTeam)
    {
        return (typeTeam == TYPE_TEAM.Left) ? commander_L.IsEmptyUnitDataArray() : commander_R.IsEmptyUnitDataArray();
    }

    #region ##### Listener #####

    private System.Action _refreshEvent;
    public void AddOnRefreshCommanderData(System.Action act) => _refreshEvent += act;
    public void RemoveOnRefreshCommanderData(System.Action act) => _refreshEvent -= act;
    #endregion


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
        totalUnits_L.Clear();
        totalUnits_R.Clear();

        var dataArrayL = DataStorage.Instance.GetRandomDatasOrZero<UnitData>(100);
        var dataArrayR = DataStorage.Instance.GetRandomDatasOrZero<UnitData>(100);

        dataArrayL = dataArrayL.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3 && data.TypeUnitClass != TYPE_UNIT_CLASS.Wizard).ToArray();
        dataArrayR = dataArrayR.Where(data => data.SkeletonDataAsset != null && data.Icon != null && data.Tier == 3 && data.TypeUnitClass != TYPE_UNIT_CLASS.Wizard).ToArray();


        var uCardsL = UnitCard.Create(dataArrayL);
        var uCardsR = UnitCard.Create(dataArrayR);

        totalUnits_L.AddRange(uCardsL);
        totalUnits_R.AddRange(uCardsR);
    }
}



public class MockGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _dragPanel;

    [SerializeField]
    private UIMockBattleField _uiBattleField;

    [SerializeField]
    private UIOutpost _lOutpost;

    [SerializeField]
    private UIOutpost _rOutpost;

    [SerializeField]
    private UIUnitOutpostBarrack _uiBarrack;

    [SerializeField]
    private UnityEngine.UI.Button _startGameBtn;

    [SerializeField]
    private UnityEngine.UI.Button _backBtn;

    public Transform dragPanel => _dragPanel;

    private void Start()
    {
        MockGameOutpost.InitializeMockGameOutpost();
        MockGameData.instance.InitializeUnits();

        _uiBattleField.Initialize();

        _uiBarrack.Initialize();
        _uiBarrack.SetOnUnitInformationListener(ShowUnitInformation);


        _lOutpost.Initialize();
        _lOutpost.SetOnUnitListener(() => 
            {
                if (_uiBarrack.isActiveAndEnabled)
                    _uiBarrack.Hide();
                else
                    _uiBarrack.Show(TYPE_TEAM.Left, MockGameData.instance.totalUnits_L);
            }
        );

        _lOutpost.SetOnUnitInformationListener(ShowUnitInformation);
        _lOutpost.SetOnSkillInformationListener(ShowSkillInformation);



        _rOutpost.Initialize();
        _rOutpost.SetChallenge(MockGameOutpost.Current.IsChallenge(), MockGameOutpost.Current.GetChallengeLevel());
        _rOutpost.SetOnUnitListener(() =>
            {
                if (_uiBarrack.isActiveAndEnabled)
                    _uiBarrack.Hide();
                else
                    _uiBarrack.Show(TYPE_TEAM.Right, MockGameData.instance.totalUnits_R);
            }
        );

        _rOutpost.SetOnUnitInformationListener(ShowUnitInformation);
        _rOutpost.SetOnSkillInformationListener(ShowSkillInformation);

        _startGameBtn.onClick.AddListener(StartGame);

        _backBtn.onClick.AddListener(OnBackClicked);
    }

    private void OnDestroy()
    {
        _uiBarrack.CleanUp();
    }

    private void ShowUnitInformation(UnitCard uCard)
    {
        var uiUnitInfor = UICommon.Current.GetUICommon<UIUnitInformation>();
        uiUnitInfor.Show(uCard, Input.mousePosition);
    }

    private void ShowSkillInformation(SkillData skillData, Vector2 screenPosition)
    {
        var ui = UICommon.Current.GetUICommon<UISkillInformation>();
        ui.Show(skillData, screenPosition);
    }


    public void StartGame()
    {
        if (MockGameOutpost.Current.IsEmptyUnitDataArray(TYPE_TEAM.Left))
        {
            Debug.Log("CommanderL Empty");
            return;
        }
        if (MockGameOutpost.Current.IsEmptyUnitDataArray(TYPE_TEAM.Right))
        {
            Debug.Log("CommanderR Empty");
            return;
        }

        LoadManager.SetNextSceneName("Test_BattleField");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

    private void OnBackClicked()
    {
        LoadManager.SetNextSceneName("Test_MainTitle");
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadManager.LoadSceneName);
    }

}
