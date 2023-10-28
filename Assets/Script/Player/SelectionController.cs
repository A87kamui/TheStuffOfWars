using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> selectedList = new List<GameObject>();
    private Vector3 anchor;
    [SerializeField] bool isSelected = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-10f, 0.5f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the positions from where the selection will be anchored or start and expand from
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isSelected)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("HitLayer"));
                transform.position = hit.point;
                anchor = hit.point;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("TileLayer"));
                //Set Destination
                foreach (GameObject gameObject in selectedList)
                {
                    if (didHit)
                    {
                        gameObject.GetComponent<PathFinder>().GetNewPath(hit.transform.position);
                    }
                }
            }
        }
        //Expands the selection window towards the mouse
        if (Input.GetKey(KeyCode.Mouse0) && !isSelected)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("HitLayer"));
            transform.localScale = new Vector3(hit.point.x - anchor.x, 0.5f, hit.point.z - anchor.z);
            transform.position = new Vector3(anchor.x + transform.localScale.x / 2, 0.25f, anchor.z + transform.localScale.z / 2);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && !isSelected)
        {
            if (selectedList.Count > 0)
            {
                isSelected = true;

            }

            transform.localScale = new Vector3(0, 1.0f, 0);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isSelected = false;
            selectedList.Clear();
        }
    }

    //Adds the troops inside the selection window to the list of selected troops
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable") && !selectedList.Contains(other.gameObject))
        {
            selectedList.Add(other.gameObject);
        }
    }


    //Remove the troops that are not inside the selection window from the list of selected troops
    private void OnTriggerExit(Collider other)
    {
        if (!isSelected && other.gameObject.CompareTag("Selectable") && selectedList.Contains(other.gameObject))
        {
            selectedList.Remove(other.gameObject);
        }
    }
}