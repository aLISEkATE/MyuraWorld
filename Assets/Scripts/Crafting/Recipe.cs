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
}