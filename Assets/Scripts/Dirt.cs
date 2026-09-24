using UnityEngine;

public class Dirt : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int ID;

    public bool isWatered;
    public bool hasSeed;
    public int seedID;

    [Header("Seed Growth")]
    public int daysPassed = 0;
    public int growthDays = 0;
    public bool isGrown = false;

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
        TimeManager.onDayChanged += OnDayChanged;
    }

    private void OnDisable()
    {
        TimeManager.onDayChanged -= OnDayChanged;
    }

    private void OnDayChanged()
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

    public void Plant(int ID, int requiredGrowthDays)
    {
        hasSeed = true;
        seedID = ID;

    //    daysPassed = 0;
        growthDays = requiredGrowthDays;
        isGrown = false;

        Debug.Log("Seed Planted!");
        Debug.Log("Seed Received ID - " + ID);
        Debug.Log("Growth required - " + growthDays + " days");
    }
}