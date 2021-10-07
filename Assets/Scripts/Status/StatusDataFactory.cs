using System;

public class StatusDataFactory
{
    public static T Create<T>() where T : IStatusEffect
    {
        return Activator.CreateInstance<T>();
    }

    public static T Create<T>(StatusValue.TYPE_VALUE typeValue, float value) where T : IStatusValue
    {
        return (T)Activator.CreateInstance(typeof(T), typeValue, value);
    }
}



