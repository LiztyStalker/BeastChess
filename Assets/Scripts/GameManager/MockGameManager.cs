using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class MockGameData {

//    private static MockGameData _instance;
//    public static MockGameData instance
//    {
//        get
//        {
//            if(_instance == null)
//            {
//                _instance = new MockGameData();
//            }
//            return _instance;
//        }
//    }


//    public CommanderActor _lCommanderActor;
//    public CommanderActor _rCommanderActor;
    
//}



public class MockGameManager : MonoBehaviour
{

    [SerializeField]
    UIBattleField uiBattleField;

    void Start()
    {
        uiBattleField.Initialize();
    }



    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_MockGame");
    }

}
