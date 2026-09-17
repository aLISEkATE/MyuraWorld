
using UnityEngine;

public class Seed : Tool
{
    [Header("Seed Settings")]
    [SerializeField]
    private float gridSize = 1f;

    public override void UseItem()
    {
        Plant();
    }

    private void Plant()
    {
        // Find the player
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("No GameObject with the Player tag was found.");
            return;
        }

        // Snap player's position to the grid
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(playerTransform.position.x / gridSize) * gridSize,
            Mathf.Round(playerTransform.position.y / gridSize) * gridSize
        );

        // Find all existing dirt
        Dirt[] existingDirt = FindObjectsByType<Dirt>(
            FindObjectsSortMode.None
        );

        // Look for dirt in this grid cell
        foreach (Dirt dirt in existingDirt)
        {
            Vector2 dirtPosition = dirt.transform.position;

            if (Vector2.Distance(dirtPosition, snappedPosition) < 0.01f)
            {
                // Plant in the dirt
                dirt.Plant();

                return;
            }
        }

        Debug.Log("No dirt found at this grid position.");
    }
}