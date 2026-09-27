using UnityEngine;

public class RecipeUI : MonoBehaviour
{
    public GameObject RecipeContent;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = RecipeContent.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = RecipeContent.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        RecipeContent.SetActive(false);
    }

    public void ShowRecipe()
    {
        RecipeContent.SetActive(true);

        // Slightly see-through
        canvasGroup.alpha = 0.85f;
    }

    public void HideRecipe()
    {
        RecipeContent.SetActive(false);
    }

    public void ToggleRecipe()
    {
        if (RecipeContent.activeSelf)
            HideRecipe();
        else
            ShowRecipe();
    }
}