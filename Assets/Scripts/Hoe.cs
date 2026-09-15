using UnityEngine;

public class Hoe : Tool
{
    public GameObject dirtPrefab;

    [SerializeField] private float gridSize = 1f;

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
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")?.transform;

        if (dirtPrefab == null || playerTransform == null)
        {
            Debug.LogError("Assign dirtPrefab and tag the player as Player.");
            return;
        }

        // Snap the player's position to the grid
        Vector2 snappedPosition = new Vector2(
            Mathf.Round(playerTransform.position.x / gridSize) * gridSize,
            Mathf.Round(playerTransform.position.y / gridSize) * gridSize
        );

        // Check if something is already occupying this grid cell
        Collider2D existingCollider = Physics2D.OverlapBox(
            snappedPosition,
            new Vector2(gridSize, gridSize),
            0f
        );

        if (existingCollider != null)
        {
            // Only prevent placement if the collider belongs to dirt
            Dirt existingDirt = existingCollider.GetComponent<Dirt>();

            if (existingDirt != null)
            {
                Debug.Log("There is already dirt here!");
                return;
            }
        }

        // Create the dirt at the snapped position
        GameObject newDirt = Instantiate(
            dirtPrefab,
            snappedPosition,
            Quaternion.identity
        );

        Dirt dirt = newDirt.GetComponent<Dirt>();

        if (dirt != null)
        {
            dirt.SetID(nextDirtID);

            Debug.Log(
                "Created dirt with ID: " +
                dirt.GetID() +
                " at " +
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

