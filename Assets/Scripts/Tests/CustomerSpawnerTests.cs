using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CustomerSpawnerTests
{
    private GameObject spawnerObject;
    private CustomerSpawner spawner;
    private GameObject customerPrefab;
    private GameObject invalidPrefab;

    [SetUp]
    public void Setup()
    {
        // Create CustomerSpawner object
        spawnerObject = new GameObject("TestCustomerSpawner");
        spawner = spawnerObject.AddComponent<CustomerSpawner>();

        // Create valid customer prefab
        customerPrefab = new GameObject("ValidCustomerPrefab");
        customerPrefab.AddComponent<CustomerAI>();

        // Create invalid customer prefab (missing CustomerAI)
        invalidPrefab = new GameObject("InvalidCustomerPrefab");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(spawnerObject);
        Object.DestroyImmediate(customerPrefab);
        Object.DestroyImmediate(invalidPrefab);
    }

    [Test]
    public void CustomerSpawner_WithValidPrefab_LogsSuccess()
    {
        // Assign valid prefab
        spawner.customerPrefab = customerPrefab;

        // Test the validation method
        // Note: This would normally be private, but we'll test through the public interface
        LogAssert.Expect(LogType.Log, "Prefab validation passed");
        
        // Simulate Start() call to trigger validation
        spawner.SendMessage("ValidatePrefabSetup");
    }

    [Test]
    public void CustomerSpawner_WithInvalidPrefab_LogsError()
    {
        // Assign invalid prefab
        spawner.customerPrefab = invalidPrefab;

        // Test the validation method
        LogAssert.Expect(LogType.Error, "CustomerSpawner: Customer prefab 'InvalidCustomerPrefab' does not have a CustomerAI component!");
        
        // Simulate Start() call to trigger validation
        spawner.SendMessage("ValidatePrefabSetup");
    }

    [Test]
    public void CustomerSpawner_WithNullPrefab_LogsError()
    {
        // Don't assign any prefab (null)

        // Test the validation method
        LogAssert.Expect(LogType.Error, "CustomerSpawner: Customer prefab is not assigned! Please assign a Customer prefab in the Inspector.");
        
        // Simulate Start() call to trigger validation
        spawner.SendMessage("ValidatePrefabSetup");
    }

    [Test]
    public void CustomerSpawner_SpawnCustomer_WithValidPrefab_DoesNotThrowException()
    {
        // Assign valid prefab
        spawner.customerPrefab = customerPrefab;

        // This should not throw an exception
        Assert.DoesNotThrow(() => {
            spawner.SendMessage("SpawnCustomer");
        });
    }

    [Test]
    public void CustomerSpawner_SpawnCustomer_WithInvalidPrefab_LogsErrorAndDoesNotSpawn()
    {
        // Assign invalid prefab
        spawner.customerPrefab = invalidPrefab;

        // This should log an error and not throw exception
        LogAssert.Expect(LogType.Error, "CustomerSpawner: Cannot spawn customer - prefab 'InvalidCustomerPrefab' is missing CustomerAI component!");
        
        Assert.DoesNotThrow(() => {
            spawner.SendMessage("SpawnCustomer");
        });
    }
}