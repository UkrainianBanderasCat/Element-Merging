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

}
