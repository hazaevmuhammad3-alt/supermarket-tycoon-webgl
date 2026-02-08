#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class CustomerSpawnerValidator : EditorWindow
{
    [MenuItem("Tools/Customer Spawner Validator")]
    public static void ShowWindow()
    {
        GetWindow<CustomerSpawnerValidator>("Customer Spawner Validator");
    }

    void OnGUI()
    {
        GUILayout.Label("Customer Spawner Validation", EditorStyles.boldLabel);

        if (GUILayout.Button("Validate All Customer Spawners"))
        {
            ValidateAllSpawners();
        }

        if (GUILayout.Button("Create Test Customer Prefab"))
        {
            CreateTestCustomerPrefab();
        }
    }

    void ValidateAllSpawners()
    {
        CustomerSpawner[] spawners = FindObjectsOfType<CustomerSpawner>();
        
        if (spawners.Length == 0)
        {
            EditorUtility.DisplayDialog("Validation Result", "No CustomerSpawner components found in the scene.", "OK");
            return;
        }

        int validCount = 0;
        int invalidCount = 0;

        foreach (CustomerSpawner spawner in spawners)
        {
            if (spawner.customerPrefab == null)
            {
                Debug.LogError($"CustomerSpawner on {spawner.gameObject.name}: Customer prefab is not assigned!");
                invalidCount++;
                continue;
            }

            CustomerAI customerAI = spawner.customerPrefab.GetComponent<CustomerAI>();
            if (customerAI == null)
            {
                Debug.LogError($"CustomerSpawner on {spawner.gameObject.name}: Customer prefab '{spawner.customerPrefab.name}' is missing CustomerAI component!");
                invalidCount++;
            }
            else
            {
                Debug.Log($"CustomerSpawner on {spawner.gameObject.name}: VALID - Prefab '{spawner.customerPrefab.name}' has CustomerAI component.");
                validCount++;
            }
        }

        string message = $"Validation Complete!\n\nValid Spawners: {validCount}\nInvalid Spawners: {invalidCount}\nTotal: {spawners.Length}";
        EditorUtility.DisplayDialog("Validation Result", message, "OK");
    }

    void CreateTestCustomerPrefab()
    {
        // Create a test customer prefab programmatically
        GameObject customerObject = new GameObject("TestCustomerPrefab");
        customerObject.AddComponent<CustomerAI>();

        // Add a simple visual representation
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visual.transform.SetParent(customerObject.transform);
        visual.transform.localPosition = Vector3.zero;
        visual.name = "Visual";

        // Create prefab
        string prefabPath = "Assets/Prefabs/Customer/TestCustomerPrefab.prefab";
        
        // Ensure directory exists
        System.IO.Directory.CreateSystem(System.IO.Path.GetDirectoryName(prefabPath));

        #if UNITY_2018_3_OR_NEWER
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(customerObject, prefabPath);
        #else
        GameObject prefab = PrefabUtility.CreatePrefab(prefabPath, customerObject);
        #endif

        DestroyImmediate(customerObject);

        Debug.Log($"Test customer prefab created at: {prefabPath}");
        EditorUtility.DisplayDialog("Prefab Created", $"Test customer prefab created at:\n{prefabPath}", "OK");
    }
}
#endif