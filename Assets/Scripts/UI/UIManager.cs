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
        EnsureHud();
        SetupUI();
        SetupButtons();
        UpdateDisplays();
    }

    void EnsureHud()
    {
        if (moneyText != null && customerCountText != null && gameTimeText != null
            && buildShelfButton != null && buildFridgeButton != null && buildCheckoutButton != null)
        {
            return;
        }

        RectTransform parentTransform = GetComponent<RectTransform>();
        if (parentTransform == null)
        {
            parentTransform = gameObject.AddComponent<RectTransform>();
        }

        GameObject hudPanel = new GameObject("HUDPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        hudPanel.transform.SetParent(parentTransform, false);

        RectTransform panelRect = hudPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 1f);
        panelRect.anchorMax = new Vector2(0f, 1f);
        panelRect.pivot = new Vector2(0f, 1f);
        panelRect.anchoredPosition = new Vector2(20f, -20f);
        panelRect.sizeDelta = new Vector2(640f, 140f);

        Image panelImage = hudPanel.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.7f);

        Font hudFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        moneyText = CreateHudText("MoneyText", hudPanel.transform, hudFont, new Vector2(20f, -20f), 24, FontStyle.Bold);
        moneyText.text = "$0.00";

        gameTimeText = CreateHudText("TimeText", hudPanel.transform, hudFont, new Vector2(20f, -54f), 20, FontStyle.Normal);
        gameTimeText.text = "Time: 00:00";

        customerCountText = CreateHudText("CustomersText", hudPanel.transform, hudFont, new Vector2(20f, -86f), 20, FontStyle.Normal);
        customerCountText.text = "Customers: 0";

        buildShelfButton = CreateHudButton("BuildShelfButton", hudPanel.transform, hudFont, "Build Shelf", new Vector2(-20f, -20f));
        buildFridgeButton = CreateHudButton("BuildFridgeButton", hudPanel.transform, hudFont, "Build Fridge", new Vector2(-20f, -60f));
        buildCheckoutButton = CreateHudButton("BuildCheckoutButton", hudPanel.transform, hudFont, "Build Checkout", new Vector2(-20f, -100f));
    }

    Text CreateHudText(string name, Transform parent, Font font, Vector2 anchoredPosition, int fontSize, FontStyle fontStyle)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObject.transform.SetParent(parent, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(0f, 1f);
        textRect.pivot = new Vector2(0f, 1f);
        textRect.anchoredPosition = anchoredPosition;
        textRect.sizeDelta = new Vector2(220f, 26f);

        Text text = textObject.GetComponent<Text>();
        text.font = font;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.color = Color.white;
        text.alignment = TextAnchor.UpperLeft;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    Button CreateHudButton(string name, Transform parent, Font font, string label, Vector2 anchoredPosition)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(1f, 1f);
        buttonRect.anchorMax = new Vector2(1f, 1f);
        buttonRect.pivot = new Vector2(1f, 1f);
        buttonRect.anchoredPosition = anchoredPosition;
        buttonRect.sizeDelta = new Vector2(180f, 32f);

        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(0.18f, 0.45f, 0.75f, 0.95f);

        Button button = buttonObject.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = buttonImage.color;
        colors.highlightedColor = new Color(0.22f, 0.55f, 0.85f, 1f);
        colors.pressedColor = new Color(0.12f, 0.35f, 0.6f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        CreateButtonLabel(buttonObject.transform, font, label);
        return button;
    }

    void CreateButtonLabel(Transform parent, Font font, string label)
    {
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        labelObject.transform.SetParent(parent, false);

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text labelText = labelObject.GetComponent<Text>();
        labelText.font = font;
        labelText.fontSize = 16;
        labelText.fontStyle = FontStyle.Bold;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.text = label;
        labelText.raycastTarget = false;
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

    public void PurchaseStock(string productName, int quantity)
    {
        if (InventoryManager.Instance != null)
        {
            bool success = InventoryManager.Instance.PurchaseStock(productName, quantity);
            if (success)
            {
                Debug.Log($"Purchased {quantity}x {productName}");
            }
            else
            {
                Debug.Log($"Failed to purchase {productName} - not enough money");
            }
        }
    }

    public void OnShelfClicked(ShelfController shelf)
    {
        if (shelf == null) return;

        if (string.IsNullOrEmpty(shelf.stockedProductName))
        {
            Debug.Log("Shelf is empty - showing product selection UI");
        }
        else
        {
            int availableStock = InventoryManager.Instance.GetStock(shelf.stockedProductName);
            int canStock = Mathf.Min(availableStock, shelf.maxCapacity - shelf.currentStock);
            
            if (canStock > 0)
            {
                shelf.StockShelf(shelf.stockedProductName, canStock);
            }
            else if (availableStock == 0)
            {
                Debug.Log($"No {shelf.stockedProductName} in inventory - purchase from shop (S key)");
            }
        }
    }
}
