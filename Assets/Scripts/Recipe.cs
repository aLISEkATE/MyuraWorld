using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public string recipeName;

    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;


    //get component gameobject recipe content
    //tracks which items are listed in recipe input
    //displays them in recipe content window but only shows the icon(if thats somehow possible)
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

} 

    
    


    [System.Serializable]
    public class ItemTypeAndCount
    {
    public Item item;
    public int count;

    public ItemTypeAndCount(Item i, int c)
    {
        item = i;
        count = c;
    }



}