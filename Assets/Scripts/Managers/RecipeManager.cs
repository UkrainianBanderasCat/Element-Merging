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
        public string RecipeElementLeft;
        public string RecipeElementRight;
        public string RecipeOutputElement;
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
            recipe.SetRecipeElementLeft(ElementManager.instance.GetElement(loadedRecipe.RecipeElementLeft));
            recipe.SetRecipeElementRight(ElementManager.instance.GetElement(loadedRecipe.RecipeElementRight));
            recipe.SetRecipeOutputElement(ElementManager.instance.GetElement(loadedRecipe.RecipeOutputElement));

            recipes.Add(recipe);
        }
    }

}
