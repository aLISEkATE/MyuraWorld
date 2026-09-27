using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject inventoryPanel;

    private Recipe[] recipes;

    private void Awake()
    {
        recipes = GetComponentsInChildren<Recipe>(true);

        Debug.Log("Found " + recipes.Length + " recipes.");

        if (inventoryPanel == null)
        {
            Debug.LogError("CraftingScript: Inventory Panel is not assigned!", this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Find which Recipe was clicked
        Recipe clickedRecipe = eventData.pointerPress != null
            ? eventData.pointerPress.GetComponent<Recipe>()
            : null;

        if (clickedRecipe == null)
        {
            clickedRecipe = eventData.pointerEnter != null
                ? eventData.pointerEnter.GetComponentInParent<Recipe>()
                : null;
        }

        if (clickedRecipe == null)
        {
            Debug.LogWarning("Clicked object is not a Recipe.", this);
            return;
        }

        if (CheckInventory(clickedRecipe))
        {
            Craft(clickedRecipe);
        }
    }

    public bool CheckInventory(Recipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogError("CheckInventory: Recipe is null!");
            return false;
        }

        if (recipe.input == null)
        {
            Debug.LogError("CheckInventory: Recipe input is null!", recipe);
            return false;
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("CheckInventory: Inventory Panel is null!", this);
            return false;
        }

        foreach (ItemTypeAndCount required in recipe.input)
        {
            if (required == null)
            {
                Debug.LogError("Recipe contains a null ingredient!", recipe);
                return false;
            }

            if (required.item == null)
            {
                Debug.LogError("Recipe ingredient has no Item assigned!", recipe);
                return false;
            }

            int amountFound = 0;

            foreach (Transform slotTransform in inventoryPanel.transform)
            {
                if (slotTransform == null)
                    continue;

                Slot slot = slotTransform.GetComponent<Slot>();

                if (slot == null || slot.currentItem == null)
                    continue;

                Item item = slot.currentItem.GetComponent<Item>();

                if (item == null)
                    continue;

                if (item.ID == required.item.ID)
                {
                    amountFound += item.quantity;
                }
            }

            if (amountFound < required.count)
            {
                return false;
            }
        }

        return true;
    }

    private void Craft(Recipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogError("Craft: Recipe is null!");
            return;
        }

        Debug.Log("Crafting: " + recipe.recipeName);

        if (recipe.input != null)
        {
            // Remove ingredients
            foreach (ItemTypeAndCount required in recipe.input)
            {
                if (required == null || required.item == null)
                    continue;

                RemoveItem(required.item.ID, required.count);
            }
        }

        if (recipe.output != null)
        {
            // Add crafted items
            foreach (ItemTypeAndCount output in recipe.output)
            {
                if (output == null || output.item == null)
                    continue;

                AddItem(output.item, output.count);
            }
        }
    }

    private void RemoveItem(int itemID, int amount)
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("RemoveItem: Inventory Panel is null!", this);
            return;
        }

        if (amount <= 0)
            return;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item item = slot.currentItem.GetComponent<Item>();

            if (item == null || item.ID != itemID)
                continue;

            int removed = item.RemoveFromStack(amount);
            amount -= removed;

            if (item.quantity <= 0)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }

            if (amount <= 0)
                return;
        }

        Debug.LogWarning(
            "RemoveItem: Could not remove the full amount of item ID " + itemID
        );
    }

    private void AddItem(Item itemPrefab, int amount)
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("AddItem: Inventory Panel is null!", this);
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("AddItem: Item prefab is null!");
            return;
        }

        if (amount <= 0)
            return;

        // First try to find an existing stack
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item existingItem = slot.currentItem.GetComponent<Item>();

            if (existingItem == null)
                continue;

            if (existingItem.ID == itemPrefab.ID)
            {
                existingItem.AddToStack(amount);
                return;
            }
        }

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem != null)
                continue;

            GameObject newItem = itemPrefab.CloneItem(amount, slotTransform);


            if (newItem == null)
            {
                Debug.LogError("AddItem: Failed to clone item!", this);
                return;
            }

            RectTransform itemRect = newItem.GetComponent<RectTransform>();

            if (itemRect != null)
            {
                itemRect.anchoredPosition = Vector2.zero;
                itemRect.localRotation = Quaternion.identity;
            }

            slot.currentItem = newItem;
            return;
               
        }
    }
}


