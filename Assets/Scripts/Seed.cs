
using NUnit.Framework;
using UnityEngine;

public class Seed : Tool
{
    [Header("Seed Settings")]
    [SerializeField]
    private float gridSize = 1f;

    [Header("Growth Settings")]
    [SerializeField] private int growthDays = 3;
    private int plantedDay;
    private int daysPassed;
    public bool isGrown;
    public override void UseItem()
    {
        Plant();
    }
        private void Plant()
    {
        // Find the player
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("No GameObject with the Player tag was found.");
            return;
        }

        // Snap the player's position to the grid
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(playerTransform.position.x / gridSize) * gridSize,
            Mathf.Round(playerTransform.position.y / gridSize) * gridSize
        );

        // Check if a dirt tile already exists at this grid cell
        Dirt[] existingDirt = FindObjectsByType<Dirt>(
            FindObjectsSortMode.None
        );

        foreach (Dirt dirt in existingDirt)
        {
             Vector2 dirtPosition = dirt.transform.position;

            if (Vector2.Distance(dirtPosition, snappedPosition) < 0.01f)
            {
                
               if ( dirt.hasSeed == true)
            {
                Debug.Log("There is already a seed in this dirt!");
                return;
            } 
                dirt.Plant(ID, growthDays);
                Debug.Log("Seed planted ID -" + ID );
                return;
            }
           
        }
        
    }
      public void SetPlantedDay()
    {
        plantedDay = TimeManager.Day;
        isGrown = false;

        Debug.Log("Seed planted on day " + plantedDay);
    }
        private void OnEnable()
    {
        TimeManager.onDayChanged += CheckGrowth;
    }

    private void OnDisable()
    {
        TimeManager.onDayChanged -= CheckGrowth;
    }

    private void CheckGrowth()
    {
        daysPassed = TimeManager.Day - plantedDay;

        if (daysPassed >= growthDays)
        {
            isGrown = true;
            
            Debug.Log("Seed has grown!");
        }
    }
        
    }
  

   