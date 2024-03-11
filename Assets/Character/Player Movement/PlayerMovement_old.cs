using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_old : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    //private Animation animState;
    //private BlendTree blendtree;

    private float inputX;
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float slopeCheckDistance = 1f;
    [SerializeField] private float slopeCheckXOffset = 0f;
    [SerializeField] private float slopeCheckYOffset = 0f;

    private bool inputJump;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool grounded = true;      //Debug Purposes
    [SerializeField] private bool onSlope = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;

    //[SerializeField] private Vector3 boundsCenter;      //Debug Purposes
    //[SerializeField] private Vector3 boundsSize;       //Debug Purposes

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask slopeMask;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private Vector2 playerGroundPos;

    [SerializeField] private PhysicsMaterial2D sharedMaterial;



    private enum MovementState { idle, moving, jumping, falling, landed }
    private MovementState state;
    
    //private Vector2 move;


    // Start is called before the first frame update
    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (coll == null)
        {
            coll = GetComponent<BoxCollider2D>();
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        //blendtree = GetComponent<BlendTree>();
        //if (animState == null)
        //{
        //    animState == GetComponent<Animation>();
        //}

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Instantiate(shotSpawn, firingPoint.position, transform.rotation);
        //}
        isGrounded();


        inputX = Input.GetAxisRaw("Horizontal"); //looking direction

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && state != MovementState.landed)
        {
            rb.sharedMaterial = null;
            Jump();
        }

        //blendtree.children[0].motion;
    }

    void FixedUpdate()
    {
        //grounded = isGrounded();    // Debugging Purposes
        Movement();
        slopeCheck();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
    }

    void Movement()
    {
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

        if (inputX > 0.0f && !isFacingRight)      //Checks for contradictions in player's right direction
        {
            Flip();
        }
        else if (inputX < 0.0f && isFacingRight)  //Checks for contradictions in player's left direction
        {
            Flip();
        }

        if (!isGrounded() && rb.velocity.y > 0.0f)
        {
            isFalling = false;
            state = MovementState.jumping;
            anim.SetInteger("States", (int)state);
            anim.SetBool("isGrounded", isGrounded());
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isFalling", isFalling);
        }
        else if (!isGrounded() && !onSlope && rb.velocity.y < 0.0f)
        {
            rb.sharedMaterial = null;
            isFalling = true;
            isJumping = false;
            state = MovementState.falling;
            anim.SetInteger("States", (int)state);
            anim.SetBool("isGrounded", grounded);
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isFalling", isFalling);
        }
        else if (isGrounded() && isFalling)
        {
            rb.sharedMaterial = null;
            isFalling = false;
            state = MovementState.landed;
            anim.SetInteger("States", (int)state);
            anim.SetBool("isGrounded", grounded);
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isFalling", isFalling);
        }
        else if ((inputX > 0.0f || inputX < 0.0f) && isGrounded())
        {
            rb.sharedMaterial = null;
            state = MovementState.moving;
            anim.SetInteger("States", (int)state);
            anim.SetBool("isGrounded", grounded);
        }
        else if (inputX == 0 && isGrounded())
        {
            //rb.velocity = Vector2.zero;
            rb.sharedMaterial = sharedMaterial;     //Reference the material under script for PlayerMovement in the Player's inspector window.
            state = MovementState.idle;
            anim.SetInteger("States", (int)state);
            anim.SetBool("isGrounded", grounded);
        }
    }

    bool isGrounded()
    {
        //boundsCenter = coll.bounds.center;
        //boundsSize = coll.bounds.size;
        //return Physics2D.BoxCast(boundsCenter, boundsSize, 0f, Vector2.down, 0.1f, groundMask);
        

        return grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }

    void slopeCheck()
    {
        playerGroundPos = coll.bounds.center + new Vector3(slopeCheckXOffset, slopeCheckYOffset, 0);

        RaycastHit2D rayHit = Physics2D.Raycast(playerGroundPos, Vector2.down, slopeCheckDistance, slopeMask);

        Debug.DrawRay(rayHit.point, rayHit.normal, Color.red);

        if (rayHit)
        {
            onSlope = true;
        }
        else
        {
            onSlope = false;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

}


