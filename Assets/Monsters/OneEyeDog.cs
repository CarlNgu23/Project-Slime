using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEyeDog : MonoBehaviour
{
    // public GameObject player;
    // public bool flip;
    // private Rigidbody2D rb;
    // private Animator anim;
    // public bool isWalking;

    // public float speed;
    // [SerializeField] private Transform enemy;

    //[SerializeField] private float speed;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxColl;
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer=Mathf.Infinity;
    private Animator anim;
    private Monsters mon;


    // Start is called before the first frame update
    void Start()
    {
        //rb= GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        mon=GetComponentInParent<Monsters>();

    }

    // Update is called once per frame
    void Update()
    {
        //  Vector3 scale=transform.localScale;
        // if(player.transform.position.x<transform.position.x){
        //     scale.x=Mathf.Abs(scale.x) * -1 *(flip? -1:1);
        //     transform.Translate(speed*Time.deltaTime*-1,0,0);
            
        // }
        // else{
        //     scale.x=Mathf.Abs(scale.x)*(flip? -1:1);
        //     transform.Translate(speed*Time.deltaTime,0,0);
            
        // }
        // transform.localScale=scale;

        cooldownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if(cooldownTimer>= attackCooldown)
            {
                cooldownTimer=0;
                anim.SetTrigger("dashAttack");
            }
        }

        if(mon != null)
        {
            mon.enabled=!PlayerInSight();
        }
}

private bool PlayerInSight()
{
    RaycastHit2D hit= Physics2D.BoxCast(boxColl.bounds.center+transform.right* range*transform.localScale.x* colliderDistance,
new Vector3(boxColl.bounds.size.x *range,boxColl.bounds.size.y,boxColl.bounds.size.z),0,Vector2.left,0,playerLayer );

    return hit.collider != null;
}

private void OnDrawGizmos()
{
    Gizmos.color=Color.red;
    Gizmos.DrawWireCube(boxColl.bounds.center+transform.right *range*transform.localScale.x *colliderDistance,new Vector3(boxColl.bounds.size.x *range,boxColl.bounds.size.y,boxColl.bounds.size.z));

}
   

//    private void DamagePlayer()
//    {
//         if (PlayerInSight())
//         {
//             playerHealth.TakeDamage(damage);
//         }
//    }

}
