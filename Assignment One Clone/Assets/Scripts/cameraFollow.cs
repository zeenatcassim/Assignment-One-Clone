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
    private Vector2 defaultMaxOffset = new Vector2(1, 1); // Default max offset from the player
    private Vector2 extendedMaxOffset = new Vector2(9, 5); // Extended max offset when Shift is pressed
    private Vector2 currentMaxOffset;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        currentMaxOffset = defaultMaxOffset;
    }

    void LateUpdate()
    {
        // Check if Shift key is pressed
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        currentMaxOffset = isShiftPressed ? extendedMaxOffset : defaultMaxOffset;

        Vector3 desiredPosition = player.position + offset;
        Vector3 mousePos = Input.mousePosition;

        // Calculate screen edges
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Adjust camera position based on cursor position and Shift key
        float horizontalAdjustment = (mousePos.x - screenWidth / 2) / screenWidth * currentMaxOffset.x * 2;
        float verticalAdjustment = (mousePos.y - screenHeight / 2) / screenHeight * currentMaxOffset.y * 2;

        desiredPosition.x += horizontalAdjustment;
        desiredPosition.y += verticalAdjustment;

        // Clamp the desired position to stay within the max offset range from the player
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, player.position.x - currentMaxOffset.x, player.position.x + currentMaxOffset.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, player.position.y - currentMaxOffset.y, player.position.y + currentMaxOffset.y);

        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
