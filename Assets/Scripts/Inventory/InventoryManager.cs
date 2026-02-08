using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory")]
    public List<ProductData> availableProducts = new List<ProductData>();
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

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
        InitializeProducts();
    }

    void InitializeProducts()
    {
        availableProducts.Add(new ProductData("Bread", 2.50f, 5.00f));
        availableProducts.Add(new ProductData("Milk", 3.00f, 6.00f));
        availableProducts.Add(new ProductData("Cheese", 4.00f, 8.00f));
        availableProducts.Add(new ProductData("Juice", 2.00f, 4.50f));
        availableProducts.Add(new ProductData("Soda", 1.50f, 3.00f));

        foreach (var product in availableProducts)
        {
            inventory[product.productName] = 0;
        }

        Debug.Log($"Initialized {availableProducts.Count} products");
    }

    public bool PurchaseStock(string productName, int quantity)
    {
        ProductData product = availableProducts.Find(p => p.productName == productName);
        if (product == null)
        {
            Debug.LogWarning($"Product {productName} not found!");
            return false;
        }

        float totalCost = product.purchasePrice * quantity;

        if (FinanceManager.Instance != null && FinanceManager.Instance.CanAfford(totalCost))
        {
            FinanceManager.Instance.RemoveMoney(totalCost);
            AddStock(productName, quantity);
            Debug.Log($"Purchased {quantity}x {productName} for ${totalCost}");
            return true;
        }

        Debug.Log($"Not enough money to purchase {quantity}x {productName}");
        return false;
    }

    public void AddStock(string productName, int quantity)
    {
        if (inventory.ContainsKey(productName))
        {
            inventory[productName] += quantity;
        }
        else
        {
            inventory[productName] = quantity;
        }
    }

    public bool RemoveStock(string productName, int quantity)
    {
        if (!inventory.ContainsKey(productName) || inventory[productName] < quantity)
        {
            return false;
        }

        inventory[productName] -= quantity;
        return true;
    }

    public int GetStock(string productName)
    {
        if (inventory.ContainsKey(productName))
        {
            return inventory[productName];
        }
        return 0;
    }

    public ProductData GetProductData(string productName)
    {
        return availableProducts.Find(p => p.productName == productName);
    }
}

[System.Serializable]
public class ProductData
{
    public string productName;
    public float purchasePrice;
    public float sellPrice;

    public ProductData(string name, float purchase, float sell)
    {
        productName = name;
        purchasePrice = purchase;
        sellPrice = sell;
    }
}
