using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target; // The target that the camera will follow (your boat)
    public float smoothSpeed = 0.125f; // The speed with which the camera will follow
    public Vector3 offset; // Offset between the camera and the target
    public GameObject inventory; // Reference to the inventory UI panel

    void FixedUpdate()
    {
        // Desired position of the camera based on the target's position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Check if the inventory panel is active
        if (inventory.activeSelf)
        {
            // Offset the camera to the right by 25%
            transform.position = new Vector3(smoothedPosition.x + (Screen.width * 0.55f / Camera.main.pixelWidth), smoothedPosition.y, transform.position.z);
        }
        else
        {
            // Update the camera's position but keep the z-value the same to avoid changing the camera depth
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}
