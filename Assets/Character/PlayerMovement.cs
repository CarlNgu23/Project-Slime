using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float slopeCheckXOffset = 0f;
    [SerializeField] private float slopeCheckYOffset = 0f;


    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool grounded = true;      //Debug Purposes
    [SerializeField] private bool onSlope = false;

    //[SerializeField] private Vector3 boundsCenter;      //Debug Purposes
    //[SerializeField] private Vector3 boundsSize;       //Debug Purposes

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask slopeMask;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private Vector2 playerGroundPos;




    private enum MovementState { idle, moving, jumping, falling, attack }
    private MovementState state;
    //private float dirX=0f;
    private float inputX;
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
        if (anim== null)
        {
            anim = GetComponent<Animator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Instantiate(shotSpawn, firingPoint.position, transform.rotation);
        //}
        inputX = Input.GetAxisRaw("Horizontal"); //looking direction

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();

        }
    }

    void FixedUpdate()
    {
        grounded = isGrounded();    // Debugging Purposes

        state = MovementState.idle;

        Movement();

        slopeCheck();

        anim.SetInteger("States", (int)state);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
        else if ((inputX > 0.0f || inputX < 0.0f) && isGrounded())
        {
            state = MovementState.moving;
            anim.SetInteger("States", (int)state);
        }
        else if ((inputX > 0.0f || inputX < 0.0f) && isGrounded())
        {
            state = MovementState.moving;
            anim.SetInteger("States", (int)state);
        }
        else if (!isGrounded() && !onSlope)
        {
            state = MovementState.jumping;
            anim.SetInteger("States", (int)state);
        }
        //else if (rb.velocity.y < -0.1f)
        //{
        //    state = MovementState.falling;
        //}
    }

    bool isGrounded()
    {
        //boundsCenter = coll.bounds.center;
        //boundsSize = coll.bounds.size;
        //return Physics2D.BoxCast(boundsCenter, boundsSize, 0f, Vector2.down, 0.1f, groundMask);

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
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


