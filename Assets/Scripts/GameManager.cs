using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public WorldElement selectedElement;

    public List<Element> elements;
    public List<Recipe> recipes;

    public List<WorldElement> worldElements;

    [SerializeField] private GameObject worldElementObject;
    [SerializeField] private Transform worldElementHolder;
    [SerializeField] private GameObject mergeSucessScreen;
    [SerializeField] private TextMeshProUGUI elementName;
    [SerializeField] private Image elementSpriteDisplay;
    [SerializeField] private TextMeshProUGUI elementNameDisplay;

    public string elementNameDisplayText;
    public bool hoveringOverElement;
    private bool mergeSucessScreenActive;

    public GameManager()
    {
        instance = this;
    }

    public void MergeElements(Element leftElement, Element rightElement)
    {
        foreach (Recipe recipe in recipes)
        {
            if ((recipe.GetRecipeElementLeft() == leftElement && recipe.GetRecipeElementRight() == rightElement)
                || (recipe.GetRecipeElementLeft() == rightElement && recipe.GetRecipeElementRight() == leftElement))
            {
                foreach (WorldElement worldElement in worldElements)
                {
                    if (worldElement.GetElement() == recipe.GetRecipeOutputElement())
                    {
                        return;
                    }
                }
                GameObject newElement = Instantiate(worldElementObject, worldElementHolder);
                newElement.GetComponent<WorldElement>().Initialize(recipe.GetRecipeOutputElement());
                worldElements.Add(newElement.GetComponent<WorldElement>());
                mergeSucessScreen.SetActive(true);
                elementName.text = recipe.GetRecipeOutputElement().GetName();
                elementSpriteDisplay.sprite = recipe.GetRecipeOutputElement().GetSprite();
                mergeSucessScreenActive = true;
                break;
            }
        }
    }

    public void Update()
    {
        if (mergeSucessScreenActive)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                mergeSucessScreen.GetComponent<Animator>().SetTrigger("Hide");
                mergeSucessScreenActive = false;
            }
        }
        if(hoveringOverElement && !mergeSucessScreenActive)
        {
            elementNameDisplay.transform.localPosition = Vector3.Lerp(elementNameDisplay.transform.localPosition,
                new Vector3(0.0f, -478.86f), Time.deltaTime * 20f);
        }
        else
        {
            elementNameDisplay.transform.localPosition = Vector3.Lerp(elementNameDisplay.transform.localPosition,
                new Vector3(0.0f, -598.86f), Time.deltaTime * 20f);
        }
        elementNameDisplay.text = elementNameDisplayText;
    }

    public Element GetElement(string id)
    {
        foreach (Element element in elements)
        {
            if (element.GetID() == id)
            {
                return element;
            }
        }
        Debug.LogError("Error: Failed to find any elements with the ID \"" + id + "\"");
        return null;
    }
    public Recipe GetRecipe(string id)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.GetID() == id)
            {
                return recipe;
            }
        }
        Debug.LogError("Error: Failed to find any recipes with the ID \"" + id + "\"");
        return null;
    }

}
