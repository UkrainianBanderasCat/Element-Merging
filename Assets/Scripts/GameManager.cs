using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;  

    public WorldElement selectedElement;
    public WorldElement hoveredElement;

    public UnityAction NewMergedElement;
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

    public Vector3 elementNameDisplayTextOffset;

    public AudioClip newElementSound;
    public AudioClip pickupSound;
    public AudioClip dropSound;
    public AudioClip existingElementSound;


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
                        AudioSource.PlayClipAtPoint(existingElementSound, new Vector2(0f, 0f), 0.4f);
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

                AudioSource.PlayClipAtPoint(newElementSound, new Vector2(0f, 0f));
                break;
            }

            else
            {
                AudioSource.PlayClipAtPoint(existingElementSound, new Vector2(0f, 0f), 0.4f);
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
                NewMergedElement.Invoke();
            }
        }

        if(hoveringOverElement && !mergeSucessScreenActive)
        {
            Vector3 hoveredElementPosition = hoveredElement.gameObject.transform.position;
            elementNameDisplay.transform.position = hoveredElementPosition + elementNameDisplayTextOffset;

            //--Fix animation for text--
            //elementNameDisplay.transform.position = Vector3.Lerp(hoveredElementPosition,
            //    hoveredElementPosition + elementNameDisplayTextOffset, Time.deltaTime * 20f);
        }

        else
        {
            elementNameDisplay.transform.position = new Vector3(0f, -100f);

            //--Fix animation for text--
            //elementNameDisplay.transform.localPosition = Vector3.Lerp(elementNameDisplay.transform.localPosition,
            //    new Vector3(0.0f, -598.86f), Time.deltaTime * 20f);
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
