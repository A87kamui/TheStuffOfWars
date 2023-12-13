using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFight : MonoBehaviour
{
    public EnemyMover enemyMover;
    public GameObject target;
    public Animator animator;
    public AttackController attackController;
    [SerializeField] Vector3 towardsTarget;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField][Range(0.0f, 10.0f)] public float speed = 1.0f;
    [SerializeField] float obstacleBumpSpeed;

    float radiusOfSatisfaction = 4.0f;
    bool astar = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            
            attackController.target = target;
            enemyMover.StopCoroutines();
            RunKinematicArrive();
            if((target.transform.position - transform.position).magnitude > 30)
            {
                target = null;
            }
            astar = false;
        }
        else
        {
            if(!astar)
            {
                attackController.target = null;
                enemyMover.RecalcuatePath(true);
                astar = true;
            }
        }
    }
    /// <summary>
    /// Recalculates path
    /// </summary>
    void RunKinematicArrive()
    {
        towardsTarget = target.transform.position - transform.position;

        if (target.tag == "Tower")
        {
            radiusOfSatisfaction = 7.0f;
        }
        else
        {
            radiusOfSatisfaction = 4.0f;
        }

        // Check to see if the character is close enough to the target
        if (towardsTarget.magnitude <= radiusOfSatisfaction)
        {
            //Debug.Log("Close enough");
            // Close enough to stop
            towardsTarget.Normalize();

            Quaternion targertRot = Quaternion.LookRotation(towardsTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, targertRot, turnSpeed);
            animator.SetTrigger("isAttacking");
            return;
        }

        // Normalize vector so we only use the direction
        towardsTarget.Normalize();

        // Face the target
        Quaternion targetRotation = Quaternion.LookRotation(towardsTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed);

        // Move along our forward vector (the direction we're facing)
        Vector3 newPosition = transform.position;
        newPosition += transform.forward * speed * Time.deltaTime;

        transform.position = newPosition;
    }
}