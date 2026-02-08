using UnityEngine;
using System.Collections.Generic;

public class CustomerAI : MonoBehaviour
{
    [Header("Customer Settings")]
    public float moveSpeed = 3f;
    public int itemsToPurchase = 3;
    
    [Header("State")]
    public CustomerState currentState = CustomerState.Shopping;
    public List<string> shoppingCart = new List<string>();
    public float totalSpent = 0f;

    private Vector3 targetPosition;
    private ShelfController targetShelf;
    private CheckoutController targetCheckout;
    private float stateTimer = 0f;

    public enum CustomerState
    {
        Shopping,
        MovingToShelf,
        TakingProduct,
        MovingToCheckout,
        InQueue,
        Checkout,
        Leaving
    }

    void Start()
    {
        StartShopping();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case CustomerState.Shopping:
                UpdateShopping();
                break;
            case CustomerState.MovingToShelf:
                UpdateMovingToShelf();
                break;
            case CustomerState.TakingProduct:
                UpdateTakingProduct();
                break;
            case CustomerState.MovingToCheckout:
                UpdateMovingToCheckout();
                break;
            case CustomerState.InQueue:
                UpdateInQueue();
                break;
            case CustomerState.Checkout:
                UpdateCheckout();
                break;
            case CustomerState.Leaving:
                UpdateLeaving();
                break;
        }
    }

    void StartShopping()
    {
        currentState = CustomerState.Shopping;
        stateTimer = 0f;
    }

    void UpdateShopping()
    {
        if (shoppingCart.Count >= itemsToPurchase)
        {
            FindCheckout();
            return;
        }

        if (stateTimer > 1f)
        {
            FindRandomShelf();
        }
    }

    void FindRandomShelf()
    {
        ShelfController[] shelves = FindObjectsOfType<ShelfController>();
        if (shelves.Length > 0)
        {
            ShelfController shelf = shelves[Random.Range(0, shelves.Length)];
            if (shelf.currentStock > 0)
            {
                targetShelf = shelf;
                targetPosition = shelf.transform.position + Vector3.forward * 2f;
                currentState = CustomerState.MovingToShelf;
            }
        }
        stateTimer = 0f;
    }

    void UpdateMovingToShelf()
    {
        MoveTowards(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            currentState = CustomerState.TakingProduct;
            stateTimer = 0f;
        }
    }

    void UpdateTakingProduct()
    {
        if (stateTimer > 1f && targetShelf != null)
        {
            string productName;
            float price;
            
            if (targetShelf.TryTakeProduct(out productName, out price))
            {
                shoppingCart.Add(productName);
                totalSpent += price;
                Debug.Log($"Customer picked up {productName} for ${price}");
            }

            currentState = CustomerState.Shopping;
            stateTimer = 0f;
        }
    }

    void FindCheckout()
    {
        CheckoutController[] checkouts = FindObjectsOfType<CheckoutController>();
        if (checkouts.Length > 0)
        {
            targetCheckout = checkouts[0];
            targetPosition = targetCheckout.GetQueuePosition(this);
            currentState = CustomerState.MovingToCheckout;
        }
        else
        {
            Debug.Log("No checkout available, customer leaving without purchase");
            currentState = CustomerState.Leaving;
            targetPosition = transform.position + Vector3.back * 20f;
        }
    }

    void UpdateMovingToCheckout()
    {
        MoveTowards(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            if (targetCheckout != null)
            {
                targetCheckout.AddToQueue(this);
                currentState = CustomerState.InQueue;
            }
        }
    }

    void UpdateInQueue()
    {
        if (targetCheckout != null)
        {
            targetPosition = targetCheckout.GetQueuePosition(this);
            MoveTowards(targetPosition);
        }
    }

    public void StartCheckout()
    {
        currentState = CustomerState.Checkout;
        stateTimer = 0f;
    }

    void UpdateCheckout()
    {
        if (stateTimer > 2f)
        {
            CompleteCheckout();
        }
    }

    void CompleteCheckout()
    {
        if (FinanceManager.Instance != null)
        {
            FinanceManager.Instance.AddMoney(totalSpent);
            Debug.Log($"Customer completed purchase: ${totalSpent} for {shoppingCart.Count} items");
        }

        currentState = CustomerState.Leaving;
        targetPosition = transform.position + Vector3.back * 20f;
    }

    void UpdateLeaving()
    {
        MoveTowards(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 1f || stateTimer > 10f)
        {
            Destroy(gameObject);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
