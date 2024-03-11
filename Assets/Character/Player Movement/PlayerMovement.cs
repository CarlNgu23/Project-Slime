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
    public InputActionReference attackAction;   //Player's attack input.
    public Vector2 inputValue;      //Value of the Player's Move inputs.
    //Physics
    public float moveSpeed;
    public float jumpForce;
    public float groundRayCheckDistance;    //The distance for Raycast to detect ground.
    public LayerMask groundMask;
    //Animation
    public float attackTime;    //Time for attack animation to complete.
    public float landingTime;   //Time for landing animation to complete.
    private int currentState;
    public float waitTime;     //New game time for animation to complete a loop.
    public bool isAttacking;
    public bool isLanded;
    public bool isGrounded;
    public bool isFacingRight;
    //Animator states hashing.
    private int idle = Animator.StringToHash("Idle");
    private int move = Animator.StringToHash("Move");
    private int jump = Animator.StringToHash("Jump");
    private int fall = Animator.StringToHash("Fall");
    private int land = Animator.StringToHash("Land");
    private int attack = Animator.StringToHash("Attack");

    private void OnEnable()
    {   //Jump and Attack inputs are single press occurrance, so they require event handling.
        jumpAction.action.performed += Jump;    //When Jump input is pressed, do Jump()
        attackAction.action.performed += Attack;    //When attack input is pressed, do Attack()
    }

    private void OnDisable()
    {   //Disable upon no action.
        jumpAction.action.performed -= Jump;
        attackAction.action.performed -= Attack;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        isAttacking = true;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //// Awake, Update //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputValue = moveAction.action.ReadValue<Vector2>();//Detect player's input. Values are between -1 and 1 being left and right respectively. 0 is no input.
        GroundCheck();
        CheckMove();
        CheckDirection();
        var updateState = UpdateState();
        isAttacking = false;
        isLanded = false;
        if (updateState == currentState)
            return;
        animator.CrossFade(updateState, 0, 0);
        currentState = updateState;
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public void CheckMove()
    {
        if (inputValue.x == 1 || inputValue.x == -1)        //Moves the Slime based on inputValue.
            rb2D.velocity = new Vector2(inputValue.x * moveSpeed, rb2D.velocity.y);
        else if (inputValue.x == 0 && isGrounded)
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
    }

    private int UpdateState()
    {
        //Time.time is current game time. Game time is the time that has past since the game started.
        int KeepState(int state, float animationDuration)
        {
            waitTime = Time.time + animationDuration;
            return state;
        }
        if (Time.time < waitTime)//Checks for if waitTime is required. If it's required, waitTime is greater than current game time.
            return currentState;
        if (isAttacking)//Attacking
            return KeepState(attack, attackTime);
        if (currentState == fall && isGrounded)//Landing
            return KeepState(land, landingTime);
        if (rb2D.velocity.normalized.y > 0.01f)//Jumping
            return jump;
        if (rb2D.velocity.normalized.y < -0.01f)//Falling
            return fall;
        if (isGrounded && inputValue.x == 1 || inputValue.x == -1)//Moving
            return move;
        return idle;    //Idle
    }

    public bool GroundCheck()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundRayCheckDistance, groundMask);
        if (groundHit)
            return isGrounded = true;
        else
            return isGrounded = false;
    }

    public void CheckDirection()
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
}