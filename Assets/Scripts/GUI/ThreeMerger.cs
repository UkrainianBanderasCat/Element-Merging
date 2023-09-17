using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeMerger : MonoBehaviour
{
    List<Element> elements = new() { null,null,null};
    public List<GameObject> spots = new(3);
    public static ThreeMerger instance;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpot(GameObject spot, Element element)
    {
        elements[spots.IndexOf(spot)] = element;
        spot.GetComponent<Image>().sprite = element.GetSprite();
        spot.GetComponent<Image>().color = Color.white;
    }

    public void AttemptRecipe()
    {
        GameManager.instance.MergeElements(elements);
    }
}
