using System;


public interface IUse
{
    void UseItem();
}

public interface ITrigger
{
    event Action Triggered;
}