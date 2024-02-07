using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : Monster
{
    [SerializeField] float flyingSpeed;

    // the constructor follow by (hp, damage)
    public EnemyBat() : base(50,5)
    {


    }



    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
   public void Update()
    {
        base.Update();
    }


    protected override void Die()
    {
        base.Die();
        Debug.Log("bat died");
    }


    public void Fly()
    {
       
    }

}
