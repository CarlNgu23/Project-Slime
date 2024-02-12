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



    private bool canDash = true;

    private float lastDash;
    private float dashTimeLeft;
    private float lastImageXpos;
    [SerializeField]private Transform player;//where to shoot from
    [SerializeField]private GameObject dashPrefab; //dash attack
    [SerializeField]private Animator anim;// enable the attack

    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim= GetComponent<Animator>(); 
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

    void DashTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 dashTarget = transform.position + direction * dashDistance;
        
        StartCoroutine(DoDash(dashTarget));
    }

    IEnumerator DoDash(Vector3 target)
    {
        canDash = false;
        dashTimeLeft=dashTimeLeft;
        anim.SetTrigger("dashAttack");
        while (transform.position != target)
        {   
            
            transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);
            
            // AfterImagePool.Instance.GetFromPool();
            // lastImageXpos=transform.position.x;
            yield return null;
        }
        canDash = true;
       
    }

    // void StopDash()
    // {
    //     anim.ResetTrigger("dashAttack");
    // }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
