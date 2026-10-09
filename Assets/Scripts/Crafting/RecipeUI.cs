using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Recipe recipe; 
    private Transform content;
    private RecipeItemUI recipeItemPrefab;
    private CraftingScript craftingScript;

    private void Awake()
    {
       
        
        content = transform.Find("Content");
        if (content == null)
        {
            Debug.LogError($"{name}: Could not find a child named 'Content'!", this);
            return;
        }
        content.gameObject.SetActive(false);
    }

  
    public void Initialize(RecipeItemUI prefab, CraftingScript crafting)
    {
        recipeItemPrefab = prefab;
        craftingScript = crafting;
        
   
    recipe = GetComponent<Recipe>(); 

    GenerateRecipeItems();
}
    private void GenerateRecipeItems()
{
    if (recipe == null)
    {
        Debug.LogError($"{name}: Recipe is missing!", this);
        return;
    }

    if (recipeItemPrefab == null)
    {
        Debug.LogError($"{name}: RecipeItemUI prefab is missing!", this);
        return;
    }

    if (content == null)
    {
        Debug.LogError($"{name}: Content is missing!", this);
        return;
    }

    if (recipe.input == null || recipe.input.Length == 0)
    {
        Debug.LogWarning($"{name}: Recipe has no ingredients.", this);
        return;
    }

   
    foreach (Transform child in content)
    {
        Destroy(child.gameObject);
    }

    Debug.Log(
        $"{name}: Generating {recipe.input.Length} recipe items."
    );


    foreach (ItemTypeAndCount recipeItem in recipe.input)
    {
        if (recipeItem == null)
        {
            Debug.LogWarning($"{name}: Found null recipe input.");
            continue;
        }

        if (recipeItem.item == null)
        {
            Debug.LogWarning(
                $"{name}: Recipe input has no Item assigned."
            );
            continue;
        }

        RecipeItemUI newItem =
            Instantiate(recipeItemPrefab, content);

        int inventoryAmount = 0;

        if (craftingScript != null)
        {
            inventoryAmount =
                craftingScript.GetInventoryAmount(
                    recipeItem.item
                );
        }

        newItem.Setup(
            recipeItem,
            inventoryAmount
        );

        Debug.Log(
            $"{name}: Created {recipeItem.item.name} x{recipeItem.count}"
        );
    }
}

  

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowRecipe();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideRecipe();
    }

    public void ShowRecipe()
    {
        if (content != null)
        {
            content.gameObject.SetActive(true);
        }
    }

    public void HideRecipe()
    {
        if (content != null)
        {
            content.gameObject.SetActive(false);
        }
    }
}