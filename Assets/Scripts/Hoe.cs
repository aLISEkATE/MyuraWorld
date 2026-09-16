using UnityEngine;

public class Hoe : Tool
{
    [Header("Dirt Settings")]
    public GameObject dirtPrefab;

    [SerializeField]
    private float gridSize = 1f;

    private static int nextDirtID = 0;

    public static void SetNextDirtID(int nextID)
    {
        nextDirtID = nextID;
    }

    public override void UseItem()
    {
        PlaceDirt();
    }

    private void PlaceDirt()
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
                Debug.Log("There is already dirt on this grid cell!");
                return;
            }
        }

        // No dirt exists here, so create it
        GameObject newDirt = Instantiate(
            dirtPrefab,
            snappedPosition,
            Quaternion.identity
        );

        // Get the Dirt component
        Dirt newDirtComponent = newDirt.GetComponent<Dirt>();

        if (newDirtComponent != null)
        {
            newDirtComponent.SetID(nextDirtID);

            Debug.Log(
                "Created dirt with ID: " +
                newDirtComponent.GetID() +
                " at grid position: " +
                snappedPosition
            );

            nextDirtID++;
        }
        else
        {
            Debug.LogError(
                "The dirtPrefab does not have a Dirt component!"
            );
        }
    }
}

