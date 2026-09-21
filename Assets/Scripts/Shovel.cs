using UnityEngine;

public class Shovel : Tool
{
    [Header("Dirt Settings")]
    public GameObject dirtPrefab;

    [SerializeField]
    private float gridSize = 1f;

   
    public override void UseItem()
    {
        RemoveDirt();
    }

    private void RemoveDirt()
    {
        // Find the player
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")?.transform;

        if (dirtPrefab == null)
        {
            Debug.LogError("Assign a dirtPrefab to the Hoe.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("No GameObject with the Player tag was found.");
            return;
        }

        // Snap the player's position to the grid
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(playerTransform.position.x / gridSize) * gridSize,
            Mathf.Round(playerTransform.position.y / gridSize) * gridSize
        );

        // Check if a dirt tile already exists at this grid cell
        Dirt[] existingDirt = FindObjectsByType<Dirt>(
            FindObjectsSortMode.None
        );

        foreach (Dirt dirt in existingDirt)
        {
            Vector2 dirtPosition = dirt.transform.position;

            // Compare the dirt's grid position with the position
            // where we are trying to place new dirt.
            if (Vector2.Distance(dirtPosition, snappedPosition) < 0.01f)
            {

                Destroy(dirt.gameObject);
                Debug.Log("Dirt removed.");
                return;
            }
        }
    }
}


