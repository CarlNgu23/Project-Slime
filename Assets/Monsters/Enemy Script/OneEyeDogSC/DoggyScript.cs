using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggyScript : MonoBehaviour
{   //this is testing
    
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int Destination;

//to patrol
    void Update()
    {
        if(Destination ==0 )
        {
            transform.position= Vector2.MoveTowards(transform.position, patrolPoints[0].position,moveSpeed *Time.deltaTime);
            
            if(Vector2.Distance(transform.position,patrolPoints[0].position)<0.2f)
            {   
                transform.localScale= new Vector3(1.5f,1.5f,0);
                Destination=1;
            }
        }
        if(Destination == 1 )
        {
            transform.position= Vector2.MoveTowards(transform.position, patrolPoints[1].position,moveSpeed *Time.deltaTime);
            
            if(Vector2.Distance(transform.position,patrolPoints[1].position)<0.2f)
            {
                transform.localScale= new Vector3(-0.5f,0.5f,0);
                Destination=0; //moves back 
            }
        }
    }
    
}
