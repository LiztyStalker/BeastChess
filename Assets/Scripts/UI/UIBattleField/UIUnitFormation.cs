using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitFormation : MonoBehaviour
{
    private Image[] _formationImageArray;

    public void Initialize()
    {
        _formationImageArray = new Image[9];
        for(int i = 0; i < _formationImageArray.Length; i++)
        {
            var obj = new GameObject();
            obj.AddComponent<RectTransform>();
            var image = obj.AddComponent<Image>();
            _formationImageArray[i] = image;
        }
    }

    public void CleanUp()
    {
        _formationImageArray = null;
    }

    public void ShowFormation(UnitCard _uCard)
    {
        List<int> list = new List<int>();

        var arr = _uCard.UnitKeys;
        for (int i = 0; i < arr.Length; i++)
        {
            var cell = _uCard.formationCells[i];

            int index = (cell.x + 1) + ((cell.y + 1) * 3);

            if (!_uCard.IsDead(arr[i]))
            {
                _formationImageArray[index].color = Color.green;
            }
            else
            {
                _formationImageArray[index].color = Color.red;
            }

            list.Add(index);
        }

        for (int i = 0; i < _formationImageArray.Length; i++)
        {
            if (!list.Contains(i))
            {
                _formationImageArray[i].color = Color.gray;
            }
        }
    }
}
