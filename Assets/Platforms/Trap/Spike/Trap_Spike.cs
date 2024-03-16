using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] public int dmg;
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] public float hitBoxCDTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            //Debug.Log("??????");
            Stats.Instance.health -= dmg;
            hitbox.enabled = false;
            StartCoroutine(ShowHitBox());
        }
    }


    IEnumerator ShowHitBox()
    { 
        yield return new WaitForSeconds(hitBoxCDTime);
        hitbox.enabled = true;

    }
}
