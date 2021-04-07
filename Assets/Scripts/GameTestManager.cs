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
    bool isTest = false;

    [SerializeField]
    int count = 4;

    [HideInInspector]
    public int turnCount;

    CommanderActor _leftCommandActor;
    CommanderActor _rightCommandActor;

    Coroutine co;

    public TYPE_TEAM _typeTeam;

    public bool isEnd = false;

    // Start is called before the first frame update
    void Awake()
    {
        _fieldManager.Initialize();

        var units = _unitManager.GetRandomUnit(4);

        _leftCommandActor = new CommanderActor(units, 1);
        _rightCommandActor = new CommanderActor(units, 1);

        _unitManager.CreateCastleUnit(_fieldManager, TYPE_TEAM.Left);
        _unitManager.CreateCastleUnit(_fieldManager, TYPE_TEAM.Right);

    }

    private void Start()
    {
        if (isTest)
            CreateFieldUnit(_rightCommandActor, TYPE_TEAM.Right);
    }

    public int GetCastleHealth(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.nowCastleHealthValue;
            case TYPE_TEAM.Right:
                return _rightCommandActor.nowCastleHealthValue;
        }
        return -1;
    }

    public float GetCastleHealthRate(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.GetCastleHealthRate();
            case TYPE_TEAM.Right:
                return _rightCommandActor.GetCastleHealthRate();
        }
        return -1f;
    }

    public UnitData[] GetLeftUnits()
    {
        return _leftCommandActor.unitDataArray;
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
                return _leftCommandActor.ToSupplyString();
            case TYPE_TEAM.Right:
                return _rightCommandActor.ToSupplyString();
        }
        return null;
    }
    
    public bool IsUpgradeSupply(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                return _leftCommandActor.IsUpgradeSupply();
            case TYPE_TEAM.Right:
                return _rightCommandActor.IsUpgradeSupply();
        }
        return false;
    }

    IEnumerator TurnCoroutine()
    {

        if(_typeTeam == TYPE_TEAM.Left)
            yield return _unitManager.ActionUnits(_fieldManager, _typeTeam);
        else if (!isTest && _typeTeam == TYPE_TEAM.Right)
            yield return _unitManager.ActionUnits(_fieldManager, _typeTeam);

        if (IsGameEnd())
        {
            co = null;
            isEnd = true;
            yield break;
        }

        if (!isTest)
        {
            if (_typeTeam == TYPE_TEAM.Right)
            {
                if (Random.Range(0f, 100f) < 80f)
                {
                    for (int i = 0; i < count; i++)
                    {
                        CreateUnit(_rightCommandActor);
                        yield return new WaitForSeconds(Setting.FREAM_TIME);
                    }
                }
                else
                {
                    if (IsUpgradeSupply(TYPE_TEAM.Right))
                    {
                        IncreaseUpgrade(TYPE_TEAM.Right);
                        yield return new WaitForSeconds(Setting.FREAM_TIME);
                    }
                }
            }
        }

        switch (_typeTeam)
        {
            case TYPE_TEAM.Left:
                _typeTeam = TYPE_TEAM.Right;
                _rightCommandActor.Supply();
                break;
            case TYPE_TEAM.Right:
                _typeTeam = TYPE_TEAM.Left;
                _leftCommandActor.Supply();
                turnCount++;
                break;
        }
        yield return null;
        co = null;
    }

    public void IncreaseHealth(int damageValue, TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                _rightCommandActor.IncreaseHealth(damageValue);
                break;
            case TYPE_TEAM.Right:
                _leftCommandActor.IncreaseHealth(damageValue);
                break;
        }
    }

    public void IncreaseUpgrade(TYPE_TEAM typeTeam)
    {
        switch (typeTeam)
        {
            case TYPE_TEAM.Left:
                _leftCommandActor.UpgradeSupply();
                break;
            case TYPE_TEAM.Right:
                _rightCommandActor.UpgradeSupply();
                break;
        }

        if (co == null)
            co = StartCoroutine(TurnCoroutine());
    }

    public bool IsSupply(UnitData uData)
    {
        return (_typeTeam == TYPE_TEAM.Left) ? _leftCommandActor.IsSupply(uData) : _rightCommandActor.IsSupply(uData);
    }

    public void CreateUnit(CommanderActor cActor)
    {
        var block = _fieldManager.GetRandomBlock(_typeTeam);
        if (block != null && block.unitActor == null)
        {
            var unit = cActor.unitDataArray[Random.Range(0, cActor.unitDataArray.Length)];
            if (cActor.IsSupply(unit))
            {
                cActor.UseSupply(unit);
                _unitManager.CreateUnit(unit, block, _typeTeam);
            }
        }
    }

    public void CreateFieldUnit(CommanderActor cActor, TYPE_TEAM typeTeam)
    {
        var blocks = _fieldManager.GetAllBlocks(typeTeam);

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block != null && block.unitActor == null)
            {
                var unit = cActor.unitDataArray[0];
                if (unit != null)
                {
                    _unitManager.CreateUnit(unit, block, typeTeam);
                }
            }
        }
    }

    public void DragUnit(UnitData uData)
    {
        _unitManager.DragUnit(uData);
    }

    public void DropUnit(UnitData uData)
    {
        if (_unitManager.DropUnit(uData, _typeTeam))
        {
            switch (_typeTeam)
            {
                case TYPE_TEAM.Left:
                    _leftCommandActor.UseSupply(uData);
                    break;
                case TYPE_TEAM.Right:
                    _rightCommandActor.UseSupply(uData);
                    break;
            }
        }
    }

    private bool IsGameEnd()
    {
        return _leftCommandActor.IsEmptyCastleHealth() || _rightCommandActor.IsEmptyCastleHealth();
    }
}
