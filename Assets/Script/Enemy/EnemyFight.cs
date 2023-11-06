using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFight : MonoBehaviour
{
    public EnemyMover enemyMover;
    public Transform playerTroop;


    [SerializeField] Vector3 towardsTarget;
    [SerializeField] Transform parentTransform;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField][Range(0.0f, 10.0f)] public float speed = 10.0f;

    float radiusOfSatisfaction = 1.0f;
    bool foundPlayer = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (foundPlayer)
        {
            RunKinematicArrive();
        }
    }

    /// <summary>
    /// Detect player inside collision 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Selectable" && !foundPlayer)
        {
            playerTroop = other.gameObject.transform;
            enemyMover.StopCoroutines();
            foundPlayer = true;
        }
    }//*/



    /// <summary>
    /// Recalculates path
    /// </summary>
    void RunKinematicArrive()
    {

        towardsTarget = playerTroop.position - parentTransform.position;

        // Check to see if the character is close enough to the target
        if (towardsTarget.magnitude <= radiusOfSatisfaction)
        {
            // Close enough to stop
            return;
        }

        // Normalize vector so we only use the direction
        towardsTarget.Normalize();

        // Face the target
        Quaternion targetRotation = Quaternion.LookRotation(towardsTarget);
        parentTransform.rotation = Quaternion.Lerp(parentTransform.rotation, targetRotation, turnSpeed);

        // Move along our forward vector (the direction we're facing)
        Vector3 newPosition = parentTransform.position;
        newPosition += parentTransform.forward * speed * Time.deltaTime;

        parentTransform.position = newPosition;
    }

    /// <summary>
    /// Compare object that exit to what we are pursuing
    /// If object exits, go towards player base
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Selectable" && foundPlayer)
        {
            if (other.gameObject == playerTroop.gameObject)
            {
                foundPlayer = false;
                enemyMover.RecalcuatePath(true);
            }   
        }
    }
}