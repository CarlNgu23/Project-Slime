using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public GameObject main_camera;
    public float parallaxFactorX;
    public float parallaxFactorY;
    public float startPos;
    public float endPos;
    public float distance;
    public float moveX;
    public float moveY;
    public float offsetX;
    public float offsetY;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        distance = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        endPos = main_camera.transform.position.x * (1 - parallaxFactorX);
        moveX = (main_camera.transform.position.x + offsetX) * parallaxFactorX;
        moveY = (main_camera.transform.position.y + offsetY) * parallaxFactorY;
        transform.position = new Vector2(moveX, moveY);

        //Infinite Scrolling
        if ((startPos + distance) < endPos)
        {
            startPos += distance;
        }
        else if ((startPos - distance) > endPos)
        {
            startPos -= distance;
        }
    }
}