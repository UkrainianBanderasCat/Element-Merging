using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Element", menuName = "Game/Element")]
public class Element : ScriptableObject
{

    [SerializeField] private string ElementName;
    [SerializeField] private string ElementID;
    [SerializeField] private Sprite ElementSprite;

    public string GetName()
    {
        return ElementName;
    }
    public string GetID()
    {
        return ElementID;
    }
    public Sprite GetSprite()
    {
        return ElementSprite;
    }

}
