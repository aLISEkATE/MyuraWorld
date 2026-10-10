using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{   
    
    public GameObject dirtPrefab;
    public GameObject placeablePrefab;

    private string saveLocation;

    private InventoryController inventoryController;
    private HotbarController hotbarController;

    
    private void Start()
    {   Debug.Log("SaveController Start is running!");
        saveLocation = Path.Combine(
            Application.persistentDataPath,
            "saveData.json"
        );
        inventoryController =
            FindFirstObjectByType<InventoryController>();

        hotbarController =
            FindFirstObjectByType<HotbarController>();

        Debug.Log(
            "CURRENT SCENE: " +
            UnityEngine.SceneManagement.SceneManager
                .GetActiveScene().name
        );
            if (UnityEngine.SceneManagement.SceneManager
    .GetActiveScene().name == "SampleScene")
        {
            Debug.Log("Starting LoadGame...");
            LoadGame();
        }
        else
        {
            Debug.LogWarning("Not in SampleScene. Save not loaded.");
        }
       
    }

    public void SaveGame()
    {     Debug.Log("SAVE BUTTON PRESSED!");
        // Find player
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        GameObject[] dirtObjects =
           GameObject.FindGameObjectsWithTag("Dirt");
        GameObject[] placeableObjects =
           GameObject.FindGameObjectsWithTag("Placeable");
    
        Debug.Log("Found " + dirtObjects.Length + " dirt objects.");
        Debug.Log("Found " + placeableObjects.Length + " placeable objects.");

        List<DirtSaveData> dirtData =
            new List<DirtSaveData>();
        List<PlaceableSaveData> placeableData =
            new List<PlaceableSaveData>();

                foreach (GameObject dirtObject in dirtObjects)
                {
                    Debug.Log("Checking dirt object: " + dirtObject.name);

                    Dirt dirt =
                        dirtObject.GetComponent<Dirt>();

                        if (dirt != null)
                        {
                            Debug.Log(
                                "Saving Dirt ID: " +
                                dirt.GetID()
                            );

                            DirtSaveData data =
                                new DirtSaveData();

                            data.dirtID = dirt.GetID();
                            data.position = dirt.transform.position;
                            data.isWatered = dirt.isWatered;
                            data.hasSeed = dirt.hasSeed;
                            data.seedID = dirt.seedID;
                            data.daysPassed = dirt.daysPassed;
                            data.growthDays = dirt.growthDays;
                            data.isGrown = dirt.isGrown;

                            dirtData.Add(data);
                        }
                        else
                        {
                            Debug.LogWarning(
                                dirtObject.name +
                                " has the Dirt tag but no Dirt component!"
                            );
                        }
                }

        Debug.Log(
            "Total dirt saved: " +
            dirtData.Count
        );
            foreach (GameObject placeableObject in placeableObjects)
                {
                    Debug.Log("Checking placeable object: " + placeableObject.name);

                    Placeable placeable =
                        placeableObject.GetComponent<Placeable>();

                        if (placeable != null)
                        {
                            Debug.Log(
                                "Saving placable ID: " +
                                placeable.GetID()
                            );

                            PlaceableSaveData data =
                                new PlaceableSaveData();

                            data.placeableID = placeable.GetID();
                            data.position = placeable.transform.position;
      

                            placeableData.Add(data);
                        }
                        else
                        {
                            Debug.LogWarning(
                                placeableObject.name +
                                " has the Placeable tag but no Dirt component!"
                            );
                        }
                }

        Debug.Log(
            "Total placeables saved: " +
            placeableData.Count
        );

        // Create SaveData
        SaveData saveData =
            new SaveData();

        saveData.playerPosition =
            player.transform.position;

        saveData.dirtData =
            dirtData;

        saveData.placeableData =
            placeableData;

        saveData.inventorySaveData =
            inventoryController.GetInventoryItems();

        saveData.hotbarSaveData =
            hotbarController.GetHotbarItems();

        saveData.day = 
            TimeManager.Day;

        saveData.hour = 
            TimeManager.Hour;

        saveData.minute = 
            TimeManager.Minute;

        // Convert to JSON and save
        string json =
            JsonUtility.ToJson(saveData, true);

        File.WriteAllText(
            saveLocation,
            json
        );

        Debug.Log("Game saved!");
    }

    public void LoadGame()
{
    if (!File.Exists(saveLocation))
    {
        Debug.Log("No save file found. Starting fresh.");
        return;
    }

    string json = File.ReadAllText(saveLocation);

    if (string.IsNullOrWhiteSpace(json))
    {
        Debug.LogWarning("Save file is empty. Starting fresh.");
        return;
    }

    SaveData saveData = JsonUtility.FromJson<SaveData>(json);

    if (saveData == null)
    {
        Debug.LogError("Save data could not be loaded.");
        return;
    }
    TimeManager timeManager = FindFirstObjectByType<TimeManager>();

    if (timeManager != null)
    {
        timeManager.SetTime(
            saveData.day,
            saveData.hour,
            saveData.minute
        );
    }

    GameObject player =
        GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position =
                saveData.playerPosition;
        }


            if (saveData.dirtData != null)
            {
            int highestDirtID = -1;

            foreach (DirtSaveData data in saveData.dirtData)
            {
                GameObject newDirt = Instantiate(
                    dirtPrefab,
                    data.position,
                    Quaternion.identity
                );

                Dirt dirt = newDirt.GetComponent<Dirt>();

                if (dirt != null)
                {
                    dirt.SetID(data.dirtID);
                    dirt.isWatered = data.isWatered;
                    dirt.hasSeed = data.hasSeed;
                    dirt.seedID = data.seedID;
                    dirt.daysPassed = data.daysPassed;
                    dirt.growthDays = data.growthDays;
                    dirt.isGrown = data.isGrown;
                    if (data.dirtID > highestDirtID)
                        highestDirtID = data.dirtID;
                }
                else
                {
                    Debug.LogError("Loaded dirt prefab does not have a Dirt component!");
                }
            }

            Hoe.SetNextDirtID(highestDirtID + 1);
            
            }

         if (saveData.placeableData!= null)
            {
            int highestPlaceableID = -1;

            foreach (PlaceableSaveData data in saveData.placeableData)
            {
                GameObject newPlaceable = Instantiate(
                    placeablePrefab,
                    data.position,
                    Quaternion.identity
                );

                Placeable placeable = newPlaceable.GetComponent<Placeable>();

                if (placeable != null)
                {
                    placeable.SetID(data.placeableID);
                    placeable.transform.position = data.position;
                   
                    if (data.placeableID > highestPlaceableID)
                        highestPlaceableID = data.placeableID;
                }
                else
                {
                    Debug.LogError("Loaded placeable prefab does not have a Dirt component!");
                }
            }

            PlaceableItem.SetNextPlaceableID(highestPlaceableID + 1);
            
            }

        if (saveData.inventorySaveData != null)
        {
            inventoryController.SetInventoryItems(
                saveData.inventorySaveData
            );
        }


        if (saveData.hotbarSaveData != null)
        {
            hotbarController.SetHotbarItems(
                saveData.hotbarSaveData
            );
        }

        Debug.Log("Game loaded!");
    }

    private Dirt FindDirtByID(int id)
    {
        Dirt[] allDirt =
            FindObjectsByType<Dirt>(
                FindObjectsSortMode.None
            );

        foreach (Dirt dirt in allDirt)
        {
            if (dirt.GetID() == id)
            {
                return dirt;
            }
        }

        return null;
    }
    private Placeable FindPlaceableByID(int id)
    {
        Placeable[] allPlaceable =
            FindObjectsByType<Placeable>(
                FindObjectsSortMode.None
            );

        foreach (Placeable placeable in allPlaceable)
        {
            if (placeable.GetID() == id)
            {
                return placeable;
            }
        }

        return null;
    }
}

