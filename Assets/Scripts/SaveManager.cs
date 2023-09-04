using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [System.Serializable]
    public class ElementData
    {
        public string id;
        public Vector3 position;
    }

    [System.Serializable]
    public class ElementsList
    {
        public List<ElementData> elements = new List<ElementData>();
    }

    private bool hasSaveData;

    public SaveManager()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        hasSaveData = PlayerPrefs.HasKey("elementData");
    }

    public void Save()
    {
        List<WorldElement> elements = GameManager.instance.worldElements;
        ElementsList elementsList = new ElementsList();

        foreach (WorldElement element in elements)
        {
            ElementData newElement = new ElementData();

            newElement.id = element.GetElement().GetID();
            newElement.position = element.transform.position;

            elementsList.elements.Add(newElement);
        }

        string json = JsonUtility.ToJson(elementsList);

        PlayerPrefs.SetString("elementData", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        ElementsList elementsList = new ElementsList();

        string json = PlayerPrefs.GetString("elementData");
        elementsList = JsonUtility.FromJson<ElementsList>(json);

        foreach (ElementData element in elementsList.elements)
        {
            GameManager.instance.CreateElement(ElementManager.instance.GetElement(element.id), element.position);
        }
    }

    public void ResetSaveData()
    {
        PlayerPrefs.DeleteKey("elementData");
        hasSaveData = false;
    }

    public bool HasSaveData()
    {
        return hasSaveData;
    }
}
