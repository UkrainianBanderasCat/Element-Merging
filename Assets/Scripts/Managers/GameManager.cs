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
    [SerializeField] private GameObject elementNameDisplayHolder;
    [SerializeField] private GameObject elementSpriteDisplayHolder;
    [SerializeField] private GameObject elementNameDisplayPrefab;
    [SerializeField] private GameObject elementSpriteDisplayPrefab;




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
    public bool playSoundEffects;

    //Callback trace for the WorldElements (we can't add them in the foreach)
    public List<WorldElement> WaitingElements = new List<WorldElement>();


    //Name and sprites used to update the merge successful screen.
    [SerializeField] private List<string> elementNames = new();
    [SerializeField] private List<Sprite> elementSpriteDisplay = new();

    public GameManager() => instance = this;

    public void Start()
    {
        ModManager.instance.InitMods();
        MilestonesManager.instance.InitMilestones();

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
                        CreateElement(element, new Vector2(0f + Random.Range(0f, 2f), 0f + Random.Range(0f, 2f)), false);
                    }
                }

                Release();


                if (hasBeenDone)
                {
                    AudioSource.PlayClipAtPoint(elementMergeFailureSound, new Vector2(0f, 0f), 0.4f);
                    return;
                }

                ShowMergeSucessScreen();
                updateMergeSucessScreen();

                NewMergedElement.Invoke();

                // Playing Merge Audio
                if (playSoundEffects)
                {
                    AudioSource.PlayClipAtPoint(elementMergeSound, new Vector2(0f, 0f));
                }

                SaveManager.instance.Save();
                return;
            }
        }
    }

    public GameObject CreateElement(Element element, Vector2 position, bool silent = true)
    {
        // Instantiating New Element
        if (element == null)
        {
            Debug.LogError("Error: Unable to create null element");
            return null;
        }

        GameObject newElement = Instantiate(worldElementObject, worldElementHolder);
        newElement.GetComponent<WorldElement>().Initialize(element);
        Hold(newElement);
        if (!silent)
        {
            elementNames.Add(element.GetName());
            elementSpriteDisplay.Add(element.GetSprite());
        }

        newElement.transform.position = position;

        return newElement;
    }

    void Hold(GameObject newElement)
    {
        WaitingElements.Add(newElement.GetComponent<WorldElement>());
    }

    public void Release()
    {
        WaitingElements.ForEach(element => worldElements.Add(element));
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
                ClearElementNames();
                ClearElementSprites();
            }
        }

        // Element Name Display Management
        if (hoveringOverElement && !mergeSucessScreenActive)
        {
            Vector3 hoveredElementPosition = hoveredElement.gameObject.transform.position + new Vector3(.75f,.5f);
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

    void updateMergeSucessScreen()
    {
        int j = 0;
        int elementNamesCount = elementNames.Count;

        float step = (4 / elementNamesCount);

        float pos = -(elementNamesCount / 2);

        //Debug.Log(pos);

        int childCount = elementSpriteDisplayHolder.transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            GameObject.Destroy(elementSpriteDisplayHolder.transform.GetChild(i).gameObject);
            GameObject.Destroy(elementNameDisplayHolder.transform.GetChild(i).gameObject);

        }

        for (int i = 0; i < elementNamesCount; i++)
        {
            GameObject spriteDisplay = GameObject.Instantiate(elementSpriteDisplayPrefab, elementSpriteDisplayHolder.transform);
            GameObject nameDisplay = GameObject.Instantiate(elementNameDisplayPrefab, elementNameDisplayHolder.transform);

            spriteDisplay.transform.localPosition = new Vector2(250f * pos, elementSpriteDisplayHolder.transform.GetChild(j).transform.localPosition.y);
            nameDisplay.transform.localPosition = new Vector2(250f * pos, elementNameDisplayHolder.transform.GetChild(j).transform.localPosition.y);

            spriteDisplay.GetComponent<Image>().sprite = elementSpriteDisplay[i];
            nameDisplay.GetComponent<TMP_Text>().text = elementNames[i];

            pos += step;
        }
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
        ClearElementNames();
        ClearElementSprites();
        Release();
    }

    void ClearElementNames()
    {
        elementNames.Clear();
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in elementNameDisplayHolder.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    void ClearElementSprites()
    {
        elementSpriteDisplay.Clear();
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in elementSpriteDisplayHolder.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

}
