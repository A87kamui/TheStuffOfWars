using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 5.0f)] float speed = 1.0f;
    [SerializeField] float obstacleBumpSpeed = 1.0f;
    [SerializeField] Animator animator;

    List<Node> path = new List<Node>();
    GridManager gridManager;
    PathFinder pathFinder;
    public GameObject selected;

    /// <summary>
    /// Inititialize variables
    /// </summary>
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();    // Access to GridManager script
        pathFinder = FindObjectOfType<PathFinder>();  // Access to PathFinder script
    }

    /// <summary>
    /// Recalculates path
    /// </summary>
    public void MovePath(List<Node> pathNode)
    {
        StopAllCoroutines();    //Stops all coroutine

        // Clear current path stored
        // So that path starts empty  
        path.Clear();
        path = pathNode;
        StartCoroutine(FollowPath());   // Start a coroutine
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
            while (travelPercent < 1.0f)
            {
                // Each frame, add Time.deltaTime
                travelPercent += Time.deltaTime * speed;
                // Update current position
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);

                // Wait for the end of the frame to continue
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    /// <summary>
    /// Actions taken when enemy reaches end of the path
    /// </summary>
    private void FinishPath()
    {
        animator.SetBool("isWalking", false);
        // Deactivate game object after reaching the end of the path
        //gameObject.SetActive(false);
    }

    /// <summary>
    /// Move around obsticles
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag != "Obstacle")
        {
            return;
        }
            
        // Calculate vector from player to obstacle
        Vector3 toObstacle = collision.gameObject.transform.position - transform.position;
        toObstacle.Normalize();
        toObstacle.y = 0f;


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
}