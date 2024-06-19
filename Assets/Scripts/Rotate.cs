using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotationCenter; // Reference to the object we want to rotate around
    public float rotateSpeed = 50f; // Speed of rotation in degrees per second

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
    }

    void OrbitAround()
    {
        // Calculate the orbit movement
        Vector3 relativePos = transform.position - rotationCenter.position;
        Quaternion rotation = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
        relativePos = rotation * relativePos;
        transform.position = rotationCenter.position + relativePos;
    }
}
