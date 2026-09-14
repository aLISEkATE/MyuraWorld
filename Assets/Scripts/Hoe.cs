using UnityEngine;

public class Hoe : Tool
{
    public GameObject dirtPrefab;

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

        Instantiate(
            dirtPrefab,
            positionUnderPlayer,
            Quaternion.identity
        );
    }
}