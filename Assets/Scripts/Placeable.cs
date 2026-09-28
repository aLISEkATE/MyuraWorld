using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;
   
    public int GetID()
    {
        return ID;
    }

    public void SetID(int id)
    {
        ID = id;
    }

}