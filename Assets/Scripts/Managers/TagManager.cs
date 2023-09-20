using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TagManager : MonoBehaviour
{

    public static TagManager instance;

    public List<Tag> tags;

    public TagManager()
    {
        instance = this;
    }

    public Tag GetTag(string id)
    {
        foreach (Tag tag in tags)
        {
            if (tag.GetID() == id)
            {
                return tag;
            }
        }
        return null;
    }

}