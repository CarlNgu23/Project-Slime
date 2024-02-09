using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{   
    [SerializeField]private Transform leftEdge;
    [SerializeField]private Transform rightEdge;

    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;
    
    private  Vector3 initScale;
    private bool movingLeft;
    
    [SerializeField] private float idleDur;
    private float idleTimer;
    [SerializeField]private Animator Enemy_anim;


    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {   
        if(movingLeft)
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
        enemy.localScale=new Vector3(Mathf.Abs(initScale.x)* dir,initScale.y,initScale.z);

    //moving to that dir
        enemy.position= new Vector3(enemy.position.x+Time.deltaTime*dir*speed, enemy.position.y,enemy.position.z);
    }
}
