using UnityEngine.Assertions;

public class CommanderCamp
{
    private ICommanderActor _leftCommandActor = null;
    private ICommanderActor _rightCommandActor = null;

    public static CommanderCamp Create(ICommanderActor lActor, ICommanderActor rActor)
    {
        return new CommanderCamp(lActor, rActor);
    }

    private CommanderCamp(ICommanderActor lActor, ICommanderActor rActor)
    {
        _leftCommandActor = lActor;
        _rightCommandActor = rActor;
    }


    public ICommanderActor GetCommanderActor(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor;
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor;
        AssertTeam(typeTeam);
        return null;
    }

    public UnitCard[] GetUnitDataArray(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor.unitDataArray;
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor.unitDataArray;
        AssertTeam(typeTeam);
        return null;
    }

    public int NowSupplyValue(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor.nowSupplyValue;
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor.nowSupplyValue;
        AssertTeam(typeTeam);
        return 0;
    }

    public float GetSupplyRate(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor.GetSupplyRate();
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor.GetSupplyRate();
        AssertTeam(typeTeam);
        return 0;
    }

    public void SetTypeBattleTurns(TYPE_TEAM typeTeam, TYPE_BATTLE_TURN[] typeBattleTurns)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.SetTypeBattleTurns(typeBattleTurns);
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.SetTypeBattleTurns(typeBattleTurns);
        AssertTeam(typeTeam);
    }

    public void AddAllHealthListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.AddHealthListener(act);
        _rightCommandActor.AddHealthListener(act);
    }

    public void RemoveAllHealthListener(System.Action<TYPE_TEAM, int, float> act)
    {
        _leftCommandActor.RemoveHealthListener(act);
        _rightCommandActor.RemoveHealthListener(act);
    }

    public void DecreaseHealth(TYPE_TEAM typeTeam, int value)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.DecreaseHealth(value);
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.DecreaseHealth(value);
        AssertTeam(typeTeam);
    }

    public void AddSupplyListener(TYPE_TEAM typeTeam, System.Action<TYPE_TEAM, int, float> act)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.AddSupplyListener(act);
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.AddSupplyListener(act);
        AssertTeam(typeTeam);
    }

    public void RemoveSupplyListener(TYPE_TEAM typeTeam, System.Action<TYPE_TEAM, int, float> act)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.RemoveSupplyListener(act);
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.RemoveSupplyListener(act);
        AssertTeam(typeTeam);
    }

    public void Supply(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.Supply();
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.Supply();
        AssertTeam(typeTeam);
    }

    public void UseSupply(TYPE_TEAM typeTeam, UnitCard uCard)
    {
        if (typeTeam == TYPE_TEAM.Left)
            _leftCommandActor.UseSupply(uCard);
        else if (typeTeam == TYPE_TEAM.Right)
            _rightCommandActor.UseSupply(uCard);

        AssertTeam(typeTeam);
    }

    public bool IsSupply(UnitCard uCard, TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor.IsSupply(uCard);
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor.IsSupply(uCard);
        AssertTeam(typeTeam);
        return false;
    }

    private void AssertTeam(TYPE_TEAM typeTeam)
    {
        Assert.IsTrue(typeTeam == TYPE_TEAM.Left || typeTeam == TYPE_TEAM.Right, $"{typeTeam} 적용된 팀이 없습니다");
    }

    public bool IsGameEnd()
    {
        return _leftCommandActor.IsEmptyCastleHealth() || _rightCommandActor.IsEmptyCastleHealth();
    }

    public void RecoveryAllUnits()
    {
        _leftCommandActor.RecoveryUnits();
        _rightCommandActor.RecoveryUnits();
    }


    public TYPE_BATTLE_RESULT GameResult()
    {
        if (!_leftCommandActor.IsEmptyCastleHealth() && !_rightCommandActor.IsEmptyCastleHealth())
        {
            return TYPE_BATTLE_RESULT.Draw;
        }
        if (_leftCommandActor.IsSurrender())
        {
            return TYPE_BATTLE_RESULT.Defeat;
        }
        else if (_rightCommandActor.IsSurrender())
        {
            return TYPE_BATTLE_RESULT.Victory;
        }
        else
        {
            return TYPE_BATTLE_RESULT.Draw;
        }
    }

    public TYPE_BATTLE_TURN[] GetTypeBattleTurns(TYPE_TEAM typeTeam)
    {
        if (typeTeam == TYPE_TEAM.Left)
            return _leftCommandActor.GetTypeBattleTurns();
        else if (typeTeam == TYPE_TEAM.Right)
            return _rightCommandActor.GetTypeBattleTurns();
        AssertTeam(typeTeam);
        return null;
    }


}
