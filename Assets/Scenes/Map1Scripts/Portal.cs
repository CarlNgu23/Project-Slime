//Developed by Carl Ngu
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public BoxCollider2D portalCollider;
    public GameObject player;

    private void Awake()
    {
        portalCollider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && Vector2.Distance(player.transform.position, transform.position) < 0.5f)
        {
            portalCollider.enabled = true;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
