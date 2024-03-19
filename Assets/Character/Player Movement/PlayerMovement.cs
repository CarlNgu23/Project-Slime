//Developed by Carl Ngu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public CameraFollow cameraFollow;
    [Header("Components")]
    private Rigidbody2D rb2D;
    private Animator animator;
    [Header("Physical Inputs")]
    public InputActionReference moveAction;     //Player's Move inputs.
    public InputActionReference jumpAction;     //Player's Jump input.
    public InputActionReference attackAction;   //Player's Attack input.
    public InputActionReference dashAction;     //Player's Dash input.
    public Vector2 inputValue;      //Value of the Player's Move inputs.
    public float jumpValue;
    [Header("Detections")]
    public Vector2 groundBoxSize;//For detection for ground.
    public Vector2 wallBoxSize;//For detection for walls.
    public Vector2 enemyBoxSize;//For detection for enemies.
    private BoxCollider2D boxCollider2D;
    [Header("Movement Speed")]
    public float moveSpeed;
    [Header("Jump")]
    public float jumpForce;
    public float jumpHangGravity;
    public float jumpHangTime;
    public float newJumpHangTime;
    public Vector2 wallJumpPos;
    [Header("Wall Jump")]
    public float wallJumpForceX;
    public float wallJumpForceY;
    public float wallJumpWaitTime;//Wall Jump Cooldown.
    private float newWallJumpTime;//Lock wall jump.
    [Header("Wall Slide")]
    public float wallSlideSpeed;
    public float wallSlideAccelerationRate;
    [Header("Dash")]
    public float dashSpeed;
    public float dashTimeOut;//The time to stop dashing physics and animation.
    public float dashTimeOutWeight;//The weight to cancel out dash force.
    private float newDashTimeOut;//Lock dash until Time.time + dashTimeOut.
    public float dashWaitTime;//Dash cooldown.
    private float newDashWaitTime;//Lock dash until Time.time + dashWaitTime.
    //private int lastWall;//For limiting the number of jump on the same wall.
    //private int currentWall;
    [Header("Gravity")]
    public float fallingGravity;
    public float slidingGravity;
    public float wallJumpGravity;
    //public float groundRayCheckDistance;    //The distance for Raycast to detect ground.
    //public float wallRayCheckDistance;    //The distance for Raycast to detect ground.
    [Header("Layer Masks")]
    public LayerMask groundMask;
    public LayerMask wallMask;
    public LayerMask enemyMask;
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ///// Animation //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Attack")]
    public float attackTime;    //Time for attack animation to complete.
    public bool isAttacking;
    [Header("Landing Animation Time")]
    public float landingTime;   //Time for landing animation to complete.
    private int currentState;
    public bool isFacingRight;
    [Header("Wall Detect")]
    public bool isRightWalled;
    public bool isLeftWalled;
    [Header("Enemy Detect")]
    public bool isRightEnemy;
    public bool isLeftEnemy;
    [Header("Idle")]
    public bool isIdle;
    [Header("Falling")]
    public bool isFalling;
    [Header("Grounded")]
    public bool isGrounded;
    [Header("isJumping")]
    public bool isJumping;
    public bool isJumpHanging;
    [Header("WallJumping")]
    public bool isWallJumping;
    [Header("Dashing")]
    public bool isDashing;
    [Header("Sliding")]
    public bool isSliding;
    [Header("Landing")]
    public bool isLanded;
    public float waitTime;     //New game time for animation to complete a loop.
    //Animator states hash. Used for activating animation states.
    private int idle = Animator.StringToHash("Idle");
    private int move = Animator.StringToHash("Move");
    private int dash = Animator.StringToHash("Dash");
    private int jump = Animator.StringToHash("Jump");
    private int wallJump = Animator.StringToHash("Wall Jump");
    private int wallSlide = Animator.StringToHash("Wall Slide");
    private int fall = Animator.StringToHash("Fall");
    private int land = Animator.StringToHash("Land");
    private int attack = Animator.StringToHash("Attack");
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// Input System EVENTs and METHODs ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        void DoJump()
        {
            if (rb2D.velocity.y < 0)
                jumpForce -= rb2D.velocity.y;
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            isGrounded = false;
        }
        if (isGrounded)     //Regular Jump
        {
            if (!isRightWalled && !isLeftWalled)
                DoJump();
            else if ((isRightWalled || isLeftWalled) && inputValue.x == 0)
                DoJump();
        }
        else if (isRightWalled && isFacingRight)
            WallJump(-1);
        else if (isLeftWalled && !isFacingRight)
            WallJump(1);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        isAttacking = true;
    }

    public void DoDash(InputAction.CallbackContext context)
    {
        isDashing = true;
        if (isFacingRight && !isWallJumping)
            Dash(1);
        if (!isFacingRight && !isWallJumping)
            Dash(-1);
    }
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// Awake, Update /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputValue = moveAction.action.ReadValue<Vector2>();//Detect player's input. Values are between -1 and 1 being left and right respectively. 0 is no input.
        jumpValue = jumpAction.action.ReadValue<float>();//Detect player's space bar input.
        //Debug.Log(jumpValue)
        CheckCamera();
        GroundCheck();
        WallCheck_EnemyCheck();
        CheckDash();
        CheckJump();//The order for these methods are crucial. Don't move them.
        CheckJumpHang();
        CheckFall();
        CheckMove();
        //CheckLanded();
        CheckDirection();
        var updateState = UpdateState();//Decides the next animation state the animator should be in.
        isAttacking = false;
        if (updateState == currentState)//If state hasn't changed, return.
            return;
        animator.CrossFade(updateState, 0, 0);//Allow smooth transitioning between animation states.
        currentState = updateState;//Update currentState.
    }
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// /// BASIC MOVEMENT and ANIMATION Update ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        if (isWallJumping)
            return wallJump;
        if (isDashing)
            return dash;
        if (isAttacking)//Attacking
            return KeepState(attack, attackTime);
        if (isFalling && isGrounded)//Landing
        {
            isFalling = false;
            return KeepState(land, landingTime);
        }
        if (isJumping)//Jumping
            return jump;
        if (isSliding && !isWallJumping)//Sliding
        {
            //rb2D.gravityScale = slidingGravity;//Set gravity to desired gravity when sliding.
            isFalling = false;
            WallSlide();
            return wallSlide;
        }
        if (isFalling && !isSliding)//Falling
        {
            return fall;
        }
        if (isGrounded && inputValue.x == 1 || inputValue.x == -1)//Moving
            return move;
        if (isIdle)//Idle
            return idle;
        return -1;//If no state detected
    }

    public void CheckMove()
    {
        if ((inputValue.x == 1 || inputValue.x == -1) && !isDashing && Time.time > newWallJumpTime)        //Moves the Slime based on player's inputValue.
        {
            if (boxCollider2D.IsTouchingLayers(wallMask) || isDashing)//If player is near a right wall or dashing, disable right movement to prevent conflicts.
            {
                if (inputValue.x == 1 && isRightWalled)
                    return;
                if (inputValue.x == -1 && isLeftWalled)
                    return;
            }
            if (boxCollider2D.IsTouchingLayers(enemyMask) || isDashing)
            {
                if (inputValue.x == 1 && isRightEnemy)
                    return;
                if (inputValue.x == -1 && isLeftEnemy)
                    return;
            }
            isIdle = false;
            if ((isJumping || isFalling || isWallJumping || isSliding) && !isGrounded)//Check for in air states to allow Y-axis movements.
                rb2D.velocity = new Vector2(inputValue.x * moveSpeed, rb2D.velocity.y);
            else if (isGrounded && jumpValue == 0)
                rb2D.velocity = new Vector2(inputValue.x * moveSpeed, 0f);//If grounded, glue to the ground. Helps prevent random fall and land states due to sudden change in velocity.
        }
        else if (inputValue.x == 0 && isGrounded && !isJumping && !isFalling && !isWallJumping && !isSliding && !isDashing && !isAttacking && !isJumpHanging)//If no input, stop moving and idle.
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            isIdle = true;
        }       
    }

    public void CheckJump()
    {
        if (rb2D.velocity.y > 0.1f)
        {
            isSliding = false;
            JumpHang();
        }
    }

    public void CheckFall()
    {
        if (rb2D.velocity.normalized.y < -0.1f)
        {
            if ((isRightWalled || isLeftWalled) && (inputValue.x == 1 || inputValue.x == -1))
            {//While falling and if a wall is nearby, allow sliding.
                isWallJumping = false;
                isJumping = false;
                isSliding = true;
                wallJumpPos = transform.position;
                //Debug.Log(wallJumpPos);
                return;
            }
            isFalling = true;
            rb2D.gravityScale = fallingGravity;//Set gravity to desired gravity when falling.
            isJumping = false;
            isWallJumping = false;//Deactivate wall jump status as soon as player starts to fall.
        }
        return;
    }


    public void GroundCheck()
    {
        isGrounded= Physics2D.BoxCast(transform.position - new Vector3(0,0.18f), groundBoxSize, 0, Vector2.down, 0, groundMask);
        if (isGrounded)
        {
            rb2D.gravityScale = 1.0f;
            isSliding = false;
            isJumpHanging = false;
        }
    }

    public void WallCheck_EnemyCheck()//Check for walls to allow player to do things like wall jump.
    {
        isRightWalled = Physics2D.BoxCast(transform.position + new Vector3(0.2f, 0f, 0f), wallBoxSize, 0f, Vector2.right, 0f, wallMask);
        isLeftWalled = Physics2D.BoxCast(transform.position - new Vector3(0.2f, 0f, 0f), wallBoxSize, 0f, Vector2.left, 0f, wallMask);
        isRightEnemy = Physics2D.BoxCast(transform.position + new Vector3(0.2f, 0f, 0f), enemyBoxSize, 0f, Vector2.right, 0f, enemyMask);
        isLeftEnemy = Physics2D.BoxCast(transform.position - new Vector3(0.2f, 0f, 0f), enemyBoxSize, 0f, Vector2.left, 0f, enemyMask);
        if (!isRightWalled || !isLeftWalled)
            isSliding = false;
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
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// COMPLEX MOVEMENT //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void WallJump(int direction)
    {
        //Overloaded with constraints to prevent super jumping due to multiplying forces from different jump mechanics.
        if (!isGrounded && Time.time > newWallJumpTime)
        {
            //Time.timeScale = 0.3333f;
            Vector2 jumpForce = new Vector2(wallJumpForceX * direction, wallJumpForceY);
            if (rb2D.velocity.normalized.y < 0.1f)
                jumpForce.y -= rb2D.velocity.y;
            isWallJumping = true;
            rb2D.gravityScale = wallJumpGravity;
            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            Flip();
            newWallJumpTime = Time.time + wallJumpWaitTime;//Wall Jump Cooldown
        }
    }

    public void WallSlide()
    {
        float jumpForce = wallSlideSpeed - rb2D.velocity.y;
        float movement = jumpForce * wallSlideAccelerationRate;
        movement = Mathf.Clamp(movement, -Mathf.Abs(jumpForce) * (1/Time.deltaTime), Mathf.Abs(jumpForce) * (1 / Time.deltaTime));
        rb2D.AddForce(movement * Vector2.up);
    }

    public void Dash(float direction)//Rudimentary, not fully developed. Discovery phase. Will check tutorial if stuck.
    {
        if (Time.time > newDashWaitTime)
        {
            rb2D.AddForce(new Vector2(direction * dashSpeed, 0f), ForceMode2D.Impulse);//The dash physics.
            newDashTimeOut = Time.time + dashTimeOut;//Get time to cancel dash.
            newDashWaitTime = Time.time + dashWaitTime;//Get dash cooldown.
        }
    }

    public void CheckDash()
    {
        if (isDashing && Time.time > newDashTimeOut)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x / dashTimeOutWeight, rb2D.velocity.y / dashTimeOutWeight);//Stop player from dashing and slow down.
            isDashing = false;
        }
    }

    public void JumpHang()//Allow player to stay in the air during the peak of their jump longer.
    {
        if (rb2D.velocity.y < (jumpForce/2) && !isJumpHanging && Time.time > newJumpHangTime)
        {
            isJumpHanging = true;
            rb2D.gravityScale = jumpHangGravity;
            newJumpHangTime = Time.time + jumpHangTime;
        }
    }
    public void CheckJumpHang()
    {
        if (isJumpHanging && Time.time > newJumpHangTime)
        {
            rb2D.gravityScale = fallingGravity;
        }
    }

    public void CheckCamera()
    {
              
            
    }
}