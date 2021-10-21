using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformationAbility : MonoBehaviour
{
    [SerializeField]
    private Text _attackValueText;

    [SerializeField]
    private Text _attackCountText;

    [SerializeField]
    private Text _attackBaseText;

    [SerializeField]
    private Text _attackRangeTypeText;

    [SerializeField]
    private Text _attackRangeText;

    [SerializeField]
    private Text _sightRangeText;

    [SerializeField]
    private Text _attackPriorityTypeText;

    [SerializeField]
    private Text _attackTargetCountText;

    [SerializeField]
    private Text _movementTypeText;

    [SerializeField]
    private Text _movementValueText;

    [SerializeField]
    private Text _defensiveValueText;

    [SerializeField]
    private Text _priorityValueText;

    public void SetData(UnitCard uCard)
    {
        _attackValueText.text = uCard.damageValue.ToString();
        _attackCountText.text = uCard.attackCount.ToString();

        _attackBaseText.text = (uCard.BulletData == null) ? "근거리" : "원거리";// uCard.BulletData.name;
        _attackRangeTypeText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_TARGET_RANGE), uCard.AttackTargetData.TypeTargetRange.ToString(), "Name");

        if (uCard.AttackTargetData.TargetStartRange > 0)
        {
            _attackRangeText.text = $"{uCard.AttackTargetData.TargetStartRange}~{uCard.AttackTargetData.TargetRange}";
        }
        else
        {
            _attackRangeText.text = uCard.AttackTargetData.TargetRange.ToString();
        }
        _sightRangeText.text = "-";// uCard.AttackTargetData.TargetStartRange.ToString();

        _attackPriorityTypeText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_TARGET_PRIORITY), uCard.AttackTargetData.TypeTargetPriority.ToString(), "Name");
        _attackTargetCountText.text = uCard.AttackTargetData.TargetCount.ToString();

        _defensiveValueText.text = uCard.defensiveValue.ToString();
        _priorityValueText.text = uCard.priorityValue.ToString();
        _movementValueText.text = uCard.movementValue.ToString();
        _movementTypeText.text = TranslatorStorage.Instance.GetTranslator("MetaData", typeof(TYPE_MOVEMENT), uCard.typeMovement.ToString(), "Name");
    }
}
