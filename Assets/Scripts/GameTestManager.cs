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


    Coroutine co;

    public TYPE_TEAM _typeTeam;

    public bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        _fieldManager.Initialize();

        for (int i = 0; i < count; i++)
        {
            CreateUnit(TYPE_TEAM.Left);
            CreateUnit(TYPE_TEAM.Right);
        }
    }

    // Update is called once per frame
    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.Return))
//        {
            if(co == null)
                co = StartCoroutine(TurnCoroutine());
//        }
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

        for (int i = 0; i < count; i++)
        {
            CreateUnit(_typeTeam);
            yield return new WaitForSeconds(Setting.FREAM_TIME);
        }

        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                _typeTeam = TYPE_TEAM.Right;
                break;
            case TYPE_TEAM.Right:
                _typeTeam = TYPE_TEAM.Left;
                turnCount++;
                break;
        }
        yield return null;
        co = null;
    }

    private void CreateUnit(TYPE_TEAM typeTeam)
    {
        var block =_fieldManager.GetRandomBlock(typeTeam);
        if(block.unitActor == null) _unitManager.CreateUnit(block, typeTeam);
    }
}
