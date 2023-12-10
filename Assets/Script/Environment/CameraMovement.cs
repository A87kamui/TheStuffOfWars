using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float camSpeed = 60.0f;
    [SerializeField] float boundary = 100.0f;
    [SerializeField] Vector2Int camLimit;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (Input.mousePosition.y >= Screen.height - boundary)
        {
            position.x -= camSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= boundary)
        {
            position.x += camSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - boundary)
        {
            position.z += camSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= boundary)
        {
            position.z -= camSpeed * Time.deltaTime;
        }

        position.x = Mathf.Clamp(position.x, 70, camLimit.x);
        position.z = Mathf.Clamp(position.z, 60, camLimit.y);

        transform.position = position;
    }
}
