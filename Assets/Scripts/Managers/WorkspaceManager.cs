using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceManager : MonoBehaviour
{
    public GameObject worldElementsHolder;
    public float movementSpeed;
    public float sizeChange = 0.005f; // Adjust this value for slower or less sensitive zooming
    public float size = 1f;
    public float min;
    public float max;
    float interactionNum = 0.25f;
    private Vector2 touchStartPos;
    private float initialSize;

    // Start is called before the first frame update
    void Start()
    {
        initialSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            size = 5f;
            worldElementsHolder.transform.position = new Vector2(0, 0);
        }

        // Platform-dependent code for Android
#if UNITY_ANDROID
        HandleAndroidInput();
#endif
        float movementX = mouseX;
        float movementY = mouseY;

        // Apply movements to the object
        worldElementsHolder.transform.Translate(Vector3.up * movementY * movementSpeed);
        worldElementsHolder.transform.Translate(Vector3.right * movementX * movementSpeed);

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && size < max) // forward
        {
            size += sizeChange;
            interactionNum += 0.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && size > min) // backwards
        {
            size -= sizeChange;
            interactionNum -= 0.1f;
        }

        Camera.main.orthographicSize = size;
    }

    // Platform-specific input handling for Android
#if UNITY_ANDROID
    private void HandleAndroidInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                touchStartPos = touch1.position - touch2.position;
                initialSize = size;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 touchCurrentPos = touch1.position - touch2.position;
                float deltaPinch = touchStartPos.magnitude - touchCurrentPos.magnitude;
                size = Mathf.Clamp(initialSize + deltaPinch * sizeChange, min, max);
                interactionNum += 0.1f;
            }
        }
    }
#endif
}
