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
            if(GUILayout.Button("Forward Action"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Forward);
            }
            if (GUILayout.Button("Shoot Action"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Shoot);

            }
            if (GUILayout.Button("Guard Action"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Guard);

            }
            if (GUILayout.Button("Charge Action"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Charge);
            }
            if (GUILayout.Button("Backward Action"))
            {
                _gameManager.NextTurnTester(TYPE_BATTLE_TURN.Backward);
            }



        });

        

        //if (isTest)
        //{
        //    CreateFieldUnit(_rightCommandActor, TYPE_TEAM.Right);
        //}
    }

}

#endif