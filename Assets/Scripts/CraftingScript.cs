using System.Runtime.CompilerServices;
using UnityEngine;

public class CraftingScript : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Recipe recipe;

    public void CheckInventory()
    {
        // Check every ingredient required by the recipe
        foreach (ItemTypeAndCount required in recipe.input)
        {
            int amountFound = 0;

            // Look through every inventory slot
            foreach (Transform slotTransform in inventoryPanel.transform)
            {
                Slot slot = slotTransform.GetComponent<Slot>();

                if (slot == null || slot.currentItem == null)
                    continue;

                Item item = slot.currentItem.GetComponent<Item>();

                if (item == null)
                    continue;

                // Is this the item we're looking for?
                if (item == required.item)
                {
                    amountFound += item.quantity;
                }
            }

            // Not enough of this ingredient
            if (amountFound < required.count)
            {
                Debug.Log(
                    "Not enough " + required.item.Name +
                    ". Required: " + required.count +
                    ", Found: " + amountFound
                );

                return;
            }
        }

        // If we reached this point, every ingredient is available
        Debug.Log("All ingredients available!");

        Craft();
    }

    private void Craft()
    {
        Debug.Log("Crafting " + recipe.recipeName);

        // We'll remove the ingredients and add the output here.
    }
}
    //get component gameobject recipe content
    //public bool HasEnough()
   // {
       // if GameObject(list) from InventoryController inventoryItems has equal or more of Recipe (public) ItemTypeAndCount input;
       //return true
       //else false
    //}
    //tracks which items are listed in recipe input
    //displays t}hem in recipe content window but only shows the icon(if thats somehow possible)
    //count is generated from recipe item(count)
    //
    // private showRecipeItems(){}
    //recipe contend is hidden my default
    //when user hovers over recipe
    //show recipe
    
    //private NotEnoughItems(){}
    //if inventory item count < input count
    //gameobject get component text mesh pro named Amount
    // red -> input count
    //display get component<gameobject> recipe content
    
    //private UseRecipe(){}
    //when user lmb clicks on recipe
    //if inventory item count >= input count
    //CraftingScript.craft();
    //else return;

