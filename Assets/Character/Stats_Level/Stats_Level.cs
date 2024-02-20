using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats_Level : MonoBehaviour
{
    [SerializeField] public int level = 1;
    [SerializeField] public int currentExp = 0;
    [SerializeField] public int requiredExp = 100;
    [SerializeField] public int health = 100;
    [SerializeField] public int attack = 1;
    [SerializeField] public int defense = 0;
    [SerializeField] public int strength = 1;
    [SerializeField] public int dexterity = 1;
    
    //
    [SerializeField] public int ReceivedDamage;
    public Animator anim;
    public bool isDead;
    
    void Start()
   {
    anim=GetComponent<Animator>();
    
    
   }

   void FixedUpdate()
   {
       if (health < 0)
        {
            health=0;
            isDead=true;
        }
        if(isDead)
        {
            anim.Play("isDead");
            
            Die();
        }
   }
    private void OnEnable()
    {
        ExpManager.Instance.OnReward += ExpCheck;
    }
    private void OnDisable()
    {
        ExpManager.Instance.OnReward -= ExpCheck;
    }

    private void ExpCheck(int expReward)
    {
        currentExp += expReward;
        while (currentExp >= requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level += 1;
        health += 50;
        attack += 1;
        defense += 1;
        strength += 1;
        dexterity += 1;

        currentExp -= requiredExp;
        requiredExp += requiredExp + ((int)Math.Pow(level, 4)/4);
    }


//
public void TakeDamage(int ReceivedDamage)
    { 
        health -= ReceivedDamage;
        if (health < 0)
        {   
            health=0;
            isDead=true;
        }
        if(isDead)
        {
            anim.SetTrigger("isDead");
        }
    }


void Die()
    {
    
    Destroy(gameObject,0.5f);// Destroy(Item_to_destroy,time_length)
    }
}
    
   
   

