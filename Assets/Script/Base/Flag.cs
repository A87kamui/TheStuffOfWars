using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    float timer = 5.0f;
    [SerializeField] BaseController baseController;
    [SerializeField] EnemyBasesController enemyBasesController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Selectable")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                baseController.enabled = true;
                enemyBasesController.enabled = true;
                Destroy(this);
            }
        }
        
    }
}
