using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public GameObject topSlope; // Reference to the top slope object
    public GameObject bottomSlope; // Reference to the bottom slope object
    private bool isMoving = false; // Flag to track if the movement has started

    // Start is called before the first frame update
    void Start()
    {
        // Initial movement in local directions
        StartCoroutine(MoveObjectInDirection(topSlope, Vector3.up, 5.47f, 1f));
        StartCoroutine(MoveObjectInDirection(bottomSlope, Vector3.down, 6.5f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();
    }

    public void HandleDeath()
    {
        // Only start the movement if 'rotate.dead' is true and movement hasn't started
        if (rotate.dead && !isMoving)
        {
            rotate.dead = false; // Reset the flag to prevent multiple calls
            isMoving = true; // Set the flag to prevent starting the coroutine again

            // Move the slopes in their local directions relative to their current rotation
            StartCoroutine(MoveObjectInDirection(topSlope, topSlope.transform.up * -1, 5.47f, 1f));
            StartCoroutine(MoveObjectInDirection(bottomSlope, bottomSlope.transform.up, 6.5f, 1f));
        }
    }

    IEnumerator MoveObjectInDirection(GameObject obj, Vector3 direction, float distance, float duration)
    {
        if (obj == null)
        {
            yield break;
        }

        Vector3 startingPosition = obj.transform.position;
        Vector3 destination = startingPosition + direction.normalized * distance; // Calculate the new target position
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startingPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = destination; // Ensure we finish exactly at the destination

        isMoving = false; // Reset the flag if you want to allow movement again later
    }
}
