using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEyeDogAfterImageSprite : MonoBehaviour
{
    private Transform enemy;
    
    private SpriteRenderer SR;
    private SpriteRenderer enemySR;

    private Color color;

    private float alpha;//keep track of the current alpha
    private float alphaSet=0;
    private float alphaMultiplier= 0.85f; // how quickly to fade? Smaller num,the faster the sprite will fade

    private void OnEnable()
    {
        SR=GetComponent<SpriteRenderer>();
        enemy=GameObject.FindGameObjectWithTag("enemy").transform;
        enemySR=enemy.GetComponent<SpriteRenderer>();
        alpha=alphaSet;
        SR.sprite=enemySR.sprite;
        transform.position=enemy.position;
        transform.rotation=enemy.rotation;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f,1f,1f, alpha);
        SR.color=color;
    }
}
