using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 5.0f)] public float speed = 1.0f;

    List<Node> path = new List<Node>();
    GridManager gridManager;
    PathFinder pathFinder;
    [SerializeField] Animator animator;
    [SerializeField] Vector3 towardsTarget;

    float radiusOfSatisfaction = 1.5f;
    bool reachedRadiusOfSatisfaction = false;
    public Transform playerTower; 
    

    /// <summary>
    /// When object is enabled call methods to run the object
    /// </summary>
    void OnEnable()
    {
        ReturnToStart();    // Set object at start
        RecalcuatePath(true); // Recalculates Path
    }

    /// <summary>
    /// Inititialize variables
    /// </summary>
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();    // Access to GridManager script
        pathFinder = FindObjectOfType<PathFinder>();  // Access to PathFinder script
        playerTower = GameObject.Find("Player Tower_Standing").transform;
    }

    /// <summary>
    /// Recalculates path
    /// </summary>
    public void RecalcuatePath(bool resetPath)
    {
        Vector3 coordinates = Vector3.zero;

        // Check if reseting path is true
        if (resetPath)
        {
            coordinates = playerTower.position;
        }
        else
        {
            coordinates = transform.position;
        }

        StopAllCoroutines();    //Stops all coroutine
        // Clear current path stored
        // So that path starts empty  
        path.Clear();
        path = pathFinder.GetEnemyNewPath(coordinates);

        StartCoroutine(FollowPath());   // Start a coroutine
    }

    /// <summary>
    /// Return object back to start of path
    /// </summary>
    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    /// <summary>
    /// Move character following the given waypoint with a wait time of 1 seconds
    /// for each move to the next waypoint
    /// </summary>
    IEnumerator FollowPath()
    {
        animator.SetBool("isWalking", true);
        //yield return new WaitForSeconds(1.0f); = Wait for 1 second then continue
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position; // Store starting position
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);  // Store ending position
            float travelPercent = 0.0f; // Value to represent the travel percent in Vector3.LERP

            // Always face the endPosition
            transform.LookAt(endPosition);

            // updates travelPercent
            // and position until it reaches endPosition
            while (travelPercent < 1.0f && !reachedRadiusOfSatisfaction)
            {
                // Each frame, add Time.deltaTime
                travelPercent += Time.deltaTime * speed;
                // Update current position
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                reachedRadiusOfSatisfaction = CheckRadiusOfSatisfaction();
                // Wait for the end of the frame to continue
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private bool CheckRadiusOfSatisfaction()
    {
        towardsTarget = playerTower.position - transform.position;

        if (towardsTarget.magnitude <= radiusOfSatisfaction)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Actions taken when enemy reaches end of the path
    /// </summary>
    private void FinishPath()
    {
        //animator.SetBool("isWalking", false);
        // Deactivate game object after reaching the end of the path
        //gameObject.SetActive(false);
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
}