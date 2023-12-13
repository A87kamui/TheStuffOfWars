using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    float timer = 0f;
    public GameObject target;
    [SerializeField] int damageValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target)
        {
            collision.gameObject.GetComponentInChildren<Slider>().value -= damageValue;
            PlayerFight player = null;
            EnemyFight enemy = null;
            if(collision.gameObject.CompareTag("Selectable"))
            {
                player = collision.gameObject.GetComponent<PlayerFight>();
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                enemy = collision.gameObject.GetComponent<EnemyFight>();
            }
            if(player != null)
            {
                if(player.target == null || player.target.CompareTag("Tower"))
                {
                    player.target = this.GetComponentInParent<EnemyFight>().gameObject;
                }
            }
            else if (enemy != null)
            {
                if (enemy.target == null || enemy.target.CompareTag("Tower"))
                {
                    enemy.target = this.GetComponentInParent<PlayerFight>().gameObject;
                }
            }
            timer = 3;
        }
    }
}
