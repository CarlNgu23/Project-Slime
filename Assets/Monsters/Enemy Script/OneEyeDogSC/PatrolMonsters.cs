using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMonsters : MonoBehaviour
{   
    [SerializeField]private Transform leftEdge;
    [SerializeField]private Transform rightEdge;

    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;
    
    private  Vector2 initScale;
    private bool movingLeft;
    
    [SerializeField] private float idleDur;
    private float idleTimer;
    [SerializeField]private Animator Enemy_anim;
    
/*Chasing*/
    public Transform player; //keeps track of the player
    public bool isChasing;
    public float chaseDis; //how close for the monster to start chasing


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   

        if(isChasing)
        {   
            if(enemy.position.x > player.position.x)//if player on the left
            {  
           
               if((enemy.position.x>=leftEdge.position.x ))
                {   
                    Enemy_anim.SetBool("isWalking",true);
                    Monster_movement(-1);
                    enemy.position += Vector3.left * speed *Time.deltaTime;
                }
                else
                {   
                    Enemy_anim.SetBool("isWalking",true);
                    DirectionChange();
                    enemy.position += Vector3.right * speed *Time.deltaTime;
                   
                }
                
            }
            else if(enemy.position.x < player.position.x)//if player on the right
            {   

                if(enemy.position.x<=leftEdge.position.x)
                {   
                    Enemy_anim.SetBool("isWalking",true); 
                    enemy.position += Vector3.left * speed *Time.deltaTime;
                    Monster_movement(-1);
                   
                    
                }
                else
                {   
                    Enemy_anim.SetBool("isWalking",true);
                    enemy.position += Vector3.right* speed *Time.deltaTime;
                    DirectionChange();
                    
                    
                }
            }
        }
        else
        {   
            if(Vector2.Distance(transform.position,player.position)< chaseDis) //if player is close to the monster
            {   
                isChasing=true;
                
            }
/*Og patrol*/
            if(movingLeft && !isChasing)
            {
                if(enemy.position.x>=leftEdge.position.x)
                {
                    Monster_movement(-1);
                }
                else
                {
                    DirectionChange();
                }
            }
            else{
                if(enemy.position.x<=rightEdge.position.x)
                {
                    Monster_movement(1);
                }
                else
                {
                    DirectionChange();
                }
            }
        }

    }
        
        
   

    private void DirectionChange()
    {   
        Enemy_anim.SetBool("isWalking",false);

        idleTimer += Time.deltaTime;

        if(idleTimer>idleDur)
        {
            movingLeft =!movingLeft;
        }
       
    }

    private void Awake()
    {
        initScale=enemy.localScale;
    }
    
    private void Monster_movement(int dir)
    {   
        idleTimer=0;
        Enemy_anim.SetBool("isWalking",true);
        
    //facing that dir
        enemy.localScale=new Vector2(Mathf.Abs(initScale.x)* dir,initScale.y);

    //moving to that dir
        enemy.position= new Vector2(enemy.position.x+Time.deltaTime*dir*speed, enemy.position.y);
    }
}
