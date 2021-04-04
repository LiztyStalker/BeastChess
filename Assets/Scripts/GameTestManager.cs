using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting
{
    public const float FREAM_TIME = 0.01f;
}

public class GameTestManager : MonoBehaviour
{

    [SerializeField]
    FieldManager _fieldManager;

    [SerializeField]
    UnitManager _unitManager;

    [SerializeField]
    int count = 4;

    [HideInInspector]
    public int turnCount;

    [SerializeField]
    int _maxSupplyValueLeft = 10;
    int _nowSupplyValueLeft = 10;

    [SerializeField]
    int _maxSupplyValueRight = 10;
    int _nowSupplyValueRight = 10;


    Coroutine co;

    public TYPE_TEAM _typeTeam;

    public bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        _fieldManager.Initialize();

        //for (int i = 0; i < count; i++)
        //{
        //    CreateRandomUnit(TYPE_TEAM.Left);
        //    CreateRandomUnit(TYPE_TEAM.Right);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (co == null)
                co = StartCoroutine(TurnCoroutine());
        }
        else
        {
            if (_typeTeam == TYPE_TEAM.Right)
            {
                if (co == null)
                    co = StartCoroutine(TurnCoroutine());
            }
        }

    }


    public string GetSupply(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return string.Format("{0}/{1}", _nowSupplyValueLeft, _maxSupplyValueLeft);
            case TYPE_TEAM.Right:
                return string.Format("{0}/{1}", _nowSupplyValueRight, _maxSupplyValueRight);
        }
        return null;
    }

    IEnumerator TurnCoroutine()
    {
        yield return _unitManager.ActionUnits(_fieldManager, _typeTeam);

        if (_fieldManager.IsGameEnd(_typeTeam))
        {
            co = null;
            isEnd = true;
            yield break;
        }

        if (_typeTeam == TYPE_TEAM.Right)
        {
            for (int i = 0; i < count; i++)
            {
                CreateRandomUnit(_typeTeam);
                yield return new WaitForSeconds(Setting.FREAM_TIME);
            }
        }

        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                _typeTeam = TYPE_TEAM.Right;
                _nowSupplyValueRight = _maxSupplyValueRight;
                break;
            case TYPE_TEAM.Right:
                _typeTeam = TYPE_TEAM.Left;
                _nowSupplyValueLeft = _maxSupplyValueLeft;
                turnCount++;
                break;
        }
        yield return null;
        co = null;
    }

    private void CreateRandomUnit(TYPE_TEAM typeTeam)
    {
        var block =_fieldManager.GetRandomBlock(typeTeam);
        if (block.unitActor == null)
        {
            //CommandActor 필요 - 보급, 사용 유닛, 스킬 등을 보관하고 있음
            _unitManager.CreateRandomUnit(block, typeTeam);
        }
    }

    public bool IsSupply(UnitData uData)
    {
        return (_typeTeam == TYPE_TEAM.Left) ? (_nowSupplyValueLeft - uData.costValue) >= 0 : (_nowSupplyValueRight - uData.costValue) >= 0;
    }

    public void DragUnit(UnitData uData)
    {
        _unitManager.DragUnit(uData);
    }

    public void DropUnit(UnitData uData)
    {
        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                _nowSupplyValueLeft -= uData.costValue;
                break;
            case TYPE_TEAM.Right:
                _nowSupplyValueRight -= uData.costValue;
                break;
        }

        _unitManager.DropUnit(uData, _typeTeam);
    }
}
