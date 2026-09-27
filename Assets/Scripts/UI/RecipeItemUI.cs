using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;

    public void Setup(ItemTypeAndCount recipeItem, int inventoryAmount)
    {
        Debug.Log(
            $"Setting up RecipeItemUI: " +
            $"Item = {recipeItem?.item?.name}, " +
            $"Count = {recipeItem?.count}"
        );

        if (recipeItem == null)
        {
            Debug.LogError("RecipeItemUI: recipeItem is NULL!", this);
            return;
        }

        if (recipeItem.item == null)
        {
            Debug.LogError(
                "RecipeItemUI: recipeItem.item is NULL!",
                this
            );
            return;
        }


        if (icon == null)
        {
            Debug.LogError(
                "RecipeItemUI: Icon reference is NOT assigned!",
                this
            );
        }
        else
        {
            Sprite itemIcon = recipeItem.item.GetIcon();

            if (itemIcon == null)
            {
                Debug.LogError(
                    $"RecipeItemUI: {recipeItem.item.name} returned a NULL icon!",
                    this
                );
            }
            else
            {
                icon.sprite = itemIcon;
                icon.enabled = true;
            }
        }



        if (amount == null)
        {
            Debug.LogError(
                "RecipeItemUI: Amount reference is NOT assigned!",
                this
            );
        }
        else
        {
            amount.text = recipeItem.count.ToString();

            if (inventoryAmount < recipeItem.count)
            {
                amount.color = Color.red;
            }
            else
            {
                amount.color = Color.white;
            }
        }
    }
}