using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public BoxCollider2D portalCollider;
    public BoxCollider2D playerCollider;

    private void Awake()
    {
        portalCollider = GetComponent<BoxCollider2D>();
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            portalCollider.enabled = true;
        }
    }
    private void OnTriggerEnter2D()
    {
        if (playerCollider.IsTouching(portalCollider))
        {
            SceneManager.LoadScene(2);
        }
    }
}
