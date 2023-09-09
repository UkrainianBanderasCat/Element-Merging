using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldElement : MonoBehaviour
{

    [SerializeField] private Element element;

    [SerializeField] private float hoverScale;
    [SerializeField] private float defaultScale;

    private bool dragging = false;
    private bool hovering = false;
    private bool hoveringOverElement = false;
    private Vector2 dragDelta;
    private Vector2 previousPosition;

    public WorldElement collidingWorldElement;

    public void Initialize(Element element)
    {
        this.element = element;
        GetComponent<SpriteRenderer>().sprite = element.GetSprite();
    }
 
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = element.GetSprite();
    }

    private void Update()
    {
        if (dragging)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + dragDelta;
            GetComponent<SpriteRenderer>().sortingOrder = 1000;
            GameManager.instance.selectedElement = this;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        if ((hovering && !dragging && GameManager.instance.selectedElement == null) || hoveringOverElement)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(hoverScale, hoverScale, hoverScale), Time.deltaTime * 20f);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale, defaultScale, defaultScale), Time.deltaTime * 20f);
        }
    }

    private void OnMouseOver()
    {
        hovering = true;
        GameManager.instance.hoveringOverElement = true;
        GameManager.instance.elementNameDisplayText = element.GetName();
        GameManager.instance.hoveredElement = this;

        if (GameManager.instance.selectedElement == null)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1000;
        }
    }
    private void OnMouseExit()
    {
        hovering = false;
        GameManager.instance.hoveringOverElement = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        GameManager.instance.selectedElement = null;
        GameManager.instance.hoveredElement = null;
    }
    private void OnMouseDown()
    {
        dragging = true;
        dragDelta = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        previousPosition = transform.position;

        if (GameManager.instance.playSoundEffects)
        {
            AudioSource.PlayClipAtPoint(GameManager.instance.elementGrabSound, new Vector2(0f, 0f), 0.3f);
        }
    }
    private void OnMouseUp()
    {
        dragging = false;
        hoveringOverElement = false;
        if (collidingWorldElement != null)
        {
            transform.position = previousPosition;
            List<Element> MergedElements = new List<Element>();
            MergedElements.Add(collidingWorldElement.GetElement());
            MergedElements.Add(element);
            GameManager.instance.MergeElements(MergedElements);
            collidingWorldElement = null;
        }

        if (!IsObjectInView())
        {
            // Return to the previous position
            transform.position = previousPosition;
        }

        if (GameManager.instance.playSoundEffects)
        {
            AudioSource.PlayClipAtPoint(GameManager.instance.elementDropSound, new Vector2(0f, 0f), 0.3f);
        }
    }

    private bool IsObjectInView()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Make sure you have a camera in your scene.");
            return false;
        }

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is within the viewport (0 to 1 in both x and y)
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dragging)
        {
            collidingWorldElement = collision.gameObject.GetComponent<WorldElement>();
            hoveringOverElement = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingWorldElement = null;
        hoveringOverElement = false;
    }


    public Element GetElement()
    {
        return element;
    }

}
