using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    [SerializeField] public float speed;    //platform moving speed 
    [SerializeField] public float waitTime; //the time waiting before moving back
    [SerializeField] public Transform[] movPos; //give two point range
    private int i;
    



    // Start is called before the first frame update
    void Start()
    {
        i = 0;
   
    }

    // Update is called once per frame
    void Update()
    {
       platformMove();
    }

    private void platformMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, movPos[i].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movPos[i].position) < 0.1f)
        {
            if (waitTime < 0.0f)        // may have more point to move ,when touch the last point ,reset to 0
            {
                if (i != movPos.Length - 1)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }

                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // if player stand on the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the player as a child of the moving platform so that the player will follow the platform's movement
            collision.transform.parent = transform;     
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // if player left the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Removes the player from the child when exit platform. 
            collision.transform.parent = null;
        }
    }
}
