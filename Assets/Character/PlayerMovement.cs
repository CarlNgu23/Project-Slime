using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    //[SerializeField] private GameObject shotSpawn;
    //[SerializeField] private Transform firingPoint;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool grounded = true;

    //[Range(0.1f,1f)];
    //[SerializeField] private float fireRate=0.5f;




    private enum MovementState { idle, moving, jumping, falling, attack }
    private MovementState state;
    //private float dirX=0f;
    private float dirX;
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
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
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

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
    }

    void FixedUpdate()
    {
        //looking direction
        dirX = Input.GetAxisRaw("Horizontal");

        
        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        //bool isFacingRight=true;

        state = MovementState.idle;

        if (dirX > 0f && !isFacingRight)
        {//right 
            Flip();
        }
        else if (dirX < 0f && isFacingRight)
        {//left
            Flip();
        }
        else if ((dirX > 0f || dirX < 0f) && isGrounded())
        {
            state = MovementState.moving;
            anim.SetInteger("States", (int)state);
        }

        if (rb.velocity.y > 0.1f)
        {
            grounded = isGrounded();
            state = MovementState.jumping;
            anim.SetInteger("States", (int)state);
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("States", (int)state);

        //anim.SetBool("isGrounded", isGrounded());


        //if (!grounded && state != MovementState.jumping)
        //{
        //    grounded = true;
        //}
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, jumpableGround);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


}


