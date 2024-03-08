using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEyeDog : Monster
{
    //private Animator anim;
    //private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //health = 5;
        expReward = 6000;
        // anim=GetComponent<Animator>();
        // rb=GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        base.Update();
    }
   

    
}
