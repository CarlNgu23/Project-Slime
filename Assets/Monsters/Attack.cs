using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float limit_distance;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monster;
    [SerializeField] private BoxCollider2D attack;
    [SerializeField] private float start_Time;
    [SerializeField] private float wait_Time;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private Animator attack_Animation;

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<BoxCollider2D>();
        attack_Animation = monster.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Check_Distance();
    }

    private void Check_Distance()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < limit_distance && !isAttacking)
        {
            isAttacking = true;
            //Debug.Log("Within distance.");
            StartCoroutine(ActivateAttack());
        }
    }
    //Start attack routine
    IEnumerator ActivateAttack()
    {
        yield return new WaitForSeconds(start_Time);    //Waits set amount of time before attacking.
        attack_Animation.SetTrigger("Attack");
        attack_Animation.SetBool("isAttacking", true);
        attack.enabled = true;
        //Debug.Log("Attack Enabled.");
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(wait_Time);     //Waits set amount of time after attacking. Can be used for cooldown.
        attack.enabled = false;
        attack_Animation.SetBool("isAttacking", false);
        isAttacking = false;
        //Debug.Log("Attack Disabled.");
    }

    private void OnTriggerEnter2D(Collider2D attack)
    {
        if (attack.gameObject.CompareTag("Player"))
        {
            Stats.Instance.health -= 1;
            //Debug.Log("Dealt Damage.");
        }
    }
}
