using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    FieldBlock _block;

    [SerializeField]
    private Vector2Int _fieldSize = new Vector2Int(17, 7);

    [SerializeField]
    private float _length = 1.25f;


    public void SetFieldSizeAndLength(Vector2Int size, float length = 1.25f)
    {
        _fieldSize = size;
        _length = length;
    }

    public void Initialize() {
        CreateBlocks();
    }

    private void CreateBlocks()
    {
        var fieldBlocks = new FieldBlock[_fieldSize.y][];

        var startX = -((float)_fieldSize.x) * _length * 0.5f + _length * 0.5f;
        var startY = -((float)_fieldSize.y) * _length * 0.5f + _length * 0.5f;

        for (int y = 0; y < _fieldSize.y; y++)
        {
            fieldBlocks[y] = new FieldBlock[_fieldSize.x];

            for (int x = 0; x < _fieldSize.x; x++)
            {
                if (_block == null)
                {
                    var obj = DataStorage.Instance.GetDataOrNull<GameObject>("FieldBlock", null, null);
                    _block = obj.GetComponent<FieldBlock>();
                }
                var block = Instantiate(_block);
                block.transform.SetParent(transform);
                block.SetCoordinate(new Vector2Int(x, y));
                block.transform.localPosition = new Vector3(startX + ((float)x) * _length, startY + ((float)y) * _length, 0f);
                block.gameObject.SetActive(true);

                fieldBlocks[y][x] = block;
            }
        }

        FieldManager.Initialize(fieldBlocks, _fieldSize);
    }

    public void CleanUp()
    {
        FieldManager.CleanUp();
    }

}
