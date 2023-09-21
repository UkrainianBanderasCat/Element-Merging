using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tag", menuName = "Game/Tag")]
public class Tag : ScriptableObject
{

    [SerializeField] private string TagID;
    [SerializeField] private List<string> ReferenceElements;

    // Getters
    public string GetID()
    {
        return TagID;
    }
    public List<string> GetReferenceElementIDs()
    {
        return ReferenceElements;
    }

    // Setters
    public void SetID(string id)
    {
        TagID = id;
    }
    public void SetReferenceElements(List<string> referenceElements)
    {
        ReferenceElements = referenceElements;
    }

}
