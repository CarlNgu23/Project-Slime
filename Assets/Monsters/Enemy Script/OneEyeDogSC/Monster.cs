using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Monster : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int ReceivedDamage;
    [SerializeField] public float flashtime;
    [SerializeField] public int expReward;
    private Animator anim;
    private SpriteRenderer monsterSprite;
    private Color originalColor;
    public bool isDead;

    // Start is called before the first frame update
    public void Start()
    {
        monsterSprite = GetComponent<SpriteRenderer>();
        originalColor = monsterSprite.color;
        anim=GetComponent<Animator>();
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
   public void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (health <= 0)
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
    public void TakeDamage(int ReceivedDamage)
    { 
        
        health -= ReceivedDamage;
        FlashColor(flashtime);

        if (health <= 0)
        {
            health=0;
            isDead=true;
        }
        if(isDead)
        {
            anim.SetTrigger("isDead");
            
        }
    
    }


    void FlashColor(float time)
    {
        monsterSprite.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        monsterSprite.color = originalColor;
    }

    public void Die()
    {
        ExpManager.Instance.GiveExp(expReward);
        //anim.SetTrigger("isDead");
        Destroy(gameObject, 0.9f);
    }

    
}
