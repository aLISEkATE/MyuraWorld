using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 0.2f;
    public float maxDropDistance = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
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
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if(dropSlot== null)
        {
            GameObject dropItem = eventData.pointerEnter;

            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if(dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            if (!isWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
            }

               
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
   
   bool isWithinInventory(Vector2 mousePosition) 
    {
       RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
       return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }


    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(playerTransform == null)
        {
            Debug.LogError("missing 'Player' tag");
        }

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance,maxDropDistance);
    

        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;
        Debug.Log((Vector2)playerTransform.position + dropOffset);
        Instantiate(gameObject, dropPosition, Quaternion.identity);
        Destroy(gameObject);
        // InventoryController.inventoryItems.Remove(gamedObject);
    }
}
 