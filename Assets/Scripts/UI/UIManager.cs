using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject buildMenuPanel;
    public GameObject shopMenuPanel;

    [Header("Display Text")]
    public Text moneyText;
    public Text customerCountText;
    public Text gameTimeText;

    [Header("Build Buttons")]
    public Button buildShelfButton;
    public Button buildFridgeButton;
    public Button buildCheckoutButton;

    private bool isBuildMenuOpen = false;
    private bool isShopMenuOpen = false;

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
        SetupUI();
        SetupButtons();
    }

    void Update()
    {
        UpdateDisplays();
        HandleInput();
    }

    void SetupUI()
    {
        if (buildMenuPanel != null)
            buildMenuPanel.SetActive(false);
        
        if (shopMenuPanel != null)
            shopMenuPanel.SetActive(false);
    }

    void SetupButtons()
    {
        if (buildShelfButton != null)
        {
            buildShelfButton.onClick.AddListener(() => StartPlacement(GridManager.PlaceableType.Shelf));
        }

        if (buildFridgeButton != null)
        {
            buildFridgeButton.onClick.AddListener(() => StartPlacement(GridManager.PlaceableType.Fridge));
        }

        if (buildCheckoutButton != null)
        {
            buildCheckoutButton.onClick.AddListener(() => StartPlacement(GridManager.PlaceableType.Checkout));
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMenu();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleShopMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllMenus();
        }
    }

    public void ToggleBuildMenu()
    {
        isBuildMenuOpen = !isBuildMenuOpen;
        if (buildMenuPanel != null)
        {
            buildMenuPanel.SetActive(isBuildMenuOpen);
        }

        if (isBuildMenuOpen && shopMenuPanel != null)
        {
            shopMenuPanel.SetActive(false);
            isShopMenuOpen = false;
        }
    }

    public void ToggleShopMenu()
    {
        isShopMenuOpen = !isShopMenuOpen;
        if (shopMenuPanel != null)
        {
            shopMenuPanel.SetActive(isShopMenuOpen);
        }

        if (isShopMenuOpen && buildMenuPanel != null)
        {
            buildMenuPanel.SetActive(false);
            isBuildMenuOpen = false;
        }
    }

    public void CloseAllMenus()
    {
        if (buildMenuPanel != null)
            buildMenuPanel.SetActive(false);
        
        if (shopMenuPanel != null)
            shopMenuPanel.SetActive(false);

        isBuildMenuOpen = false;
        isShopMenuOpen = false;
    }

    void StartPlacement(GridManager.PlaceableType type)
    {
        if (GridManager.Instance != null)
        {
            float cost = FinanceManager.Instance.GetPlacementCost(type);
            
            if (FinanceManager.Instance.CanAfford(cost))
            {
                FinanceManager.Instance.RemoveMoney(cost);
                GridManager.Instance.StartPlacement(type);
                CloseAllMenus();
            }
            else
            {
                Debug.Log($"Cannot afford {type}. Cost: ${cost}");
            }
        }
    }

    void UpdateDisplays()
    {
        if (GameManager.Instance != null && gameTimeText != null)
        {
            int minutes = Mathf.FloorToInt(GameManager.Instance.gameTime / 60f);
            int seconds = Mathf.FloorToInt(GameManager.Instance.gameTime % 60f);
            gameTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        }

        if (customerCountText != null)
        {
            int customerCount = FindObjectsOfType<CustomerAI>().Length;
            customerCountText.text = $"Customers: {customerCount}";
        }
    }

    public void UpdateMoneyDisplay(float amount)
    {
        if (moneyText != null)
        {
            moneyText.text = $"${amount:F2}";
        }
    }

    public void ShowPurchaseUI(string productName)
    {
        Debug.Log($"Show purchase UI for {productName}");
    }
}
