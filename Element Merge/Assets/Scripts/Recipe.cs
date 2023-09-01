using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{

    [SerializeField] private string RecipeID;
    [SerializeField] private Element RecipeElementLeft;
    [SerializeField] private Element RecipeElementRight;
    [SerializeField] private Element RecipeOutputElement;

    public string GetID()
    {
        return RecipeID;
    }
    public Element GetRecipeElementLeft()
    {
        return RecipeElementLeft;
    }
    public Element GetRecipeElementRight()
    {
        return RecipeElementRight;
    }
    public Element GetRecipeOutputElement()
    {
        return RecipeOutputElement;
    }

}
