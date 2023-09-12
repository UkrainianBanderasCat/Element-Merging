using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
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


    public void LoadElementsInMod(string d)
    {
        if (!(d.EndsWith("Elements") || d.EndsWith("Elements" + Path.DirectorySeparatorChar)))
        {
            return;
        }

        foreach (string filePath in Directory.GetFiles(d))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string json = sr.ReadToEnd();

                LoadedElement loadedElement = new LoadedElement();
                loadedElement = JsonUtility.FromJson<LoadedElement>(json);

                Element element = ScriptableObject.CreateInstance<Element>();


                // Load the sprite from Resources
                Sprite sprite = LoadNewSprite(Path.Combine(Directory.GetParent(d).FullName, loadedElement.ElementSpriteSrc));

            if (sprite == null)
            {
                continue;
            }

            //Debug.Log("Loaded : " + loadedElement.ElementName);
            element.SetName(loadedElement.ElementName);
            element.SetID(loadedElement.ElementID);
            element.SetSprite(sprite);
            
            elements.Add(element);
        }
    }

    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 16f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }
    
    public static Texture2D LoadTexture(string FilePath)
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
