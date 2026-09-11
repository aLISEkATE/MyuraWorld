using UnityEngine;
using UnityEngine.InputSystem;

public class HoeScript : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject dirtPrefab;
    public GameObject hoePrefab;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && IsHoeInUse())
        {
            PlaceDirt();
        }
    }

    private bool IsHoeInUse()
    {
        if (hotbarPanel == null || hoePrefab == null)
        {
            return false;
        }

        foreach (Transform child in hotbarPanel.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null && slot.currentItem == hoePrefab)
            {
                Debug.Log("Hoe is in use");
                return true;            
            }
        }

        return false;
    }

    public void PlaceDirt()
    {        
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (dirtPrefab == null || playerTransform == null)
        {   
            Debug.LogError("Assign dirtPrefab and tag the player as Player.");
            return;
            
        }
        Vector3 positionUnderPlayer = playerTransform.position + Vector3.down;
        Instantiate(dirtPrefab, positionUnderPlayer, Quaternion.identity);
    }
}
