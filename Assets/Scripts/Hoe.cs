using UnityEngine;

public class Hoe : Tool
{
    public GameObject dirtPrefab;

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

        Vector2 positionUnderPlayer = playerTransform.position;

        GameObject newDirt = Instantiate(
            dirtPrefab,
            positionUnderPlayer,
            Quaternion.identity
        );

        Dirt dirt = newDirt.GetComponent<Dirt>();

        if (dirt != null)
        {
            dirt.SetID(nextDirtID);

            Debug.Log("Created dirt with ID: " + dirt.GetID());

            nextDirtID++;
        }
        else
        {
            Debug.LogError("The dirtPrefab does not have a Dirt component!");
        }
    }
}