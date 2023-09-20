using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceManager : MonoBehaviour
{
    public GameObject worldElementsHolder;
    public float movementSpeed;
    public float sizeChange;
    public float size = 1f;
    public float min;
    public float max;
    float interactionNum = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            size = 5f;
            worldElementsHolder.transform.position = new Vector2(0, 0);
        }
        
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float movementX = mouseX;
            float movementY = mouseY;

            // Apply movements to the object
            worldElementsHolder.transform.Translate(Vector3.up * movementY * movementSpeed);
            worldElementsHolder.transform.Translate(Vector3.right * movementX * movementSpeed);
        }

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
}
