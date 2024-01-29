using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
   

    private enum MovementState{ idle,moving,jumping,falling,attack}
    private float dirX=0f;
    private float dirNX=0f;


    // Start is called before the first frame update
    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
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
        
         dirX= Input.GetAxisRaw("right");
        rb.velocity=new Vector2(dirX*10f,rb.velocity.y);
        dirNX= Input.GetAxisRaw("left");
        rb.velocity=new Vector2(dirNX*-10f,rb.velocity.y);
      
        if (Input.GetButtonDown("Jump")){
            rb.velocity= new Vector2(rb.velocity.x,8f);
            anim.SetBool("jumping",true);
        }
        else{
            anim.SetBool("jumping",false);
        }

        AnimationUpdate();
    }

    private void AnimationUpdate(){
        MovementState state;
         if(dirX>0f ){//right
            rb.velocity= new Vector2(8f,rb.velocity.y);
       
            state=MovementState.moving;
            sprite.flipX=false;
          
        }
        else if(dirNX<0f ){//left
            rb.velocity= new Vector2(-8f,rb.velocity.y);
     
            state=MovementState.moving;
            sprite.flipX=true;
        }
        if(rb.velocity.y> 0.1f){
            state=MovementState.jumping;
        }
        else if(rb.velocity.y<0f){
            state=MovementState.falling;
        }
        else{
            state=MovementState.idle;
        }
        anim.SetInteger("state",(int)state);
    }
}

 
