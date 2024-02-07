using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    //base value
    public int health;
    public int damage;

    public Monster(int initialHp, int initialDamage)
    {
        health = initialHp;
        damage = initialDamage;
    }

    // Update is called once per frame
   public void Update()
    {
        if (health <= 0)
        {
            Die();
        }

    }


    public void TakeDamage(int amount)
    {
        health -= amount;

    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }



    /*
      
     this code use in player attack, just copy paste to attack class

       void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"){
            other.GetComponent<Monster>().TakeDamage(AttackDmg);
        }
    }
         
     */
}
