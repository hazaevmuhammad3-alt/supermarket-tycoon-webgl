using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [Header("Shelf Settings")]
    public int maxCapacity = 20;
    public string stockedProductName = "";
    public int currentStock = 0;

    [Header("Visuals")]
    public Color emptyColor = Color.red;
    public Color fullColor = Color.green;

    private Renderer shelfRenderer;

    void Start()
    {
        shelfRenderer = GetComponent<Renderer>();
        UpdateVisual();
    }

    void OnMouseDown()
    {
        if (InventoryManager.Instance != null)
        {
            ShowStockingUI();
        }
    }

    public void StockShelf(string productName, int quantity)
    {
        if (string.IsNullOrEmpty(stockedProductName))
        {
            stockedProductName = productName;
        }

        if (stockedProductName != productName)
        {
            Debug.Log("This shelf is stocked with a different product!");
            return;
        }

        int spaceAvailable = maxCapacity - currentStock;
        int amountToStock = Mathf.Min(quantity, spaceAvailable);

        if (InventoryManager.Instance.RemoveStock(productName, amountToStock))
        {
            currentStock += amountToStock;
            UpdateVisual();
            Debug.Log($"Stocked {amountToStock}x {productName} on shelf. Current: {currentStock}/{maxCapacity}");
        }
    }

    public bool TryTakeProduct(out string productName, out float price)
    {
        productName = stockedProductName;
        price = 0f;

        if (currentStock > 0 && !string.IsNullOrEmpty(stockedProductName))
        {
            currentStock--;
            
            ProductData product = InventoryManager.Instance.GetProductData(stockedProductName);
            if (product != null)
            {
                price = product.sellPrice;
            }

            UpdateVisual();

            if (currentStock == 0)
            {
                stockedProductName = "";
            }

            return true;
        }

        return false;
    }

    void UpdateVisual()
    {
        if (shelfRenderer != null)
        {
            float stockRatio = (float)currentStock / maxCapacity;
            shelfRenderer.material.color = Color.Lerp(emptyColor, fullColor, stockRatio);
        }
    }

    void ShowStockingUI()
    {
        Debug.Log($"Shelf clicked - Current stock: {currentStock}/{maxCapacity} of {stockedProductName}");
    }
}
