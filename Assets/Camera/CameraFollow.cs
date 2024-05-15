// Created by Carl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset; // Set this to a Y value of -1.8f to start below the player
    public float dampingTime;
    public Vector2 currentVelocity;

    [Header("Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector3 position;

    private void Start()
    {
        // Set the initial camera position
        Vector3 startPosition = player.position + cameraOffset;
        transform.position = new Vector3(
            Mathf.Clamp(startPosition.x, minX, maxX),
            Mathf.Clamp(startPosition.y, minY, maxY),
            startPosition.z
        );
    }

    private void Update()
    {
        AdjustCameraBounds();
        position = player.position + cameraOffset;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = Vector2.SmoothDamp(transform.position, position, ref currentVelocity, dampingTime);
    }

    void AdjustCameraBounds()
    {
        //Platform#1
        if (player.position.x < 10f)
        {
            minY = -1.8f;
            maxY = 1.8f;
        }
        //Platform#2
        if (player.position.y <= -4.35 && player.position.x >= 9)
        {
            minY = -100f;
        }
    }
}
