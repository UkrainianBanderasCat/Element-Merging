using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Manager Script References
    public static GameManager instance;
    private ElementManager elementManager;
    private RecipeManager recipeManager;

    // Interaction Data
    public WorldElement selectedElement;
    public WorldElement hoveredElement;

    // World Data
    public List<WorldElement> worldElements;

    // Antonin pls name this
    public UnityAction NewMergedElement;

    // GameObject and Component References
    [SerializeField] private GameObject worldElementObject;
    [SerializeField] private Transform worldElementHolder;
    [SerializeField] private GameObject mergeSucessScreen;
    [SerializeField] private TextMeshProUGUI elementName;
    [SerializeField] private Image elementSpriteDisplay;
    [SerializeField] private TextMeshProUGUI elementNameDisplay;

    // GUI Data
    public string elementNameDisplayText;
    public bool hoveringOverElement;
    private bool mergeSucessScreenActive;
    public Vector3 elementNameDisplayTextOffset;

    // Audio Clip Data
    public AudioClip elementMergeSound;
    public AudioClip elementGrabSound;
    public AudioClip elementDropSound;
    public AudioClip elementMergeFailureSound;

    public GameManager()
    {
        instance = this;
    }

    public void MergeElements(Element leftElement, Element rightElement)
    {
        foreach (Recipe recipe in recipeManager.recipes)
        {
            // Matching Elements with Recipe
            if ((recipe.GetRecipeElementLeft() == leftElement && recipe.GetRecipeElementRight() == rightElement)
                || (recipe.GetRecipeElementLeft() == rightElement && recipe.GetRecipeElementRight() == leftElement))
            {
                // Checking if element already has been merged
                foreach (WorldElement worldElement in worldElements)
                {
                    if (worldElement.GetElement() == recipe.GetRecipeOutputElement())
                    {
                        AudioSource.PlayClipAtPoint(elementMergeFailureSound, new Vector2(0f, 0f), 0.4f);
                        return;
                    }
                }
                // Instantiating New Element
                GameObject newElement = Instantiate(worldElementObject, worldElementHolder);
                newElement.GetComponent<WorldElement>().Initialize(recipe.GetRecipeOutputElement());
                worldElements.Add(newElement.GetComponent<WorldElement>());
                mergeSucessScreen.SetActive(true);
                elementName.text = recipe.GetRecipeOutputElement().GetName();
                elementSpriteDisplay.sprite = recipe.GetRecipeOutputElement().GetSprite();
                mergeSucessScreenActive = true;
                // Playing Merge Audio
                AudioSource.PlayClipAtPoint(elementMergeSound, new Vector2(0f, 0f));
                return;
            }
        }
    }

    public void Update()
    {
        // Merge Sucess Screen Management
        if (mergeSucessScreenActive)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                mergeSucessScreen.GetComponent<Animator>().SetTrigger("Hide");
                mergeSucessScreenActive = false;
                NewMergedElement.Invoke();
            }
        }

        // Element Name Display Management
        if (hoveringOverElement && !mergeSucessScreenActive)
        {
            Vector3 hoveredElementPosition = hoveredElement.gameObject.transform.position;
            elementNameDisplay.transform.position = hoveredElementPosition + elementNameDisplayTextOffset;
            //--Fixed animation for text--
            //elementNameDisplay.transform.position = Vector3.Lerp(hoveredElementPosition,
            //    hoveredElementPosition + elementNameDisplayTextOffset, Time.deltaTime * 20f);
            //--Old Code--
        }

        else
        {
            elementNameDisplay.transform.position = new Vector3(0f, -100f);
            //--Fixed animation for text--
            //elementNameDisplay.transform.localPosition = Vector3.Lerp(elementNameDisplay.transform.localPosition,
            //    new Vector3(0.0f, -598.86f), Time.deltaTime * 20f);
            //--Old Code--
        }
        elementNameDisplay.text = elementNameDisplayText;
    }

}
