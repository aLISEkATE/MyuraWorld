using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{   
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    public GameObject[] seedPrefabs;

    public static List<GameObject> inventoryItems = new List<GameObject>();


    void Awake()
    {   
        itemDictionary = FindFirstObjectByType<ItemDictionary>(); 
    }

    public int GetItemCount(int itemID)
    {
    int count = 0;

    foreach (GameObject itemObject in inventoryItems)
    {
        Item item = itemObject.GetComponent<Item>();

        if (item != null && item.ID == itemID)
        {
            count++;
        }
    }

    return count;
    }
    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null &&  slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;

                 // Add the item to the inventory list
                inventoryItems.Add(newItem);
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }
    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData{ itemID =item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }

         return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach(Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        
        foreach(InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;

                    inventoryItems.Add(item);
                    // inventoryItems.Add(item);
    
                }
            }
        }
    }
}
