using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float camSpeed = 60.0f;
    //[SerializeField] float boundary = 100.0f;   // Use with Mouse position
    [SerializeField] float xLowerBoundary = 30.0f;
    [SerializeField] float yLowerBoundary = 50.0f;
    [SerializeField] Vector2Int camLimit;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey("w"))// || Input.mousePosition.y >= Screen.height - boundary); // Input.mousePosition.y >= Screen.height - boundary)
        {
            position.x -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))// || Input.mousePosition.y <= boundary)
        {
            position.x += camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))// || Input.mousePosition.x >= Screen.width - boundary)
        {
            position.z += camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))// || Input.mousePosition.x <= boundary)
        {
            position.z -= camSpeed * Time.deltaTime;
        }

        position.x = Mathf.Clamp(position.x, xLowerBoundary, camLimit.x);
        position.z = Mathf.Clamp(position.z, yLowerBoundary, camLimit.y);

        transform.position = position;
    }
}
