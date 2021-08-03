using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommanderOutpost : MonoBehaviour
{

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


    private int _index = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _lBtn.onClick.AddListener(OnLeftClicked);
        _rBtn.onClick.AddListener(OnRightClicked);

        _commanders = DataStorage.Instance.GetCommanders();

        ShowCommander();
    }

    private void OnDestroy()
    {
        _lBtn.onClick.RemoveListener(OnLeftClicked);
        _rBtn.onClick.RemoveListener(OnRightClicked);
    }

    private void ShowCommander()
    {
        var commanderData = _commanders[_index];

        _icon.sprite = commanderData.icon;
        _nameText.text = commanderData.name;
        _influenceText.text = commanderData.typeInfluence.ToString();
        //_edeologyText.text = commanderData;
        _masterText.text = commanderData.typeCommanderMaster.ToString();
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
}
