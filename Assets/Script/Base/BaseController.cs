using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    public GameObject[] pool;
    [SerializeField] GameObject troopPrefab;
    [SerializeField][Range(0, 50)] int poolSize = 7;
    [SerializeField] GameObject spawner;
    [SerializeField] bool ableToSpawn = true;
    public int count = 0;

    public Slider timerSlider;
    [SerializeField] float timer = 3f;
    float currentTimer = 0;

    /// <summary>
    /// Calls method to populate pool
    /// </summary>
    void Awake()
    {
        PopulatePool();
        timerSlider.gameObject.SetActive(false);
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
            pool[i] = Instantiate(troopPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timerSlider.maxValue = timer;   // The number to go up to
        timerSlider.value = 0;  // Initial value
    }

    // Update is called once per frame
    void Update()
    {
        CreateTroop();
        if (timerSlider.gameObject.activeInHierarchy == true)
        {
            // fill the fill object on slider
            currentTimer += Time.deltaTime;
            timerSlider.value = currentTimer;
        } 

    }
    
    /// <summary>
    /// Create a obstacle with right moust click
    /// </summary>
    private void CreateTroop()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("TowerLayer"));

            if (didHit && hit.transform == transform)
            {
                StartCoroutine(SpawnTimer());
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
        if (ableToSpawn && count < poolSize)
        {
            ableToSpawn = false;
            timerSlider.gameObject.SetActive(true);
            currentTimer = 0;
            timerSlider.value = 0;
            yield return new WaitForSeconds(timer);
            SpawnTroop();
            ableToSpawn = true;
            timerSlider.gameObject.SetActive(false);  
        }   
    }

    /// <summary>
    /// Spawn troop
    /// </summary>
    private void SpawnTroop()
    {
        if (count >= poolSize)
        {
            ableToSpawn = false;
            return;
        }
        else
        {
            pool[count].transform.position = spawner.transform.position;
            pool[count].SetActive(true);

            count++;
        }   
    }
}
