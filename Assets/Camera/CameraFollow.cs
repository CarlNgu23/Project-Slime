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
        if (player.position.x < 10f)
        {
            minY = -2f;
            maxY = 2f;
        }
        else
        {
            maxY = 100f;
        }
    }
}
