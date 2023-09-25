using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Milestone", menuName = "Game/Milestone")]
public class Milestone : ScriptableObject
{
    public bool IsCompleted = false;

    public Condition condition;
    public string MilestoneName;
    public string MilestoneDescription;
    public string MilestoneID;
    public Sprite MilestoneSprite;
    public int UnlockedPanel;

    public int UnlockedPanel;

    //Condition Managing
    public int ConditionType;

    public int NecessaryAmount;
    public string SelectedElementID;
    public Element SelectedElement;
    public string rewardID;
    public Element reward;
    public List<string> SelectedElementsIDs = new List<string>();
    public List<Element> SelectedElements = new List<Element>();
    public List<Milestone> NeededMilestones = new List<Milestone>();

    //End of condition Managing

    public Milestone() { }
    public string GetName()
    {
        return MilestoneName;
    }
    public string GetDescription()
    {
        return MilestoneDescription;
    }
    public string GetID()
    {
        return MilestoneID;
    }
    public Sprite GetSprite()
    {
        return MilestoneSprite;
    }

    public bool hasReward()
    {
        return !(reward is null);
    }
    public Element GetReward()
    {
        if (!hasReward()) { Debug.LogError($"The Milestone \"{MilestoneName}\" doesn't have any reward"); }
        return reward;
    }
    public Condition GetCondition()
    {
        if(condition == null)
        {
            CreateCondition();
        }
        return condition;
    }

    public void Init()
    {
        if (rewardID != "")reward = ElementManager.instance.GetElement(rewardID);

        {
            foreach (string id in SelectedElementsIDs)
            {
                Element element = ElementManager.instance.GetElement(id);
                if (element != null)
                {
                    SelectedElements.Add(element);
                }
                else
                {
                    Debug.LogError($"The Milestone \"{MilestoneName}\" has an invalid selected element ID: {id}");
                }
            }
        }

        if (SelectedElementID != "")
            SelectedElement = ElementManager.instance.GetElement(SelectedElementID);

        // Debug.Log(GetName() + " completed " + IsCompleted);
        IsCompleted = false;
    }

    void CreateCondition()
    {
        switch (ConditionType)
        {
            case 0: //Number of Items required
                condition = new ItemCountCondition(NecessaryAmount);
                break;

            case 1: //Specific item required
                condition = new ItemUnlockedCondition(SelectedElement);
                break;

            case 2: //Mulitple items required
                condition = new ItemsUnlockedCondition(SelectedElements);
                break;

            case 3: //Multiple milestones required
                condition = new MilestonesUnlockedCondtion(NeededMilestones);
                break;
        }
    }
}

public abstract class Condition
{
    public abstract bool isMet(List<WorldElement> unlockedElements);

}

public class ItemCountCondition : Condition
{
    [SerializeField] public int wantedCount;

    public ItemCountCondition(int count)
    {
        wantedCount = count;
    }

    public override bool isMet(List<WorldElement> unlockedElements)
    {
        return unlockedElements.Count >= wantedCount;
    }
}

public class ItemUnlockedCondition : Condition
{
    [SerializeField] public Element wantedElement;

    public ItemUnlockedCondition(Element element)
    {
        wantedElement = element;
    }
    public override bool isMet(List<WorldElement> unlockedElements)
    {
        foreach(WorldElement worldElement in unlockedElements)
        {
            if(worldElement.GetElement() == wantedElement)
            {
                return true;
            }
        }

        return false;
    }
}

public class ItemsUnlockedCondition : Condition
{
    List<Element> wantedElements;

    public ItemsUnlockedCondition(List<Element> selectedElements)
    {
        wantedElements = selectedElements;
    }

    public override bool isMet(List<WorldElement> unlockedElements)
    {
        ItemUnlockedCondition checker = new ItemUnlockedCondition(unlockedElements[0].GetElement());

        foreach (Element element in wantedElements)
        {
            checker.wantedElement = element;
            if(!checker.isMet(unlockedElements)) { return false; }
        }

        return true;
    }

}

public class MilestonesUnlockedCondtion : Condition
{
    List<Milestone> wantedMilestones;
    
    public MilestonesUnlockedCondtion(List<Milestone> selectedMilestones)
    {
        wantedMilestones = selectedMilestones;
    }

    public override bool isMet(List<WorldElement> unlockedElements)
    {
        foreach(Milestone milestone in wantedMilestones)
        {
            if(!milestone.GetCondition().isMet(unlockedElements)) { return false; }
        }

        return true;
    }
}