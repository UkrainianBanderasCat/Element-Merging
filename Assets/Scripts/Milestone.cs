using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Milestone", menuName = "Game/Milestone")]
public class Milestone : ScriptableObject
{
    [SerializeField] private string MilestoneName;
    [SerializeField] private string MilestoneDescription;
    [SerializeField] private string MilestoneID;
    [SerializeField] private Sprite MilestoneSprite;
    [SerializeField] private int necessaryAmount;


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
}
