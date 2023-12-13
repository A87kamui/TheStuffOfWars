using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    [SerializeField] Vector3 height;
    Vector3 leftCheck;
    Vector3 rightCheck;
    [SerializeField] private float leftRoatation = -45;
    [SerializeField] private float rightRoatation = 45;
    [SerializeField] GameObject target;
    public EnemyFight enemyFight;

    // Start is called before the first frame update
    void Start()
    {
        height = new Vector3(0, 2.5f, 0);
        enemyFight = GetComponent<EnemyFight>();
    }

    // Update is called once per frame
    void Update()
    {
        leftCheck = Quaternion.Euler(0.0f, leftRoatation, 0.0f) * transform.forward;
        rightCheck = Quaternion.Euler(0.0f, rightRoatation, 0.0f) * transform.forward;
        DetectEnemy();
    }

    /// <summary>
    /// Shoots a raycast to detect a object with "Enemy" tag
    /// </summary>
    void DetectEnemy()
    {
        RaycastHit hit0;
        RaycastHit hit1;
        RaycastHit hit2;
        Vector3 ray = transform.forward;   //Direction of ray
        int layerMask = 0 << 11;
        layerMask = ~layerMask;
        bool didHit0 = Physics.Raycast(transform.position + height, ray, out hit0, 20, layerMask);
        bool didHit1 = Physics.Raycast(transform.position + height, leftCheck, out hit1, 20, layerMask );
        bool didHit2 = Physics.Raycast(transform.position + height, rightCheck, out hit2, 20, layerMask);
        if (didHit0 && (hit0.transform.tag == "Selectable" || hit0.transform.name == "Player Tower_Standing"))
        {
            enemyFight.target = hit0.transform.gameObject;
            Debug.DrawRay(transform.position + height, ray * 20, Color.red);
        }
        else if (didHit1 && (hit1.transform.tag == "Selectable" || hit1.transform.name == "Player Tower_Standing"))
        {
            enemyFight.target = hit1.transform.gameObject;
            Debug.DrawRay(transform.position + height, leftCheck * 20, Color.red);
        }
        else if (didHit2 && (hit2.transform.tag == "Selectable" || hit2.transform.name == "Player Tower_Standing"))
        {
            enemyFight.target = hit2.transform.gameObject;
            Debug.DrawRay(transform.position + height, rightCheck * 20, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position + height, ray * 20, Color.green);
            Debug.DrawRay(transform.position + height, leftCheck * 20, Color.green);
            Debug.DrawRay(transform.position + height, rightCheck * 20, Color.green);
        }
    }
}
