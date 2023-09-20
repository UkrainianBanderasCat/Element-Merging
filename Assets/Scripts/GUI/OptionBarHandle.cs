using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionBarHandle : MonoBehaviour
{

    [SerializeField] private float outPosition;
    [SerializeField] private float inPosition;

    private bool slideDirection;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    void Update()
    {
        if (slideDirection)
        {
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, new Vector2(outPosition, rectTransform.localPosition.y), Time.deltaTime * 7f);
        }
        else
        {
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, new Vector2(inPosition, rectTransform.localPosition.y), Time.deltaTime * 7f);
        }
    }

    public void slide()
    {
        slideDirection = !slideDirection;
    }
    public void slideOut()
    {
        slideDirection = true;
    }
    public void slideIn()
    {
        slideDirection = false;
    }

}
