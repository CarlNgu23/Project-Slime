//Created by Wen
//Debugged by Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpike : MonoBehaviour
{
    public float animationTime;
    public float newTime;
    public int dmg;
    public Animator anim;
    public float hitBoxCDTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time > newTime)
        {
            anim.SetTrigger("Attack");
            Stats.Instance.health -= dmg;
            newTime = Time.time + hitBoxCDTime;
        }
    }
}
