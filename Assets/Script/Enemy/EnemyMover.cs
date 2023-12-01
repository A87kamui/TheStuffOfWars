using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 5.0f)] public float speed = 1.0f;

    public List<Node> path = new List<Node>();
    GridManager gridManager;
    PathFinder pathFinder;
    [SerializeField] Animator animator;
    [SerializeField] Vector3 towardsTarget;
    [SerializeField] float obstacleBumpSpeed = 1.0f;

    float radiusOfSatisfaction = 1.5f;
    bool reachedRadiusOfSatisfaction = false;
    public Transform playerTower;
    public bool isMoving = false;

    /// <summary>
    /// When object is enabled call methods to run the object
    /// </summary>
    void OnEnable()
    {
        //ReturnToStart();    // Set object at start
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
    /// Move character following the given waypoint with a wait time of 1 seconds
    /// for each move to the next waypoint
    /// </summary>
    IEnumerator FollowPath()
    {
        isMoving = true;
        animator.SetBool("isWalking", true);
        //yield return new WaitForSeconds(1.0f); = Wait for 1 second then continue
        for (int i = 1; i < path.Count-1; i++)
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

    /// <summary>
    /// Move around obsticles
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag != "Obstacle" || collision.gameObject.tag != "Enemy" || collision.gameObject.tag != "Selectable")
            return;

        // Calculate vector from player to obstacle
        Vector3 toObstacle = collision.gameObject.transform.position - transform.position;
        toObstacle.Normalize();
        toObstacle.y = 0f;

        //Debug.DrawRay(trans.position + Vector3.up, toObstacle, Color.yellow);
        //Debug.DrawRay(trans.position + Vector3.up, trans.right, Color.cyan);

        float dot = Vector3.Dot(transform.right, toObstacle);
        //print(dot);

        // Obstacle is on the left of the obstacle -> push player right
        if (dot < 0f)
        {
            transform.position += transform.right * obstacleBumpSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * -1f * obstacleBumpSpeed * Time.deltaTime;
        }
    }//*/

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
        isMoving = false;
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
}