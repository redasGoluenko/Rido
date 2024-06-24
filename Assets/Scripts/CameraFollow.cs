using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // Reference to the object we want to follow
    public float followSpeed = 0.5f; // Adjust this value to control the smoothness of the camera movement
    public Vector3 cameraOffset = new Vector3(0, 0, -0.1f); // Offset of the camera from the target

    // Update is called once per frame
    void Update()
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

