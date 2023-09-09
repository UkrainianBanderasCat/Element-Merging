using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{

    public static ElementManager instance;

    public List<Element> elements;
    public List<Element> createdElements;

    [System.Serializable]
    public class LoadedElement
    {
        public string ElementName;
        public string ElementID;
        public string ElementSpriteSrc;
    }

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

    public void LoadElements()
    {
        // Load all JSON files in the "Resources/Elements" folder
        string folderPath = "Elements"; // The folder path relative to "Resources"
        TextAsset[] jsonAssets = Resources.LoadAll<TextAsset>(folderPath);

        foreach (TextAsset jsonAsset in jsonAssets)
        {
            string json = jsonAsset.text;

            LoadedElement loadedElement = new LoadedElement();
            
            loadedElement = JsonUtility.FromJson<LoadedElement>(json);

            Element element = ScriptableObject.CreateInstance<Element>();


            // Load the sprite from Resources
            Sprite sprite = Resources.Load<Sprite>(loadedElement.ElementSpriteSrc);

            element.SetName(loadedElement.ElementName);
            element.SetID(loadedElement.ElementID);
            element.SetSprite(sprite);
            
            elements.Add(element);
        }
    }
}
