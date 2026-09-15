
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public GameObject dirtPrefab;

    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(
            Application.persistentDataPath,
            "saveData.json"
        );

        inventoryController = FindFirstObjectByType<InventoryController>();
        hotbarController = FindFirstObjectByType<HotbarController>();

        LoadGame();
    }

    public void SaveGame()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Find all dirt objects
        GameObject[] dirtObjects =
            GameObject.FindGameObjectsWithTag("Dirt");

        // Store dirt positions
        List<Vector3> dirtPositions = new List<Vector3>();

        foreach (GameObject dirt in dirtObjects)
        {
            dirtPositions.Add(dirt.transform.position);
        }

        // Create the SaveData object
        SaveData saveData = new SaveData();

        // Store player position
        saveData.playerPosition = player.transform.position;

        // Store dirt positions
        saveData.dirtPositions = dirtPositions;

        // Store inventory
        saveData.inventorySaveData =
            inventoryController.GetInventoryItems();

        // Store hotbar
        saveData.hotbarSaveData =
            hotbarController.GetHotbarItems();

        // Convert SaveData to JSON and save it
        File.WriteAllText(
            saveLocation,
            JsonUtility.ToJson(saveData, true)
        );

        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData =
                JsonUtility.FromJson<SaveData>(
                    File.ReadAllText(saveLocation)
                );

            // Load player position
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            player.transform.position =
                saveData.playerPosition;

            // Load dirt
            foreach (Vector3 position in saveData.dirtPositions)
            {
                Instantiate(
                    dirtPrefab,
                    position,
                    Quaternion.identity
                );
            }

            // Load inventory
            inventoryController.SetInventoryItems(
                saveData.inventorySaveData
            );

            // Load hotbar
            hotbarController.SetHotbarItems(
                saveData.hotbarSaveData
            );

            Debug.Log("Game loaded!");
        }
        else
        {
            Debug.Log("No save file found. Creating new save.");

            SaveGame();
        }
    }
}

