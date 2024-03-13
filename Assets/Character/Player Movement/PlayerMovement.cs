using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private Rigidbody2D rb2D;
    private Animator animator;
    //Physical Input
    public InputActionReference moveAction;     //Player's Move inputs.
    public InputActionReference jumpAction;     //Player's Jump input.
    public InputActionReference attackAction;   //Player's Attack input.
    public InputActionReference dashAction;     //Player's Dash input.
    public Vector2 inputValue;      //Value of the Player's Move inputs.
    //Physics
    public Vector2 groundBoxSize;//For detection
    public Vector2 wallBoxSize;//For detection
    public float moveSpeed;
    public float dashSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpWaitTime;//Wall Jump Cooldown.
    private float lockWallJumpTime;//Lock wall jump.
    public float dashTimeOut;//The time to stop dashing physics and animation.
    public float dashTimeOutWeight;//The weight to cancel out dash force.
    private float lockDashTimeOut;//Lock dash until Time.time + dashTimeOut.
    public float dashWaitTime;//Dash cooldown.
    private float lockDashWaitTime;//Lock dash until Time.time + dashWaitTime.
    private int lastWall;
    private int currentWall;
    public float fallingGravity;
    public float slidingGravity;
    public float wallJumpGravity;
    //public float groundRayCheckDistance;    //The distance for Raycast to detect ground.
    //public float wallRayCheckDistance;    //The distance for Raycast to detect ground.
    public LayerMask groundMask;
    public LayerMask wallMask;
    //Animation
    public float attackTime;    //Time for attack animation to complete.
    public float landingTime;   //Time for landing animation to complete.
    private int currentState;
    public float waitTime;     //New game time for animation to complete a loop.
    public bool isFacingRight;
    public bool isAttacking;
    public bool isJumping;
    public bool isWallJumping;
    public bool isDashing;
    public bool isFalling;
    public bool isSliding;
    public bool isLanded;
    public bool isGrounded;
    public bool isRightWalled;
    public bool isLeftWalled;
    //Animator states hash. Used for activating animation states.
    private int idle = Animator.StringToHash("Idle");
    private int move = Animator.StringToHash("Move");
    private int jump = Animator.StringToHash("Jump");
    private int fall = Animator.StringToHash("Fall");
    private int land = Animator.StringToHash("Land");
    private int attack = Animator.StringToHash("Attack");
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// Input System EVENTs and METHODs /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void OnEnable()
    {   //Jump and Attack inputs are single press occurrance, so they require event handling.
        jumpAction.action.performed += Jump;    //When Jump input is pressed, do Jump()
        attackAction.action.performed += Attack;    //When attack input is pressed, do Attack()
        dashAction.action.performed += DoDash;
    }

    private void OnDisable()
    {   //Disable upon no action.
        jumpAction.action.performed -= Jump;
        attackAction.action.performed -= Attack;
        dashAction.action.performed -= DoDash;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)     //Regular Jump
            //rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (isSliding)      //Wall Jump
        {
            rb2D.gravityScale = wallJumpGravity;
            if (isRightWalled && isFacingRight)
            {
                currentWall = -1;//Set to remember the wall that the player jumping to.
                WallJump(-1);
            }
            if (isLeftWalled && !isFacingRight)
            {
                currentWall = 1;
                WallJump(1);
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        isAttacking = true;
    }

    public void DoDash(InputAction.CallbackContext context)
    {
        if (inputValue.x == 1 && !isWallJumping)
        {
            Dash(1);
        }
        if (inputValue.x == -1 && !isWallJumping    )
        {
            Dash(-1);
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// Awake, Update /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputValue = moveAction.action.ReadValue<Vector2>();//Detect player's input. Values are between -1 and 1 being left and right respectively. 0 is no input.
        GroundCheck();
        WallCheck();
        CheckJump();//The order for these methods are crucial. Don't move them.
        CheckFall();
        CheckDash();
        CheckMove();
        CheckDirection();
        var updateState = UpdateState();//Decides the next animation state the animator should be in.
        isAttacking = false;
        isJumping = false;
        isFalling = false;
        isLanded = false;
        if (updateState == currentState)//If state hasn't changed, return.
            return;
        animator.CrossFade(updateState, 0, 0);//Allow smooth transitioning between animation states.
        currentState = updateState;//Update currentState.
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// BASIC MOVEMENT and ANIMATION Update ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private int UpdateState()
    {
        //Time.time is current game time. Game time is the time that has past since the game started.
        int KeepState(int state, float animationDuration)
        {
            waitTime = Time.time + animationDuration;//Get future timestamp to know when state can stop and change.
            return state;
        }
        if (Time.time < waitTime)//Checks for if waitTime is required. If it's required, waitTime is greater than current game time.
            return currentState;
        if (isAttacking)//Attacking
            return KeepState(attack, attackTime);
        if (currentState == fall && isGrounded)//Landing
            return KeepState(land, landingTime);
        if (isJumping)//Jumping
            return jump;
        if (isFalling && !isSliding && (!isRightWalled || isLeftWalled))//Falling
        {
            rb2D.gravityScale = fallingGravity;//Set gravity to desired gravity when falling.
            return fall;
        }
        if (isSliding)//Sliding
        {
            rb2D.gravityScale = slidingGravity;//Set gravity to desired gravity when sliding.
            return fall;
        }
        if (isGrounded && inputValue.x == 1 || inputValue.x == -1)//Moving
            return move;
        return idle;    //Idle
    }

    public void CheckMove()
    {
        if (inputValue.x == 1 || inputValue.x == -1 && !isDashing)        //Moves the Slime based on player's inputValue.
        {
            if (isRightWalled)//If player is near a right wall, disable right movement.
            {
                if (inputValue.x == 1)
                {
                    return;
                }
            }
            if (isLeftWalled)//If player is near a left wall, disable left movement.
            {
                if (inputValue.x == -1)
                {
                    return;
                }
            }
            rb2D.velocity = new Vector2(inputValue.x * moveSpeed, rb2D.velocity.y);
        }
        else if (inputValue.x == 0 && isGrounded)//If no input, stop moving.
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
    }

    public void CheckJump()
    {
        if (rb2D.velocity.normalized.y > 0.01f)
        {
            isJumping = true;
            //JumpHang();
        }
    }

    public void CheckFall()
    {
        if (rb2D.velocity.normalized.y < -0.01f)
        {
            if (isRightWalled || isLeftWalled)
            {//While falling and if a wall is nearby, allow sliding.
                isSliding = true;
            }
            else
            {
                isSliding = false;
            }
            isFalling = true;
            isWallJumping = false;//Deactivate wall jump status as soon as player starts to fall.
        }
    }


    public void GroundCheck()
    {
        //Old method.
        //RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundRayCheckDistance, groundMask);
        //if (groundHit)
        //{
        //    rb2D.gravityScale = 1;
        //    isSliding = false;
        //    return isGrounded = true;
        //}
        //else
        //    return isGrounded = false;
        isGrounded = Physics2D.BoxCast(transform.position, groundBoxSize, 0, Vector2.down, 0, groundMask);
        if (isGrounded)
        {
            rb2D.gravityScale = 1.0f;
            isSliding = false;
            lastWall = 0;//Reset 
        }
    }

    public void WallCheck()//Check for walls to allow player to do things like wall jump.
    {
        //Old Method.
        //RaycastHit2D rightWallHit = Physics2D.Raycast(transform.position, Vector2.right, wallRayCheckDistance, wallMask);
        //RaycastHit2D leftWallHit = Physics2D.Raycast(transform.position, -Vector2.right, wallRayCheckDistance, wallMask);
        isRightWalled = Physics2D.BoxCast(transform.position + new Vector3(0.2f, 0f, 0f), wallBoxSize, 0f, Vector2.right, 0f, wallMask);
        isLeftWalled = Physics2D.BoxCast(transform.position - new Vector3(0.2f,0f,0f), wallBoxSize, 0f, Vector2.left, 0f, wallMask);
    }

    public void CheckDirection()//Check contradictions in player's input and player's direction in game.
    {
        if (inputValue.x == 1f && !isFacingRight)
            Flip();
        else if (inputValue.x == -1f && isFacingRight)
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        rb2D.transform.Rotate(0f, 180f, 0f);
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// COMPLEX MOVEMENT //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public void WallJump(int direction)
    {
        //Overloaded with constraints to prevent super jumping due to multiplying forces from different jump mechanics.
        if (inputValue.x == 0 && !isGrounded && Time.time > lockWallJumpTime && currentWall != lastWall)
        {
            isWallJumping = true;
            lastWall = direction;//Set to remember the last wall the player jumped from with currentWall.
            Flip();
            rb2D.AddForce(new Vector2(direction * moveSpeed, wallJumpForce), ForceMode2D.Impulse);
            lockWallJumpTime = Time.time + wallJumpWaitTime;//Wall Jump Cooldown
        }
    }

    public void Dash(int direction)//Rudimentary, not fully developed. Discovery phase. Will check tutorial if stuck.
    {
        if (Time.time > lockDashWaitTime)
        {
            //rb2D.velocity = new Vector2(0f, 0f);
            isDashing = true;
            rb2D.AddForce(new Vector2(direction * dashSpeed, 2f), ForceMode2D.Impulse);//The dash physics.
            lockDashTimeOut = Time.time + dashTimeOut;//Get time to cancel dash.
            lockDashWaitTime = Time.time + dashWaitTime;//Get dash cooldown.
        }
    }

    public void CheckDash()
    {
        if (isDashing && Time.time > lockDashTimeOut)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x / dashTimeOutWeight, rb2D.velocity.y / dashTimeOutWeight);//Stop player from dashing and slow down.
            isDashing = false;
        }
    }

    public void JumpHang()//Allow player to stay in the air during the peak of their jump longer. TBD
    {
        //Debug.Log((isJumping || isWallJumping || isFalling) && Mathf.Abs(rb2D.velocity.y) < 6);
        //if ((isJumping || isWallJumping || isFalling) && Mathf.Abs(rb2D.velocity.y) < 0.5)
        //{
        //    rb2D.gravityScale = rb2D.gravityScale * jumpHangTime;
        //}
    }
}