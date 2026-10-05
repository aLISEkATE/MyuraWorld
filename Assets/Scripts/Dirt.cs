using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;

    public bool isWatered;
    public bool hasSeed;
    public int seedID;
    public GameObject plantPrefab;

    [Header("Seed Growth")]
    public int daysPassed = 0;
    public int growthDays = 0;
    public bool isGrown = false;
    private Seed seed;

    public int GetID()
    {
        return ID;
    }

    public void SetID(int id)
    {
        ID = id;
    }

    void Update()
    {
        SpawnPlant();
    }
    private void OnEnable()
    {
        TimeManager.onDayChanged += UpdateSeedGrowth;
    }

    private void OnDisable()
    {
        TimeManager.onDayChanged -= UpdateSeedGrowth;
    }

    private void UpdateSeedGrowth()
{
    // If there is a seed, check whether it was watered
    if (hasSeed && !isGrown)
    {
        if (isWatered)
        {
            daysPassed++;

            Debug.Log(
                $"Seed {seedID}: {daysPassed}/{growthDays} days passed"
            );

            if (daysPassed >= growthDays)
            {
                isGrown = true;
                SpawnPlant();
                Debug.Log(
                    $"Seed {seedID} is now fully grown!"
                );
            }
        }
        else
        {
            Debug.Log(
                $"Seed {seedID} was not watered. Growth did not progress."
            );
        }
    }

    // Dirt dries out at the beginning of the new day
    isWatered = false;

    Debug.Log("Dirt dried out!");
}

    public void Water()
    {
        isWatered = true;

        Debug.Log("Dirt watered!");
    }

    public void Plant(int ID, int requiredGrowthDays, GameObject plantPrefab)
    {
        hasSeed = true;
        seedID = ID;

        daysPassed = 0;
        growthDays = requiredGrowthDays;
        isGrown = false;
        this.plantPrefab = plantPrefab;

        Debug.Log("Seed Planted!");
        Debug.Log("Growth required - " + growthDays + " days");
    }

    private void SpawnPlant()
    {   
         Dirt[] existingDirt = FindObjectsByType<Dirt>(
            FindObjectsSortMode.None
        );

        foreach(Dirt dirt in existingDirt)
        {
            if (isGrown && dirt.transform.childCount == 0)
            {
                Instantiate(plantPrefab, transform.position, Quaternion.identity, transform);
            }
        }
    }
}