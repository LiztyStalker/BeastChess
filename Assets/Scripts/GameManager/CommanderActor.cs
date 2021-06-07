using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderActor
{
    private int _castleHealthValue;

    private const int SUPPLY_LEVEL_VALUE = 20;
    private const int SUPPLY_INCREASE_VALUE = 20;
    private const int SUPPLY_VALUE = 4000;
    private const int SUPPLY_ADD_VALUE = 5;
    private const int CASTLE_HEALTH_VALUE = 1000;
    private const int CASTLE_HEALTH_INCREASE_VALUE = 100;

    private int _castleHealthWeight;

    private UnitCard[] _unitDataArray;

    private int _supplyLevel;

    private int _nowSupplyValue;

    private int maxSupplyValue => SUPPLY_VALUE + SUPPLY_LEVEL_VALUE * supplyLevel;

    private int _nowCastleHealthValue;

    public int supplyValue => SUPPLY_INCREASE_VALUE + supplyLevel * SUPPLY_ADD_VALUE;

    public UnitCard[] unitDataArray => _unitDataArray;
    public int supplyLevel => _supplyLevel;
    
    public int castleHealthValue => _castleHealthValue + CASTLE_HEALTH_INCREASE_VALUE * _castleHealthWeight;
    public int nowCastleHealthValue => _nowCastleHealthValue;

    public CommanderActor(UnitCard[] unitDataArray, int level = 0)
    {
        int cycle = level;
        for(int i = 0; i < cycle; i++)
        {
            UpgradeSupply();
        }

        _supplyLevel = level;
        _unitDataArray = unitDataArray;
        _nowSupplyValue = SUPPLY_VALUE;
        _castleHealthValue = CASTLE_HEALTH_VALUE;
        _castleHealthWeight = 0;
        _nowCastleHealthValue = castleHealthValue;
    }

    public bool IsUpgradeSupply()
    {
        return _nowSupplyValue == maxSupplyValue;
    }

    public void UpgradeSupply()
    {
        var rate = (float)_nowCastleHealthValue / (float)castleHealthValue;

        _castleHealthWeight += _supplyLevel;
        _supplyLevel++;

        _nowCastleHealthValue = (int)((float)castleHealthValue * rate);

        _nowSupplyValue = 0;
    }

    public float SupplyRate()
    {
        return (float)_nowSupplyValue / (float)maxSupplyValue;
    }

    public void IncreaseHealth(int damageValue)
    {
        if (_nowCastleHealthValue - damageValue < 0)
            _nowCastleHealthValue = 0;
        else
            _nowCastleHealthValue -= damageValue;
    }

    public bool IsEmptyCastleHealth()
    {
        return _nowCastleHealthValue == 0;
    }

    public bool IsSupply(UnitCard uCard)
    {
        return (_nowSupplyValue - uCard.costValue >= 0);
    }

    public void Supply()
    {
        if (_nowSupplyValue + (supplyValue) > maxSupplyValue)
            _nowSupplyValue = maxSupplyValue;
        else
            _nowSupplyValue += supplyValue;
    }

    public void UseSupply(UnitCard uCard)
    {
        _nowSupplyValue -= uCard.costValue;
    }

    public float GetCastleHealthRate()
    {
        return (float)_nowCastleHealthValue / (float)castleHealthValue;
    }

    public string ToSupplyString()
    {
        return string.Format("{0}/{1}", _nowSupplyValue, maxSupplyValue);
    }
}
