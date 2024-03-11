using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HideSpike : MonoBehaviour
{

    [SerializeField] public GameObject hitBox;
    [SerializeField] public float time;
    [SerializeField] public int dmg;
    [SerializeField] public Animator anim;

    [SerializeField] private PolygonCollider2D PlayerHitBox;
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
        if (collision.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            Debug.Log("HIDESPIKE");
            Stats.Instance.health -= dmg;
            StartCoroutine(SpikeAttack());
           // PlayerHitBox.enabled = false;
            StartCoroutine(ShowHitBox());
        }
    }
    IEnumerator SpikeAttack()
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("Attack");
        Instantiate(hitBox, transform.position, Quaternion.identity);
    }
    IEnumerator ShowHitBox()
    {
        yield return new WaitForSeconds(hitBoxCDTime);
        PlayerHitBox.enabled = true;

    }

}
