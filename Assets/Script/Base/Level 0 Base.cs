using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0Base : MonoBehaviour
{
    GameObject[] pool;
    [SerializeField] GameObject cubePrefab;
    [SerializeField][Range(0, 50)] int poolSize = 10;
    [SerializeField] GameObject spawner;
    [SerializeField] float timer = 3.0f;
    [SerializeField] bool ableToSpawn = true;
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
            bool didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("TowerLayer"));

            if (didHit && hit.collider.tag == "Tower")
            {
                StartCoroutine(SpawnTimer());
                /*if (count >= poolSize)
                {
                    count = 0;
                    pool[count].SetActive(false);
                }

                pool[count].transform.position = spawner.transform.position;

                if (timer <= 0)
                {
                    pool[count].SetActive(true);
                    count++;
                }//*/
            }   
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// Timer to wait
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnTimer()
    {
        Debug.Log("SpawnTimer called");
        if (ableToSpawn)
        {
            Debug.Log("waiting");
            ableToSpawn = false;
            yield return new WaitForSeconds(timer);
            SpawnTroop();
            ableToSpawn = true;
            Debug.Log("able to spawn = true");
        }   
    }

    /// <summary>
    /// Spawn troop
    /// </summary>
    private void SpawnTroop()
    {

        Debug.Log("SpawnTroop");
        if (count >= poolSize)
        {
            // Need to fix.
            count = 0;
            pool[count].SetActive(false);
        }

        pool[count].transform.position = spawner.transform.position;

        pool[count].SetActive(true);
        count++;
    }
}
