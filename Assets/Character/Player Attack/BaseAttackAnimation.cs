using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackAnimation : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private PolygonCollider2D baseAttack2d;


    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (coll == null)
        {
            coll = GetComponent<BoxCollider2D>();
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

    }

    void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {


            anim.SetTrigger("Attack");
          


        }
    }

    




}
