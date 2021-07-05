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

    private void OnGUI()
    {
        IMGUIDrawer.CreateGUI(0f, 0f, 400, 1000, () => {

            //�Ʊ���ġ ������ġ

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


            //�Ʊ� ��� ��ġ
            if (GUILayout.Button("Create All Unit Random Left"))
            {
                _gameManager.CreateFieldUnit(TYPE_TEAM.Left);
            }


            //���� ��� ��ġ
            if (GUILayout.Button("Create All Unit Random Right"))
            {
                _gameManager.CreateFieldUnit(TYPE_TEAM.Right);
            }


            //��� ���� �����ϱ�
            if(GUILayout.Button("Remove All Units"))
            {
                _gameManager.ClearAllUnits();
            }

            GUILayout.Space(20f);

            //�ൿ
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

            //�ൿ
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



        });

        

        //if (isTest)
        //{
        //    CreateFieldUnit(_rightCommandActor, TYPE_TEAM.Right);
        //}
    }

}

#endif