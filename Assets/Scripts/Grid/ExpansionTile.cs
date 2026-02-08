using UnityEngine;

public class ExpansionTile : MonoBehaviour
{
    [Header("Expansion Settings")]
    public int expansionCost = 1000;
    public int addedGridWidth = 5;
    public int addedGridHeight = 5;
    public bool isUnlocked = false;

    private Renderer tileRenderer;
    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color unlockedColor = new Color(0.3f, 0.8f, 0.3f, 0.5f);

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        UpdateVisual();
    }

    void OnMouseDown()
    {
        if (!isUnlocked)
        {
            TryUnlock();
        }
    }

    void TryUnlock()
    {
        if (FinanceManager.Instance != null && FinanceManager.Instance.CanAfford(expansionCost))
        {
            FinanceManager.Instance.RemoveMoney(expansionCost);
            isUnlocked = true;
            UpdateVisual();

            Debug.Log($"Expansion unlocked for ${expansionCost}");
        }
        else
        {
            Debug.Log("Not enough money for expansion!");
        }
    }

    void UpdateVisual()
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = isUnlocked ? unlockedColor : lockedColor;
        }
    }
}
