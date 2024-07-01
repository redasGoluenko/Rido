using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByX : MonoBehaviour
{
    // Public variables for speed, set in the Unity Editor or via script
    public float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Method to move the sprite up by a specified distance at the specified speed
    public void MoveUp(float distance)
    {
        StartCoroutine(Move(Vector3.up * distance));
    }

    // Method to move the sprite down by a specified distance at the specified speed
    public void MoveDown(float distance)
    {
        StartCoroutine(Move(Vector3.down * distance));
    }

    // Coroutine to move the sprite in a given direction by a specified distance
    private IEnumerator Move(Vector3 direction)
    {
        // Detach from parent
        Transform originalParent = transform.parent;
        transform.SetParent(null);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction;
        float totalDistance = direction.magnitude;
        float traveledDistance = 0.0f;

        while (traveledDistance < totalDistance)
        {
            // Calculate movement for this frame
            float moveStep = speed * Time.deltaTime;

            // Move the sprite
            transform.position += direction.normalized * moveStep;

            // Update the traveled distance
            traveledDistance += moveStep;

            // Yield control back to the main loop and continue in the next frame
            yield return null;
        }

        // Ensure final position is exactly the target position
        transform.position = targetPosition;

        // Optionally reattach to the original parent
        // transform.SetParent(originalParent);
    }
}
