#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Test;

public class BattleTester : MonoBehaviour
{

    [SerializeField]
    GameManager _gameManager;

    bool isLeftUnit = false;
    bool isRightUnit = false;

    private void OnGUI()
    {
        IMGUIDrawer.CreateGUI(0f, 0f, 300, 1000, () => {

            //아군배치 적군배치

            var typeTeam = _gameManager.GetNowTeam();
            if(GUILayout.Button($"Change Team {typeTeam}"))
            {
                typeTeam = (typeTeam == TYPE_TEAM.Left) ? TYPE_TEAM.Right : TYPE_TEAM.Left;
                _gameManager.SetNowTeam(typeTeam);
            }


            bool isTestFormation = Settings.SingleFormation;
            if (GUILayout.Button($"Unit Formation Test {isTestFormation}"))
            {
                Settings.SingleFormation = !Settings.SingleFormation;
            }

            bool invincible = Settings.Invincible;
            if (GUILayout.Button($"Unit Invincible {invincible}"))
            {
                Settings.Invincible = !Settings.Invincible;
            }



            var units = DataStorage.Instance.GetUnits();
            if (GUILayout.Button(((!isLeftUnit) ? "Show" : "Hide") + " Unit Left"))
            {
                isLeftUnit = !isLeftUnit;
            }

            if (isLeftUnit)
            {
                //해당 병사 모두 배치
                for (int i = 0; i < units.Length; i++)
                {
                    var unit = units[i];
                    if (GUILayout.Button($"Create Unit {unit.name}"))
                    {
                        _gameManager.CreateFieldUnit(TYPE_TEAM.Left, unit);
                    }
                }
                //랜덤 병사 모두 배치
                if (GUILayout.Button("Create All Unit Random Left"))
                {
                    _gameManager.CreateFieldUnit(TYPE_TEAM.Left);
                }
            }


            if (GUILayout.Button(((!isRightUnit) ? "Show" : "Hide") + " Unit Right"))
            {
                isRightUnit = !isRightUnit;
            }

            if (isRightUnit)
            {
                //해당 병사 모두 배치
                for (int i = 0; i < units.Length; i++)
                {
                    var unit = units[i];
                    if (GUILayout.Button($"Create Unit {unit.name}"))
                    {
                        _gameManager.CreateFieldUnit(TYPE_TEAM.Right, unit);
                    }
                }
                //랜덤 적군 모두 배치
                if (GUILayout.Button("Create All Unit Random Right"))
                {
                    _gameManager.CreateFieldUnit(TYPE_TEAM.Right);
                }
            }
                       
            //모든 병력 제거하기
            if (GUILayout.Button("Remove All Units"))
            {
                _gameManager.ClearAllUnits();
            }

            GUILayout.Space(20f);

            //행동
            if(GUILayout.Button("Forward Action Left"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Forward, TYPE_BATTLE_TURN.None);
            }
            if (GUILayout.Button("Shoot Action Left"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Shoot, TYPE_BATTLE_TURN.None);

            }
            if (GUILayout.Button("Guard Action Left"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Guard, TYPE_BATTLE_TURN.None);

            }
            if (GUILayout.Button("Charge Action Left"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Charge, TYPE_BATTLE_TURN.None);
            }
            if (GUILayout.Button("Backward Action Left"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Backward, TYPE_BATTLE_TURN.None);
            }

            GUILayout.Space(20f);

            //행동
            if (GUILayout.Button("Forward Action Right"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.None, TYPE_BATTLE_TURN.Forward);
            }
            if (GUILayout.Button("Shoot Action Right"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.None, TYPE_BATTLE_TURN.Shoot);

            }
            if (GUILayout.Button("Guard Action Right"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.None, TYPE_BATTLE_TURN.Guard);

            }
            if (GUILayout.Button("Charge Action Right"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.None, TYPE_BATTLE_TURN.Charge);
            }
            if (GUILayout.Button("Backward Action Right"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.None, TYPE_BATTLE_TURN.Backward);
            }

            GUILayout.Space(20f);

            //행동
            if (GUILayout.Button("Forward Action All"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Forward, TYPE_BATTLE_TURN.Forward);
            }
            if (GUILayout.Button("Shoot Action All"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Shoot, TYPE_BATTLE_TURN.Shoot);

            }
            if (GUILayout.Button("Guard Action All"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Guard, TYPE_BATTLE_TURN.Guard);

            }
            if (GUILayout.Button("Charge Action All"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Charge, TYPE_BATTLE_TURN.Charge);
            }
            if (GUILayout.Button("Backward Action All"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Backward, TYPE_BATTLE_TURN.Backward);
            }




        });

        

        //if (isTest)
        //{
        //    CreateFieldUnit(_rightCommandActor, TYPE_TEAM.Right);
        //}
    }

}

#endif