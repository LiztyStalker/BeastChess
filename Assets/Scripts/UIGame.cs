using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    GameTestManager gameTestManager;

    [SerializeField]
    Text turnText;
    [SerializeField]
    Text deadLText;
    [SerializeField]
    Text deadRText;

    [SerializeField]
    GameObject turnL;

    [SerializeField]
    GameObject turnR;

    [SerializeField]
    GameObject gameEnd;

    [SerializeField]
    Transform tr;

    [SerializeField]
    UIUnitButton _unitButton;

    [SerializeField]
    UnitData[] _unitDataArray;

    List<UIUnitButton> list = new List<UIUnitButton>();


    private void Awake()
    {
        for (int i = 0; i < _unitDataArray.Length; i++) {
            var btn = Instantiate(_unitButton);
            btn.SetData(_unitDataArray[i]);
            btn.AddUnitDownListener(DragUnit);
            btn.AddUnitUpListener(DropUnit);
            btn.transform.SetParent(tr);
            btn.gameObject.SetActive(true);
            list.Add(btn);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _unitDataArray.Length; i++)
        {
            list[i].RemoveUnitDownListener(DragUnit);
            list[i].RemoveUnitUpListener(DragUnit);
        }

        list.Clear();
    }

    void DragUnit(UnitData uData)
    {
        _unitManager.DragUnit(uData);
    }

    void DropUnit(UnitData uData)
    {
        gameTestManager.DropUnit(uData);
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = "Turn : " + gameTestManager.turnCount.ToString();
        deadLText.text = _unitManager.deadL.ToString();
        deadRText.text = _unitManager.deadR.ToString();
        turnL.gameObject.SetActive(gameTestManager._typeTeam == TYPE_TEAM.Left);
        turnR.gameObject.SetActive(gameTestManager._typeTeam == TYPE_TEAM.Right);
        gameEnd.gameObject.SetActive(gameTestManager.isEnd);
    }
}
