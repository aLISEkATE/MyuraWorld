using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerItemCollector : MonoBehaviour
{

    private InventoryController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        
    }

    void Update()
    {
         if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Mouse pressed");
                PickUpItem();
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Debug.Log("Picked up something");
                    Destroy(collision.gameObject);
                }
            }
        }
    }
   private void PickUpItem()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit == null)
            return;

        Debug.Log("Clicked: " + hit.gameObject.name);

        if (!hit.CompareTag("Clickable"))
            return;

        SpawnItem spawnItem = hit.GetComponent<SpawnItem>();

        if (spawnItem == null)
        {
            Debug.LogWarning("Clicked object has no SpawnItem component.");
            return;
        }

        bool itemAdded = inventoryController.AddItem(spawnItem.itemPrefab);

        if (itemAdded)
        {
            Debug.Log("Picked up something");
            Destroy(hit.gameObject);
        }
    }
    }

