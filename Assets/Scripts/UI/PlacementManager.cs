using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }

    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask blockingLayers; // Layers that contain existing objects

    private GameObject currentPreviewInstance;
    private PlaceableItem currentPlaceableData;
    private SpriteRenderer previewRenderer;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (currentPreviewInstance == null) return;

     
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

     
        Vector2 snappedPos = new Vector2(
            Mathf.Round(mouseWorldPos.x / gridSize) * gridSize,
            Mathf.Round(mouseWorldPos.y / gridSize) * gridSize
        );

        currentPreviewInstance.transform.position = snappedPos;

 
        bool isAreaClear = CheckIfCellClear(snappedPos);
        previewRenderer.color = isAreaClear ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
    }

    public void StartPlacementPreview(PlaceableItem placeableItem)
    {
        ClearPreview();

        currentPlaceableData = placeableItem;
        
    
        currentPreviewInstance = new GameObject("PlacementPreview");
        previewRenderer = currentPreviewInstance.AddComponent<SpriteRenderer>();
        
        
        SpriteRenderer prefabRenderer = placeableItem.placedPrefab.GetComponent<SpriteRenderer>();
        if (prefabRenderer != null)
        {
            previewRenderer.sprite = prefabRenderer.sprite;
            previewRenderer.sortingOrder = prefabRenderer.sortingOrder + 10; 
        }

     
        previewRenderer.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void ClearPreview()
    {
        if (currentPreviewInstance != null)
        {
            Destroy(currentPreviewInstance);
        }
        currentPreviewInstance = null;
        currentPlaceableData = null;
        previewRenderer = null;
    }

    public bool TryPlaceObject()
    {
        if (currentPreviewInstance == null || currentPlaceableData == null) return false;

        Vector2 spawnPos = currentPreviewInstance.transform.position;

        
        if (!CheckIfCellClear(spawnPos))
        {
            Debug.LogWarning("Grid cell is already occupied!");
            return false;
        }

  
        Instantiate(currentPlaceableData.placedPrefab, spawnPos, Quaternion.identity);
        return true;
    }

    private bool CheckIfCellClear(Vector2 position)
    {
      
        Collider2D hit = Physics2D.OverlapCircle(position, 0.4f * gridSize, blockingLayers);
        return hit == null;
    }
}