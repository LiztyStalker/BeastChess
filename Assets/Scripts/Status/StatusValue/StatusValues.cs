using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusValueAttack : Status, IStatusValue
{    public StatusValueAttack(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueDefensive : Status, IStatusValue
{
    public StatusValueDefensive(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueProficiency : Status, IStatusValue
{
    public StatusValueProficiency(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueMovement : Status, IStatusValue
{
    public StatusValueMovement(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueCounter : Status, IStatusValue
{
    public StatusValueCounter(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueRevCounter : Status, IStatusValue
{
    public StatusValueRevCounter(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueMaxHealth : Status, IStatusValue
{
    public StatusValueMaxHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueAttackCount : Status, IStatusValue
{
    public StatusValueAttackCount(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueNowHealth : Status, IStatusValue
{
    public StatusValueNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValuePriority : Status, IStatusValue
{
    public StatusValuePriority(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueAttackRange : Status, IStatusValue
{
    public StatusValueAttackRange(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}


public class StatusValueAttackStartRange : Status, IStatusValue
{
    public StatusValueAttackStartRange(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}


public class StatusValueSkillCastRate : Status, IStatusValue
{
    public StatusValueSkillCastRate(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}
