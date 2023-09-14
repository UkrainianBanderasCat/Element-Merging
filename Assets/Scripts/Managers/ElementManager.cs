using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ElementManager : MonoBehaviour
{

    public static ElementManager instance;

    public List<Element> elements;

    [System.Serializable]
    public class LoadedElement
    {
        public string ElementName;
        public string ElementID;
        public string ElementSpriteSrc;
    }

    [System.Serializable]
    public class LoadedElementsList
    {
        public LoadedElement[] elements;
    }


    public ElementManager() => instance = this;

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
        // Load the JSON file from the "Resources" folder
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Elements");

        if (jsonTextAsset == null)
        {
            Debug.LogError("Elements JSON file not found in Resources.");
            return;
        }

        LoadedElementsList loadedElementsList = JsonUtility.FromJson<LoadedElementsList>(jsonTextAsset.text);

        foreach (LoadedElement loadedElement in loadedElementsList.elements)
        {
            Element element = ScriptableObject.CreateInstance<Element>();

            // Load the sprite from Resources
            Sprite sprite = Resources.Load<Sprite>(loadedElement.ElementSpriteSrc);

            if (sprite == null)
            {
                Debug.LogError("Sprite not found for element: " + loadedElement.ElementName);
                continue;
            }

            element.SetName(loadedElement.ElementName);
            element.SetID(loadedElement.ElementID);
            element.SetSprite(sprite);

            elements.Add(element);
        }
    }


    public void LoadElementsInMod(string d)
    {
        // if (!(d.EndsWith("Elements") || d.EndsWith("Elements" + Path.DirectorySeparatorChar)))
        // {
        //     return;
        // }

        
        string filePath = Path.Combine(d, "Elements.json");
        Debug.Log(filePath);
        using (StreamReader sr = new StreamReader(filePath))
        {
            string json = sr.ReadToEnd();

            LoadedElementsList loadedElementsList = JsonUtility.FromJson<LoadedElementsList>(json);

            foreach (LoadedElement loadedElement in loadedElementsList.elements)
            {
                Element element = ScriptableObject.CreateInstance<Element>();

                // Load the sprite from Resources
                Sprite sprite = LoadNewSprite(Path.Combine(Directory.GetParent(d).FullName, loadedElement.ElementSpriteSrc));

                if (sprite == null)
                {
                    Debug.LogError("Sprite not found for element: " + loadedElement.ElementName);
                    continue;
                }

                element.SetName(loadedElement.ElementName);
                element.SetID(loadedElement.ElementID);
                element.SetSprite(sprite);

                elements.Add(element);
            }
        }
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 16f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);

        if (SpriteTexture == null)
        {
            Debug.Log("Failed to load texture with path: " + FilePath);
            return null;
        }

        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }
    
    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))
                Tex2D.filterMode = FilterMode.Point;// Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture

        }
        return null;                     // Return null if load failed
    }
}