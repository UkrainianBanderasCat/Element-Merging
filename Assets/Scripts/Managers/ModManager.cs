using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ModManager : MonoBehaviour
{
    public static ModManager instance;
    private List<string> installedModNames = new List<string>();
    
    public ModManager()
    {
        instance = this;
    }

    public void AddMod(string modPath)
    {
        //Add mod to mod folder
    }

    public List<string> GetInstalledModsNames()
    {
        return installedModNames;
    }

    public void InitMods()
    {
#if !UNITY_ANDROID
        InitModsComputer();
#else
            InitModsAndroid();
#endif
    }

    void InitModsAndroid()
    {
        // Path to the StreamingAssets folder
        string projectPath = Application.persistentDataPath;

        // Path to the Mods folder within StreamingAssets
        string modsFolderPath = Path.Combine(projectPath, "Mods");
        Debug.Log(modsFolderPath);

        // Check if the Mods folder exists
        if (!Directory.Exists(modsFolderPath))
        {
            Directory.CreateDirectory(modsFolderPath);
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
            string modName = ReadModInfo(modDirectory);
            if (!string.IsNullOrEmpty(modName))
            {
                installedModNames.Add(modName);
            }

            ElementManager.instance.LoadElementsInMod(modDirectory);
            RecipeManager.instance.LoadRecipesInMod(modDirectory);
        }
    }

    void InitModsComputer()
    {
        // Path to the StreamingAssets folder
        string projectPath = Application.dataPath;

        // Path to the Mods folder within StreamingAssets
        string modsFolderPath = Path.Combine(projectPath, "Mods");
        Debug.Log(modsFolderPath);

        // Check if the Mods folder exists
        if (!Directory.Exists(modsFolderPath))
        {
            Directory.CreateDirectory(modsFolderPath);
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
            string modName = ReadModInfo(modDirectory);
            if (!string.IsNullOrEmpty(modName))
            {
                installedModNames.Add(modName);
            }

            ElementManager.instance.LoadElementsInMod(modDirectory);
            RecipeManager.instance.LoadRecipesInMod(modDirectory);
        }
    }

    string ReadModInfo(string modDirectory)
    {
        // Path to the info.txt file within the mod directory
        string infoFilePath = Path.Combine(modDirectory, "info.txt");

        if (File.Exists(infoFilePath))
        {
            // Read the content of the info.txt file
            string[] lines = File.ReadAllLines(infoFilePath);

            // Initialize variables to store the name and description
            string modName = null;
            string modDescription = null;

            // Loop through each line to find the name and description
            foreach (string line in lines)
            {
                if (line.StartsWith("name="))
                {
                    modName = line.Substring(5); // Get the name part after "name="
                }
                else if (line.StartsWith("description="))
                {
                    modDescription = line.Substring(12); // Get the description part after "description="
                }
            }

            // Log the mod name and description
            if (!string.IsNullOrEmpty(modName))
            {
                Debug.Log("Mod Name: " + modName);
            }
            if (!string.IsNullOrEmpty(modDescription))
            {
                Debug.Log("Mod Description: " + modDescription);
            }

            return modName; // Return the mod name
        }
        else
        {
            Debug.LogWarning("info.txt not found in mod directory: " + modDirectory);
            return null; // Return null if the mod name is not found
        }
    }
}
