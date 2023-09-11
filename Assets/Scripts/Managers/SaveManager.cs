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

    public struct MilestoneData
    {
        public string id;
        public bool unlocked;

        public MilestoneData(string id, bool unlocked)
        {
            this.id = id;
            this.unlocked = unlocked;
        }
    }

    [System.Serializable]
    public class ElementsList
    {
        public List<ElementData> elements = new List<ElementData>();
    }

    public class MilestoneList
    {
        public List<MilestoneData> milestones = new List<MilestoneData>();
    }
    private bool hasSaveData;

    public SaveManager()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        hasSaveData = PlayerPrefs.HasKey("elementData");
    }

    public void Save()
    {
        List<WorldElement> elements = GameManager.instance.worldElements;
        ElementsList elementsList = new ElementsList();

        List<Milestone> milestones = MilestonesManager.instance.milestones;
        MilestoneList milestoneList = new MilestoneList();

        foreach (WorldElement element in elements)
        {
            ElementData newElement = new ElementData();

            newElement.id = element.GetElement().GetID();
            newElement.position = element.transform.position;

            elementsList.elements.Add(newElement);
        }

        foreach (Milestone milestone in milestones)
        {
             milestoneList.milestones.Add(new MilestoneData(milestone.name, milestone.IsCompleted));
        }

        string json = JsonUtility.ToJson(elementsList);
        string milestoneJson = JsonUtility.ToJson(milestoneList);

        PlayerPrefs.SetString("elementData", json);
        PlayerPrefs.Save();

        PlayerPrefs.SetString("milestoneData", milestoneJson);
        PlayerPrefs.Save();

    }

    public void Load()
    {
        ElementsList elementsList = new ElementsList();

        string json = PlayerPrefs.GetString("elementData");
        elementsList = JsonUtility.FromJson<ElementsList>(json);

        MilestoneList milestoneList = JsonUtility.FromJson<MilestoneList>(PlayerPrefs.GetString("milestoneData")); //Retrieve milestones form playerpref in json and convert it back
        

        foreach (ElementData element in elementsList.elements)
        {
            GameManager.instance.CreateElement(ElementManager.instance.GetElement(element.id), element.position);
        }



        foreach (MilestoneData milestoneData in milestoneList.milestones)
        {
            MilestonesManager.instance.UpdateList(MilestonesManager.instance.GetMilestoneByName(milestoneData.id));
        }

        GameManager.instance.Release();
    }

    public void ResetSaveData()
    {
        PlayerPrefs.DeleteKey("elementData");
        PlayerPrefs.DeleteKey("milestoneData");
        hasSaveData = false;
    }

    public bool HasSaveData()
    {
        hasSaveData = PlayerPrefs.HasKey("elementData");
        Debug.Log(hasSaveData);
        return hasSaveData;
    }
}
