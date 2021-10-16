using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommanderOutpost : MonoBehaviour
{

    [SerializeField]
    private TYPE_TEAM _typeTeam;

    CommanderData[] _commanders;

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

        _commanders = DataStorage.Instance.GetAllDataArrayOrZero<CommanderData>();

        ShowCommander();
        RefreshCost();

        MockGameOutpost.instance.AddOnRefreshCommanderData(ShowCommander);

        _uiSkill.Initialize();
    }

    public void CleanUp()
    {
        MockGameOutpost.instance.RemoveOnRefreshCommanderData(ShowCommander);
        _lBtn.onClick.RemoveListener(OnLeftClicked);
        _rBtn.onClick.RemoveListener(OnRightClicked);
    }


    private void ShowCommander()
    {
        var commanderData = _commanders[_index];

        _icon.sprite = commanderData.icon;
        _nameText.text = commanderData.CommanderName;
        _influenceText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_INFLUENCE), commanderData.typeInfluence.ToString(), "Name");
        _tribeText.text = commanderData.tribeData.name;
        _masterText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_COMMANDER_MASTER), commanderData.typeCommanderMaster.ToString(), "Name"); 

        MockGameOutpost.instance.SetCommanderCard(CommanderCard.Create(commanderData), _typeTeam);

        _leadershipText.text = MockGameOutpost.instance.GetLeadershipText(_typeTeam);

        _uiSkill.SetSkill(commanderData.skills);
    }

    public void RefreshCost()
    {
        _costText.text = MockGameOutpost.instance.GetCostValue(_typeTeam).ToString();
        _ironText.text = MockGameOutpost.instance.GetIronValue(_typeTeam).ToString();
    }

    private void OnLeftClicked()
    {
        if (_index - 1 < 0)
            _index = _commanders.Length - 1;
        else
            _index--;

        ShowCommander();

    }

    private void OnRightClicked()
    {
        if (_index + 1 >= _commanders.Length)
            _index = 0;
        else
            _index++;

        ShowCommander();
    }



    #region ##### Listener #####

    public void SetOnSkillInformationListener(System.Action<SkillData, Vector2> act) => _uiSkill.SetOnSkillInformationListener(act);

    #endregion
}
