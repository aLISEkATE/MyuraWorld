using UnityEngine;

public class PlaceableItem : Tool
{
    [Header("Placement Settings")]
    public GameObject placeablePrefab;

    [SerializeField]
    private float gridSize = 1f;

    private static int nextPlaceableID = 0;

    public static void SetNextPlaceableID(int nextID)
    {
        nextPlaceableID = nextID;
    }

    public override void UseItem()
    {
        PlaceItem();
    }

    private void PlaceItem()
    {
        // Find the player
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")?.transform;

        if (placeablePrefab == null)
        {
            Debug.LogError("Assign a placeablePrefab to the Hoe.");
            return;
        }

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

        // Check if something is already placed here
        Placeable[] existingPlaceables =
            FindObjectsByType<Placeable>(FindObjectsSortMode.None);

        foreach (Placeable placeable in existingPlaceables)
        {
            Vector2 placeablePosition = placeable.transform.position;

            if (Vector2.Distance(placeablePosition, snappedPosition) < 0.01f)
            {
                Debug.Log("There is already a Placeable on this grid cell!");
                return;
            }
        }

        // Place the object
        GameObject newPlaceable = Instantiate(
            placeablePrefab,
            snappedPosition,
            Quaternion.identity
        );

        // Get Placeable component
        Placeable newPlaceableComponent =
            newPlaceable.GetComponent<Placeable>();

        if (newPlaceableComponent == null)
        {
            Debug.LogError(
                "The placeablePrefab does not have a Placeable component!"
            );

            Destroy(newPlaceable);
            return;
        }

        // Give it an ID
        newPlaceableComponent.SetID(nextPlaceableID);

        Debug.Log(
            "Created Placeable with ID: " +
            newPlaceableComponent.GetID() +
            " at grid position: " +
            snappedPosition
        );

        nextPlaceableID++;

        // Only remove the inventory item AFTER successful placement
        RemoveOneFromInventory();
    }

    private void RemoveOneFromInventory()
    {
        // Find the Slot containing this Hoe
        Slot[] slots = FindObjectsByType<Slot>(
            FindObjectsSortMode.None
        );

        foreach (Slot slot in slots)
        {
            if (slot.currentItem == gameObject)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item == null)
                {
                    Debug.LogError(
                        "The Hoe inventory object does not have an Item component!"
                    );
                    return;
                }

                // More than one in the stack
                if (item.quantity > 1)
                {
                    item.RemoveFromStack(1);

                    Debug.Log(
                        "Removed 1 Hoe from stack. Remaining: " +
                        item.quantity
                    );
                }
                // Last item in stack
                else
                {
                    slot.currentItem = null;

                    Destroy(gameObject);

                    Debug.Log("Removed last Hoe from inventory.");
                }

                return;
            }
        }

        Debug.LogError(
            "Could not find a Slot containing this Hoe!"
        );
    }
}