using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{

    [SerializeField] private string RecipeID;
    [SerializeField] private List<Element> RecipeElements;
    [SerializeField] private List<Element> RecipeOutputElements;

    public string GetID()
    {
        return RecipeID;
    }
    public List<Element> GetRecipeElements()
    {
        return RecipeElements;
    }

    public List<Element> GetRecipeOutputElements()
    {
        return RecipeOutputElements;
    }

    public bool CanCraftWith(List<Element> elements) //Code I shamelessly stole from stackoverflow to check if two lists (of the same type) have exactly the same elements in any order for any size of list.
    {
        var cnt = new Dictionary<Element, int>();
        foreach (Element s in RecipeElements)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (Element s in RecipeElements)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }

}
