using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ModManager : MonoBehaviour
{
    public static ModManager instance;
    private void Awake()
    {
        instance = this;
        Debug.Log(Application.persistentDataPath);
    }
    public void InitMods()
    {
        // Path to the StreamingAssets folder
        string streamingAssetsPath = Application.dataPath + "/StreamingAssets";

        // Path to the Mods folder within StreamingAssets
        string modsFolderPath = Path.Combine(streamingAssetsPath, "Mods");

        // Check if the Mods folder exists
        if (!Directory.Exists(modsFolderPath))
        {
            Debug.LogError("Mods folder not found in StreamingAssets.");
            return;
        }

        // Get all subdirectories (each subdirectory represents a mod)
        string[] modDirectories = Directory.GetDirectories(modsFolderPath);

        if (modDirectories.Length == 0)
        {
            Debug.Log("No mod directories found.");
            return;
        }

        foreach (string modDirectory in modDirectories)
        {
            Debug.Log(modDirectory);
            ElementManager.instance.LoadElementsInMod(modDirectory);
            RecipeManager.instance.LoadRecipesInMod(modDirectory);
        }
    }

}
