using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBaseController : MonoBehaviour
{
    GameObject[] pool;
    [SerializeField] GameObject troopPrefab;
    [SerializeField][Range(0, 50)] int poolSize = 7;
    [SerializeField] GameObject spawner;
    [SerializeField] bool ableToSpawn = true;
    int count = 0;

    public Slider timerSlider;
    [SerializeField] float timer = 3f;
    float currentTimer = 0;

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
            pool[i] = Instantiate(troopPrefab, spawner.transform);
            pool[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timerSlider.maxValue = timer;   // The number to go up to
        timerSlider.value = 0;  // Initial value
        timerSlider.gameObject.SetActive(false);
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
        StartCoroutine(SpawnTimer());
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
        // Loop to active troops back from the start
        if (count >= poolSize)
        {
            ableToSpawn = false;
            return;
            // Need to fix.
            count = 0;
            pool[count].SetActive(false);
        }

        //---------------------------NEED to add check for number of troops active
        //---------------------------If number of troops active < poolSize = keep spawning new troops


        pool[count].transform.position = spawner.transform.position;

        pool[count].SetActive(true);
        count++;
    }
}
