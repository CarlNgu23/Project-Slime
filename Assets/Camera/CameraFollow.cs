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
        // Adjust minY and maxY based on the player's position plus the camera offset
        if (player.position.y + cameraOffset.y > maxY)
        {
            maxY = player.position.y + cameraOffset.y;
        }

        if (player.position.y + cameraOffset.y < minY)
        {
            minY = player.position.y + cameraOffset.y;
        }
    }
}
