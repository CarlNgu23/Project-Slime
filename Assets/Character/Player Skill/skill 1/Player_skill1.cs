using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_skill1 : MonoBehaviour
{

    [SerializeField] private int damage;    //player skill1  damage
    [SerializeField] public float waitTime; //waitTime hitbox disappear time
    [SerializeField] public float startTime; //startTime hit box appear time
    private Animator anim;
    private PolygonCollider2D skill1;
    [SerializeField] private GameObject player;
    [SerializeField] private int cost;  // hp cost for spelling
    [SerializeField] public float cooldownTime = 2.0f;  // skill cooldown unit second

    private bool isCooldown = false;
    private float cooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        GetComponent<Animator>();
        skill1 = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SkillAttack();

        UpdateCooldownTimer();
    }

     void SkillAttack()
    {
        if (Input.GetButtonDown("Skill_1") && !isCooldown)
        {
            if (player.GetComponent<Stats_Level>().SkillCost(cost))
            {
                anim.SetTrigger("Skill_1");
                StartCoroutine(StartAttack());

                StartCooldown();
            }
            else
            {
                Debug.Log("no enough hp");
            }
        }
        else if(Input.GetButtonDown("Skill_1") && isCooldown)
        {
            Debug.Log("Skill in cooldown");
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        skill1.enabled = true;
        StartCoroutine(disableHitBox());
    }


    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(waitTime);
        skill1.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Monster>().TakeDamage(damage);
        }
    }


    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
    }

    void UpdateCooldownTimer()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0.0f)
            {
                // cooldown end reset.
                isCooldown = false;
                Debug.Log("cooldown end");
            }
        }
    }

    }
