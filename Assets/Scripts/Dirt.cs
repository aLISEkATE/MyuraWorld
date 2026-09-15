
using UnityEngine;

public class Dirt : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;

    public bool isWatered;
    public bool hasSeed;

    public int GetID()
    {
        return ID;
    }

    public void SetID(int id)
    {
        ID = id;
    }
}

