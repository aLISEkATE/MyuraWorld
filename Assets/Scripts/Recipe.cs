using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public string recipeName;

    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;
}


[System.Serializable]
public class ItemTypeAndCount
{
    public Item item;
    public int count;

    public ItemTypeAndCount(Item i, int c)
    {
        item = i;
        count = c;
    }
}