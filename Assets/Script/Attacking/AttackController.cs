using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    float timer = 0f;
    public Animator animator;
    public GameObject target;
    public bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (isAttacking && target != null)
        {
            transform.LookAt(target.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isAttacking = true;
        if (other.gameObject.name == "Player Tower_Standing" && timer < 0 && this.tag == "Enemy")
        {          
            other.gameObject.GetComponentInChildren<Slider>().value -= 15;
            timer = 3;
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
            other.gameObject.GetComponentInChildren<Slider>().value -= 1;
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

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
