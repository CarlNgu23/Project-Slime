using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Collider2D portalCollider;

    private void Awake()
    {
        portalCollider = GetComponent<Collider2D>();
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
        SceneManager.LoadScene(2);
    }
}
