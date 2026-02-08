using UnityEngine;
using UnityEditor;
using System.IO;

public class WebGLBuildHelper
{
    [MenuItem("Build/WebGL Build")]
    public static void BuildWebGL()
    {
        string buildPath = EditorUtility.SaveFolderPanel("Choose WebGL Build Location", "", "WebGLBuild");
        
        if (string.IsNullOrEmpty(buildPath))
            return;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        Debug.Log($"WebGL build completed at: {buildPath}");
    }

    [MenuItem("Build/Validate Project")]
    public static void ValidateProject()
    {
        bool hasErrors = false;

        // Check if GridManager has prefabs assigned
        var gridManager = Object.FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            if (gridManager.shelfPrefab == null)
            {
                Debug.LogError("GridManager: Shelf Prefab is not assigned!");
                hasErrors = true;
            }
            if (gridManager.fridgePrefab == null)
            {
                Debug.LogError("GridManager: Fridge Prefab is not assigned!");
                hasErrors = true;
            }
            if (gridManager.checkoutPrefab == null)
            {
                Debug.LogError("GridManager: Checkout Prefab is not assigned!");
                hasErrors = true;
            }
        }

        // Check if CustomerSpawner has prefab assigned
        var customerSpawner = Object.FindObjectOfType<CustomerSpawner>();
        if (customerSpawner != null)
        {
            if (customerSpawner.customerPrefab == null)
            {
                Debug.LogError("CustomerSpawner: Customer Prefab is not assigned!");
                hasErrors = true;
            }
        }

        if (hasErrors)
        {
            Debug.LogError("Project validation failed! Please fix the errors above.");
        }
        else
        {
            Debug.Log("Project validation passed! All prefabs are assigned correctly.");
        }
    }
}
