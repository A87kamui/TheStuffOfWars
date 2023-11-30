using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasesController : MonoBehaviour
{
    GameObject[] pool;
    [SerializeField] GameObject troopPrefab;
    [SerializeField][Range(0, 50)] int poolSize = 7;
    [SerializeField] GameObject[] spawner;
    [SerializeField] bool ableToSpawn = true;
    int count = 0;
    int spawnerNumber;

    [SerializeField] float timer = 3f;

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
            yield return new WaitForSeconds(timer);
            SpawnTroop();
            ableToSpawn = true;
        }
    }

    /// <summary>
    /// Spawn troop
    /// </summary>
    private void SpawnTroop()
    {
        if (count >= poolSize)
        {
            // Need to fix.
            count = 0;
            pool[count].SetActive(false);
        }

        spawnerNumber = Random.Range(0, 2);
        pool[count].transform.position = spawner[spawnerNumber].transform.position;
        pool[count].SetActive(true);
        count++;
    }
}
