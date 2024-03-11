using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] public int dmg;
    [SerializeField] public float destoryTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destoryTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            Debug.Log("HITBOX");
            Stats.Instance.health -= dmg;
        }
    }
}
