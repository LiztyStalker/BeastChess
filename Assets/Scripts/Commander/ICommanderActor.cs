using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommanderActor : ICaster
{
    UnitCard[] unitDataArray { get; }
    int supplyLevel { get; }

    int supplyValue { get; }

    int castleHealthValue { get; }
    int nowCastleHealthValue { get; }

    int maxLeadershipValue { get; }
    int nowLeadershipValue { get; }
    TYPE_COMMANDER_MASTER typeCommanderMaster { get; }

    TYPE_BATTLE_TURN[] GetTypeBattleTurns();
    void UpgradeSupply();

    float SupplyRate();

    void IncreaseHealth(int damageValue);

    bool IsEmptyCastleHealth();

    bool IsSupply(UnitCard uCard);

    void Supply();

    void UseSupply(UnitCard uCard);

    void ReturnSupply(UnitCard uCard);

    float GetCastleHealthRate();

    string ToSupplyString();

    void RecoveryUnits();

    new TYPE_TEAM typeTeam { get; set; }

    //군단에 위임할 필요 있음
    bool IsEnoughLeadership(UnitCard uCard);
    void AddCard(UnitCard uCard);
    void RemoveCard(UnitCard uCard);
    void SetCommanderCard(CommanderCard cmdCard);
}
