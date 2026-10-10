using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingScript : MonoBehaviour, IPointerClickHandler
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Recipe UI")]
    [SerializeField] private RecipeItemUI recipeItemPrefab;

    private Recipe[] recipes;

    private void Start()
    {
       
        recipes = GetComponentsInChildren<Recipe>(true);
        Debug.Log("Found " + recipes.Length + " recipes.");

        if (inventoryPanel == null)
            Debug.LogError("CraftingScript: Inventory Panel is not assigned!", this);

        if (recipeItemPrefab == null)
            Debug.LogError("CraftingScript: Recipe Item UI prefab is not assigned!", this);

       
        RecipeUI[] recipeUIs = GetComponentsInChildren<RecipeUI>(true);

        foreach (RecipeUI recipeUI in recipeUIs)
        {
            recipeUI.Initialize(recipeItemPrefab, this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Recipe clickedRecipe = null;

        if (eventData.pointerPress != null)
        {
            clickedRecipe = eventData.pointerPress.GetComponent<Recipe>();
        }

        if (clickedRecipe == null && eventData.pointerEnter != null)
        {
            clickedRecipe =
                eventData.pointerEnter.GetComponentInParent<Recipe>();
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
            Debug.LogError(
                "CheckInventory: Recipe input is null!",
                recipe
            );
            return false;
        }

        foreach (ItemTypeAndCount required in recipe.input)
        {
            if (required == null || required.item == null)
            {
                Debug.LogError(
                    "Recipe contains an invalid ingredient!",
                    recipe
                );
                return false;
            }

            int amountFound = GetInventoryAmount(required.item);

            if (amountFound < required.count)
            {
                return false;
            }
        }

        return true;
    }


    public int GetInventoryAmount(Item item)
    {
        if (item == null || inventoryPanel == null)
            return 0;

        int amountFound = 0;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item inventoryItem =
                slot.currentItem.GetComponent<Item>();

            if (inventoryItem == null)
                continue;

            if (inventoryItem.ID == item.ID)
            {
                amountFound += inventoryItem.quantity;
            }
        }

        return amountFound;
    }

 

    private void Craft(Recipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogError("Craft: Recipe is null!");
            return;
        }

        Debug.Log("Crafting: " + recipe.recipeName);

        // Remove ingredients.
        if (recipe.input != null)
        {
            foreach (ItemTypeAndCount required in recipe.input)
            {
                if (required == null || required.item == null)
                    continue;

                RemoveItem(
                    required.item.ID,
                    required.count
                );
            }
        }

        // Add outputs.
        if (recipe.output != null)
        {
            foreach (ItemTypeAndCount output in recipe.output)
            {
                if (output == null || output.item == null)
                    continue;

                AddItem(
                    output.item,
                    output.count
                );
            }
        }
    }



    private void RemoveItem(int itemID, int amount)
    {
        if (inventoryPanel == null)
        {
            Debug.LogError(
                "RemoveItem: Inventory Panel is null!",
                this
            );
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

            Item item =
                slot.currentItem.GetComponent<Item>();

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
            "RemoveItem: Could not remove the full amount of item ID "
            + itemID
        );
    }



    private void AddItem(Item itemPrefab, int amount)
    {
        if (inventoryPanel == null)
        {
            Debug.LogError(
                "AddItem: Inventory Panel is null!",
                this
            );
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("AddItem: Item prefab is null!");
            return;
        }

        if (amount <= 0)
            return;

        // First try to add to an existing stack.
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item existingItem =
                slot.currentItem.GetComponent<Item>();

            if (existingItem == null)
                continue;

            if (existingItem.ID == itemPrefab.ID)
            {
                existingItem.AddToStack(amount);
                return;
            }
        }

        // Otherwise find an empty slot.
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (slotTransform == null)
                continue;

            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem != null)
                continue;

            GameObject newItem =
                itemPrefab.CloneItem(
                    amount,
                    slotTransform
                );

            if (newItem == null)
            {
                Debug.LogError(
                    "AddItem: Failed to clone item!",
                    this
                );
                return;
            }

            RectTransform itemRect =
                newItem.GetComponent<RectTransform>();

            if (itemRect != null)
            {
                itemRect.anchoredPosition = Vector2.zero;
                itemRect.localRotation = Quaternion.identity;
            }

            slot.currentItem = newItem;
            return;
        }

        Debug.LogWarning(
            "AddItem: No empty inventory slot available."
        );
    }
}