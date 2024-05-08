using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public GameObject camera;
    public float parallaxFactor;
    public float startPos;
    public float endPos;
    public float distance;
    public float move;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        distance = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        endPos = (camera.transform.position.x * (1 - parallaxFactor));
        move = (camera.transform.position.x * parallaxFactor);

        transform.position = new Vector2(startPos + move, transform.position.y);

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
