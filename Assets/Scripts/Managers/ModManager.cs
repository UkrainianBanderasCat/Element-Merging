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
        // Load all JSON files in the "Resources/Elements" folder
        string folderPath = Path.Combine(Application.persistentDataPath, "Mods"); // The folder path relative to "Resources"
        TextAsset[] jsonAssets = Resources.LoadAll<TextAsset>(folderPath);

        foreach (string dir in Directory.GetDirectories(folderPath))
        {
            foreach (string d in Directory.GetDirectories(dir))
            {
                ElementManager.instance.LoadElementsInMod(d);
                RecipeManager.instance.LoadRecipesInMod(d);
            }
        }
    }
}
