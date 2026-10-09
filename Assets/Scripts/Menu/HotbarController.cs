using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{

    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;

    public int currentSlotNumber;

    private ItemDictionary itemDictionary;

    private Key[] hotbarKeys;

    private void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        hotbarKeys = new Key[slotCount];
        for(int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

        // Update is called once per frame
        private void Update()
        {
            if (PauseController.IsGamePaused)
            {
                 return;
            }
               
            // Select hotbar slot
            for (int i = 0; i < slotCount; i++)
            {
                if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
                {
                    currentSlotNumber = i;
                }
            }

            // Use selected item
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                UseSelectedItem();
            }
        }
        public void UseSelectedItem()
        {
            if (PauseController.IsGamePaused)
            {
                 return;
            }

            Slot slot = hotbarPanel.transform
                .GetChild(currentSlotNumber)
                .GetComponent<Slot>();

            if (slot.currentItem == null)
            {
                Debug.Log("No item in selected slot.");
                return;
            }

            IUse usableItem = slot.currentItem.GetComponent<IUse>();

            if (usableItem != null)
            {
                usableItem.UseItem();
            }
            else
            {
                Debug.Log("Selected item cannot be used.");
            }
        }
    public List<HotbarSaveData> GetHotbarItems()
    {
        List<HotbarSaveData> hotbarData = new List<HotbarSaveData>();
        foreach(Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new HotbarSaveData{ itemID = item.ID, quantity = item.quantity, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }

         return hotbarData;
    }

 public void SetHotbarItems(List<HotbarSaveData> hotbarSaveData)
{
    // Remove all existing slots
    for (int i = hotbarPanel.transform.childCount - 1; i >= 0; i--)
    {
        DestroyImmediate(hotbarPanel.transform.GetChild(i).gameObject);
    }

    // Create fresh slots
    for (int i = 0; i < slotCount; i++)
    {
        Instantiate(slotPrefab, hotbarPanel.transform);
    }

    // Load saved items
    foreach (HotbarSaveData data in hotbarSaveData)
    {
        if (data.slotIndex >= slotCount)
            continue;

        Slot slot = hotbarPanel.transform
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

