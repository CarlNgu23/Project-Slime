//Created by Carl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 position;
    public Vector3 cameraOffset;
    public float dampingTime;
    public Vector2 currentVelocity;

    [Header("Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Update()
    {
        CheckArea();
        position = player.position + cameraOffset;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = Vector2.SmoothDamp(transform.position, position, ref currentVelocity, dampingTime);
    }

    void CheckArea()
    {
        //Platform#1
        if (player.position.y < 1.8 && player.position.y > -4.35)
        {
            minY = -1.8f;
            maxY = 1.8f;
        }
        //Platform#2
        if (player.position.y <= -4.35 && player.position.x >= 9)
        {
            minY = -5.2f;
        }
    }
}
