using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject customerPrefab;
    public float spawnInterval = 5f;
    public int maxCustomers = 10;

    [Header("Spawn Area")]
    public Vector3 spawnOffset = new Vector3(0, 0, -15);

    private float spawnTimer = 0f;
    private int currentCustomerCount = 0;

    void Start()
    {
        ValidatePrefabSetup();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && currentCustomerCount < maxCustomers)
        {
            SpawnCustomer();
            spawnTimer = 0f;
        }

        currentCustomerCount = FindObjectsOfType<CustomerAI>().Length;
    }

    void ValidatePrefabSetup()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerSpawner: Customer prefab is not assigned! Please assign a Customer prefab in the Inspector.");
            return;
        }

        // Check if prefab has CustomerAI component
        CustomerAI customerAI = customerPrefab.GetComponent<CustomerAI>();
        if (customerAI == null)
        {
            Debug.LogError($"CustomerSpawner: Customer prefab '{customerPrefab.name}' does not have a CustomerAI component! " +
                          "Please ensure the prefab has the CustomerAI script attached.");
        }
        else
        {
            Debug.Log($"CustomerSpawner: Prefab validation passed. Customer prefab '{customerPrefab.name}' has CustomerAI component.");
        }
    }

    void SpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerSpawner: Cannot spawn customer - customer prefab is not assigned!");
            return;
        }

        // Validate prefab has CustomerAI component before instantiation
        CustomerAI prefabCustomerAI = customerPrefab.GetComponent<CustomerAI>();
        if (prefabCustomerAI == null)
        {
            Debug.LogError($"CustomerSpawner: Cannot spawn customer - prefab '{customerPrefab.name}' is missing CustomerAI component!");
            return;
        }

        Vector3 spawnPosition = transform.position + spawnOffset;
        spawnPosition.x += Random.Range(-5f, 5f);
        
        try
        {
            // Instantiate as GameObject (safe instantiation)
            GameObject customerObject = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
            
            // Safely get the CustomerAI component from the instantiated object
            CustomerAI customerAI = customerObject.GetComponent<CustomerAI>();
            
            if (customerAI == null)
            {
                Debug.LogError($"CustomerSpawner: Instantiated customer object '{customerObject.name}' does not have CustomerAI component! This should not happen if prefab validation passed.");
                return;
            }

            Debug.Log($"Customer spawned successfully at {spawnPosition}. CustomerAI component validated.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"CustomerSpawner: Exception occurred while spawning customer: {e.Message}");
        }
    }

    // Inspector validation - runs in editor when component is modified
    void OnValidate()
    {
        if (Application.isPlaying)
            return;

        // Validate prefab assignment in Inspector
        if (customerPrefab != null)
        {
            CustomerAI customerAI = customerPrefab.GetComponent<CustomerAI>();
            if (customerAI == null)
            {
                Debug.LogWarning($"CustomerSpawner: Prefab '{customerPrefab.name}' is missing CustomerAI component!");
            }
        }
    }
}
