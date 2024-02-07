using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : Monster
{
    // the constructor follow by (hp, damage)
    public EnemyEye() : base(100, 15)
    {


    }

    // Start is called before the first frame update
   public void Start()
    {
        
    }

    // Update is called once per frame
   public void Update()
    {
        
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("bat died");
    }

}
