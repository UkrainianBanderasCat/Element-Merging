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

    public Recipe()
    {
        Debug.Log("test");
    }

    //Get
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
        foreach(Element element in RecipeElements)
        {
            if (element.GetID().Contains("tag:"))
            {
                string tagID = "";
                tagID = element.GetID().Replace("tag:", "");
                List<string> taggedElementIDs = TagManager.instance.GetTag(tagID).GetReferenceElementIDs();
                List<Element> taggedElements = new List<Element>();
                foreach (string elementID in taggedElementIDs)
                {
                    taggedElements.Add(ElementManager.instance.GetElement(elementID));
                }
                if (contains(elements, taggedElements))
                {
                    continue;
                }
            }
            
            if (elements.Contains(element))
            {
                continue;
            }
            return false;
        }
        return true;
    }

    private bool contains(List<Element> a, List<Element> b)
    {
        foreach (Element e in a)
        {
            if (b.Contains(e))
            {
                return true;
            }
        }
        return false;
    }

    //Set
    public void SetID(string id)
    {
        RecipeID = id;
    }
    public void SetRecipeElements(List<Element> elements)
    {
        RecipeElements = elements;
    }
    public void SetRecipeOutputElements(List<Element> elements)
    {
        RecipeOutputElements = elements;
    }

}
