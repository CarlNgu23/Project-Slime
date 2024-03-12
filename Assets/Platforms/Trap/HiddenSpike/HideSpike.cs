using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HideSpike : MonoBehaviour
{

    [SerializeField] public GameObject HiddenhitBox;
    [SerializeField] public float time;
    [SerializeField] public int dmg;
    [SerializeField] public Animator anim;

    [SerializeField] public BoxCollider2D hitbox;
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
            Debug.Log("HIDESPIKE");
            Stats.Instance.health -= dmg;
            StartCoroutine(SpikeAttack());
       
            StartCoroutine(ShowHitBox());
        }
    }
    IEnumerator SpikeAttack()
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("Attack");
        Instantiate(HiddenhitBox, transform.position, Quaternion.identity);
    }
    IEnumerator ShowHitBox()
    {
        yield return new WaitForSeconds(hitBoxCDTime);
        hitbox.enabled = true;

    }

}
