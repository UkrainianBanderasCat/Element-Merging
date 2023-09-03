using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Milestone", menuName = "Game/Milestone")]
public class Milestone : ScriptableObject
{
    public Condition condition;
    public string MilestoneName;
    public string MilestoneDescription;
    public string MilestoneID;
    public Sprite MilestoneSprite;
    public int necessaryAmount;
    public Element elementRequired;
    public Element reward;

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

    public int GetConditionAmount()
    {
        return necessaryAmount;
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
        return condition;
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
    [SerializeField] Element wantedElement;

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