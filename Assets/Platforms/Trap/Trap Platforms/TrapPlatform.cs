using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{

    [SerializeField] private Animator anima;
    [SerializeField] private BoxCollider2D box2d;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anima.SetTrigger("Collapse");
            Debug.Log("TRAPTRAPTRAP");
        }
    }

   
    void DisableBoxCollider2D()
    {
        box2d.enabled = false;
    }

    void DestoryThis()
    {
        Destroy(gameObject);
    }
}
