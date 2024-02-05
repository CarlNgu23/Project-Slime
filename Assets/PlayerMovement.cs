using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
   [SerializeField] private LayerMask jumpableGound;
   [SerializeField] private float moveSpeed=2f;
   [SerializeField] private float jumpForce=5f;

    private enum MovementState{ idle,moving,jumping,falling,attack}
    private float dirX=0f;
    private float dirNX=0f;


    // Start is called before the first frame update
    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        coll=GetComponent<BoxCollider2D>();
        sprite=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {   
        
        FixedUpdate();
        AnimationUpdate();
    }

     
 private void FixedUpdate()
    {
         //looking direction
          dirX= Input.GetAxisRaw("right");
         //rb.velocity=new Vector2(dirX*10f,rb.velocity.y);
         dirNX= Input.GetAxisRaw("left");
         //rb.velocity=new Vector2(dirNX*-10f,rb.velocity.y);
      
        if (Input.GetButtonDown("Jump")&& isGrounded()){
            rb.velocity= new Vector2(rb.velocity.x,jumpForce);
        
        }
    }
    private void AnimationUpdate(){

        MovementState state=MovementState.idle; //default
         
        if(dirX>0f){//right 
            rb.velocity= new Vector2(moveSpeed,rb.velocity.y);
            state=MovementState.moving;
            sprite.flipX=false;
        }
        else if(dirNX<0f){//left
            rb.velocity= new Vector2(-moveSpeed,rb.velocity.y);
            state=MovementState.moving;
            sprite.flipX=true;
        }
        if(rb.velocity.y>0.1f){
            state=MovementState.jumping;
        }
        else if(rb.velocity.y<-.1f){
            state=MovementState.falling;
        }
       
        anim.SetInteger("state",(int)state);
    }
    private bool isGrounded(){//checks if the player touches the ground
        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size,0f,Vector2.down,0.1f,jumpableGound);
    }


}

 
