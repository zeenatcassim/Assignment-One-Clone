using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float edgeOffset = 100f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 mousePos = Input.mousePosition;

        // Calculate screen edges
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Adjust camera position based on cursor position
        if (mousePos.x < edgeOffset)
        {
            desiredPosition.x -= (edgeOffset - mousePos.x) / edgeOffset;
        }
        if (mousePos.x > screenWidth - edgeOffset)
        {
            desiredPosition.x += (mousePos.x - (screenWidth - edgeOffset)) / edgeOffset;
        }
        if (mousePos.y < edgeOffset)
        {
            desiredPosition.y -= (edgeOffset - mousePos.y) / edgeOffset;
        }
        if (mousePos.y > screenHeight - edgeOffset)
        {
            desiredPosition.y += (mousePos.y - (screenHeight - edgeOffset)) / edgeOffset;
        }

        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
