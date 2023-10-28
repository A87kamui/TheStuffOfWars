using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Base : MonoBehaviour
{
    GameObject[] pool;
    [SerializeField] GameObject cubePrefab;
    [SerializeField][Range(0, 50)] int poolSize = 10;
    [SerializeField] GameObject spawner;
    int count = 0;

    /// <summary>
    /// Calls method to populate pool
    /// </summary>
    void Awake()
    {
        PopulatePool();
    }


    /// <summary>
    /// Instantiate objects to pool array
    /// </summary>
    private void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            // Instantiate an enemy at parent's transform position
            pool[i] = Instantiate(cubePrefab, transform);
            pool[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CreateObstacle();
    }

    /// <summary>
    /// Create a obstacle with right moust click
    /// </summary>
    private void CreateObstacle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("TowerLayer"));
            Debug.Log("Create Obstacle");
            //Debug.Log("Hit " + hit.collider.gameObject.ToString());

            if (hit.colliderInstanceID != null && hit.collider.tag == "Tower")
            {
                // Add timer function

                Debug.Log("Hit tower tag");

                if (count >= poolSize)
                {
                    count = 0;
                    pool[count].SetActive(false);
                }

                pool[count].transform.position = spawner.transform.position;
                pool[count].SetActive(true);
                count++;
            }
            else
            {
                Debug.Log("Return");
                return;
            }
        }
    }
}
