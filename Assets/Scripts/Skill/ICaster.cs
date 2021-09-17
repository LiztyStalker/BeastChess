using UnityEngine;

public interface ICaster
{
    SkillData[] skills { get; }
    TYPE_TEAM typeTeam { get; }

    Vector3 position { get; }
}
