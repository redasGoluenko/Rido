using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotationCenter; // Reference to the object we want to rotate around
    public float rotateSpeed = 50f; // Speed of rotation in degrees per second
    public bool clockwise = true; // Direction of rotation
    public float desiredDistance = 5f; // The desired distance from the rotation center
    public float correctionSpeed = 2f; // Speed at which the distance correction happens

    // Update is called once per frame
    void Update()
    {
        // Ensure rotationCenter is assigned in the Inspector or find it in Start() if it's not assigned.
        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center not assigned!");
            return;
        }

        // Orbit around the rotationCenter
        OrbitAround();

        // Adjust the distance to the desired distance
        CorrectDistance();
    }

    void OrbitAround()
    {
        // Calculate the orbit movement
        Vector3 relativePos = transform.position - rotationCenter.position;

        // Determine rotation direction based on clockwise boolean
        float direction = clockwise ? 1f : -1f;

        Quaternion rotation = Quaternion.Euler(0, 0, direction * rotateSpeed * Time.deltaTime);
        relativePos = rotation * relativePos;
        transform.position = rotationCenter.position + relativePos;
    }

    void CorrectDistance()
    {
        // Calculate the current distance from the rotation center
        Vector3 toCenter = transform.position - rotationCenter.position;
        float currentDistance = toCenter.magnitude;

        // Check if the distance is different from the desired distance
        if (Mathf.Abs(currentDistance - desiredDistance) > 0.01f) // Adding a small tolerance
        {
            // Calculate the direction to adjust the position
            Vector3 direction = toCenter.normalized;

            // Interpolate towards the correct distance
            Vector3 targetPosition = rotationCenter.position + direction * desiredDistance;

            // Move the object slightly towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, correctionSpeed * Time.deltaTime);
        }
    }
}
