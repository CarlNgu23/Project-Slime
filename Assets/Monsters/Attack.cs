using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attack_Range;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monster;
    [SerializeField] private BoxCollider2D attack;
    [SerializeField] private float wait_Time;       //Cooldown time.
    [SerializeField] private float start_Time;      //Time for animation to be at the "hit" frame.
    [SerializeField] private float end_Time;        //Time for animation to complete + 0.2 second.
    [SerializeField] private float cooldown_Time;
    [SerializeField] public static bool isAttacking_Ref = false;       //Used as reference in Detection script to detect with attack is complete and in cooldown.
    [SerializeField] private bool isAttacking = false;                 //Used to prevent StartCoroutine from stacking.
    [SerializeField] private Animator animations;

    private Rigidbody2D monster_rb2d;
    
    

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<BoxCollider2D>();
        monster_rb2d = monster.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Check_Distance();
    }

    private void Check_Distance()
    {
        //Debug.Log(Detection.isCPUMove);
        if (!Detection.isCPUMove && Vector2.Distance(transform.position, player.transform.position) <= attack_Range && !isAttacking)
        {
            isAttacking = isAttacking_Ref = true;
            monster_rb2d.velocity = new Vector2(0f, 0f);    //Stop movement when player is within attack range.
            StartCoroutine(WaitBeforeAttack());
        }
    }
    //Add Delay to allow player a chance to dodge.
    IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(wait_Time);
        StartCoroutine(AttackAnimation());
    }
    //To synchronize animation with Collider2D physics. It's important to know the time when the animation is at the "hit" frame.
    IEnumerator AttackAnimation()
    {
        animations.SetTrigger("Attack");
        animations.SetBool("isAttacking", true);
        yield return new WaitForSeconds(start_Time);          //This should be the time it takes for the animation to reach the "hit" frame in the Animation window. Ask Carl if you don't understand this.
        StartCoroutine(ActivateAttack());
    }
    //Start attack routine
    IEnumerator ActivateAttack()
    {
        //Debug.Log("Attack Enabled.");
        attack.enabled = true;
        yield return new WaitForSeconds(end_Time);           //Wait set amount of time for attack animation to complete. Add 0.2 second more to the remaining time for the animation to complete.
        //Debug.Log("Attack disabled.");                     //Total animation time - start_Time = end_Time + 0.2
        animations.SetBool("isAttacking", false);
        isAttacking_Ref = false;
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        //Debug.Log("Waiting Cooldown.");
        yield return new WaitForSeconds(cooldown_Time);         //Wait set amount of time after attacking. Used for cooldown.
        //Debug.Log("Attack out of cooldown.");
        isAttacking = false;
    }

    public void OnTriggerStay2D()
    {
        if (attack.IsTouchingLayers(playerMask))
        {
            Stats.Instance.health -= 1;
            //Debug.Log("Dealt Damage.");
            attack.enabled = false;
        }
    }
}
