using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject player;
    public bool flip;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 scale=transform.localScale;
        if(player.transform.position.x<transform.position.x){
            scale.x=Mathf.Abs(scale.x) * -1 *(flip? -1:1);
            transform.Translate(speed*Time.deltaTime*-1,0,0);
        }
        else{
            scale.x=Mathf.Abs(scale.x)*(flip? -1:1);
            transform.Translate(speed*Time.deltaTime,0,0);
        }
        transform.localScale=scale;
    }
}
