using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{

    public static RecipeManager instance;

    public List<Recipe> recipes;

    public RecipeManager()
    {
        instance = this;
    }

    [System.Serializable]
    public class LoadedRecipe
    {
        public string RecipeID;
        public string[] RecipeElements;
        public string[] RecipeOutputElements;
    }

    public Recipe GetRecipe(string id)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.GetID() == id)
            {
                return recipe;
            }
        }
        Debug.LogError("Error: Unable to find any recipes of the ID \"" + id + "\"");
        return null;
    }

    public void LoadRecipes()
    {
        Debug.Log("Loading Recipes!");
        // Load all JSON files in the "Resources/Recipes" folder
        string folderPath = "Recipes"; // The folder path relative to "Resources"
        TextAsset[] jsonAssets = Resources.LoadAll<TextAsset>(folderPath);

        foreach (TextAsset jsonAsset in jsonAssets)
        {
            string json = jsonAsset.text;

            LoadedRecipe loadedRecipe = new LoadedRecipe();

            loadedRecipe = JsonUtility.FromJson<LoadedRecipe>(json);

            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            // Load the sprite from Resources
            recipe.SetID(loadedRecipe.RecipeID);

            List<Element> recipeElements = new List<Element>();
            foreach (string e in loadedRecipe.RecipeElements)
            {
                recipeElements.Add(ElementManager.instance.GetElement(e));
            }

            List<Element> recipeOutputElements = new List<Element>();
            foreach (string e in loadedRecipe.RecipeOutputElements)
            {
                recipeOutputElements.Add(ElementManager.instance.GetElement(e));
            }

            recipe.SetRecipeElements(recipeElements);
            recipe.SetRecipeOutputElements(recipeOutputElements);

            recipes.Add(recipe);
        }
    }

}
