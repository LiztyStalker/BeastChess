using UnityEngine;
using UnityEngine.UI;

public class UIUnitInformationCost : MonoBehaviour
{

    [SerializeField]
    private Text _employCostText;

    [SerializeField]
    private Text _maintenanceCostText;

    [SerializeField]
    private Text _appearCostText;

    public void SetData(UnitCard uCard)
    {
        _employCostText.text = uCard.employCostValue.ToString();
        _maintenanceCostText.text = uCard.maintenenceCostValue.ToString();
        _appearCostText.text = uCard.AppearCostValue.ToString();
    }
}
