using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Monster : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] public float flashtime;
    

    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    
    [SerializeField] private BoxCollider2D boxColl;
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer=Mathf.Infinity;
    private Animator anim;
    //private Rigidbody2D rb;
    private PatrolMonsters mon;
    
    
   
    //flash when by hit
    private SpriteRenderer sr;
    private Color originlColor;


    // Start is called before the first frame update
    public void Start()
    {   
        //rb=GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
        mon=GetComponentInParent<PatrolMonsters>();
        originlColor = sr.color;
       
    }

    // Update is called once per frame
   public void Update()
    {   
        
        if (health <= 0)
        { 
        
        Destroy(gameObject);
        
        }
        cooldownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if(cooldownTimer>= attackCooldown)
            {
                cooldownTimer=0;
                anim.SetBool("isWalking",false);
                RangedDashAttack();
            }
        }

        if(mon != null)
        {
            mon.enabled=!PlayerInSight();
        }
    }
    public void TakeDamage(int damage)
    { 
        
        health -= damage;
        FlashColor(flashtime);
    }


    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        sr.color = originlColor;

    }

    private bool PlayerInSight()
    {   
        RaycastHit2D hit= Physics2D.BoxCast(boxColl.bounds.center+transform.right* range*transform.localScale.x* colliderDistance,
        new Vector3(boxColl.bounds.size.x *range,boxColl.bounds.size.y,boxColl.bounds.size.z),0,Vector2.left,0,playerLayer );

        return hit.collider != null;
    }

//DO NOT DELETE: this is to see the enemy's sight (TESTING PURPOSES)
    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(boxColl.bounds.center+transform.right *range*transform.localScale.x *colliderDistance,new Vector3(boxColl.bounds.size.x *range,boxColl.bounds.size.y,boxColl.bounds.size.z));

    }

    private void RangedDashAttack()
    {
        cooldownTimer=2;
        anim.SetTrigger("dashAttack");

    }
   
    
 
}
