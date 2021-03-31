using System.Collections;
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
