//Created by Wen
//Debugged by Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int dmg;
    public float hitBoxCDTime;
    private float newTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time > newTime)
        {
            newTime = Time.time + hitBoxCDTime;
            Stats.Instance.health -= dmg;
        }
    }
}
