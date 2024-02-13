using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Monster
{


    // Start is called before the first frame update
   public new void Start()
    {
        base.Start();
        health = 5;
        expReward = 1000;
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
    }
}
