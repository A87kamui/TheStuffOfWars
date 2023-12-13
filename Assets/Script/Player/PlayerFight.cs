using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFight : MonoBehaviour
{
    //Enemy targeting variables
    public EnemyMover enemyMover;
    public GameObject target;

    //Player variables
    public PathFinder pathFinder;
    public PlayerMover playerMover;
    public Animator animator;
    public AttackController attackController;
    [SerializeField] Vector3 towardsTarget;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField][Range(0.0f, 10.0f)] public float speed = 1.0f;
    [SerializeField] float obstacleBumpSpeed;

    float radiusOfSatisfaction = 4f;
    public bool ranOnce = false;

    private void Start()
    {
        attackController = GetComponentInChildren<AttackController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            attackController.target = target;
            RaycastHit hit;
            Vector3 ray = target.transform.position - transform.position;
            bool didHit = Physics.Raycast(transform.position + new Vector3(0, 2.5f, 0), ray, out hit);
            Debug.DrawRay(transform.position + new Vector3(0, 2.5f, 0), ray, Color.white);
            if((didHit && hit.transform.gameObject == target.transform.gameObject) || ray.magnitude < 30)
            {
                ranOnce = false;
                animator.SetBool("isWalking", true);
                playerMover.StopAllCoroutines();
                RunKinematicArrive();
            }
            else
            {
                if(!ranOnce)
                {
                    ranOnce = true;
                    if(enemyMover != null && enemyMover.isMoving)
                    {
                        List<Node> path = pathFinder.ReturnNewPath(target.transform.position);
                        pathFinder.GetNewPath(GridManager.instance.GetPositionFromCoordinates(path[(path.Count / 2) + 1].coordinates));
                    }
                    
                }                
            }
        }
        if (target != null && !target.activeSelf)
        {
            target = null;
        }
    }

    /// <summary>
    /// Recalculates path
    /// </summary>
    void RunKinematicArrive()
    {
        towardsTarget = target.transform.position - transform.position;

        if (towardsTarget.magnitude <= radiusOfSatisfaction)
        {
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
