using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> selectedList = new List<GameObject>();
    private Vector3 anchor;
    [SerializeField] bool isSelected = false;
    public GameObject indicator;
    float timer = 3.0f;

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
                //Ignore trees or any other obstcale. Hit the plane, troops or towers
                int layerMask = 1 << 9;
                layerMask = ~layerMask;
                bool didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
                
                if(hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.name == "Enemy Tower_Standing")
                {
                    foreach(GameObject gb in selectedList)
                    {
                        AttackController attack = gb.GetComponentInChildren<AttackController>();
                        attack.target = hit.transform.gameObject;
                        PlayerFight fight = gb.GetComponent<PlayerFight>();
                        fight.enemyMover = hit.rigidbody.gameObject.GetComponent<EnemyMover>();
                        fight.target = hit.rigidbody.gameObject;
                    }
                }
                else
                {
                    if(hit.transform.gameObject.name != "Player Tower_Standing")
                    {
                        Vector2Int temp = GridManager.instance.GetCoordinatesFromPosition(hit.point);
                        Node selectedNode = GridManager.instance.GetNode(temp);

                        indicator.transform.position = new Vector3(hit.point.x, 1.5f, hit.point.z);
                        indicator.SetActive(true);
                        timer = 3.0f;

                        if (selectedNode != null && GridManager.instance.GetNode(temp).isWalkable)
                        {
                            //Set Destination of Troops selected
                            foreach (GameObject gb in selectedList)
                            {
                                if (didHit)
                                {
                                    AttackController attack = gb.GetComponentInChildren<AttackController>();
                                    attack.target = null;
                                    if (gb.activeSelf)
                                    {
                                        gb.GetComponent<PathFinder>().GetNewPath(hit.point);
                                    }


                                }
                            }
                        }
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
            transform.position = new Vector3(-10f, 0.5f, -10f);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isSelected = false;
            foreach(GameObject gb in selectedList)
            {
                gb.GetComponent<PlayerMover>().selected.SetActive(false);
            }
            selectedList.Clear();
        }

        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            indicator.SetActive(false);
        }

    }

    //Adds the troops inside the selection window to the list of selected troops
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable") && !selectedList.Contains(other.gameObject))
        {
            selectedList.Add(other.gameObject);
            other.gameObject.GetComponent<PlayerMover>().selected.SetActive(true);
        }
    }


    //Remove the troops that are not inside the selection window from the list of selected troops
    private void OnTriggerExit(Collider other)
    {
        if (!isSelected && other.gameObject.CompareTag("Selectable") && selectedList.Contains(other.gameObject))
        {
            selectedList.Remove(other.gameObject);
            other.GetComponent<PlayerMover>().selected.SetActive(false);
        }
    }
}