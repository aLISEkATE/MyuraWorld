using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            InventoryController inventoryController = FindFirstObjectByType<InventoryController>();
            if (inventoryController != null)
            {
                itemPrefabs = new List<Item>();
                foreach (GameObject itemPrefab in inventoryController.itemPrefabs)
                {
                    if (itemPrefab != null)
                    {
                        Item item = itemPrefab.GetComponent<Item>();
                        if (item != null)
                        {
                            itemPrefabs.Add(item);
                        }
                    }
                }
            }
        }

        for(int i = 0; i< itemPrefabs.Count; i++)
        {
         if(itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }   
        }

        foreach (Item item in itemPrefabs)
        {
            if (item != null)
            {
                itemDictionary[item.ID] = item.gameObject;
            }
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found");
        }
        return prefab;
    }
    // Update is called once per frame
 
}
