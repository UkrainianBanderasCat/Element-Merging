using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
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

    [System.Serializable]
    public class LoadedRecipesList
    {
        public LoadedRecipe[] recipes;
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
        //Debug.LogError("Error: Unable to find any recipes of the ID \"" + id + "\"");
        return null;
    }

    public void LoadRecipes()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Recipes");

        if (jsonTextAsset == null)
        {
            Debug.LogError("Recipes JSON file not found in Resources.");
            return;
        }

        LoadedRecipesList loadedRecipesList = JsonUtility.FromJson<LoadedRecipesList>(jsonTextAsset.text);

        foreach (LoadedRecipe loadedRecipe in loadedRecipesList.recipes)
        {
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.SetID(loadedRecipe.RecipeID);

            List<Element> recipeElements = new List<Element>();
            foreach (string e in loadedRecipe.RecipeElements)
            {
                Element element = ElementManager.instance.GetElement(e);
                if (element != null)
                {
                    recipeElements.Add(element);
                }
                else
                {
                    recipeElements.Add(new Element(e));
                    //Debug.LogWarning("Element not found: " + e + " for recipe: " + loadedRecipe.RecipeID);
                }
            }

            List<Element> recipeOutputElements = new List<Element>();
            foreach (string e in loadedRecipe.RecipeOutputElements)
            {
                Element element = ElementManager.instance.GetElement(e);
                if (element != null)
                {
                    recipeOutputElements.Add(element);
                }
                else
                {
                    //Debug.LogWarning("Element not found: " + e + " for recipe: " + loadedRecipe.RecipeID);
                }
            }

            recipe.SetRecipeElements(recipeElements);
            recipe.SetRecipeOutputElements(recipeOutputElements);

            recipes.Add(recipe);
        }
    }

    public void LoadRecipesInMod(string d)
    {
        // if (!(d.EndsWith("Recipes") || d.EndsWith("Recipes" + Path.DirectorySeparatorChar)))
        // {
        //     return;
        // }

        string filePath = Path.Combine(d, "Recipes", "Recipes.json");
        using (StreamReader sr = new StreamReader(filePath))
        {
            string json = sr.ReadToEnd();

            LoadedRecipesList loadedRecipesList = JsonUtility.FromJson<LoadedRecipesList>(json);

            foreach (LoadedRecipe loadedRecipe in loadedRecipesList.recipes)
            {
                Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
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

    public List<Recipe> GetAvailableRecipes()
    {
        List<Recipe> availableRecipes = new List<Recipe>();

        List<Element> elements = new List<Element>();

        foreach (WorldElement worldElement in GameManager.instance.worldElements)
        {
            elements.Add(worldElement.GetElement());
        }

        foreach (Recipe recipe in RecipeManager.instance.recipes)
        {
            // Check if the recipe's output elements are already in the worldElements list
            bool recipeCrafted = recipe.GetRecipeOutputElements().All(outputElement => elements.Contains(outputElement));

            // If the recipe is already crafted, continue to the next recipe
            if (recipeCrafted)
            {
                continue;
            }

            // Check if the player has the required elements (or tags) to craft the recipe
            bool canCraftRecipe = true;

            foreach (Element recipeElement in recipe.GetRecipeElements())
            {
                bool hasElementOrTag = false;

                foreach (Element playerElement in elements)
                {
                    if (playerElement.GetID() == recipeElement.GetID())
                    {
                        hasElementOrTag = true;
                        break;
                    }
                }

                if (!hasElementOrTag)
                {
                    // Check if it's a tag and if the player has elements with that tag
                    if (recipeElement.GetID().StartsWith("tag:"))
                    {
                        string tagID = recipeElement.GetID().Replace("tag:", "");
                        if (TagManager.instance.GetTag(tagID) != null)
                        {
                            List<string> taggedElementIDs = TagManager.instance.GetTag(tagID).GetReferenceElementIDs();
                            foreach (string elementID in taggedElementIDs)
                            {
                                if (elements.Any(element => element.GetID() == elementID))
                                {
                                    hasElementOrTag = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!hasElementOrTag)
                {
                    canCraftRecipe = false;
                    break;
                }
            }

            if (canCraftRecipe)
            {
                //Debug.Log("Can craft " + recipe.GetID());
                availableRecipes.Add(recipe);
            }
        }

        return availableRecipes;
    }
}
