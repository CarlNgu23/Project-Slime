using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseAttack : MonoBehaviour
{
    public float startTime;
    public float endTime;
    private BoxCollider2D baseAttack2d;
    private PlayerMovement playerMovement;

    private void OnEnable()
    {
        playerMovement.attackAction.action.performed += Attack;
    }

    private void OnDisable()
    {
        playerMovement.attackAction.action.performed -= Attack;
    }

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        baseAttack2d = GetComponent<BoxCollider2D>();
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (Time.time < playerMovement.waitTime)
            return;
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {   //startTime is the time to wait for animation hit frame.
        yield return new WaitForSeconds(startTime);
        baseAttack2d.enabled = true;
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {   //waitTime is the time to wait for animation to complete.
        yield return new WaitForSeconds(endTime);
        baseAttack2d.enabled = false;
    }
}
