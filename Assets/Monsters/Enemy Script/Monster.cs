using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Monster : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] public float flashtime;

    //flash when by hit
    private SpriteRenderer sr;
    private Color originlColor;


    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originlColor = sr.color;
    }

    // Update is called once per frame
   public void Update()
    {
        if (health <= 0)
        { 
        Destroy(gameObject);
        }
    }
    public void TakeDamage(int damage)
    { 
        
        health -= damage;
        FlashColor(flashtime);
    }


    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        sr.color = originlColor;

    }


}
