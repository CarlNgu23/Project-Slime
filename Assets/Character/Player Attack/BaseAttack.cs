using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [SerializeField] public int damage;    //player damage
    [SerializeField] public float waitTime; //waitTime hitbox disappear time
    [SerializeField] public float startTime; //startTime hit box appear time
    private Animator anim;
    private BoxCollider2D baseAttack2d;
    [SerializeField]private GameObject player;
    //[SerializeField] private GameObject enemy;
    [SerializeField] private LayerMask enemy_Layer;
    // Start is called before the first frame update


    void Start()
    {     
        anim = player.GetComponent<Animator>();
        GetComponent<Animator>();
        baseAttack2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        damage = Stats.Instance.attack;    //References the attack stats for base damage.
        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }
        
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        baseAttack2d.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(waitTime);
        baseAttack2d.enabled = false;
    }

    //private void OnTriggerEnter2D()
    //{
    //    if (baseAttack2d.IsTouchingLayers(enemy_Layer))
    //    {
    //        Debug.Log("Collided");
    //        Monster_Stats.health -= (Stats.Instance.attack - Monster_Stats.defense);
    //    }
    //}

}
