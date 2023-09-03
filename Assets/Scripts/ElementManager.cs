using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{

    public static ElementManager instance;

    public List<Element> elements;

    public ElementManager()
    {
        instance = this;
    }

    public Element GetElement(string id)
    {
        foreach (Element element in elements)
        {
            if (element.GetID() == id)
            {
                return element;
            }
        }
        Debug.LogError("Error: Unable to find any elements with the ID \"" + id + "\"");
        return null;
    }

}
