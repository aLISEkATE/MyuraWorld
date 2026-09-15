using UnityEngine;

public abstract class Tool : Item, IUse
{
    public float useCooldown = 0.1f;

    public override void UseItem()

    {
        Debug.Log("Using Tool " + Name);
    }
}