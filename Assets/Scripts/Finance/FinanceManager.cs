using UnityEngine;

public class FinanceManager : MonoBehaviour
{
    public static FinanceManager Instance { get; private set; }

    [Header("Finance")]
    public float currentMoney = 5000f;
    public float totalRevenue = 0f;
    public float totalExpenses = 0f;

    [Header("Costs")]
    public float shelfCost = 100f;
    public float fridgeCost = 200f;
    public float checkoutCost = 150f;

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
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        totalRevenue += amount;
        UpdateUI();
        Debug.Log($"Money added: ${amount}. Current: ${currentMoney}");
    }

    public void RemoveMoney(float amount)
    {
        currentMoney -= amount;
        totalExpenses += amount;
        UpdateUI();
        Debug.Log($"Money spent: ${amount}. Current: ${currentMoney}");
    }

    public bool CanAfford(float amount)
    {
        return currentMoney >= amount;
    }

    public float GetProfit()
    {
        return totalRevenue - totalExpenses;
    }

    void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoneyDisplay(currentMoney);
        }
    }

    public float GetPlacementCost(GridManager.PlaceableType type)
    {
        switch (type)
        {
            case GridManager.PlaceableType.Shelf:
                return shelfCost;
            case GridManager.PlaceableType.Fridge:
                return fridgeCost;
            case GridManager.PlaceableType.Checkout:
                return checkoutCost;
            default:
                return 0f;
        }
    }
}
