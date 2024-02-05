using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlimeShot : MonoBehaviour
{ 

[Range(1,10)]
[SerializeField] private float speed=10f;

[Range(1,10)]
[SerializeField] private float lifeTime=3f;
private SpriteRenderer sprite;
private float moveSpeed=2f;
private bool FacingRight=false;
private Rigidbody2D rb;
//private SpriteRenderer sprite;
private float dirX=0f;

private void Start(){
     rb=GetComponent<Rigidbody2D>();
     Destroy(gameObject,lifeTime);
    
}

// private void Update(){
//     FixedUpdate();
//     AnimationUpdate();
// }
private void FixedUpdate(){

  
   
     rb.velocity=transform.right*speed;
     
     // if(sprite.flipX==true){
     //      rb.velocity=-transform.right*speed;
     // }
     
     
    //AnimationUpdate();
}
// void Flip()
// 	{
// 		FacingRight = !FacingRight;
// 		Vector3 theScale = (transform.forward * rb.velocity.y) + (transform.right * rb.velocity.x);
// 		//theScale.x *= -1;
// 		transform.localScale = theScale;
//     }



}
