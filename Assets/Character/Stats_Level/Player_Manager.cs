//Developed by Carl Ngu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player_Manager : MonoBehaviour
{
    public delegate void PlayerAttackDelegator();
    public event PlayerAttackDelegator OnPlayerAttack;
    public AudioSource audioSource;
    public AudioClip parrySuccessSound;
    public SpriteRenderer SpriteRenderer;
    private Color originalColor = Color.white;
    public float flashDuration = 0.1f;
    public bool isParrying = false; //Flag to check parry state
    public bool canParry = true;
    public float parryDuration = 0.5f;
    public float parryCooldown = 2f;
    private Animator animator;
    public GameObject parrySlice;
    private ScreenShake screenShake;



    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        screenShake = Camera.main.GetComponent<ScreenShake>();

    }

    private void Update()
    {
        if (Stats.Instance.health <= 0)
        {
            SceneManager.LoadScene(2);
            Stats.Instance.health = Stats.Instance.maxHP;
        }
        if (Input.GetKeyDown(KeyCode.X) && canParry)
        {
            StartCoroutine(StartParry());
        }
    }

    public void isAttacking()
    {
        OnPlayerAttack?.Invoke();
    }

    public LayerMask player_Mask;

    //When a monster die, the ExpManager will become enabled and references ExpCheck.
    private void OnEnable()
    {
        ExpManager.Instance.OnReward += ExpCheck;
    }
    //The ExpManager will become disabled when nothing happens.
    private void OnDisable()
    {
        ExpManager.Instance.OnReward -= ExpCheck;
    }

    private void ExpCheck(int expReward)
    {
        Stats.Instance.currentExp += expReward;
        while (Stats.Instance.currentExp >= Stats.Instance.requiredExp)
        {
            LevelUp();
        }
    }
    public void FlashRed()
    {
        Debug.Log("FlashRed method called.");
        StartCoroutine(FlashRedRoutine());
    }

    private IEnumerator FlashRedRoutine()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            SpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
            SpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Player color set to red.");
            yield return new WaitForSeconds(flashDuration);
            SpriteRenderer.color = originalColor;
            Debug.Log("Player color reset to original.");
        }
    }
    private void LevelUp()
    {
        UpdateStats();
    }
    public void UpdateStats()
    {
        Stats.Instance.level += 1;
        Stats.Instance.maxHP += 50;
        Stats.Instance.health = Stats.Instance.maxHP;
        Stats.Instance.attack += 1;
        Stats.Instance.defense += 1;
        Stats.Instance.strength += 1;
        Stats.Instance.dexterity += 1;
        Stats.Instance.currentExp -= Stats.Instance.requiredExp;
        Stats.Instance.requiredExp += Stats.Instance.requiredExp + ((int)Math.Pow(Stats.Instance.level, 4) / 4);
    }

    private IEnumerator StartParry()
    {
        //Activate Parry State
        parrySlice.SetActive(true);
        isParrying = true;
        canParry = false;
        Debug.Log("Parry Started.");

        // Play parry start animation
        //implement later

        yield return new WaitForSeconds(parryDuration);
        isParrying = false;
        parrySlice.SetActive(false);
        Debug.Log("Parry Ended");
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
        Debug.Log("Parry cooldown ended.");
    }

    public void PlayParrySuccessEffect()
    {
        // Play parry success sound
        audioSource.PlayOneShot(parrySuccessSound);

        // Trigger screen shake
        if (screenShake != null)
        {
            StartCoroutine(screenShake.Shake(0.2f, 0.1f)); // Adjust duration and magnitude as needed
        }
    }
}

