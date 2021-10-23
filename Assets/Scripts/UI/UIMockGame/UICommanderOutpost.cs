using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UICommanderOutpost : MonoBehaviour
{

    [SerializeField]
    private TYPE_TEAM _typeTeam;

    private CommanderData[] _commanders;

    [SerializeField]
    private Button _lBtn;

    [SerializeField]
    private Button _rBtn;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _tribeText;

    [SerializeField]
    private Text _influenceText;

    [SerializeField]
    private Text _masterText;

    [SerializeField]
    private Text _costText;

    [SerializeField]
    private Text _ironText;

    [SerializeField]
    private Text _leadershipText;

    [SerializeField]
    private UICommanderSkill _uiSkill;

    private int _index = 0;

    public void Initialize()
    {
        _lBtn.onClick.AddListener(OnLeftClicked);
        _rBtn.onClick.AddListener(OnRightClicked);

        _lBtn.gameObject.SetActive(true);
        _rBtn.gameObject.SetActive(true);

        _commanders = DataStorage.Instance.GetAllDataArrayOrZero<CommanderData>();

        _commanders = _commanders.Where(data => data.IsAppearBarracks).ToArray();

        _uiSkill.Initialize();
    }

    public void CleanUp()
    {
        _lBtn.onClick.RemoveListener(OnLeftClicked);
        _rBtn.onClick.RemoveListener(OnRightClicked);
    }


    public void RefreshCommanderCard(RegionMockGameActor region)
    {
        var commanderCard = region.commanderActor.GetCommanderCard();

        _icon.sprite = commanderCard.Icon;
        _nameText.text = commanderCard.CommanderName;
        _influenceText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_INFLUENCE), commanderCard.TypeInfluence.ToString(), "Name");
        _tribeText.text = commanderCard.TribeName;
        _masterText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_COMMANDER_MASTER), commanderCard.TypeCommanderMaster.ToString(), "Name");
        _leadershipText.text = $"{region.nowLeadershipValue}/{region.maxLeadershipValue}";

        _costText.text = region.costValue.ToString();
        //_ironText.text = region.costValue.ToString();


        _uiSkill.SetSkill(commanderCard.skills);
    }

    public void SetChallenge(bool isChallenge)
    {
        _lBtn.gameObject.SetActive(!isChallenge);
        _rBtn.gameObject.SetActive(!isChallenge);
    }

    private void OnLeftClicked()
    {
        if (_index - 1 < 0)
            _index = _commanders.Length - 1;
        else
            _index--;

        SetCommanderCardEvent();
        RefreshEvent();

    }

    private void OnRightClicked()
    {
        if (_index + 1 >= _commanders.Length)
            _index = 0;
        else
            _index++;

        SetCommanderCardEvent();
        RefreshEvent();
    }

    private void SetCommanderCardEvent()
    {
        var commanderData = _commanders[_index];
        _commanderDataEvent?.Invoke(CommanderCard.Create(commanderData), _typeTeam);
    }

    private void RefreshEvent()
    {
        _refreshEvent?.Invoke(_typeTeam);
    }

    #region ##### Listener #####

    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _uiSkill.SetOnSkillInformationListener(act);


    public System.Action<CommanderCard, TYPE_TEAM> _commanderDataEvent;
    public void SetOnCommanderDataListener(System.Action<CommanderCard, TYPE_TEAM> act) => _commanderDataEvent = act;


    public System.Action<TYPE_TEAM> _refreshEvent;
    public void AddOnRefreshListener(System.Action<TYPE_TEAM> act) => _refreshEvent += act;
    public void RemoveOnRefreshListener(System.Action<TYPE_TEAM> act) => _refreshEvent -= act;

    #endregion
}
