using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField][Range(0, 50)] int poolSize = 5;
    [SerializeField][Range(0.1f, 30.0f)] float spawnTimer = 1.0f;

    GameObject[] pool;

    /// <summary>
    /// Calls method to populate pool
    /// </summary>
    void Awake()
    {
        PopulatePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start a coroutine with method stated
        StartCoroutine(SpawnEnemy());
    }

    /// <summary>
    /// Instantiate objects to pool array
    /// </summary>
    void PopulatePool()
    {
        pool = new GameObject[poolSize];
        if (pool == null)
        {
            return;
        }
        for (int i = 0; i < pool.Length; i++)
        {
            // Instantiate an enemy at parent's transform position
            pool[i] = Instantiate(enemyPrefab, transform);

            // Set object active to false
            pool[i].SetActive(false);
        }
    }

    /// <summary>
    /// Instantiate the enemy object while a condition is true
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();

            // Waits for certain amount of seconds before continuing
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    /// <summary>
    /// Checks for first inactive object in pool
    /// Set it active and return early
    /// </summary>
    private void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }
}
