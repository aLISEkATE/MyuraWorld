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

    public static InventoryController Instance {get; private set;}
    //public static List<GameObject> inventoryItems = new List<GameObject>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        itemDictionary = FindFirstObjectByType<ItemDictionary>(); 
    }
   
    private void Start()
    {
        // Create empty inventory slots.
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(
                slotPrefab,
                inventoryPanel.transform
            ).GetComponent<Slot>();

            slot.currentItem = null;
        }
    }

    // public int GetItemCount(int itemID)
    // {
    // int count = 0;

    // foreach (GameObject itemObject in inventoryItems)
    // {
    //     Item item = itemObject.GetComponent<Item>();

    //     if (item != null && item.ID == itemID)
    //     {
    //         count++;
    //     }
    // }

    // return count;
    // }
    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null &&  slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if(slotItem != null && slotItem.ID == itemToAdd.ID)
                {
                    slotItem.AddToStack();
                    return true;
                }

           
               // inventoryItems.Add(newItem);
            }
        }

        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null &&  slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
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
                invData.Add(new InventorySaveData{ 
                    itemID =item.ID, 
                    slotIndex = slotTransform.GetSiblingIndex(), 
                    quantity = item.quantity
                    });
                Debug.Log("Inventory data: " + item.ID + " " + slotTransform.GetSiblingIndex() + " " + item.quantity);
            } 
        }

         return invData;
    }

   public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
{
    // Remove all existing slots
    for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
    {
        DestroyImmediate(inventoryPanel.transform.GetChild(i).gameObject);
    }

    // Create fresh slots
    for (int i = 0; i < slotCount; i++)
    {
        Instantiate(slotPrefab, inventoryPanel.transform);
    }

    // Load saved items
    foreach (InventorySaveData data in inventorySaveData)
    {
        if (data.slotIndex >= slotCount)
            continue;

        Slot slot = inventoryPanel.transform
            .GetChild(data.slotIndex)
            .GetComponent<Slot>();

        GameObject itemPrefab =
            itemDictionary.GetItemPrefab(data.itemID);

        if (itemPrefab == null)
            continue;

        GameObject item =
            Instantiate(itemPrefab, slot.transform);

        item.GetComponent<RectTransform>().anchoredPosition =
            Vector2.zero;

        Item itemComponent =
            item.GetComponent<Item>();

        if (itemComponent != null)
        {  
            itemComponent.quantity = data.quantity;
            itemComponent.UpdateQuantityDisplay();
            
        }
         slot.currentItem = item;
    }
}
}
