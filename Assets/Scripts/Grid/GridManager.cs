using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int gridWidth = 20;
    public int gridHeight = 15;
    public float cellSize = 2f;

    [Header("Prefabs")]
    public GameObject shelfPrefab;
    public GameObject fridgePrefab;
    public GameObject checkoutPrefab;

    [Header("Visual")]
    public bool showGrid = true;
    public Color gridColor = Color.green;

    private GridCell[,] grid;
    private GameObject currentPreview;
    private PlaceableType currentPlacementType = PlaceableType.None;

    public enum PlaceableType
    {
        None,
        Shelf,
        Fridge,
        Checkout
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGrid();
    }

    void Update()
    {
        if (currentPlacementType != PlaceableType.None)
        {
            HandlePlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                grid[x, z] = new GridCell(x, z);
            }
        }

        Debug.Log($"Grid initialized: {gridWidth}x{gridHeight}");
    }

    public void StartPlacement(PlaceableType type)
    {
        currentPlacementType = type;
        CreatePreview();
    }

    void CreatePreview()
    {
        GameObject prefab = GetPrefabForType(currentPlacementType);
        if (prefab != null)
        {
            currentPreview = Instantiate(prefab);
            SetPreviewTransparency(currentPreview, 0.5f);
        }
    }

    void HandlePlacementPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            Vector3 gridPos = WorldToGridPosition(hit.point);
            Vector3 worldPos = GridToWorldPosition((int)gridPos.x, (int)gridPos.z);

            if (currentPreview != null)
            {
                currentPreview.transform.position = worldPos;

                bool canPlace = CanPlaceAt((int)gridPos.x, (int)gridPos.z);
                SetPreviewColor(currentPreview, canPlace ? Color.green : Color.red);
            }
        }
    }

    void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            Vector3 gridPos = WorldToGridPosition(hit.point);
            int gridX = (int)gridPos.x;
            int gridZ = (int)gridPos.z;

            if (CanPlaceAt(gridX, gridZ))
            {
                GameObject prefab = GetPrefabForType(currentPlacementType);
                if (prefab != null)
                {
                    Vector3 worldPos = GridToWorldPosition(gridX, gridZ);
                    GameObject placed = Instantiate(prefab, worldPos, Quaternion.identity);
                    SetPreviewTransparency(placed, 1f);

                    grid[gridX, gridZ].isOccupied = true;
                    grid[gridX, gridZ].placedObject = placed;

                    Debug.Log($"Placed {currentPlacementType} at grid position ({gridX}, {gridZ})");
                }
            }
        }
    }

    void CancelPlacement()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        currentPlacementType = PlaceableType.None;
        Debug.Log("Placement cancelled");
    }

    public bool CanPlaceAt(int x, int z)
    {
        if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            return false;

        return !grid[x, z].isOccupied;
    }

    public Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x + (gridWidth * cellSize) / 2f) / cellSize);
        int z = Mathf.FloorToInt((worldPosition.z + (gridHeight * cellSize) / 2f) / cellSize);
        return new Vector3(x, 0, z);
    }

    public Vector3 GridToWorldPosition(int x, int z)
    {
        float worldX = (x * cellSize) - (gridWidth * cellSize) / 2f + cellSize / 2f;
        float worldZ = (z * cellSize) - (gridHeight * cellSize) / 2f + cellSize / 2f;
        return new Vector3(worldX, 0, worldZ);
    }

    GameObject GetPrefabForType(PlaceableType type)
    {
        switch (type)
        {
            case PlaceableType.Shelf:
                return shelfPrefab;
            case PlaceableType.Fridge:
                return fridgePrefab;
            case PlaceableType.Checkout:
                return checkoutPrefab;
            default:
                return null;
        }
    }

    void SetPreviewTransparency(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }

    void SetPreviewColor(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.color = color;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            Gizmos.color = gridColor;

            for (int x = 0; x <= gridWidth; x++)
            {
                Vector3 start = GridToWorldPosition(x, 0);
                Vector3 end = GridToWorldPosition(x, gridHeight);
                start.y = 0.01f;
                end.y = 0.01f;
                Gizmos.DrawLine(start, end);
            }

            for (int z = 0; z <= gridHeight; z++)
            {
                Vector3 start = GridToWorldPosition(0, z);
                Vector3 end = GridToWorldPosition(gridWidth, z);
                start.y = 0.01f;
                end.y = 0.01f;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}

[System.Serializable]
public class GridCell
{
    public int x;
    public int z;
    public bool isOccupied;
    public GameObject placedObject;

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.isOccupied = false;
        this.placedObject = null;
    }
}
