using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject rotationCenter; // Reference to the object we want to follow
    public GameObject player; // Reference to the player object
    public Rotate rotate; // Reference to the Rotate script

    public float followSpeed = 0.5f; // Adjust this value to control the smoothness of the camera movement
    public Vector3 cameraOffset = new Vector3(0, 0, -0.001f); // Offset of the camera from the target 
    
    void Update()
    {
        if (!rotate.isMenu)
        {
            if (rotate.dead)
            {
                FollowTarget(player);
            }
            else
            {
                FollowTarget(rotationCenter);
            }
        }      
    }

    void FollowTarget(GameObject target)
    {
        if (target != null)
        {
            // Calculate the target position with the offset
            Vector3 targetPosition = target.transform.position + cameraOffset;
            // Use Lerp to smoothly interpolate between the camera's current position and the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}

