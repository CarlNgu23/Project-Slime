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
    public string MonsterID;

    public float knockbackForceX = 10f; // Adjust this value as needed
    public float knockbackForceY = 5f; // Adjust this value as needed

    private void Awake()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        animations = GetComponent<Animator>();
        detection = GetComponent<Detection>();
        expManager = GameObject.Find("ExpManager").GetComponent<ExpManager>();
        cpu_Movement = GetComponent<CPU_Movement>();
    }

    private void Start()
    {
        // Ensure the mass is set to a reasonable value
        rgbd2D.mass = 1f; // Adjust as needed, this is a typical value for testing
    }

    private void Update()
    {
        AnimationTransition();
        if (hp <= 0)
        {
            isDying_Ref = true;     // A reference to prevent chasing when dying.
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitBox.IsTouchingLayers(player_BasicAttack_Mask))
        {
            TakeDamage(other.transform.position);
        }
    }

    private void TakeDamage(Vector3 attackerPosition)
    {
        hp -= ((Stats.Instance.attack + Stats.Instance.strength) - defense);

        // Always flash red and apply knockback
        StartCoroutine(FlashRed());
        Vector2 knockbackDirection = (transform.position - attackerPosition).normalized;
        Debug.Log("Knockback Direction: " + knockbackDirection);
        ApplyKnockback(new Vector2(knockbackDirection.x, 1), new Vector2(knockbackForceX, knockbackForceY)); // Adjust the knockback force as needed

        if (hp <= 0)
        {
            // Trigger death animation
            DieAnimation();
        }
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private void ApplyKnockback(Vector2 knockbackDirection, Vector2 knockbackForce)
    {
        rgbd2D.velocity = Vector2.zero; // Stop current movement
        Vector2 force = new Vector2(knockbackDirection.x * knockbackForce.x, knockbackForce.y);
        Debug.Log("Applying Knockback Force: " + force);
        rgbd2D.AddForce(force, ForceMode2D.Impulse);
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
        FindObjectOfType<QuestManager>().UpdateQuestRequirement(MonsterID, 1);
    }
}
