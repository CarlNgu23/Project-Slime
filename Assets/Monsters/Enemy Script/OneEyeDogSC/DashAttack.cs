using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{   
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private PolygonCollider2D dashAttack;
    public float dashDistance = 5f;
    private bool canDash = false;
    [SerializeField] private int damage;
    [SerializeField] public float waitTime; //waitTime hitbox disappear time
    [SerializeField] public float startTime;
    [SerializeField]private GameObject enemy;
    [SerializeField]private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        anim = enemy.GetComponent<Animator>();
        GetComponent<Animator>();
        dashAttack = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        Attack();
    }

     void Attack()
    {
        if ( IsPlayerInRange())
        {
            anim.SetTrigger("dashAttack");
            StartCoroutine(StartAttack());
        }
    }

     bool IsPlayerInRange()
    {   
        
        return Vector2.Distance(transform.position, player.position) < dashDistance;
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        dashAttack.enabled = true;
        StartCoroutine(disableHitBox());
    }
    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(waitTime);
        dashAttack.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            other.GetComponent<Stats_Level>().TakeDamage(damage);
        }
    }
}
