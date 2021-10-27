using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleFieldSettings
{
    public const int BATTLE_ROUND_COUNTER = 3;
    public const int BATTLE_TURN_COUNTER = 3;


    public const float RECOVERY_HEALTH_RATE = 0.5f;

    public const float FRAME_TIME = 0.01f;
    public const float FRAME_END_TIME = 0.25f;
    public const float BULLET_MOVEMENT = 0.8f;
    public const float MAX_UNIT_MOVEMENT = 0.04f;
    public const float MIN_UNIT_MOVEMENT = 0.06f;

    public static bool Invincible = false;
    public static bool SingleFormation = false;

}
