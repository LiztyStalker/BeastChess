using UnityEngine;
using UnityEngine.UI;

public class UIBattleTurnPanel : MonoBehaviour
{
    [SerializeField]
    private Animator _leftAnimator;

    [SerializeField]
    private Text _leftOrderText;

    [SerializeField]
    private Animator _rightAnimator;

    [SerializeField]
    private Text _rightOrderText;

    public void SetAnimator(bool isActive)
    {
        _leftAnimator.SetBool("isAppear", isActive);
        _rightAnimator.SetBool("isAppear", isActive);
    }

    public void SetBattleTurnOrderText(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN typeBattleTurn)
    {
        if(typeTeam == TYPE_TEAM.Left)
        {
            _leftOrderText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_BATTLE_TURN), typeBattleTurn.ToString(), "Name");
        }
        else
        {
            _rightOrderText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_BATTLE_TURN), typeBattleTurn.ToString(), "Name");
        }
    }

}
