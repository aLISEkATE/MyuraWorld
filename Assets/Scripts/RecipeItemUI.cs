using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    private Image icon;
    private TextMeshProUGUI amount;

    private void Awake()
    {
        icon = GetComponent<Image>();
        amount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
    }

   public void Setup(ItemTypeAndCount ingredient, int inventoryAmount)
{
    if (ingredient == null)
    {
        Debug.LogError("RecipeItemUI: Ingredient is null!", this);
        return;
    }

    if (ingredient.item == null)
    {
        Debug.LogError("RecipeItemUI: Ingredient has no Item assigned!", this);
        return;
    }

    // Get the icon from the actual Item
    icon.sprite = ingredient.item.GetIcon();

    // Show required amount
    amount.text = ingredient.count.ToString();

    // Red if player doesn't have enough
    if (inventoryAmount < ingredient.count)
    {
        amount.color = Color.red;
    }
    else
    {
        amount.color = Color.white;
    }
}
}