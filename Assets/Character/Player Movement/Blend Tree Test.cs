using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb2D;
    private Animator animator;
    public Vector2 inputValue;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    private bool isFacingRight = true;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public bool isGrounded;
    public bool isWall;
    public bool isJumping;
    public bool isFalling;
    public bool isLanded;
    public float groundRayCheckDistance;
    public float rightRayWallCheckDistance;
    public float leftRayWallCheckDistance;
    private Vector2 lastPosition;

    private void OnEnable()
    {
        jumpAction.action.performed += Jump;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
        if (isWall)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputValue = moveAction.action.ReadValue<Vector2>();    //Detect player's input. Values are between -1 and 1 being left and right respectively. 0 is no input.
        animator.SetFloat("X", inputValue.x);       //To let the Animator know the X value at all time.
        
        CheckDirection();
        GroundCheck();
        WallCheck();
        CheckMove();
        CheckJump();
        CheckFalling();
    }

    public bool GroundCheck()   //Ground Check
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundRayCheckDistance, groundMask);
        if (groundHit)
        {
            animator.SetFloat("isLanded", 0);
            animator.SetFloat("isGrounded", 1);
            return isGrounded = true;
        }
        else
        {
            animator.SetFloat("isGrounded", 0);
            return isGrounded = false;
        }
    }

    public bool WallCheck()     //Wall Check
    {
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, rightRayWallCheckDistance, wallMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -Vector2.right, leftRayWallCheckDistance, wallMask);
        if (rightHit)
        {
            return isWall = true;
        }
        else if (leftHit)
        {
            return isWall = true;
        }
        else
        {
            return isWall = false;
        }
    }

    public void CheckMove()
    {
        if (inputValue.x == 1 || inputValue.x == -1)        //Moves the Slime based on inputValue.
        {
            if (isWall)
            {

            }
            rb2D.velocity = new Vector2(inputValue.x * moveSpeed, rb2D.velocity.y);
        }
        else if (inputValue.x == 0 && isGrounded)
        {
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        }
    }

    public void CheckJump()
    {
        if (rb2D.velocity.normalized.y > 0.0f)
        {
            lastPosition = transform.position;
            animator.SetFloat("Y", 1);
        }
    }

    public void CheckFalling()
    {
        if (rb2D.velocity.normalized.y < 0.0f)
        {
            animator.SetFloat("Y", -1);
            isFalling = true;
        }
    }

    

    public void CheckLanding()
    {
        if (rb2D.velocity.normalized.y == 0.0f)
        {
            animator.SetFloat("isLanded", 1);
            animator.SetFloat("Y", 0);
        }
    }

    public void CheckDirection()
    {
        if (inputValue.x == 1f && !isFacingRight)
        {
            Flip();
        }
        else if (inputValue.x == -1f && isFacingRight)
        {
            Flip();
        }
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        rb2D.transform.Rotate(0f, 180f, 0f);
    }
}