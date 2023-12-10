using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    //Enemy targeting variables
    public EnemyMover enemyMover;
    public GameObject target;

    //Player variables
    public PathFinder pathFinder;
    public AttackController attackController;
    [SerializeField] Vector3 towardsTarget;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField][Range(0.0f, 10.0f)] public float speed = 1.0f;
    [SerializeField] float obstacleBumpSpeed;

    bool enemyNear = false;
    float radiusOfSatisfaction = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        attackController = GetComponent<AttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            RaycastHit hit;
            Vector3 ray = target.transform.position - transform.position;
            bool didHit = Physics.Raycast(transform.position, ray, out hit);
            if(enemyNear || (didHit && hit.transform.gameObject == target.transform.gameObject))
            {
                //print("Kine");
                RunKinematicArrive();
                attackController.target = target;
            }
            else
            {
                //print("A*");
                List<Node> path = pathFinder.ReturnNewPath(target.transform.position);
                pathFinder.GetNewPath(GridManager.instance.GetPositionFromCoordinates(path[(path.Count / 2) + 1].coordinates));
            }
        }   
    }

    /// <summary>
    /// Recalculates path
    /// </summary>
    void RunKinematicArrive()
    {
        towardsTarget = target.transform.position - transform.position;

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
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed);

        // Move along our forward vector (the direction we're facing)
        Vector3 newPosition = transform.position;
        newPosition += transform.forward * speed * Time.deltaTime;

        transform.position = newPosition;
    }

    /// <summary>
    /// Sets enemyNear so that troop can run kinematic arrive
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == target)
        {
            enemyNear = true;
        }
    }

    /// <summary>
    /// Sets enemyNear so that troop can run A*
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == target)
        {
            enemyNear = false;
        }    
    }
}
