using UnityEngine;

public interface ICaster
{
    SkillData[] skills { get; }
    TYPE_BATTLE_TEAM typeTeam { get; }

    Vector3 position { get; }
}
