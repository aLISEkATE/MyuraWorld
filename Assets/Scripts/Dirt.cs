
using NUnit.Framework;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;

    public bool isWatered;
    public bool hasSeed;
    public int seedID;
    public int GetID()
    {
        return ID;
    }

    public void SetID(int id)
    {
        ID = id;
    }

   private void OnEnable()
   {
      TimeManager.onDayChanged += Dry;
   }

   private void OnDisable()
   {
      TimeManager.onDayChanged -= Dry;
   }

   private void Dry()
   {
      isWatered = false;
      Debug.Log("Dirt dried out!");
   } 
     public void Water()
    {
        isWatered = true;
        Debug.Log("Dirt watered!");
    }

          public void Plant(int ID)
    {
        hasSeed = true;
        seedID = ID;

        Debug.Log("Seed Planted!");
        Debug.Log("Seed Recieved ID -" + ID );
    }
}

