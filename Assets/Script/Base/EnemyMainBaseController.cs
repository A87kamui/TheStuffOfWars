using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMainBaseController : MonoBehaviour
{
    public GameObject[] pool;
    [SerializeField] GameObject troopPrefab;
    [SerializeField][Range(0, 50)] int poolSize = 7;
    [SerializeField] GameObject[] spawner;
    [SerializeField] bool ableToSpawn = true;
    public int count = 0;
    int spawnerNumber;

    //public Slider timerSlider;
    [SerializeField] float timer = 3f;
    //float currentTimer = 0;

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
            pool[i] = Instantiate(troopPrefab, transform);
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
        CreateTroop();
    }

    /// <summary>
    /// Create a obstacle with right moust click
    /// </summary>
    private void CreateTroop()
    {
        if (count < poolSize)
        {
            StartCoroutine(SpawnTimer());
        }
        
    }

    /// <summary>
    /// Timer to wait
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnTimer()
    {
        if (ableToSpawn)
        {
            ableToSpawn = false;
            /*timerSlider.gameObject.SetActive(true);
            currentTimer = 0;
            timerSlider.value = 0;/*/
            yield return new WaitForSeconds(timer);
            SpawnTroop();          
            //timerSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Spawn troop
    /// </summary>
    private void SpawnTroop()
    {
        // Loop to active troops back from the start
        if (count >= poolSize)
        {
            ableToSpawn = false;
            return;
        }
        else
        {
            spawnerNumber = Random.Range(0, 2);
            pool[count].transform.position = spawner[spawnerNumber].transform.position;
            pool[count].SetActive(true);
            count++;
            ableToSpawn = true;
        }   
    }
}