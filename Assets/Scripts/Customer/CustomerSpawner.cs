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

    void SpawnCustomer()
    {
        if (customerPrefab != null)
        {
            Vector3 spawnPosition = transform.position + spawnOffset;
            spawnPosition.x += Random.Range(-5f, 5f);
            
            GameObject customer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Customer spawned at {spawnPosition}");
        }
        else
        {
            Debug.LogWarning("Customer prefab not assigned!");
        }
    }
}
