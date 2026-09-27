using UnityEngine;

public abstract class Tool : Item, IUse
{

    public override void UseItem()

    {
        Debug.Log("Using Tool " + Name);
    }
}