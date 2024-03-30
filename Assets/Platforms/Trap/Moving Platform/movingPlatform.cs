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
}
