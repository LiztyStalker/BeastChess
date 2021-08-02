using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitFormation : MonoBehaviour
{
    [SerializeField]
    Image[] formationImageArray;

    public void ShowFormation(UnitCard _uCard)
    {


        List<int> list = new List<int>();

        var arr = _uCard.unitArray;
        for (int i = 0; i < arr.Length; i++)
        {
            var cell = _uCard.formationCells[i];

            int index = (cell.x + 1) + ((cell.y + 1) * 3);

            if (!_uCard.IsDead(arr[i]))
            {
                formationImageArray[index].color = Color.green;
            }
            else
            {
                formationImageArray[index].color = Color.red;
            }

            list.Add(index);
        }

        for (int i = 0; i < formationImageArray.Length; i++)
        {
            if (!list.Contains(i))
            {
                formationImageArray[i].color = Color.gray;
            }
        }
    }
}
