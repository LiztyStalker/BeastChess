//Activator로 생성할때 Protected를 생성할 수 있는지 확인 필요

public class StatusValueAttack : StatusValue, IStatusValue
{
    public StatusValueAttack(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueDefensive : StatusValue, IStatusValue
{
    public StatusValueDefensive(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueProficiency : StatusValue, IStatusValue
{
    public StatusValueProficiency(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueMovement : StatusValue, IStatusValue
{
    public StatusValueMovement(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueChargeMovement : StatusValue, IStatusValue
{
    public StatusValueChargeMovement(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueCounter : StatusValue, IStatusValue
{
    public StatusValueCounter(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueRevCounter : StatusValue, IStatusValue
{
    public StatusValueRevCounter(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueMaxHealth : StatusValue, IStatusValue
{
    public StatusValueMaxHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueAttackCount : StatusValue, IStatusValue
{
    public StatusValueAttackCount(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueNowHealth : StatusValue, IStatusValue
{
    public StatusValueNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValuePriority : StatusValue, IStatusValue
{
    public StatusValuePriority(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueAttackRange : StatusValue, IStatusValue
{
    public StatusValueAttackRange(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}


public class StatusValueAttackStartRange : StatusValue, IStatusValue
{
    public StatusValueAttackStartRange(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}


public class StatusValueSkillCastRate : StatusValue, IStatusValue
{
    public StatusValueSkillCastRate(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}



public class StatusValueIncreaseNowHealth : StatusValue, IStatusValue
{
    public StatusValueIncreaseNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusValueDecreaseNowHealth : StatusValue, IStatusValue
{
    public StatusValueDecreaseNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
}

public class StatusTurnIncreaseNowHealth : StatusValue, IStatusValue, IStatusTurn
{
    public StatusTurnIncreaseNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
    public void Turn(IUnitActor uActor) => uActor.IncreaseHealth((int)value);
}

public class StatusTurnDecreaseNowHealth : StatusValue, IStatusValue, IStatusTurn
{
    public StatusTurnDecreaseNowHealth(TYPE_VALUE typeValue, float value) : base(typeValue, value)
    {
    }
    public void Turn(IUnitActor uActor) => uActor.DecreaseHealth((int)value);
}
