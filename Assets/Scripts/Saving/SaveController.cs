using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{   
    
    public GameObject dirtPrefab;

    private string saveLocation;

    private InventoryController inventoryController;
    private HotbarController hotbarController;

    private void Start()
    {
        saveLocation = Path.Combine(
            Application.persistentDataPath,
            "saveData.json"
        );

        inventoryController =
            FindFirstObjectByType<InventoryController>();

        hotbarController =
            FindFirstObjectByType<HotbarController>();

        LoadGame();
    }

    public void SaveGame()
    {
        // Find player
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        GameObject[] dirtObjects =
           GameObject.FindGameObjectsWithTag("Dirt");

        Debug.Log("Found " + dirtObjects.Length + " dirt objects.");

        List<DirtSaveData> dirtData =
            new List<DirtSaveData>();

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

        // Create SaveData
        SaveData saveData =
            new SaveData();

        saveData.playerPosition =
            player.transform.position;

        saveData.dirtData =
            dirtData;

        saveData.inventorySaveData =
            inventoryController.GetInventoryItems();

        saveData.hotbarSaveData =
            hotbarController.GetHotbarItems();

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
            Debug.Log("No save file found.");

            return;
        }

        string json =
            File.ReadAllText(saveLocation);

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(json);      

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
}

