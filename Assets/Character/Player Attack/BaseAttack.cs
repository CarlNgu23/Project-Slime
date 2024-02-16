using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{

    [SerializeField] private int damage;    //player damage
    [SerializeField] public float waitTime; //waitTime hitbox disappear time
    [SerializeField] public float startTime; //startTime hit box appear time
    private Animator anim;
    private PolygonCollider2D baseAttack2d;
    [SerializeField]private GameObject player;
    // Start is called before the first frame update


    void Start()
    {    
        if(anim!=null)
        {
            anim = player.GetComponent<Animator>();
        }
        if(baseAttack2d!=null)
        {
            baseAttack2d = GetComponent<PolygonCollider2D>();
        }
        //GetComponent<Animator>();
        baseAttack2d = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }


    void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        baseAttack2d.enabled = true;
        StartCoroutine(disableHitBox());
    }


    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(waitTime);
        baseAttack2d.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        { 
            other.GetComponent<Monster>().TakeDamage(damage);
        }
    }

}
