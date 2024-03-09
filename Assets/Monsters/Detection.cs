using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Detection : MonoBehaviour
{
    //public GameObject assignedPlatform;
    //public GameObject leftPlatformBoundary;
    //public GameObject rightPlatformBoundary;
    public float rayDistance;     //Detection Range
    public float distance_limit;  //Distance between player and monster. Need to match this to the attack object's range.
    public LayerMask playerMask;
    public GameObject player;
    public float moveSpeed;
    public Attack attack;
    public bool isFacingRight;
    public Rigidbody2D monster_RB;
    public bool isCPUMove;
    public bool isDetected;
    public bool isWaiting;
    public MonsterManager monster;

    // Start is called before the first frame update
    void Start()
    {
        //leftPlatformBoundary = assignedPlatform.transform.GetChild(0).gameObject;
        //rightPlatformBoundary = assignedPlatform.transform.GetChild(1).gameObject;
        monster_RB = GetComponent<Rigidbody2D>();
        attack = GetComponentInChildren<Attack>();
        monster = GetComponent<MonsterManager>();
        player = GameObject.FindWithTag("Player");
        isCPUMove = true;
        isDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isDetected && !attack.isAttacking_Ref && !monster.isDying_Ref && !isCPUMove)// && !isWaiting)
        //{
        //    //isWaiting = true;
        //    monster_RB.velocity = new Vector2(0f, 0f);
        //    isDetected = false;
        //    //isWaiting = false;
        //    //StartCoroutine(Deactivate());
        //}
        if (monster_RB.velocity.x > 0.1f && !isFacingRight)      //Checks for contradictions in monster's right direction
        {
            Flip();
        }
        else if (monster_RB.velocity.x < -0.1f && isFacingRight)  //Checks for contradictions in monster's left direction
        {
            Flip();
        }
        if (isDetected && Vector2.Distance(transform.position, player.transform.position) > rayDistance)
        {
            monster_RB.velocity = new Vector2(0f, 0f);
            isDetected = false;
        }
    }

    //IEnumerator Deactivate()
    //{
    //    yield return new WaitForSeconds(2);
    //    isDetected = false;
    //    isWaiting = false;
    //}

    private void FixedUpdate()
    {

        RaycastHit2D right_hit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, playerMask);
        Debug.DrawRay(right_hit.point, right_hit.normal, Color.red);
        RaycastHit2D left_hit = Physics2D.Raycast(transform.position, -Vector2.right, rayDistance, playerMask);
        Debug.DrawRay(left_hit.point, left_hit.normal, Color.red);
        if (right_hit || left_hit)
        {
            isCPUMove = false;
            isDetected = true;
            Detect(right_hit, left_hit);
        }
    }

    public void Detect(RaycastHit2D right, RaycastHit2D left)
    {
        if (right && !attack.isAttacking_Ref && !monster.isDying_Ref)   //Can change One_Eye_Dog to any monster scripts.
        {
            monster_RB.velocity = new Vector2(moveSpeed, 0f);
            Check_Distance();
            if (!isFacingRight && !attack.isAttacking_Ref)    //Checks for contradictions in monster's right direction when player is up-close.
            {
                Flip();
            }
        }
        else if (left && !attack.isAttacking_Ref && !monster.isDying_Ref)   //Change One_Eye_Dog to any monster scripts.
        {
            monster_RB.velocity = new Vector2(-moveSpeed, 0f);
            Check_Distance();
            if (isFacingRight && !attack.isAttacking_Ref)    //Checks for contradictions in monster's left direction when player is up-close.
            {
                Flip();
            }
        }
    }

    private void Check_Distance()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < distance_limit)
        {
            monster_RB.velocity = new Vector2(0f, 0f);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
