using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleStatusLayout : MonoBehaviour
{
    [SerializeField]
    private UIBattleFieldRoundLayout _uiBattleFieldRoundLayout;

    [SerializeField]
    private Text _leftHealthText;
    [SerializeField]
    private Text _rightHealthText;

    [SerializeField]
    private Slider _leftHealthSlider;
    [SerializeField]
    private Slider _rightHealthSlider;

    [SerializeField]
    private Image _emblemLIcon;
    [SerializeField]
    private Image _emblemRIcon;

    public void SetEmblems(TYPE_TEAM typeTeam, Sprite emblemSprite)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {
            _emblemLIcon.sprite = emblemSprite;
        }
        else
        {
            _emblemRIcon.sprite = emblemSprite;
        }
    }

    public void SetCastleHealth(TYPE_TEAM typeTeam, int value, float rate)
    {
        if (typeTeam == TYPE_TEAM.Left)
        {
            _leftHealthText.text = value.ToString();
            _leftHealthSlider.value = rate;
        }
        else
        {
            _rightHealthText.text = value.ToString();
            _rightHealthSlider.value = rate;
        }
    }

    public void SetBattleRound(TYPE_BATTLE_ROUND typeBattleRound)
    {
        _uiBattleFieldRoundLayout.SetBattleRound(typeBattleRound);
        //_battleRoundText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_BATTLE_ROUND), typeBattleRound.ToString(), "Name");
    }
}
