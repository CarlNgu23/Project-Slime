using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Detection : MonoBehaviour
{
    [SerializeField] private float rayDistance;     //Detection Range
    [SerializeField] private float distance_limit;  //Distance between player and monster. Need to match this to the attack object's range.
    [SerializeField] LayerMask playerMask;
    [SerializeField] private GameObject player;
    [SerializeField] public float moveSpeed;
    public Attack attack;
    public bool isFacingRight;
    private Rigidbody2D oneEyeDog_RB;
    public bool isCPUMove;
    public bool isDetected;
    public bool isWaiting;
    public One_Eye_Dog One_Eye_Dog;

    // Start is called before the first frame update
    void Start()
    {
        oneEyeDog_RB = GetComponent<Rigidbody2D>();
        attack = GetComponentInChildren<Attack>();
        One_Eye_Dog = GetComponent<One_Eye_Dog>();
        player = GameObject.FindWithTag("Player");
        isCPUMove = true;
        isDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (oneEyeDog_RB.velocity.x == moveSpeed && !isFacingRight)      //Checks for contradictions in monster's right direction
        {
            Flip();
        }
        else if (oneEyeDog_RB.velocity.x == -moveSpeed && isFacingRight)  //Checks for contradictions in monster's left direction
        {
            Flip();
        }
    }

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
        if (!attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref && !isCPUMove && !isWaiting)
        {
            isWaiting = true;
            StartCoroutine(Deactivate());
        }
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2);
        isDetected = false;
        isWaiting = false;
    }

    public void Detect(RaycastHit2D right, RaycastHit2D left)
    {
        Debug.Log(left && !attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref);
        if (right && !attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref)   //Change One_Eye_Dog to any monster scripts.
        {
            oneEyeDog_RB.velocity = new Vector2(moveSpeed, 0f);
            Check_Distance();
            if (!isFacingRight && !attack.isAttacking_Ref)    //Checks for contradictions in monster's right direction when player is up-close.
            {
                Flip();
            }
        }
        else if (left && !attack.isAttacking_Ref && !One_Eye_Dog.isDying_Ref)   //Change One_Eye_Dog to any monster scripts.
        {
            oneEyeDog_RB.velocity = new Vector2(-moveSpeed, 0f);
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
            oneEyeDog_RB.velocity = new Vector2(0f, 0f);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
