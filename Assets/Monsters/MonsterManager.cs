using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int hp = 10;
    public int atk = 5;
    public int defense = 1;
    public int exp = 100;
    public LayerMask player_BasicAttack_Mask;
    public Detection detection;
    public Animator animations;
    private Rigidbody2D rgbd2D;
    private BoxCollider2D hitBox;
    public bool isDying_Ref = false;
    public ExpManager expManager;
    public CPU_Movement cpu_Movement;

    private void Awake()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        animations = GetComponent<Animator>();
        detection = GetComponent<Detection>();
        expManager = GameObject.Find("ExpManager").GetComponent<ExpManager>();
        cpu_Movement = GetComponent<CPU_Movement>();
    }

    private void Update()
    {
        AnimationTransition();
        if (hp <= 0)
        {
            isDying_Ref = true;     //A reference to prevent chasing when dying.
            DieAnimation();
        }
    }

    private void AnimationTransition()
    {
        if (rgbd2D.velocity.x > 0.1f || rgbd2D.velocity.x < -0.1f)
        {
            animations.SetBool("isIdle", false);
        } 
        else
        {
            animations.SetBool("isIdle", true);
        }
    }

    private void OnTriggerEnter2D()
    {
        if (hitBox.IsTouchingLayers(player_BasicAttack_Mask))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        hp -= ((Stats.Instance.attack + Stats.Instance.strength) - defense);
    }

    private void DieAnimation()
    {
        rgbd2D.velocity = new Vector2(0f, 0f);
        animations.SetBool("isDead", true);
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);
        expManager.GiveExp(exp);
        Destroy(cpu_Movement.rightMonsterBoundaryGameObject);
        Destroy(cpu_Movement.leftMonsterBoundaryGameObject);
        Destroy(gameObject);
    }

}
