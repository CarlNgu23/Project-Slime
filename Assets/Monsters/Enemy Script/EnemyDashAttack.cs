using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public float dashSpeed = 10f;
    public float dashDistance = 5f;
    public float dashCooldown = 3f;
    public float distanceBetweenImages;


    [SerializeField] private bool grounded = true;  
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 EnemyGroundPos;
    
    private bool canDash = true;

    private float lastDash;
    private float dashTimeLeft;
    private float lastImageXpos;
    [SerializeField]private Transform player;//where to shoot from
    [SerializeField]private GameObject dashPrefab; //dash attack
    [SerializeField]private Animator anim;// enable the attack

    private Rigidbody2D rb;
    private BoxCollider2D coll;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim= GetComponent<Animator>(); 
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        if (canDash && IsPlayerInRange())
        {   
            DashTowardsPlayer(); 
           
            StartCoroutine(DashCooldown());
        }
    }

    bool IsPlayerInRange()
    {   
        
        return Vector3.Distance(transform.position, player.position) < dashDistance;
    }

    void DashTowardsPlayer()//direction
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 dashTarget = transform.position + direction * dashDistance;
  
        StartCoroutine(DoDash(dashTarget));
    
        
    }

    IEnumerator DoDash(Vector3 target)//movement
    {
        canDash = false;
        dashTimeLeft=dashTimeLeft;
        
        
        anim.SetTrigger("dashAttack");
        
        while (transform.position != target)
        {   
            bool grounded = isGrounded();
            anim.SetBool("isGrounded", grounded);
            if(grounded)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);
           }
            
            
             yield return null;// pauses the execution. Then continue to executing other code for one frame, then next frame it will resume 
            
        }
            //transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);

            
        
        canDash = true;
       
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
     private bool isGrounded() 
    {
   
        return grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask) ;
    }

}
