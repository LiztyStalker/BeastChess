using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommanderActor : ICaster
{
    UnitCard[] unitDataArray { get; }
    int supplyLevel { get; }

    int supplyValue { get; }

    int nowSupplyValue { get; }

    int castleHealthValue { get; }
    int nowCastleHealthValue { get; }

    int maxLeadershipValue { get; }
    int nowLeadershipValue { get; }
    TYPE_COMMANDER_MASTER typeCommanderMaster { get; }

    TYPE_BATTLE_TURN[] GetTypeBattleTurns();

    void SetTypeBattleTurns(TYPE_BATTLE_TURN[] typeBattleTurns);
    void UpgradeSupply();

    float GetSupplyRate();

    void DecreaseHealth(int damageValue);

    void RefreshHealth();

    bool IsEmptyCastleHealth();

    bool IsSupply(UnitCard uCard);

    void Supply();

    void RefreshSupply();

    void UseSupply(UnitCard uCard);

    void ReturnSupply(UnitCard uCard);

    float GetCastleHealthRate();

    string ToSupplyString();

    void RecoveryUnits();

    new TYPE_BATTLE_TEAM typeTeam { get;}

    //군단에 위임할 필요 있음
    bool IsEnoughLeadership(UnitCard uCard);
    void AddCard(UnitCard uCard);
    void RemoveCard(UnitCard uCard);
    void SetCommanderCard(CommanderCard cmdCard);

    bool IsSurrender();


    #region ##### Listener #####
    void AddHealthListener(System.Action<TYPE_BATTLE_TEAM, int, float> act);
    void RemoveHealthListener(System.Action<TYPE_BATTLE_TEAM, int, float> act);


    void AddSupplyListener(System.Action<TYPE_BATTLE_TEAM, int, float> act);
    void RemoveSupplyListener(System.Action<TYPE_BATTLE_TEAM, int, float> act);
    #endregion
}
