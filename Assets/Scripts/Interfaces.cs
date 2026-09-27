using System;
using UnityEngine;

public interface IPlace
{
   
}
public interface IUse
{
    void UseItem();
}
public interface ITrigger
{
    event Action Triggered;
    
}