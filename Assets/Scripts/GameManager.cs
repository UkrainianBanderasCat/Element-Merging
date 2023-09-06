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

    //Callback trace for the WorldElements (we can't add them in the foreach)
    public Stack<WorldElement> WaitingElements = new Stack<WorldElement>();

    public GameManager()
    {
        instance = this;
    }

    public void Start()
    {
        if (SaveManager.instance.HasSaveData())
        {
            SaveManager.instance.Load();
        }
        else
        {
            LoadStarterElements();
        }
    }

    public void MergeElements(List<Element> elements)
    {
        foreach (Recipe recipe in RecipeManager.instance.recipes)
        {
            
            // Matching Elements with Recipe
            if (recipe.CanCraftWith(elements))
            {
                bool hasBeenDone = true;

                foreach(Element element in recipe.GetRecipeOutputElements())
                {
                    bool containsElement = false;
                    foreach(WorldElement worldElement in worldElements)
                    {
                        if(worldElement.GetElement() == element)
                        {
                            containsElement = true;
                        }
                        
                    }

                    if(!containsElement)
                    {
                        hasBeenDone = false;
                        CreateElement(element, new Vector2(0f + Random.Range(0f, 2f), 0f + Random.Range(0f, 2f)));
                    }
                }

                Release();


                if (hasBeenDone)
                {
                    AudioSource.PlayClipAtPoint(elementMergeFailureSound, new Vector2(0f, 0f), 0.4f);
                    return;
                }


                ShowMergeSucessScreen();

                // Playing Merge Audio
                AudioSource.PlayClipAtPoint(elementMergeSound, new Vector2(0f, 0f));
                
                SaveManager.instance.Save();
                return;
            }
        }
    }

    public GameObject CreateElement(Element element, Vector2 position)
    {
        // Instantiating New Element
        GameObject newElement = Instantiate(worldElementObject, worldElementHolder);
        newElement.GetComponent<WorldElement>().Initialize(element);
        Hold(newElement);
        elementName.text = element.GetName();
        elementSpriteDisplay.sprite = element.GetSprite();

        newElement.transform.position = position;

        return newElement;
    }

    void Hold(GameObject newElement)
    {
        WaitingElements.Push(newElement.GetComponent<WorldElement>());
    }

    public void Release()
    {
        for(int i = 0; i<WaitingElements.Count; i++)
        {
            worldElements.Add(WaitingElements.Peek());
            WaitingElements.Pop();
        }

        WaitingElements.Clear();
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

    public bool isMergeSucessScreenActive()
    {
        return mergeSucessScreen;
    }

    public void ShowMergeSucessScreen()
    {
        mergeSucessScreen.SetActive(true);
        mergeSucessScreenActive = true;
    }

    public void LoadStarterElements()
    {
        CreateElement(ElementManager.instance.GetElement("water"), new Vector2(-4f, 0f));
        CreateElement(ElementManager.instance.GetElement("soil"), new Vector2(4f, 0f));
    }

}
