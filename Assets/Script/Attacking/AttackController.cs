using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    float timer = 0f;
    public GameObject target;

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
            if (target.CompareTag("Tower"))
            {
                collision.gameObject.GetComponentInChildren<Slider>().value -= 15;
            }
            else
            {
                collision.gameObject.GetComponentInChildren<Slider>().value -= 1;
            }
            timer = 3;
        }
        print("Collision");
    }

    /**Will remove this code once we switch the triggering of the animation somewhere else
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player Tower_Standing" && timer < 0 && this.tag == "Enemy")
        {          
            animator.SetTrigger("isAttacking");
            animator.SetBool("isWalking", false);
        }
        if (other.gameObject.name == "Enemy Tower_Standing" && timer < 0 && this.tag == "Selectable")
        {
            other.gameObject.GetComponentInChildren<Slider>().value -= 15;
            timer = 3;
            animator.SetTrigger("isAttacking");
            animator.SetBool("isWalking", false);
        }
        if (other.gameObject.tag == "Enemy" && timer < 0 && this.tag == "Selectable")
        {
            
            timer = 3;
            animator.SetTrigger("isAttacking");
            animator.SetBool("isWalking", false);
        }
        if (other.gameObject.tag == "Selectable" && timer < 0 && this.tag == "Enemy")
        {
            other.gameObject.GetComponentInChildren<Slider>().value -= 1;
            timer = 3;
            animator.SetTrigger("isAttacking");
            animator.SetBool("isWalking", false);
        }
    }
    */
}
