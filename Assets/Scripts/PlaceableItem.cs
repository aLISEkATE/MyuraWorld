using UnityEngine;

public class PlaceableItem : MonoBehaviour, IUse
{
    [Header("Placement Configuration")]
    public GameObject placedPrefab; 

   
    public void UseItem()
    {
        if (PlacementManager.Instance != null)
        {
            bool success = PlacementManager.Instance.TryPlaceObject();
            if (success)
            {
               
                Debug.Log("Item placed cleanly onto grid map.");
            }
        }
    }


    public void OnSelected()
    {
        if (PlacementManager.Instance != null)
        {
            PlacementManager.Instance.StartPlacementPreview(this);
        }
    }


    public void OnDeselected()
    {
        if (PlacementManager.Instance != null)
        {
            PlacementManager.Instance.ClearPreview();
        }
    }
}