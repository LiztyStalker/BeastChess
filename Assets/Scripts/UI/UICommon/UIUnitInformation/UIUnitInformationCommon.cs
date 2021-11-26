using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformationCommon : MonoBehaviour
{

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private UIUnitFormation _uiUnitFormation;

    [SerializeField]
    private UIUnitSkillLayout _uiUnitSkillLayout;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _influenceText;

    [SerializeField]
    private Text _tierText;

    [SerializeField]
    private Text _groupText;

    [SerializeField]
    private Text _classText;

    [SerializeField]
    private Image _classImage;

    [SerializeField]
    private Text _squadText;

    [SerializeField]
    private Text _positionText;


    public void Initialize()
    {
        _uiUnitFormation.Initialize();
        _uiUnitSkillLayout.Initialize();
    }

    public void CleanUp()
    {
        _uiUnitFormation.CleanUp();
        _uiUnitSkillLayout.CleanUp();
    }


    public void SetData(UnitCard uCard)
    {
        _uiUnitSkillLayout.Show(uCard);
        _uiUnitFormation.ShowFormation(uCard);
        _icon.sprite = uCard.Icon;
        _nameText.text = uCard.UnitName;
        _influenceText.text = "-";
        _tierText.text = uCard.Tier.ToString();
        _groupText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_GROUP), uCard.TypeUnitGroup.ToString(), "Name");
        _classText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_CLASS), uCard.TypeUnitClass.ToString(), "Name");
        _positionText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_UNIT_FORMATION), uCard.TypeUnit.ToString(), "Name");
        _squadText.text = string.Format("{0} / {1}", uCard.LiveSquadCount, uCard.SquadCount);
        _classImage.sprite = DataStorage.Instance.GetDataOrNull<Sprite>(uCard.TypeUnitClass.ToString(), "Icon_Class", null);
    }



    public void SetOnSkillLayoutEvent(System.Action<SkillData, Vector2> act) => _uiUnitSkillLayout.SetOnSkillInformationEvent(act);

}
