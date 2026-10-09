using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 0.4f;
    public float maxDropDistance = 0.5f;

    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
        inventoryController = InventoryController.Instance;
    }
  
    public void OnBeginDrag(PointerEventData eventData)
    {
      originalParent = transform.parent;
      transform.SetParent(transform.root);
      canvasGroup.blocksRaycasts = false;
      canvasGroup.alpha = 0.6f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //Enables raycasts
        canvasGroup.alpha = 1f; //No longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //Slot where item dropped
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot == originalSlot)
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (dropSlot != null)
        {
            //Is a slot under drop point
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                if(draggedItem.ID == targetItem.ID)
                {
                    targetItem.AddToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    //Slot has an item - swap items
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
                }
            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
            }
        }
        else
        {
            //No slot under drop point
            //If where we're dropping is not within the inventory
            if (!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }
            else
            {
                //Snap back to og slot
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center
            }
        }
    }
   
   bool IsWithinInventory(Vector2 mousePosition) 
    {
       RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
       return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

void DropItem(Slot originalSlot)
{
    Item item = GetComponent<Item>();

    if (item == null)
    {
        Debug.LogError("DropItem: Item component is missing.");
        return;
    }

    // Remember the quantity before removing anything.
    int originalQuantity = item.quantity;

    // Find the player.
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player == null)
    {
        Debug.LogError("DropItem: Missing 'Player' tag.");
        return;
    }

    Transform playerTransform = player.transform;

    // Remove the item from the inventory slot.
    originalSlot.currentItem = null;

    // Spawn one world item for EVERY item in the stack.
    for (int i = 0; i < originalQuantity; i++)
    {
        // Pick a random direction.
        Vector2 randomDirection = Random.insideUnitCircle;

        // Prevent a zero-length direction.
        if (randomDirection.sqrMagnitude < 0.01f)
        {
            randomDirection = Vector2.right;
        }

        randomDirection.Normalize();

        // Pick a random distance from the player.
        float dropDistance = Random.Range(
            minDropDistance,
            maxDropDistance
        );

        // Calculate this item's individual drop position.
        Vector2 dropPosition =
            (Vector2)playerTransform.position +
            randomDirection * dropDistance;

        // Spawn one item.
        GameObject dropItem = Instantiate(
            gameObject,
            dropPosition,
            Quaternion.identity
        );

        // Make sure it isn't part of the inventory hierarchy.
        dropItem.transform.SetParent(null);

        // Re-apply the world position.
        dropItem.transform.position = dropPosition;

        // Every dropped object represents exactly ONE item.
        Item droppedItem = dropItem.GetComponent<Item>();

        if (droppedItem != null)
        {
            droppedItem.quantity = 1;
        }
        else
        {
            Debug.LogError(
                "DropItem: Spawned object has no Item component."
            );
        }
    }

    // Destroy the inventory object containing the stack.
    Destroy(gameObject);
}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }

    }

    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if(item == null || item.quantity <= 1) return;

        int splitAmount = item.quantity/2;
        if(splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneItem(splitAmount, inventoryController.inventoryPanel.transform);

        if (inventoryController == null || newItem == null) return;
        foreach(Transform slotTransform in inventoryController.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }

        item.AddToStack(splitAmount);
        Destroy(newItem);
    }

}
 